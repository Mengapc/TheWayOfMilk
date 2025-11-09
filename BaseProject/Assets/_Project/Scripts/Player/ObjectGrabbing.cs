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
    [Tooltip("Referência ao SphereCollider que define a área de 'pegar'.")]
    [SerializeField] private SphereCollider grabCollider;
    [Tooltip("A distância máxima para pegar um objeto.")]
    [SerializeField] private float grabRange = 2f;

    private GameObject currentGrabbableObject = null;
    private GameObject grabObject = null;
    private Rigidbody grabObjectRb = null;
    private bool isNearGrabbableDistance = false;
    public bool IsNearGrabbableDistance { get { return isNearGrabbableDistance; } }
    public bool GrabbingObject { get { return grabbingObject; } }
    public bool IsNearGrabbable { get; private set; }

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


    public bool IsCharging { get { return isCharging; } }
    private float currentChargeTime = 0f;
    public float CurrentChargeTime { get { return currentChargeTime; } }
    public float MaxChargeTime { get { return tempoMaximoDeCarga; } }


    [Header("Referências Externas")]
    [Tooltip("Referência para o script de movimento do jogador. Essencial para a nova lógica de direção.")]
    [SerializeField] private Movement movementScript;
    [SerializeField] private Direction direction;
    [Tooltip("Camera do jogo")]
    [SerializeField] private Camera cam;

    [Header("Controle de Animação")]
    [Tooltip("Referência ao controlador de animação do player.")]
    [SerializeField] private PlayerAnimationController animController;
    private bool isCharging = false;
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

    private void OnTriggerEnter(Collider collision)
    {
        // --- LÓGICA DE DISTÂNCIA PARA PEGAR OBJETO (CORRIGIDA) ---
        // Só checa a distância SE houver um objeto no trigger
        if (currentGrabbableObject != null)
        {
            isNearGrabbableDistance = IsWithinGrabRange(currentGrabbableObject.transform.position);

            if (collision.gameObject.CompareTag("Ball") && !grabbingObject)
            {
                IsNearGrabbable = true;
            }
            else
            {
                // Se não há objeto no trigger, com certeza não estamos perto
                isNearGrabbableDistance = false;
            }
        }
    }
    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            IsNearGrabbable = false;
        }
    }

    // Chamado a cada frame
    private void Update()
    {

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
                if (currentGrabbableObject)
                {
                    GrabObject_DisgrabObject(currentGrabbableObject);
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
        currentGrabbableObject = null;
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

    private bool IsWithinGrabRange(Vector3 objectPosition)
    {
        if (currentGrabbableObject == null) return false;
        float distance = Vector3.Distance(objectPosition, transform.position + grabCollider.center);
        
        if (distance <= grabRange)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    #endregion

}