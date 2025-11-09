using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;
using Unity.VisualScripting;
using System.Collections;

// O nome da sua classe é ObjectGrabbing
public class ObjectGrabbing : MonoBehaviour
{
    #region Variaveis
    [Header("Configurações de Pegar Objeto")]
    [Tooltip("O ponto na 'mão' do personagem onde o objeto ficará preso.")]
    [SerializeField] private Transform handPoint;

    [Tooltip("A camada (Layer) dos objetos que podem ser pegos.")]
    [SerializeField] private LayerMask grabbingLayer;
    [Tooltip("Segurando o objeto.")]
    [SerializeField] private bool grabbingObject = false;
    public bool GrabbingObject { get { return grabbingObject; } }

    [Header("Configurações de Arremesso")]
    [Tooltip("A força horizontal MÍNIMA do arremesso (distância).")]
    [SerializeField] private float horizontalForceMin = 7f;
    [Tooltip("A força horizontal MÁXIMA do arremesso (distância).")]
    [SerializeField] private float horizontalForceMax = 25f;
    [Tooltip("A força vertical MÍNIMA do arremesso (altura).")]
    [SerializeField] private float verticallForceMin = 3f;
    [Tooltip("A força vertical MÁXIMA do arremesso (altura).")]
    [SerializeField] private float verticallForceMax = 8f;
    [Tooltip("O tempo em segundos segurando o botão para atingir a força máxima.")]
    [SerializeField] private float tempoMaximoDeCarga = 2f;

    [Header("Referências Externas")]
    [Tooltip("Referência para o script de movimento do jogador. Essencial para a nova lógica de direção.")]
    [SerializeField] private Movement movementScript;
    [SerializeField] private Direction direction;
    [Tooltip("Camera do jogo")]
    [SerializeField] private Camera cam;

    [Header("Controle de Animação")]
    [Tooltip("Referência ao controlador de animação do player.")]
    [SerializeField] private PlayerAnimationController animController;

    private GameObject grabObject = null;
    private Rigidbody grabObjectRb = null;

    [Header("Controle de Alcance (Auto-Configurado)")]
    [Tooltip("Referência ao SphereCollider que define a área de 'pegar'.")]
    [SerializeField] private SphereCollider grabCollider;

    public bool IsNearGrabbable { get; private set; }

    // Array para a checagem de física (melhor performance)
    private Collider[] nearbyObjects = new Collider[5];

    // Variáveis para controlar o tempo de carregamento
    private bool isCharging = false;
    public bool IsCharging { get { return isCharging; } }

    private float currentChargeTime = 0f;

    // Expõe o tempo atual e o máximo para o script da UI
    public float CurrentChargeTime { get { return currentChargeTime; } }
    public float MaxChargeTime { get { return tempoMaximoDeCarga; } }
    #endregion

    #region Métodos da Unity

    // Pega o SphereCollider automaticamente
    private void Awake()
    {
        if (grabCollider == null)
        {
            grabCollider = GetComponent<SphereCollider>();
        }

        if (grabCollider == null)
        {
            Debug.LogError("ObjectGrabbing: Nenhum SphereCollider encontrado! " +
                           "Adicione um SphereCollider ou arraste-o manualmente.", this);
        }
    }

    // Garante que o estado inicial está correto
    private void Start()
    {
        grabbingObject = false;
    }

    // Chamado a cada frame
    private void Update()
    {
        // --- CHECAGEM CONSTANTE DE OBJETOS PRÓXIMOS ---
        if (grabCollider != null)
        {
            System.Array.Clear(nearbyObjects, 0, nearbyObjects.Length);

            int numFound = Physics.OverlapSphereNonAlloc(
                transform.position + grabCollider.center,
                grabCollider.radius,
                nearbyObjects,
                grabbingLayer);

            IsNearGrabbable = (numFound > 0);
        }
        else
        {
            IsNearGrabbable = false;
        }
        // --- FIM DA CHECAGEM ---


        // --- LÓGICA DE CARREGAMENTO E ROTAÇÃO ---
        if (isCharging)
        {
            // Incrementa o tempo de carregamento
            currentChargeTime = Mathf.Min(currentChargeTime + Time.deltaTime, tempoMaximoDeCarga);

            // Força o jogador a olhar na direção da mira (do script Direction.cs)
            if (direction != null && movementScript != null && direction.directionVector != Vector3.zero)
            {
                // Chama a função de rotação no script de movimento
                movementScript.RotateTowards(direction.directionVector);
            }
        }
    }
    #endregion

    #region Callbacks de Input (Input System)

    // Tenta pegar ou soltar um objeto
    public void InteractionGrabbing(InputAction.CallbackContext context)
    {
        // Se apertou e NÃO está segurando nada -> Tenta Pegar
        if (context.started && grabObject == null)
        {
            if (grabCollider == null) return;

            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, grabbingLayer))
            {
                // Checa a distância usando o collider
                float distance = Vector3.Distance(hit.transform.position, transform.position + grabCollider.center);

                if (distance <= grabCollider.radius)
                {
                    GrabObject_DisgrabObject(hit.collider.gameObject);
                }
            }
        }
        // Se apertou e JÁ está segurando -> Solta
        else if (context.started && grabObject != null)
        {
            GrabObject_DisgrabObject(grabObject);
        }
    }

    // Controla o carregamento e execução do arremesso
    public void PullGrabobject(InputAction.CallbackContext context)
    {
        // Botão foi pressionado (começou a carregar)
        if (context.started && grabObject != null)
        {
            isCharging = true;
            currentChargeTime = 0f;
            animController?.SetCharging(true); // Avisa o Animator
            movementScript.canMove = false; // Impede o movimento durante o carregamento

            // Trava a rotação normal de movimento
            if (movementScript != null) movementScript.overrideRotation = true;
        }

        // Botão foi solto (executa o arremesso)
        if (context.canceled && grabObject != null)
        {
            // Calcula a força baseada no tempo de carregamento
            float chargePercent = currentChargeTime / tempoMaximoDeCarga;
            float horizontalForce = Mathf.Lerp(horizontalForceMin, horizontalForceMax, chargePercent);
            float verticalForce = Mathf.Lerp(verticallForceMin, verticallForceMax, chargePercent);

            // Pega a direção do script 'Direction'
            Vector3 throwDirection = direction.directionVector;
            Vector3 throwForce = throwDirection * horizontalForce + Vector3.up * verticalForce;

            // Solta o objeto e aplica a força
            grabObject.transform.SetParent(null);
            grabObjectRb.isKinematic = false;
            grabObjectRb.AddForce(throwForce, ForceMode.Impulse);
            movementScript.canMove = true; // Libera o movimento novamente

            // Limpa todas as variáveis de estado
            grabObjectRb = null;
            grabObject = null;
            grabbingObject = false;
            isCharging = false;
            currentChargeTime = 0f;

            // Avisa o Animator
            animController?.SetCharging(false);
            animController?.TriggerThrow();
            animController?.SetHolding(false);

            // Libera a rotação de movimento
            if (movementScript != null) movementScript.overrideRotation = false;
        }
    }
    #endregion

    #region Lógica Interna

    // Gerencia o ato de pegar ou soltar o objeto
    private void GrabObject_DisgrabObject(GameObject objectGrab)
    {
        if (grabObject == null) // Lógica para PEGAR
        {
            if (objectGrab.CompareTag("Ball"))
            {

                StartCoroutine(Pegar(objectGrab));
            }
        }
        else if (grabObject != null) // Lógica para SOLTAR
        {
            StartCoroutine(Disgrab(objectGrab));
        }
    }

    private IEnumerator Pegar(GameObject gameObject)
    {         
        grabObject = gameObject;
        grabObject.transform.SetParent(handPoint);
        grabObject.transform.position = handPoint.transform.position;
        grabObject.transform.rotation = handPoint.transform.rotation;
        grabObjectRb = grabObject.GetComponent<Rigidbody>();
        grabObjectRb.isKinematic = true;
        grabbingObject = true;
        animController?.SetHolding(true);
        animController?.TriggerCollect();
        yield return null;
    }
    private IEnumerator Disgrab(GameObject gameObject)
    {
        grabObject.transform.SetParent(null);
        grabObjectRb.isKinematic = false;
        grabObjectRb = null;
        grabObject = null;
        grabbingObject = false;
        movementScript.canMove = true; // Garante que o movimento está liberado
        animController?.SetHolding(false);
        animController?.TriggerDrop();
        yield return null;
    }

    #endregion

    #region Gizmos

    // Desenha o Gizmo de alcance no editor
    private void OnDrawGizmosSelected()
    {
        if (grabCollider == null)
        {
            grabCollider = GetComponent<SphereCollider>();
        }
        if (grabCollider == null) return;

        // Desenha a esfera usando as propriedades do SphereCollider
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + grabCollider.center, grabCollider.radius);
    }
    #endregion
}