using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;
using Unity.VisualScripting;


public class ObjectGrabbing : MonoBehaviour
{
    #region Variaveis
    [Header("Configura��es de Pegar Objeto")]
    [Tooltip("Tranforme do personagem.")]
    [SerializeField] private Transform player;
    [Tooltip("O ponto na 'm�o' do personagem onde o objeto ficar� preso.")]
    [SerializeField] private Transform handPoint;
    [Tooltip("O raio da esfera para detectar objetos que podem ser pegos.")]
    [SerializeField] private float distanceGrab = 1.5f;
    [Tooltip("A camada (Layer) dos objetos que podem ser pegos.")]
    [SerializeField] private LayerMask grabbingLayer;
    [Tooltip("Segurando o objeto.")]
    [SerializeField] private bool grabbingObject;
    public bool GrabbingObject { get { return grabbingObject; } }

    [Header("Configura��es de Arremesso")]
    [Tooltip("A for�a horizontal M�NIMA do arremesso (dist�ncia).")]
    [SerializeField] private float horizontalForceMin = 7f;
    [Tooltip("A for�a horizontal M�XIMA do arremesso (dist�ncia).")]
    [SerializeField] private float horizontalForceMax = 25f;
    [Tooltip("A for�a vertical M�NIMA do arremesso (altura).")]
    [SerializeField] private float verticallForceMin = 3f;
    [Tooltip("A for�a vertical M�XIMA do arremesso (altura).")]
    [SerializeField] private float verticallForceMax = 8f;
    [Tooltip("O tempo em segundos segurando o bot�o para atingir a for�a m�xima.")]
    [SerializeField] private float tempoMaximoDeCarga = 2f;

    [Header("Refer�ncias Externas")]
    [Tooltip("Refer�ncia para o script de movimento do jogador. Essencial para a nova l�gica de dire��o.")]
    [SerializeField] private Movement movementScript;
    [SerializeField] private Direction direction;
    [Tooltip("Camera do jogo")]
    [SerializeField] private Camera cam;

    private GameObject grabObject = null;
    private Rigidbody grabObjectRb = null;
    #endregion

    private void Update()
    {
        //desenhar o raio de pegar objeto
        Debug.DrawRay(player.position, player.forward * distanceGrab, Color.red);
    }

    //intera��o com o objetod
    public void InteractionGrabbing(InputAction.CallbackContext context)
    {
        if (context.started && grabObject == null)
        {
            Debug.Log("Rio lan�cado");
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, grabbingLayer))
            {
                Debug.Log("Objeto atingido: " + hit.collider.gameObject.name);
                float distance = Vector3.Distance(hit.transform.position, player.position);
                Debug.Log("distancia atingido: " + distance);

                if (Vector3.Distance(hit.transform.position, player.position) <= distanceGrab)
                {
                    GrabObject_DisgrabObject(hit.collider.gameObject);
                }
            }
        }
        else if (context.started && grabObject != null)
        {
            GrabObject_DisgrabObject(grabObject);
        }
    }

    //pegar o objeto 
    private void GrabObject_DisgrabObject(GameObject objectGrab)
    {
        if (grabObject == null)
        {
            if (objectGrab.CompareTag("Ball"))
            {
                //seta como parente da m�o
                grabObject = objectGrab;
                grabObject.transform.SetParent(handPoint);
                grabObject.transform.position = handPoint.transform.position;
                grabObject.transform.rotation = Quaternion.identity;
                grabObjectRb = grabObject.GetComponent<Rigidbody>();
                grabObjectRb.isKinematic = true;
            }
        }
        else if(grabObject != null)
        {
            grabObject.transform.SetParent(null);
            grabObjectRb.isKinematic = false;
            grabObjectRb = null;
            grabObject = null;
        }
    }

    //jogar o objeto
    public void PullGrabobject(InputAction.CallbackContext context)
    {
        if (context.started && grabObject != null)
        {
            Debug.Log("Come�ou a carregar o arremesso");

        }
        if (context.canceled && grabObject != null)
        {
            Debug.Log("Soltou o bot�o de arremesso");
            // Calcular o tempo que o bot�o foi pressionado
            float chargeTime = Mathf.Clamp((float)context.time, 0, tempoMaximoDeCarga);
            float chargePercent = chargeTime / tempoMaximoDeCarga;
            // Calcular as for�as com base no tempo de carga
            float horizontalForce = Mathf.Lerp(horizontalForceMin, horizontalForceMax, chargePercent);
            float verticalForce = Mathf.Lerp(verticallForceMin, verticallForceMax, chargePercent);
            // Dire��o do arremesso baseada na dire��o do movimento do jogador
            Vector3 throwDirection = direction.directionVector;
            Vector3 throwForce = throwDirection * horizontalForce + Vector3.up * verticalForce;
            // Aplicar a for�a ao objeto
            grabObject.transform.SetParent(null);
            grabObjectRb.isKinematic = false;
            grabObjectRb.AddForce(throwForce, ForceMode.Impulse);
            // Resetar vari�veis
            grabObjectRb = null;
            grabObject = null;

        }
    }


    private void OnDrawGizmosSelected()
    {
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(player.position, distanceGrab);

    }
}

