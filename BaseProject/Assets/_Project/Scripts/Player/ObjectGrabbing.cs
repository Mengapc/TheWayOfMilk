using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;


public class ObjectGrabbing : MonoBehaviour
{
    #region Variaveis
    [Header("Configurações de Pegar Objeto")]
    [Tooltip("Tranforme do personagem.")]
    [SerializeField] private Transform player;
    [Tooltip("O ponto na 'mão' do personagem onde o objeto ficará preso.")]
    [SerializeField] private Transform handPoint;
    [Tooltip("O raio da esfera para detectar objetos que podem ser pegos.")]
    [Range(0.1f, 20f)]
    [SerializeField] private float distanceGrab = 1.5f;
    [Tooltip("O offset para a posição de pegar o objeto.")]
    [SerializeField] private Vector3 offsetGrab;
    [Tooltip("A camada (Layer) dos objetos que podem ser pegos.")]
    [SerializeField] private LayerMask grabbingLayer;
    [Tooltip("Segurando o objeto.")]
    [SerializeField] private bool grabbingObject;
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
    [SerializeField] private Movement _movementScript;
    [Tooltip("Direção do arremesso.")]
    [SerializeField] private Direction direction;
    [Tooltip("Camera do jogo")]
    [SerializeField] private Camera cam;

    private GameObject grabObject = null;
    private Rigidbody grabObjectRb = null;
    #endregion

    #region Métodos da Unity

    // Chamado a cada frame
    private void Update()
    {
        // Desenha o raio de pegar objeto no modo 'Scene' para debug
        Debug.DrawRay(player.position, player.forward * distanceGrab, Color.red);
    }

    #endregion

    #region Callbacks de Input (Input System)

    // Chamado pelo Input System para tentar pegar ou soltar um objeto
    public void InteractionGrabbing(InputAction.CallbackContext context)
    {
        // Se o botão foi apertado e NÃO estamos segurando um objeto -> Tenta Pegar
        if (context.started && grabObject == null)
        {
            Debug.Log("Raio lançado"); // Corrigi o typo de "Rio" para "Raio"
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Lança um raio a partir da câmera na camada 'grabbingLayer'
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, grabbingLayer))
            {
                Debug.Log("Objeto atingido: " + hit.collider.gameObject.name);
                float distance = Vector3.Distance(hit.transform.position, player.position + offsetGrab);
                Debug.Log("distancia atingido: " + distance);

                // Verifica se o objeto atingido está dentro do alcance
                if (Vector3.Distance(hit.transform.position, player.position) <= distanceGrab)
                {
                    GrabObject_DisgrabObject(hit.collider.gameObject);
                }
            }
        }
        // Se o botão foi apertado e JÁ estamos segurando um objeto -> Solta
        else if (context.started && grabObject != null)
        {
            GrabObject_DisgrabObject(grabObject);
        }
    }

    // Chamado pelo Input System para carregar e arremessar o objeto
    public void PullGrabobject(InputAction.CallbackContext context)
    {
        // Botão foi pressionado (começou a carregar)
        if (context.started && grabObject != null)
        {
            Debug.Log("Começou a carregar o arremesso");
            // (Pode adicionar lógica de UI aqui, se necessário)
        }

        // Botão foi solto (executa o arremesso)
        if (context.canceled && grabObject != null)
        {
            Debug.Log("Soltou o botão de arremesso");

            // 1. Calcular o tempo que o botão foi pressionado (limitado pelo tempo máximo)
            float chargeTime = Mathf.Clamp((float)context.time, 0, tempoMaximoDeCarga);
            float chargePercent = chargeTime / tempoMaximoDeCarga; // Porcentagem da força (0.0 a 1.0)

            // 2. Calcular as forças com base no tempo de carga (usando Lerp)
            float horizontalForce = Mathf.Lerp(horizontalForceMin, horizontalForceMax, chargePercent);
            float verticalForce = Mathf.Lerp(verticallForceMin, verticallForceMax, chargePercent);

            // 3. Definir a direção do arremesso (baseada na direção do jogador)
            Vector3 throwDirection = direction.directionVector;
            Vector3 throwForce = throwDirection * horizontalForce + Vector3.up * verticalForce;

            // 4. Soltar o objeto e aplicar a força
            grabObject.transform.SetParent(null);
            grabObjectRb.isKinematic = false;
            grabObjectRb.AddForce(throwForce, ForceMode.Impulse); // Impulse aplica a força instantaneamente

            // 5. Resetar variáveis
            grabObjectRb = null;
            grabObject = null;
        }
    }

    #endregion

    #region Lógica Interna

    // Função interna que gerencia o ato de pegar ou soltar o objeto
    private void GrabObject_DisgrabObject(GameObject objectGrab)
    {
        // Se não estamos segurando nada -> Pega o objeto
        if (grabObject == null)
        {
            if (objectGrab.CompareTag("Ball")) // Verifica se é um objeto 'Ball'
            {
                // Seta como parente da 'mão'
                grabObject = objectGrab;
                grabObject.transform.SetParent(handPoint);
                grabObject.transform.position = handPoint.transform.position;
                grabObject.transform.rotation = handPoint.transform.rotation; // Pega a rotação da 'mão'

                // Desativa a física (Kinematic) para que o objeto siga a mão
                grabObjectRb = grabObject.GetComponent<Rigidbody>();
                grabObjectRb.isKinematic = true;
            }
        }
        // Se já estamos segurando -> Solta o objeto
        else if (grabObject != null)
        {
            // Libera o objeto da 'mão'
            grabObject.transform.SetParent(null);

            // Reativa a física
            grabObjectRb.isKinematic = false;

            // Limpa as referências
            grabObjectRb = null;
            grabObject = null;
        }
    }



    #endregion

    #region Gizmos

    // Desenha a esfera de alcance no Editor quando o objeto está selecionado
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        // Desenha a esfera na posição do 'player' + 'offset' com o raio 'distanceGrab'
        Gizmos.DrawWireSphere(player.position + offsetGrab, distanceGrab);
    }

    #endregion
}
