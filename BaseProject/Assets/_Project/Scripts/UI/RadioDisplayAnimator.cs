using UnityEngine;
using UnityEngine.UI;
using TMPro; // Para TextMeshPro
using DG.Tweening; // Para DOTween

public class RadioDisplayAnimator : MonoBehaviour
{
    [Header("Componentes")]
    [SerializeField] private CanvasGroup radioCanvasGroup;
    [SerializeField] private Image radioImage;
    [SerializeField] private TextMeshProUGUI radioText; // Use TextMeshProUGUI
    [SerializeField] private Sprite radioFrame1; // Sem notas musicais (Image 2)
    [SerializeField] private Sprite radioFrame2; // Com notas musicais (Image 3)

    [Header("Configurações da Animação")]
    [SerializeField] private float appearDuration = 0.8f;
    [SerializeField] private float textAppearDelay = 0.4f;
    [SerializeField] private float textAppearDuration = 0.5f;
    [SerializeField] private float pulseScale = 1.05f;
    [SerializeField] private float pulseDuration = 0.8f;
    [SerializeField] private Ease pulseEase = Ease.InOutSine;
    [SerializeField] private float imageSwapInterval = 0.5f; // Tempo para alternar frames do rádio

    private bool isPlayingAnimation = false;
    private Tween currentPulseTween;
    private Coroutine imageSwapCoroutine;

    void Awake()
    {
        // Garante que os componentes estão atribuídos
        if (radioCanvasGroup == null) radioCanvasGroup = GetComponent<CanvasGroup>();
        if (radioCanvasGroup == null)
        {
            Debug.LogError("RadioDisplayAnimator: CanvasGroup não encontrado!");
            enabled = false;
            return;
        }

        // Inicializa o estado
        radioCanvasGroup.alpha = 0;
        radioCanvasGroup.interactable = false;
        radioCanvasGroup.blocksRaycasts = false;

        // Garante que o texto está invisível inicialmente
        if (radioText != null)
        {
            radioText.alpha = 0;
        }

        // Garante que a imagem inicial é o frame 1 (sem notas)
        if (radioImage != null && radioFrame1 != null)
        {
            radioImage.sprite = radioFrame1;
        }
    }

    void Start()
    {
        // Inicia a animação quando a cena carrega
        AnimateRadioIn();
    }

    public void AnimateRadioIn()
    {
        if (isPlayingAnimation) return;
        isPlayingAnimation = true;

        Sequence sequence = DOTween.Sequence();

        // 1. Fade In do CanvasGroup (rádio e texto juntos)
        sequence.Append(radioCanvasGroup.DOFade(1, appearDuration).SetEase(Ease.OutQuad));

        // 2. Pequeno movimento ou pulso inicial para o rádio (opcional, mas pode dar um toque legal)
        // Por exemplo, podemos fazer um pequeno salto para cima e para baixo
        if (radioImage != null)
        {
            Vector3 originalScale = radioImage.transform.localScale;
            sequence.Join(radioImage.transform.DOPunchScale(new Vector3(0.1f, 0.1f, 0), appearDuration * 0.7f, 5, 0.5f)); // Um pequeno "punch"
        }

        // 3. Fade In e movimento para o texto (atrasado)
        if (radioText != null)
        {
            radioText.alpha = 0; // Garante que o texto está invisível
            Vector3 originalTextPos = radioText.rectTransform.anchoredPosition;
            Vector3 startTextPos = originalTextPos + new Vector3(0, -20, 0); // Começa um pouco abaixo
            radioText.rectTransform.anchoredPosition = startTextPos;

            sequence.Insert(textAppearDelay, radioText.DOFade(1, textAppearDuration));
            sequence.Insert(textAppearDelay, radioText.rectTransform.DOAnchorPos(originalTextPos, textAppearDuration).SetEase(Ease.OutBack));
        }

        // 4. Ao completar a sequência de entrada, inicia a animação de "playing"
        sequence.OnComplete(() =>
        {
            isPlayingAnimation = false;
            StartPlayingAnimation();
        });
    }

    private void StartPlayingAnimation()
    {
        if (radioImage != null)
        {
            // Animação de pulso contínuo para a imagem do rádio
            currentPulseTween = radioImage.transform.DOScale(pulseScale, pulseDuration)
                .SetEase(pulseEase)
                .SetLoops(-1, LoopType.Yoyo); // Loop infinito, volta ao tamanho original

            // Inicia a alternância de sprites para as notas musicais
            if (radioFrame1 != null && radioFrame2 != null)
            {
                imageSwapCoroutine = StartCoroutine(SwapRadioImage());
            }
        }
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

    public void StopPlayingAnimation()
    {
        if (currentPulseTween != null)
        {
            currentPulseTween.Kill();
        }
        if (imageSwapCoroutine != null)
        {
            StopCoroutine(imageSwapCoroutine);
        }
        // Opcional: fade out ou animação de saída se necessário
    }

    // Exemplo de como você chamaria para esconder o rádio (e.g., ao sair do menu)
    public void AnimateRadioOut(float duration = 0.5f)
    {
        StopPlayingAnimation();
        radioCanvasGroup.DOFade(0, duration);
        // Opcional: Mover para fora da tela também
        // radioCanvasGroup.transform.DOLocalMoveY(radioCanvasGroup.transform.localPosition.y + moveOffsetY, duration);
    }
}