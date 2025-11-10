using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement; // 1. Necessário para carregar cenas
using System.Collections;


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

    [Header("Áudio de Digitação")] 
    [Tooltip("Array de sons de 'blip' que tocarão aleatoriamente.")] 
    [SerializeField]
    private AudioClip[] typingSoundClips;

    [Header("Áudio de Fala")]
    [SerializeField]
    private RandomLoopingSpeaker speaker;

    [Tooltip("O intervalo (em segundos) entre cada 'blip' de som.")] 
    [SerializeField]
    private float typingSoundInterval = 0.05f; 

    [Tooltip("Volume do som de digitação.")] 
    [Range(0f, 1f)] 
    [SerializeField]
    private float typingVolume = 0.5f; 

    // Referência para a Corrotina que está tocando o som
    private Coroutine typingSoundRoutine = null; 

    private AudioSource audioSource; 

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
        if (speaker == null)
        {
            speaker = GetComponent<RandomLoopingSpeaker>();
        }

        audioSource = GetComponent<AudioSource>(); 
        audioSource.playOnAwake = false; 
        audioSource.loop = true; 
        audioSource.volume = typingVolume;

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
        StopTypingSound();
    }

    private void OnTextCompleted()
    {
        if (speaker != null)
        {
            speaker.StopSpeaking();
        }

        StopTypingSound();

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
        if (speaker != null)
        {
            speaker.StopSpeaking();
        }

        StopTypingSound();

        _canAdvance = false;

        if (nextPrompt != null)
        {
            nextPrompt.SetActive(false);
        }

        // Verifica se ainda temos textos na lista
        if (index < textToDisplay.Count)
        {
            textBox.text = textToDisplay[index];

            StartTypingSound();
            
            if (speaker != null)
            {
                speaker.StartSpeaking(); // <-- AQUI
            }
        }
        // 4. MUDANÇA PRINCIPAL: Se acabaram os textos...
        else
        {
            Debug.Log("Fim do diálogo. Carregando próxima cena...");
            
            if (speaker != null)
            {
                speaker.StopSpeaking(); // Garante que pare ao carregar a cena
            }
            
            StopTypingSound();
            LoadNextScene(); // ...chama a função de carregar a cena.
        }
    }

    private void StartTypingSound()
    {
        // Verifica se temos sons e se o manager existe
        if (SoundFXManager.instance != null && typingSoundClips != null && typingSoundClips.Length > 0)
        {
            // Inicia a corrotina e salva a referência
            typingSoundRoutine = StartCoroutine(PlayTypingBlipsRoutine());
        }
    }

    private void StopTypingSound()
    {
        // Se a corrotina estiver rodando...
        if (typingSoundRoutine != null)
        {
            // ...pare ela.
            StopCoroutine(typingSoundRoutine);
            typingSoundRoutine = null;
        }
    }

    private IEnumerator PlayTypingBlipsRoutine()
    {
        // Loop infinito (será parado pelo StopTypingSound)
        while (true)
        {
            // Toca um som aleatório da lista usando seu manager
            SoundFXManager.instance.PlayRandomSoundFXClip(
                typingSoundClips,
                transform, // O som sairá do objeto da UI
                typingVolume
            );

            // Espera o intervalo definido antes de tocar o próximo "blip"
            yield return new WaitForSeconds(typingSoundInterval);
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