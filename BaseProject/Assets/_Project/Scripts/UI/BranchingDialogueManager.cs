using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

// --- ESTRUTURA DE DADOS (Nós e Escolhas) ---

[System.Serializable]
public class DialogueChoice
{
    [Tooltip("O texto que aparecerá no botão")]
    public string buttonText;

    [Tooltip("O ID (Index) do nó para onde essa escolha leva.")]
    public int targetNodeID;
}

[System.Serializable]
public class DialogueNode
{
    [TextArea(3, 5)]
    public string dialogueText;

    [Tooltip("Se vazio, usa Espaço para ir ao próximo nó. Se tiver itens, gera botões.")]
    public List<DialogueChoice> choices;
}

// --- O NOVO GERENCIADOR ---

public class BranchingDialogueManager : MonoBehaviour
{
    [Header("Referências UI")]
    [SerializeField] private TMP_Text textBox;
    [SerializeField] private GameObject nextPrompt; // Ícone de Espaço
    [SerializeField] private HUDAnimator hudanimator; // <-- SEU HUD ANIMATOR

    [Header("Configuração de Escolhas")]
    [SerializeField] private Transform optionsContainer; // Onde os botões nascem
    [SerializeField] private GameObject choiceButtonPrefab; // O modelo do botão

    [Header("Árvore de Diálogo")]
    [SerializeField] private List<DialogueNode> dialogueNodes;

    // --- SEÇÃO DE ÁUDIO DO SEU SCRIPT ANTIGO ---
    [Header("Áudio de Digitação")]
    [SerializeField] private AudioClip[] typingSoundClips;
    [SerializeField] private float typingSoundInterval = 0.05f;
    [Range(0f, 1f)]
    [SerializeField] private float typingVolume = 0.5f;

    [Header("Áudio de Fala (Boca)")]
    [SerializeField] private RandomLoopingSpeaker speaker;

    // Variáveis internas de áudio
    private Coroutine typingSoundRoutine = null;
    private AudioSource audioSource; // Mantido caso seu Speaker precise dele

    // --- SEÇÃO DE CENA ---
    [Header("Próxima Cena")]
#if UNITY_EDITOR
    public SceneAsset sceneAsset;
#endif
    [SerializeField, HideInInspector] private string sceneName;

    // Variáveis de controle
    private int _currentNodeIndex = 0;
    private bool _isWaitingForChoice = false;
    private bool _canAdvanceWithSpace = false;

    private void Awake()
    {
        // Configuração inicial (trazida do seu script)
        if (speaker == null) speaker = GetComponent<RandomLoopingSpeaker>();

        audioSource = GetComponent<AudioSource>();
        if (audioSource != null)
        {
            audioSource.playOnAwake = false;
            audioSource.loop = true;
            audioSource.volume = typingVolume;
        }

        if (nextPrompt != null) nextPrompt.SetActive(false);

        ClearChoices(); // Limpa botões visuais da UI se houver algum
    }

    private void Start()
    {
        // Inicia o diálogo se houver nós
        if (dialogueNodes.Count > 0)
        {
            ShowNode(0);
        }
    }

    private void OnEnable()
    {
        TypeWritterEffect.CompleteTextRevealed += OnTypewriterFinished;
    }

    private void OnDisable()
    {
        TypeWritterEffect.CompleteTextRevealed -= OnTypewriterFinished;
        StopTypingSound(); // Garante que o som pare
    }

    private void Update()
    {
        // Só permite espaço se NÃO for escolha E o texto já acabou
        if (Input.GetKeyDown(KeyCode.Space) && _canAdvanceWithSpace && !_isWaitingForChoice)
        {
            AdvanceLinear();
        }
    }

    // --- LÓGICA PRINCIPAL ---

    private void ShowNode(int index)
    {
        if (index < 0 || index >= dialogueNodes.Count)
        {
            EndDialogue();
            return;
        }

        _currentNodeIndex = index;
        DialogueNode node = dialogueNodes[index];

        // 1. Reseta estados visuais
        _canAdvanceWithSpace = false;
        _isWaitingForChoice = false;
        if (nextPrompt != null) nextPrompt.SetActive(false);

        // 2. Reseta Áudio (Para garantir que não encavale)
        StopTypingSound();
        if (speaker != null) speaker.StopSpeaking();

        // 3. Envia texto para o Typewriter
        textBox.text = node.dialogueText;

        // 4. INICIA OS EFEITOS DE ÁUDIO (Trazido do seu script)
        StartTypingSound();
        if (speaker != null) speaker.StartSpeaking();
    }

    // Chamado AUTOMATICAMENTE quando o typewriter termina
    private void OnTypewriterFinished()
    {
        // 1. PARA OS EFEITOS DE ÁUDIO
        if (speaker != null) speaker.StopSpeaking();
        StopTypingSound();

        DialogueNode node = dialogueNodes[_currentNodeIndex];

        // 2. Verifica se é Escolha ou Linear
        if (node.choices != null && node.choices.Count > 0)
        {
            // MODO ESCOLHA: Mostra botões
            DisplayChoices(node.choices);
            _isWaitingForChoice = true;
        }
        else
        {
            // MODO LINEAR: Libera espaço e mostra feedbacks
            _canAdvanceWithSpace = true;

            if (nextPrompt != null) nextPrompt.SetActive(true);

            // SEU FEEDBACK DO HUD
            if (hudanimator != null) hudanimator.AtivarFeedbackFade();
        }
    }

    // --- SISTEMA DE ESCOLHAS ---

    private void DisplayChoices(List<DialogueChoice> choices)
    {
        ClearChoices();

        foreach (DialogueChoice choice in choices)
        {
            GameObject newButtonObj = Instantiate(choiceButtonPrefab, optionsContainer);

            TMP_Text btnText = newButtonObj.GetComponentInChildren<TMP_Text>();
            if (btnText != null) btnText.text = choice.buttonText;

            Button btn = newButtonObj.GetComponent<Button>();
            int targetID = choice.targetNodeID;

            btn.onClick.AddListener(() => OnChoiceSelected(targetID));
        }
    }

    private void OnChoiceSelected(int targetNodeID)
    {
        ClearChoices();
        ShowNode(targetNodeID);
    }

    private void ClearChoices()
    {
        foreach (Transform child in optionsContainer)
        {
            Destroy(child.gameObject);
        }
    }

    // --- NAVEGAÇÃO ---

    private void AdvanceLinear()
    {
        // Avança para o próximo índice da lista
        ShowNode(_currentNodeIndex + 1);
    }

    private void EndDialogue()
    {
        Debug.Log("Fim do Diálogo. Carregando próxima cena...");

        // Garante que tudo pare
        StopTypingSound();
        if (speaker != null) speaker.StopSpeaking();

        if (!string.IsNullOrEmpty(sceneName))
        {
            SceneLoader.LoadScene(sceneName);
        }
        else
        {
            Debug.LogWarning("Nenhuma cena configurada.");
        }
    }

    // --- SEU SISTEMA DE ÁUDIO (Preservado) ---

    private void StartTypingSound()
    {
        if (SoundFXManager.instance != null && typingSoundClips != null && typingSoundClips.Length > 0)
        {
            typingSoundRoutine = StartCoroutine(PlayTypingBlipsRoutine());
        }
    }

    private void StopTypingSound()
    {
        if (typingSoundRoutine != null)
        {
            StopCoroutine(typingSoundRoutine);
            typingSoundRoutine = null;
        }
    }

    private IEnumerator PlayTypingBlipsRoutine()
    {
        while (true)
        {
            SoundFXManager.instance.PlayRandomSoundFXClip(
                typingSoundClips,
                transform,
                typingVolume
            );
            yield return new WaitForSeconds(typingSoundInterval);
        }
    }

    // --- VALIDAÇÃO DO EDITOR ---
#if UNITY_EDITOR
    private void OnValidate()
    {
        if (sceneAsset != null) sceneName = sceneAsset.name;
    }
#endif
}