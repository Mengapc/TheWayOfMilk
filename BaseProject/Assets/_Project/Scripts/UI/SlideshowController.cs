using System.Collections;
using UnityEngine;

public class SlideshowController : MonoBehaviour
{
    [Header("Configuração dos Slides")]
    [Tooltip("Coloque os COMPONENTES 'Canvas Group' dos seus slides aqui, EM ORDEM.")]
    [SerializeField] private CanvasGroup[] slides;

    [Header("Configuração de Tempo")]
    [Tooltip("Tempo (em segundos) para o FADE IN.")]
    [SerializeField] private float fadeInTime = 1.0f;

    [Tooltip("Tempo (em segundos) que o slide fica visível (APÓS o fade in).")]
    [SerializeField] private float holdTime = 3.0f;

    [Tooltip("Tempo (em segundos) para o FADE OUT. (Também usado para o fade da música)")] // <-- MODIFICADO
    [SerializeField] private float fadeOutTime = 1.0f;

    [Header("Áudio de Fundo")] // <-- NOVO
    [Tooltip("AudioSource que tocará a música/som ambiente durante os slides.")] // <-- NOVO
    [SerializeField] private AudioSource ambientAudioSource; // <-- NOVO

    [Header("Ação Final")]
    [Tooltip("O GameObject que será ativado quando o último slide terminar (ex: o painel de diálogo).")]
    [SerializeField] private GameObject objectToActivateOnEnd;

    private void Start()
    {
        // Toca o áudio de fundo (deve estar configurado para Loop)
        if (ambientAudioSource != null) // <-- NOVO
        {
            ambientAudioSource.Play(); // <-- NOVO
        }

        StartCoroutine(PlaySlideshowRoutine());
    }

    private IEnumerator PlaySlideshowRoutine()
    {
        // 1. Garante que o objeto final esteja desativado
        if (objectToActivateOnEnd != null)
        {
            objectToActivateOnEnd.SetActive(false);
        }

        // 2. Garante que todos os slides comecem INVISÍVEIS (mas ATIVOS)
        foreach (CanvasGroup slide in slides)
        {
            slide.alpha = 0;
            // ... (resto da configuração do slide)
        }

        // 3. Loop principal: MUDAMOS PARA 'for' PARA SABER QUAL É O ÚLTIMO
        for (int i = 0; i < slides.Length; i++) // <-- MODIFICADO
        {
            CanvasGroup currentSlide = slides[i]; // <-- NOVO

            // --- FADE IN ---
            yield return StartCoroutine(FadeCanvasGroup(currentSlide, 0f, 1f, fadeInTime));

            // ... (interactable = true, etc.) ...

            // --- HOLD (ESPERA) ---
            yield return new WaitForSeconds(holdTime);

            // --- FADE OUT ---
            currentSlide.interactable = false;
            currentSlide.blocksRaycasts = false;

            // --- LÓGICA DE FADE OUT (SLIDE E MÚSICA) ---

            // Se for o ÚLTIMO slide...
            if (i == slides.Length - 1) // <-- NOVO
            {
                // ...começa o fade da música (sem esperar)
                if (ambientAudioSource != null) // <-- NOVO
                {
                    StartCoroutine(FadeAudioSource(ambientAudioSource, ambientAudioSource.volume, 0f, fadeOutTime)); // <-- NOVO
                }
            }

            // Espera o fade out do slide (que agora acontece em paralelo com o da música)
            yield return StartCoroutine(FadeCanvasGroup(currentSlide, 1f, 0f, fadeOutTime)); // <-- MODIFICADO
        }

        // 4. Ativa o objeto final (diálogo)
        if (objectToActivateOnEnd != null)
        {
            Debug.Log("Slideshow terminado. Ativando o diálogo.");
            objectToActivateOnEnd.SetActive(true);
        }
        // ... (resto)
    }

    // Corrotina helper que anima o 'alpha' de um CanvasGroup
    private IEnumerator FadeCanvasGroup(CanvasGroup cg, float startAlpha, float endAlpha, float duration)
    {
        // ... (Nenhuma mudança aqui) ...
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            cg.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        cg.alpha = endAlpha;
    }

    // --- NOVA CORROTINA HELPER PARA FADE DE ÁUDIO --- // <-- NOVO
    private IEnumerator FadeAudioSource(AudioSource audioSource, float startVolume, float endVolume, float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            // Calcula o novo volume usando Lerp (interpolação)
            audioSource.volume = Mathf.Lerp(startVolume, endVolume, elapsedTime / duration);

            // Incrementa o tempo e espera o próximo frame
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Garante que o valor final seja exatamente o 'endVolume'
        audioSource.volume = endVolume;

        // Se o fade out terminou, para o áudio
        if (endVolume == 0f)
        {
            audioSource.Stop();
        }
    }
}