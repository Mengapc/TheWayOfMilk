using TMPro;
using UnityEngine;

public class UpdateInfoPlayer : MonoBehaviour
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
    [Tooltip("Referência ao script de pegar objetos.")]
    [SerializeField] private ObjectGrabbing objectGrabbing;
    [Tooltip("Informação ao chegar perto do elevador.")]
    [TextArea(3, 10)]
    [SerializeField] private string infoElevador;
    [SerializeField] private bool inElevator;
    #endregion

    #region Metodos da Unity
    // Chamado quando o script é carregado pela primeira vez
    private void Start()
    {
        uiCanvas.enabled = false;
    }

    // Chamado a cada frame
    private void Update()
    {
        // Verifica se o jogador está perto do leite e não está segurando um objeto
        // Esta lógica assume que 'objectGrabbing' é atribuído no Inspector
        if (objectGrabbing != null && !objectGrabbing.GrabbingObject)
        {
            // NOTA: Esta lógica para 'infoPegar' vai sobrepor a do elevador
            // se ambas as condições forem verdadeiras.
            uiCanvas.enabled = true;
            textInfo.text = infoPegar;
        }
        // Verifica se o jogador está perto do elevador (variável 'elevator' não é nula)
        else if (inElevator)
        {
            uiCanvas.enabled = true;
            textInfo.text = infoElevador;
        }
        // Se nenhuma condição for atendida, esconde o canvas
        else
        {
            uiCanvas.enabled = false;
            textInfo.text = "";
        }
    }
    #endregion

    #region Detecção de Triggers (Gatilhos)

    // CORREÇÃO: Usamos OnTriggerEnter porque o Player usa um CharacterController.
    // Esta função é chamada quando o CharacterController entra em um Collider marcado como "Is Trigger".
    private void OnTriggerEnter(Collider other)
    {
        // Verifica se o objeto que entramos tem a tag "Elevator"
        if (other.gameObject.CompareTag("Elevator"))
        {
            inElevator = true;
            // Armazena a referência do script Elevator
        }
    }

    // CORREÇÃO: Usamos OnTriggerExit.
    // Chamado quando o CharacterController sai de um Trigger.
    private void OnTriggerExit(Collider other)
    {
        // Verifica se o objeto que saímos tem a tag "Elevator"
        if (other.gameObject.CompareTag("Elevator"))
        {
            inElevator = false;
            // Limpa a referência, pois não estamos mais no elevador
       
        }
    }
    #endregion
}

