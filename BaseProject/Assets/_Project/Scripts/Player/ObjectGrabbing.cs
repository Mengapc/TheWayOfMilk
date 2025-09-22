using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;


public class ObjectGrabbing : MonoBehaviour
{
    [Header("Configura��es de Pegar Objeto")]
    [Tooltip("O ponto na 'm�o' do personagem onde o objeto ficar� preso.")]
    [SerializeField] private Transform pontoDaMao;
    [Tooltip("O raio da esfera para detectar objetos que podem ser pegos.")]
    [SerializeField] private float raioParaPegar = 1.5f;
    [Tooltip("A camada (Layer) dos objetos que podem ser pegos.")]
    [SerializeField] private LayerMask camadaDosObjetosPegaveis;

    [Header("Configura��es de Arremesso")]
    [Tooltip("A for�a horizontal M�NIMA do arremesso (dist�ncia).")]
    [SerializeField] private float forcaHorizontalMinima = 7f;
    [Tooltip("A for�a horizontal M�XIMA do arremesso (dist�ncia).")]
    [SerializeField] private float forcaHorizontalMaxima = 25f;
    [Tooltip("A for�a vertical M�NIMA do arremesso (altura).")]
    [SerializeField] private float forcaVerticalMinima = 3f;
    [Tooltip("A for�a vertical M�XIMA do arremesso (altura).")]
    [SerializeField] private float forcaVerticalMaxima = 8f;
    [Tooltip("O tempo em segundos segurando o bot�o para atingir a for�a m�xima.")]
    [SerializeField] private float tempoMaximoDeCarga = 2f;

    [Header("Configura��es da Trajet�ria")]
    [Tooltip("O n�mero de pontos que a linha da trajet�ria ter�.")]
    [SerializeField] private int pontosDaTrajetoria = 30;
    [Tooltip("O intervalo de tempo simulado entre cada ponto da linha.")]
    [SerializeField] private float intervaloDeTempoDosPontos = 0.05f;

    [Header("Refer�ncias Externas")]
    [Tooltip("Refer�ncia para o script de movimento do jogador. Essencial para a nova l�gica de dire��o.")]
    [SerializeField] private Movement movementScript;

    private GameObject objetoSegurado = null;
    private Rigidbody objetoSeguradoRb = null;


    // Vari�veis para controlar o carregamento do arremesso
    public bool estaCarregandoArremesso = false;
    private float tempoInicioCarga;

    private void Awake()
    {


        // Se o script de movimento n�o for arrastado no Inspector, tenta peg�-lo no mesmo GameObject
        if (movementScript == null)
        {
            movementScript = GetComponent<Movement>();
            if (movementScript == null)
            {
                Debug.LogError("O script 'Movement' n�o foi encontrado ou atribu�do! Ele � necess�rio para a l�gica de arremesso.");
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
                Debug.Log("Carregando arremesso para a ESQUERDA (baseado na rota��o)");
            }
            else
            {
                movementScript.player.rotation = Quaternion.Euler(0, -90, 0);
                Debug.Log("Carregando arremesso para a DIREITA (baseado na rota��o)");
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
        // Usamos .forward para obter a dire��o "para frente" do jogador
        Vector3 vetorFinalArremesso = (movementScript.player.forward * forcaHorizontalAtual) + (Vector3.up * forcaVerticalAtual);
        objetoSeguradoRb.AddForce(vetorFinalArremesso, ForceMode.VelocityChange);

        objetoSegurado = null;
        objetoSeguradoRb = null;
    }

}

