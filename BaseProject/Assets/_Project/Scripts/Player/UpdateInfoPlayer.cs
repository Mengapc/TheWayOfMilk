using TMPro;
using UnityEngine;

// Este é o script que deve estar no mesmo GameObject do Player,
// junto com o ObjectGrabbing.cs e o SphereCollider.
public class UpdateInfoPlayer_UI : MonoBehaviour
{
    #region Variaveis
    [Header("Configurações de UI")]
    [Tooltip("Canvas de UI para exibir informações ao jogador.")]
    [SerializeField] private Canvas uiCanvas;
    [Tooltip("Texto de informação para o jogador.")]
    [SerializeField] private TextMeshProUGUI textInfo;
    [Tooltip("Informação ao chegar perto do leite.")]
    [TextArea(3, 10)]
    [SerializeField] private string infoPegar;
    [Tooltip("Informação ao estar com o leite na mão.")]
    [TextArea(3, 10)]
    [SerializeField] private string infoJogar;

    [Tooltip("Referência ao script de pegar objetos (no mesmo GameObject).")]
    [SerializeField] private ObjectGrabbing objectGrabbing;

    [Tooltip("Informação ao chegar perto do elevador.")]
    [TextArea(3, 10)]
    [SerializeField] private string infoElevador;

    // Variável para controlar se está na zona do elevador
    private bool inElevator = false;
    #endregion

    #region Metodos da Unity

    // Pega a referência automaticamente
    private void Awake()
    {
        if (objectGrabbing == null)
        {
            objectGrabbing = GetComponent<ObjectGrabbing>();
        }
    }

    // Chamado quando o script é carregado pela primeira vez
    private void Start()
    {
        uiCanvas.enabled = false;
    }

    // Chamado a cada frame
    private void Update()
    {
        // Se o objectGrabbing não estiver atribuído, não faz nada
        if (objectGrabbing == null)
        {
            uiCanvas.enabled = false;
            return;
        }

        // 1. Se estiver carregando um arremesso, esconde a UI
        if (objectGrabbing.IsCharging)
        {
            uiCanvas.enabled = false;
            textInfo.text = "";
        }
        // 2. Se estiver na zona do elevador
        else if (inElevator)
        {
            uiCanvas.enabled = true;
            textInfo.text = infoElevador;
        }
        // 3. Se NÃO estiver segurando E ESTIVER PERTO de um item
        //    (Agora pergunta ao ObjectGrabbing.IsNearGrabbable)
        else if (!objectGrabbing.GrabbingObject && objectGrabbing.IsNearGrabbable)
        {
            // Mostra a informação de como pegar um objeto
            uiCanvas.enabled = true;
            textInfo.text = infoPegar;
        }
        // 4. Se estiver segurando um objeto ou longe de tudo
        else
        {
            uiCanvas.enabled = false;
            textInfo.text = "";
        }
    }
    #endregion

    #region Detecção de Triggers (Gatilhos)

    // Esta função é chamada quando o CharacterController entra em um Collider marcado como "Is Trigger".
    private void OnTriggerEnter(Collider other)
    {
        // Verifica se o objeto que entramos tem a tag "Elevator"
        if (other.gameObject.CompareTag("Elevator"))
        {
            inElevator = true;
        }

        // Não precisamos mais checar a "Ball" aqui, 
        // o ObjectGrabbing.cs já faz isso no Update().
    }

    // Chamado quando o CharacterController sai de um Trigger.
    private void OnTriggerExit(Collider other)
    {
        // Verifica se o objeto que saímos tem a tag "Elevator"
        if (other.gameObject.CompareTag("Elevator"))
        {
            inElevator = false;
        }
    }
    #endregion
}
