using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;
using Unity.VisualScripting;

// ATENÇÃO: Este script agora é o 'ObjectGrabbing.cs'
// O nome do arquivo no canvas parece estar dessincronizado.
public class ObjectGrabbing : MonoBehaviour
{
    #region Variaveis
    [Header("Configurações de Pegar Objeto")]
    [Tooltip("Tranforme do personagem.")]
    [SerializeField] private Transform player;
    [Tooltip("O ponto na 'mão' do personagem onde o objeto ficará preso.")]
    [SerializeField] private Transform handPoint;

    // REMOVIDO: distanceGrab e offsetGrab. Usaremos o SphereCollider.

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

    // --- NOVAS VARIÁVEIS ---
    [Header("Controle de Alcance (Auto-Configurado)")]
    [Tooltip("Referência ao SphereCollider que define a área de 'pegar'.")]
    [SerializeField] private SphereCollider grabCollider; // Referência ao collider

    // Propriedade pública para a UI saber se há algo perto
    public bool IsNearGrabbable { get; private set; }

    // Array pré-alocado para a checagem de física (melhor performance)
    private Collider[] nearbyObjects = new Collider[5];
    // --- FIM DAS NOVAS VARIÁVEIS ---


    // Variáveis para controlar o tempo de carregamento
    private bool isCharging = false;
    public bool IsCharging { get { return isCharging; } } // <-- Propriedade pública
    private float currentChargeTime = 0f;
    #endregion

    #region Métodos da Unity

    // Awake é chamado antes de Start
    private void Awake()
    {
        // Pega o SphereCollider automaticamente no mesmo GameObject
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
    // Dentro de ObjectGrabbing.cs
    private void Start()
    {
        grabbingObject = false; // Garante que começa como falso
    }

    // Chamado a cada frame
    private void Update()
    {
        //desenhar o raio de pegar objeto (agora usa o collider)
        // O Gizmo OnDrawGizmosSelected faz isso de forma mais visível
        // Debug.DrawRay(player.position, player.forward * grabCollider.radius, Color.red);

        // --- CHECAGEM CONSTANTE ---
        if (grabCollider != null)
        {
            // Limpa o array de detecção
            System.Array.Clear(nearbyObjects, 0, nearbyObjects.Length);

            // Verifica constantemente se há objetos pegáveis por perto
            int numFound = Physics.OverlapSphereNonAlloc(
                player.position + grabCollider.center, // Centro da esfera (do collider)
                grabCollider.radius,                   // Raio da esfera (do collider)
                nearbyObjects,                         // Array para guardar os resultados
                grabbingLayer);                        // Camada dos objetos pegáveis

            // Se encontrou 1 ou mais objetos, marca como 'true'
            IsNearGrabbable = (numFound > 0);
        }
        else
        {
            IsNearGrabbable = false;
        }
        // --- FIM DA CHECAGEM ---


        // Lógica de Carregamento de Arremesso
        if (isCharging)
        {
            // Incrementa o tempo de carregamento, limitado ao máximo
            currentChargeTime = Mathf.Min(currentChargeTime + Time.deltaTime, tempoMaximoDeCarga);
        }
    }
    #endregion

    #region Callbacks de Input (Input System)
    //interação com o objetod
    public void InteractionGrabbing(InputAction.CallbackContext context)
    {
        // Se o botão foi apertado e NÃO estamos segurando um objeto -> Tenta Pegar
        if (context.started && grabObject == null)
        {
            if (grabCollider == null) return; // Segurança

            Debug.Log("Raio lançado");
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, grabbingLayer))
            {
                Debug.Log("Objeto atingido: " + hit.collider.gameObject.name);

                // --- LÓGICA DE DISTÂNCIA ATUALIZADA ---
                // Usa o centro e raio do SphereCollider para a checagem
                float distance = Vector3.Distance(hit.transform.position, player.position + grabCollider.center);
                Debug.Log("distancia atingido: " + distance);

                if (distance <= grabCollider.radius)
                {
                    GrabObject_DisgrabObject(hit.collider.gameObject);
                }
                // --- FIM DA ATUALIZAÇÃO ---
            }
        }
        // Se o botão foi apertado e JÁ estamos segurando um objeto -> Solta
        else if (context.started && grabObject != null)
        {
            GrabObject_DisgrabObject(grabObject);
        }
    }

    //jogar o objeto
    public void PullGrabobject(InputAction.CallbackContext context)
    {
        // Botão foi pressionado (começou a carregar)
        if (context.started && grabObject != null)
        {
            Debug.Log("Começou a carregar o arremesso");
            isCharging = true;
            currentChargeTime = 0f;
            animController?.SetCharging(true); // Avisa o Animator
        }

        // Botão foi solto (executa o arremesso)
        if (context.canceled && grabObject != null)
        {
            Debug.Log("Soltou o botão de arremesso");

            // 1. Calcular a porcentagem da força com base no tempo que carregamos
            float chargePercent = currentChargeTime / tempoMaximoDeCarga;

            // 2. Calcular as forças com base no tempo de carga (usando Lerp)
            float horizontalForce = Mathf.Lerp(horizontalForceMin, horizontalForceMax, chargePercent);
            float verticalForce = Mathf.Lerp(verticallForceMin, verticallForceMax, chargePercent);

            // 3. Definir a direção do arremesso (baseada na direção do jogador)
            Vector3 throwDirection = direction.directionVector;
            Vector3 throwForce = throwDirection * horizontalForce + Vector3.up * verticalForce;

            // 4. Soltar o objeto e aplicar a força
            grabObject.transform.SetParent(null);
            grabObjectRb.isKinematic = false;
            grabObjectRb.AddForce(throwForce, ForceMode.Impulse);

            // 5. Resetar variáveis
            grabObjectRb = null;
            grabObject = null;
            grabbingObject = false; // <-- CORREÇÃO
            isCharging = false;
            currentChargeTime = 0f;

            // 6. Avisar o Animator
            animController?.SetCharging(false); // Para de carregar
            animController?.TriggerThrow();     // Dispara a animação de arremesso
        }
    }
    #endregion

    #region Lógica Interna
    //pegar o objeto 
    private void GrabObject_DisgrabObject(GameObject objectGrab)
    {
        if (grabObject == null)
        {
            if (objectGrab.CompareTag("Ball"))
            {
                //seta como parente da mão
                grabObject = objectGrab;
                grabObject.transform.SetParent(handPoint);
                grabObject.transform.position = handPoint.transform.position;
                grabObject.transform.rotation = handPoint.transform.rotation; // Pega a rotação da 'mão'
                grabObjectRb = grabObject.GetComponent<Rigidbody>();
                grabObjectRb.isKinematic = true;
                grabbingObject = true; // <-- CORREÇÃO

                // Avisa o Animator
                animController?.SetHolding(true);
                animController?.TriggerCollect();
            }
        }
        else if (grabObject != null)
        {
            grabObject.transform.SetParent(null);
            grabObjectRb.isKinematic = false;
            grabObjectRb = null;
            grabObject = null;
            grabbingObject = false; // <-- CORREÇÃO

            // Avisa o Animator
            animController?.SetHolding(false);
            animController?.TriggerDrop();
        }
    }
    #endregion

    #region Gizmos
    // --- GIZMO ATUALIZADO ---
    private void OnDrawGizmosSelected()
    {
        // Se não tivermos o collider, tenta pegar
        if (grabCollider == null)
        {
            grabCollider = GetComponent<SphereCollider>();
        }

        // Se, depois de tentar, ainda for nulo, sai para evitar erros
        if (grabCollider == null) return;

        // Desenha o Gizmo usando EXATAMENTE o centro e o raio do SphereCollider
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(player.position + grabCollider.center, grabCollider.radius);
    }
    // --- FIM DA ATUALIZAÇÃO ---
    #endregion
}

