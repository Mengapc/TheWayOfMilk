using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;
using Unity.VisualScripting;


public class ObjectGrabbing : MonoBehaviour
{
    [Header("Configurações de Pegar Objeto")]
    [Tooltip("Tranforme do personagem.")]
    [SerializeField] private Transform player;
    [Tooltip("O ponto na 'mão' do personagem onde o objeto ficará preso.")]
    [SerializeField] private Transform handPoint;
    [Tooltip("O raio da esfera para detectar objetos que podem ser pegos.")]
    [SerializeField] private float distanceGrab = 1.5f;
    [Tooltip("A camada (Layer) dos objetos que podem ser pegos.")]
    [SerializeField] private LayerMask grabbingLayer;
    [Tooltip("Segurando o objeto.")]
    [SerializeField] private bool grabbingObject;

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
    [Tooltip("Camera do jogo")]
    [SerializeField] private Camera cam;

    private GameObject grabObject = null;
    private Rigidbody grabObjectRb = null;


    public void InteractionGrabbing(InputAction.CallbackContext context)
    {
        if (context.started && grabObject == null)
        {
            Debug.Log("Rio lançcado");
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
                //seta como parente da mão
                grabObject = objectGrab;
                grabObject.transform.SetParent(handPoint);
                grabObject.transform.position = handPoint.transform.position;
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
}

