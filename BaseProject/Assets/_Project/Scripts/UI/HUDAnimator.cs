using UnityEngine;
using DG.Tweening; // Não se esqueça de importar o DOTween!

/// <summary>
/// Controla animações de elementos da HUD usando DOTween.
/// Um objeto pulsa (escala) e outro pisca (fade de opacidade) sob demanda.
/// </summary>
public class HUDAnimator : MonoBehaviour
{
    [Header("Objeto Pulsante")]
    [Tooltip("O RectTransform do objeto que deve pulsar (mudar de escala).")]
    public RectTransform objetoPulsante;

    [Tooltip("A escala máxima que o objeto atingirá durante o pulso.")]
    public float escalaPulso = 1.1f;

    [Tooltip("A duração de cada pulso (o tempo de ir de 1.0 para 1.1, por exemplo).")]
    public float duracaoPulso = 0.7f;

    [Header("Texto com Fade (Feedback)")]
    [Tooltip("O CanvasGroup do objeto de texto que deve piscar (mudar opacidade). Adicione um CanvasGroup a este objeto!")]
    public CanvasGroup textoComFade;

    [Tooltip("A duração de cada fade (o tempo de ir de 1.0 para 0.0).")]
    public float duracaoFade = 1.0f;

    // Variáveis para guardar a referência das nossas animações (tweens)
    private Tween pulsoTween;
    private Tween fadeTween;

    // Chamado automaticamente pela Unity quando o jogo começa
    void Start()
    {
        // 1. Animação de pulso ainda começa automaticamente
        if (objetoPulsante != null)
        {
            IniciarAnimacaoPulso();
        }

        // 2. Animação de fade NÃO começa mais aqui
        // Garante que o texto de feedback comece invisível
        if (textoComFade != null)
        {
            textoComFade.alpha = 0.0f;
        }
    }

    /// <summary>
    /// Configura e inicia a animação de pulso (escala) de forma infinita.
    /// </summary>
    private void IniciarAnimacaoPulso()
    {
        // DOScale: Anima a escala do objeto
        // escalaPulso: O valor da escala para o qual queremos animar
        // duracaoPulso: O tempo que a animação leva

        // Guardamos a animação na nossa variável
        pulsoTween = objetoPulsante.DOScale(escalaPulso, duracaoPulso)
            .SetEase(Ease.InOutSine)    // Define uma suavização na animação (começa e termina devagar)
            .SetLoops(-1, LoopType.Yoyo); // Define loops: -1 = infinito, vai e volta (1.0 -> 1.1 -> 1.0 -> 1.1 ...)
    }

    // --- MODIFICADO ---
    /// <summary>
    /// ATIVA a animação de fade (opacidade). 
    /// Pode ser chamado por outros scripts (ex: GameManager, Player).
    /// </summary>
    public void AtivarFeedbackFade()
    {
        if (textoComFade == null) return;

        // Mata (para) qualquer animação de fade anterior para evitar conflitos
        // ou sobreposição.
        if (fadeTween != null)
        {
            fadeTween.Kill();
        }

        textoComFade.alpha = 1.0f;

        // Inicia a nova animação de fade
        fadeTween = textoComFade.DOFade(0.0f, duracaoFade)
            .SetEase(Ease.InOutSine)      // Suavização
            .SetLoops(-1, LoopType.Yoyo);   // Loop infinito, vai e volta (1.0 -> 0.0 -> 1.0 -> 0.0 ...)
    }

    // --- ADICIONADO ---
    /// <summary>
    /// DESATIVA a animação de fade e esconde o texto.
    /// </summary>
    public void DesativarFeedbackFade()
    {
        if (textoComFade == null) return;

        // Mata (para) a animação
        if (fadeTween != null)
        {
            fadeTween.Kill();
        }

        // Esconde o texto
        textoComFade.alpha = 0.0f;
    }


    /// <summary>
    /// Chamado pela Unity quando este GameObject é destruído (ex: ao trocar de cena).
    /// </summary>
    void OnDestroy()
    {
        if (pulsoTween != null)
        {
            // O .Kill() para a animação imediatamente e a remove do DOTween.
            pulsoTween.Kill();
        }

        if (fadeTween != null)
        {
            fadeTween.Kill();
        }
    }
}