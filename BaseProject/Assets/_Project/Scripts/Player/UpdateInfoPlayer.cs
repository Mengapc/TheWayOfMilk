using TMPro;
using UnityEngine;
using UnityEngine.UI; // Necessário para o Slider

public class UpdateInfoPlayer_UI : MonoBehaviour
{
    #region Variaveis
    [Header("Configurações de UI")]
    [Tooltip("Canvas de UI para exibir informações ao jogador (Ex: o painel 'Informacoes').")]
    [SerializeField] private GameObject infoTextPanel; // MUDADO: de Canvas para GameObject
    [Tooltip("Texto de informação para o jogador.")]
    [SerializeField] private TextMeshProUGUI textInfo;

    [Header("UI do Slider de Arremesso")]
    [Tooltip("O GameObject 'Panel' que contém o slider.")]
    [SerializeField] private GameObject chargeSliderPanel; // ADICIONADO: O painel pai
    [Tooltip("O componente Slider que está dentro do 'Panel'.")]
    [SerializeField] private Slider chargeSlider; // Referência para o Slider

    [Header("Textos de Informação")]
    [Tooltip("Informação ao chegar perto do leite.")]
    [TextArea(3, 10)]
    [SerializeField] private string infoPegar;

    [Tooltip("Referência ao script de pegar objetos (no mesmo GameObject).")]
    [SerializeField] private ObjectGrabbing objectGrabbing;

    [Tooltip("Informação ao chegar perto do elevador.")]
    [TextArea(3, 10)]
    [SerializeField] private string infoElevador;

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

    // Garante que a UI comece desligada
    private void Start()
    {
        if (infoTextPanel != null)
        {
            infoTextPanel.SetActive(false);
        }

        if (chargeSliderPanel != null)
        {
            chargeSliderPanel.SetActive(false); // Esconde o painel do slider
        }

        if (chargeSlider != null)
        {
            chargeSlider.value = 0;
        }
    }

    // Atualiza a UI a cada frame
    private void Update()
    {
        if (objectGrabbing == null)
        {
            if (infoTextPanel != null) infoTextPanel.SetActive(false);
            if (chargeSliderPanel != null) chargeSliderPanel.SetActive(false);
            return;
        }

        // 1. Se estiver carregando um arremesso, mostra o slider
        if (objectGrabbing.IsCharging)
        {
            if (infoTextPanel != null) infoTextPanel.SetActive(false); // Esconde o painel de texto

            if (chargeSliderPanel != null)
            {
                chargeSliderPanel.SetActive(true); // Ativa o PAINEL do slider

                // Calcula a percentagem (0.0 a 1.0)
                float chargePercent = objectGrabbing.CurrentChargeTime / objectGrabbing.MaxChargeTime;

                // Define o valor do slider
                if (chargeSlider != null)
                {
                    chargeSlider.value = chargePercent;
                }
            }
        }
        // 2. Se NÃO estiver carregando, esconde o slider e mostra os textos
        else
        {
            if (chargeSliderPanel != null)
            {
                chargeSliderPanel.SetActive(false); // Esconde o PAINEL do slider
                if (chargeSlider != null) chargeSlider.value = 0; // Reseta o valor
            }

            // --- Lógica de Texto ---
            if (infoTextPanel == null) return; // Segurança

            if (inElevator)
            {
                infoTextPanel.SetActive(true);
                textInfo.text = infoElevador;
            }
            else if (!objectGrabbing.GrabbingObject && objectGrabbing.IsNearGrabbable)
            {
                infoTextPanel.SetActive(true);
                textInfo.text = infoPegar;
            }
            else
            {
                infoTextPanel.SetActive(false);
                textInfo.text = "";
            }
        }
    }
    #endregion

    #region Detecção de Triggers (Gatilhos)

    // Chamado quando entra num trigger
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Elevator"))
        {
            inElevator = true;
        }
    }

    // Chamado quando sai de um trigger
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Elevator"))
        {
            inElevator = false;
        }
    }
    #endregion
}