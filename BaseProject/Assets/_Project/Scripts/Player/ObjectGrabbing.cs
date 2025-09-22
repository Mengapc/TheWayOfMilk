using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;


public class ObjectGrabbing : MonoBehaviour
{
    [Header("Configurações de Pegar Objeto")]
    [Tooltip("O ponto na 'mão' do personagem onde o objeto ficará preso.")]
    [SerializeField] private Transform pontoDaMao;
    [Tooltip("O raio da esfera para detectar objetos que podem ser pegos.")]
    [SerializeField] private float raioParaPegar = 1.5f;
    [Tooltip("A camada (Layer) dos objetos que podem ser pegos.")]
    [SerializeField] private LayerMask camadaDosObjetosPegaveis;

    [Header("Configurações de Arremesso")]
    [Tooltip("A força horizontal MÍNIMA do arremesso (distância).")]
    [SerializeField] private float forcaHorizontalMinima = 7f;
    [Tooltip("A força horizontal MÁXIMA do arremesso (distância).")]
    [SerializeField] private float forcaHorizontalMaxima = 25f;
    [Tooltip("A força vertical MÍNIMA do arremesso (altura).")]
    [SerializeField] private float forcaVerticalMinima = 3f;
    [Tooltip("A força vertical MÁXIMA do arremesso (altura).")]
    [SerializeField] private float forcaVerticalMaxima = 8f;
    [Tooltip("O tempo em segundos segurando o botão para atingir a força máxima.")]
    [SerializeField] private float tempoMaximoDeCarga = 2f;

    [Header("Referências Externas")]
    [Tooltip("Referência para o script de movimento do jogador. Essencial para a nova lógica de direção.")]
    [SerializeField] private Movement movementScript;

    private GameObject objetoSegurado = null;
    private Rigidbody objetoSeguradoRb = null;


    // Variáveis para controlar o carregamento do arremesso
    public bool estaCarregandoArremesso = false;
    private float tempoInicioCarga;

    private void Awake()
    {


        // Se o script de movimento não for arrastado no Inspector, tenta pegá-lo no mesmo GameObject
        if (movementScript == null)
        {
            movementScript = GetComponent<Movement>();
            if (movementScript == null)
            {
                Debug.LogError("O script 'Movement' não foi encontrado ou atribuído! Ele é necessário para a lógica de arremesso.");
            }
        }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (movementScript == null) return;

        if (objetoSegurado == null)
        {
            if (context.started)
            {
                TentarPegarObjeto();
            }
            return;
        }

        if (context.started)
        {
            estaCarregandoArremesso = true;
            tempoInicioCarga = Time.time;


            if (movementScript.player.transform.rotation.y > 0)
            {
                movementScript.player.rotation = Quaternion.Euler(0, 90, 0);
                Debug.Log("Carregando arremesso para a ESQUERDA (baseado na rotação)");
            }
            else
            {
                movementScript.player.rotation = Quaternion.Euler(0, -90, 0);
                Debug.Log("Carregando arremesso para a DIREITA (baseado na rotação)");
            }
        }
        else if (context.canceled && estaCarregandoArremesso)
        {
            ArremessarObjeto();
            estaCarregandoArremesso = false;

        }
    }

    private void TentarPegarObjeto()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position + transform.forward, raioParaPegar, camadaDosObjetosPegaveis);
        if (colliders.Length > 0)
        {
            Transform objetoMaisProximo = colliders.OrderBy(t => Vector3.Distance(transform.position, t.transform.position)).First().transform;
            objetoSegurado = objetoMaisProximo.gameObject;
            objetoSeguradoRb = objetoSegurado.GetComponent<Rigidbody>();

            if (objetoSeguradoRb != null)
            {
                objetoSeguradoRb.isKinematic = true;
                objetoSegurado.transform.SetParent(pontoDaMao);
                objetoSegurado.transform.localPosition = Vector3.zero;
                objetoSegurado.transform.localRotation = Quaternion.identity;
            }
        }
    }

    private void ArremessarObjeto()
    {
        if (objetoSeguradoRb == null) return;

        float duracaoCarga = Time.time - tempoInicioCarga;
        float porcentagemCarga = Mathf.Clamp01(duracaoCarga / tempoMaximoDeCarga);

        float forcaHorizontalAtual = Mathf.Lerp(forcaHorizontalMinima, forcaHorizontalMaxima, porcentagemCarga);
        float forcaVerticalAtual = Mathf.Lerp(forcaVerticalMinima, forcaVerticalMaxima, porcentagemCarga);

        objetoSegurado.transform.SetParent(null);
        objetoSeguradoRb.isKinematic = false;

        // --- LINHA CORRIGIDA ---
        // Usamos .forward para obter a direção "para frente" do jogador
        Vector3 vetorFinalArremesso = (movementScript.player.forward * forcaHorizontalAtual) + (Vector3.up * forcaVerticalAtual);
        objetoSeguradoRb.AddForce(vetorFinalArremesso, ForceMode.VelocityChange);

        objetoSegurado = null;
        objetoSeguradoRb = null;
    }

}

