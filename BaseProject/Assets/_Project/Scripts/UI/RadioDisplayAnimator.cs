using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class RadioDisplayAnimator : MonoBehaviour
{
    [Header("Componentes")]
    [SerializeField] private CanvasGroup radioCanvasGroup;
    [SerializeField] private Image radioImage;
    [SerializeField] private TextMeshProUGUI radioText;
    [SerializeField] private RectTransform textMaskRect;
    [SerializeField] private Sprite radioFrame1;
    [SerializeField] private Sprite radioFrame2;

    [Header("Configurações da Animação de Entrada")]
    [SerializeField] private float appearDuration = 0.8f;
    [SerializeField] private float textAppearDelay = 0.4f;
    [SerializeField] private float textAppearDuration = 0.5f;

    [Header("Configurações do Loop (Playing)")]
    [SerializeField] private float pulseScale = 1.05f;
    [SerializeField] private float pulseDuration = 0.8f;
    [SerializeField] private Ease pulseEase = Ease.InOutSine;
    [SerializeField] private float imageSwapInterval = 0.5f;

    [Header("Configurações do Scroll Infinito")]
    [SerializeField] private float scrollSpeed = 50f;
    [SerializeField] private float scrollGap = 100f; // NOVO: Espaço entre o fim do texto e o recomeço dele

    private bool isPlayingAnimation = false;
    private Tween currentPulseTween;
    private Tween scrollTweenOriginal; // Tween do texto original
    private Tween scrollTweenClone;    // Tween do texto clone
    private Coroutine imageSwapCoroutine;

    private TextMeshProUGUI textClone; // Variável para guardar a cópia do texto

    void Awake()
    {
        if (radioCanvasGroup == null) radioCanvasGroup = GetComponent<CanvasGroup>();
        if (radioCanvasGroup == null) { enabled = false; return; }

        radioCanvasGroup.alpha = 0;
        radioCanvasGroup.interactable = false;
        radioCanvasGroup.blocksRaycasts = false;

        if (radioText != null) radioText.alpha = 0;
        if (radioImage != null && radioFrame1 != null) radioImage.sprite = radioFrame1;
    }

    void Start()
    {
        AnimateRadioIn();
    }

    public void AnimateRadioIn()
    {
        if (isPlayingAnimation) return;
        isPlayingAnimation = true;

        // Antes de animar, removemos qualquer clone antigo se existir
        if (textClone != null) Destroy(textClone.gameObject);

        Sequence sequence = DOTween.Sequence();

        sequence.Append(radioCanvasGroup.DOFade(1, appearDuration).SetEase(Ease.OutQuad));

        if (radioImage != null)
        {
            sequence.Join(radioImage.transform.DOPunchScale(new Vector3(0.1f, 0.1f, 0), appearDuration * 0.7f, 5, 0.5f));
        }

        if (radioText != null)
        {
            radioText.alpha = 0;
            // Resetamos posição para animação de entrada
            radioText.rectTransform.anchoredPosition = new Vector3(0, -20, 0);

            sequence.Insert(textAppearDelay, radioText.DOFade(1, textAppearDuration));
            sequence.Insert(textAppearDelay, radioText.rectTransform.DOAnchorPos(Vector2.zero, textAppearDuration).SetEase(Ease.OutBack));
        }

        sequence.OnComplete(() =>
        {
            isPlayingAnimation = false;
            StartPlayingAnimation();
        });
    }

    private void StartPlayingAnimation()
    {
        // Animação do Rádio
        if (radioImage != null)
        {
            currentPulseTween = radioImage.transform.DOScale(pulseScale, pulseDuration)
                .SetEase(pulseEase)
                .SetLoops(-1, LoopType.Yoyo);

            if (radioFrame1 != null && radioFrame2 != null)
                imageSwapCoroutine = StartCoroutine(SwapRadioImage());
        }

        // Animação do Texto
        if (radioText != null && textMaskRect != null)
        {
            AnimateSeamlessScroll();
        }
    }

    // --- NOVA FUNÇÃO DE LOOP INFINITO ---
    private void AnimateSeamlessScroll()
    {
        KillScrollTweens();

        // 1. Verifica tamanho do texto
        float textWidth = radioText.preferredWidth;
        float maskWidth = textMaskRect.rect.width;

        // 2. Cria ou configura o Clone
        if (textClone == null)
        {
            // Instancia uma cópia do objeto de texto
            GameObject cloneObj = Instantiate(radioText.gameObject, radioText.transform.parent);
            textClone = cloneObj.GetComponent<TextMeshProUGUI>();

            // Remove scripts do clone para não gerar conflito (caso o prefab tenha scripts)
            Destroy(cloneObj.GetComponent<RadioDisplayAnimator>());

            // Garante que o clone tenha o mesmo texto e propriedades
            textClone.text = radioText.text;
            textClone.rectTransform.localScale = radioText.rectTransform.localScale;
        }
        else
        {
            textClone.gameObject.SetActive(true);
            textClone.text = radioText.text; // Atualiza texto caso tenha mudado
            textClone.alpha = radioText.alpha;
        }

        // 3. Cálculos de Posição
        float cycleDistance = textWidth + scrollGap;
        float duration = cycleDistance / scrollSpeed;


        // Configuração Inicial
        radioText.rectTransform.anchoredPosition = new Vector2(0, radioText.rectTransform.anchoredPosition.y);
        textClone.rectTransform.anchoredPosition = new Vector2(cycleDistance, radioText.rectTransform.anchoredPosition.y);

        scrollTweenOriginal = radioText.rectTransform.DOAnchorPosX(-cycleDistance, duration)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Restart);

        scrollTweenClone = textClone.rectTransform.DOAnchorPosX(0, duration)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Restart)
            .From(new Vector2(cycleDistance, radioText.rectTransform.anchoredPosition.y)); // Força o start position do clone
    }

    System.Collections.IEnumerator SwapRadioImage()
    {
        while (true)
        {
            yield return new WaitForSeconds(imageSwapInterval);
            if (radioImage.sprite == radioFrame1)
            {
                radioImage.sprite = radioFrame2;
            }
            else
            {
                radioImage.sprite = radioFrame1;
            }
            radioImage.SetNativeSize();
        }
    }

    private void KillScrollTweens()
    {
        if (scrollTweenOriginal != null) scrollTweenOriginal.Kill();
        if (scrollTweenClone != null) scrollTweenClone.Kill();
    }

    public void StopPlayingAnimation()
    {
        if (currentPulseTween != null) currentPulseTween.Kill();
        KillScrollTweens();
        if (imageSwapCoroutine != null) StopCoroutine(imageSwapCoroutine);

        // Esconde o clone ao parar
        if (textClone != null) textClone.gameObject.SetActive(false);
    }

    public void AnimateRadioOut(float duration = 0.5f)
    {
        StopPlayingAnimation();
        radioCanvasGroup.DOFade(0, duration);
    }
}