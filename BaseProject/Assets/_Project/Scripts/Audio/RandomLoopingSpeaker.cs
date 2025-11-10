using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class RandomLoopingSpeaker : MonoBehaviour
{
    [Header("Configuração de Fala")]
    [Tooltip("Array de clipes de áudio (um será escolhido aleatoriamente).")]
    [SerializeField] private AudioClip[] audioClips;

    [Tooltip("Volume da fala.")]
    [Range(0f, 1f)]
    [SerializeField] private float volume = 1f;

    [Header("Configuração de Fade Out")]
    [Tooltip("Duração (em segundos) do fade out ao parar de falar.")]
    [SerializeField] private float fadeOutDuration = 0.5f;

    private AudioSource audioSource;
    private Coroutine speakRoutine = null; // Para guardar a referência da Corrotina
    private Coroutine fadeRoutine = null;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.loop = false; // CRÍTICO: Não pode ser loop!
        audioSource.volume = volume;
    }

    // --- MÉTODOS PÚBLICOS ---

    // Chame este método para COMEÇAR a falar
    public void StartSpeaking()
    {
        if (fadeRoutine != null) // <-- NOVO
        {
            StopCoroutine(fadeRoutine); // <-- NOVO
            fadeRoutine = null; // <-- NOVO
        }

        // Se já estiver falando, pare antes de começar de novo
        StopSpeaking();

        audioSource.volume = volume;

        if (audioClips != null && audioClips.Length > 0)
        {
            // Inicia a corrotina e salva a referência
            speakRoutine = StartCoroutine(SpeakRoutine());
        }
    }

    // Chame este método para PARAR de falar
    public void StopSpeaking()
    {
        if (speakRoutine != null) 
        {
            StopCoroutine(speakRoutine);
            speakRoutine = null;
        }

        // 2. Se estiver tocando E não estiver já fazendo fade...
        if (audioSource.isPlaying && fadeRoutine == null) 
        {
            // ...inicie a rotina de fade out!
            fadeRoutine = StartCoroutine(FadeOutRoutine());
        }
    }

    // --- A LÓGICA PRINCIPAL ---

    private IEnumerator SpeakRoutine()
    {
        // Este loop roda "para sempre", até que StopSpeaking() seja chamado
        while (true)
        {
            // 1. Escolhe um clipe aleatório da lista
            int randIndex = Random.Range(0, audioClips.Length);

            audioSource.volume = volume;
            // 2. Configura e toca
            audioSource.clip = audioClips[randIndex];
            audioSource.volume = volume;
            audioSource.Play();

            // 3. O PONTO MÁGICO:
            // "Enquanto o som estiver tocando..."
            while (audioSource.isPlaying)
            {
                // "...espere até o próximo frame."
                yield return null;
            }

            // 4. O som terminou. O loop 'while(true)' vai repetir,
            // escolhendo outro som aleatório.
        }
    }
    private IEnumerator FadeOutRoutine()
    {
        float startVolume = audioSource.volume; // Pega o volume atual

        // Loop enquanto o volume for maior que zero
        while (audioSource.volume > 0)
        {
            // Diminui o volume gradualmente
            audioSource.volume -= startVolume * Time.deltaTime / fadeOutDuration;

            yield return null; // Espera o próximo frame
        }

        // Garante que parou tudo
        audioSource.Stop();
        audioSource.volume = volume; // Reseta o volume para a próxima vez
        fadeRoutine = null; // Libera a trava de fade
    }
}