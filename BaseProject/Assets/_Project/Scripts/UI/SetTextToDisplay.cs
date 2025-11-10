using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement; // 1. Necessário para carregar cenas

#if UNITY_EDITOR
using UnityEditor; // 2. Necessário para o SceneAsset
#endif

public class SetTextToDisplay : MonoBehaviour
{
    [Header("Configuração das Falas")]
    [SerializeField]
    private List<string> textToDisplay;

    [Header("Referências")]
    [SerializeField]
    private TMP_Text textBox;

    [Header("Feedback Visual")]
    [Tooltip("O objeto (ex: ícone de 'Espaço' ou seta) que aparece quando o texto termina")]
    [SerializeField]
    private GameObject nextPrompt;

    // 3. SEU CÓDIGO PARA SELEÇÃO DE CENA
    [Header("Próxima Cena")]
    // ----- Scene selection -----
#if UNITY_EDITOR
    public SceneAsset sceneAsset;
#endif
    [SerializeField, HideInInspector] private string sceneName;

    private int _currentTextIndex = 0;
    private bool _canAdvance = false;
    public HUDAnimator hudanimator;

    private void Awake()
    {
        if (textBox == null)
        {
            Debug.LogError("O campo 'TextBox' não foi configurado no Inspector!");
            this.enabled = false;
            return;
        }

        if (nextPrompt != null)
        {
            nextPrompt.SetActive(false);
        }
    }

    private void Start()
    {
        if (textToDisplay.Count > 0)
        {
            ShowText(0);
        }
    }

    private void OnEnable()
    {
        TypeWritterEffect.CompleteTextRevealed += OnTextCompleted;
    }

    private void OnDisable()
    {
        TypeWritterEffect.CompleteTextRevealed -= OnTextCompleted;
    }

    private void OnTextCompleted()
    {
        _canAdvance = true;

        if (nextPrompt != null)
        {
            nextPrompt.SetActive(true);
            hudanimator.AtivarFeedbackFade();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _canAdvance)
        {
            HandleInteraction();
        }
    }

    private void HandleInteraction()
    {
        _currentTextIndex++;
        ShowText(_currentTextIndex);
    }

    private void ShowText(int index)
    {
        _canAdvance = false;

        if (nextPrompt != null)
        {
            nextPrompt.SetActive(false);
        }

        // Verifica se ainda temos textos na lista
        if (index < textToDisplay.Count)
        {
            textBox.text = textToDisplay[index];
        }
        // 4. MUDANÇA PRINCIPAL: Se acabaram os textos...
        else
        {
            Debug.Log("Fim do diálogo. Carregando próxima cena...");
            LoadNextScene(); // ...chama a função de carregar a cena.
        }
    }

    // 5. NOVA FUNÇÃO PARA CARREGAR A CENA
    private void LoadNextScene()
    {
        if (!string.IsNullOrEmpty(sceneName))
        {
            // Chama o nosso novo gerenciador
            SceneLoader.LoadScene(sceneName); // <-- LINHA NOVA
        }
        else
        {
            Debug.LogWarning("Fim do diálogo, mas nenhuma cena foi configurada.");
        }
    }

    // 6. FUNÇÃO ESSENCIAL PARA SEU CÓDIGO DE CENA FUNCIONAR
    // (Copia o nome do SceneAsset para o 'sceneName' no editor)
#if UNITY_EDITOR
    private void OnValidate()
    {
        if (sceneAsset != null)
        {
            sceneName = sceneAsset.name;
        }
    }
#endif
}