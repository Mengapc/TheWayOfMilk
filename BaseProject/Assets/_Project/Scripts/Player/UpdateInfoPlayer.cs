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

    [Tooltip("Informação ao chegar perto do leite.")]
    [TextArea(3, 10)]
    [SerializeField] private string infoChegarPerto;
    [Tooltip("Informação ao chegar perto do leite.")]
    [TextArea(3, 10)]
    [SerializeField] private string infoJogar;
    [Tooltip("Informação ao chegar perto do elevador.")]
    [TextArea(3, 10)]
    [SerializeField] private string infoElevador;


    [Tooltip("Referência ao script de pegar objetos (no mesmo GameObject).")]
    [SerializeField] private ObjectGrabbing objectGrabbing;



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


    private void Update()
    {
        UpdateUI();
    }
    #endregion

    #region Funções de UI

    private void UpdateUI()
    {
        // Se não houver referência, desliga tudo e para.
        if (objectGrabbing == null)
        {
            if (infoTextPanel != null) infoTextPanel.SetActive(false);
            if (chargeSliderPanel != null) chargeSliderPanel.SetActive(false);
            return;
        }

        // --- LÓGICA DE UI HIERÁRQUICA ---

        // 1. PRIORIDADE MÁXIMA: Mostra o SLIDER DE ARREMESSO
        // Se estiver carregando
        if (objectGrabbing.IsCharging)
        {
            if (infoTextPanel != null) infoTextPanel.SetActive(false); // Esconde texto
            if (chargeSliderPanel != null)
            {
                chargeSliderPanel.SetActive(true); // Mostra slider

                if (chargeSlider != null)
                {
                    float chargePercent = objectGrabbing.CurrentChargeTime / objectGrabbing.MaxChargeTime;
                    chargeSlider.value = chargePercent;
                }
            }
        }
        // 2. Se NÃO estiver carregando, mostra os TEXTOS DE INFORMAÇÃO
        else
        {
            // Garante que o slider esteja desligado
            if (chargeSliderPanel != null)
            {
                chargeSliderPanel.SetActive(false);
                if (chargeSlider != null) chargeSlider.value = 0; // Reseta o valor
            }

            // Se não houver painel de texto, não faz mais nada
            if (infoTextPanel == null) return;

            // 2a. Texto do Elevador (tem prioridade sobre os itens)
            if (inElevator)
            {
                infoTextPanel.SetActive(true);
                textInfo.text = infoElevador;
            }
            // 2b. Texto de Jogar (se estiver segurando um item)
            else if (objectGrabbing.GrabbingObject)
            {
                infoTextPanel.SetActive(true);
                textInfo.text = infoJogar; // <--- USA O 'infoJogar'
            }

            // 2c. Texto de Pegar (se estiver perto O BASTANTE para pegar)
            // Usa a variável que calcula a distância real
            else if (objectGrabbing.IsNearGrabbableDistance)
            {
                infoTextPanel.SetActive(true);
                textInfo.text = infoPegar; // <--- USA O 'infoPegar'
            }
            // 2d. Texto de Chegar Perto (se estiver na área, mas longe)
            // Usa a variável do trigger
            else if (objectGrabbing.IsNearGrabbable)
            {
                infoTextPanel.SetActive(true);
                textInfo.text = infoChegarPerto; // <--- USA O 'infoChegarPerto'
            }
            // 3. Se nenhuma condição for atendida, esconde tudo.

            else if (objectGrabbing.GrabbingObject)
            {
                infoTextPanel.SetActive(true);
                textInfo.text = infoJogar;
            }
            else
            {
                infoTextPanel.SetActive(false);
                textInfo.text = "";
            }
        }
    }

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