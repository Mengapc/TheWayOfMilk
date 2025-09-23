using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;

    private List<ButtonEffect> buttons = new List<ButtonEffect>();

    [Header("Configurações de Entrada")]
    [SerializeField] float fadeDuration = 0.5f;
    [SerializeField] float moveOffsetY = 50f;
    [SerializeField] float staggerDelay = 0.1f;

    [Header("Configurações de Movimento Contínuo")]
    [SerializeField] float floatAmount = 5f;
    [SerializeField] float floatDuration = 1.5f;
    [SerializeField] float rotationAmount = 5f;
    [SerializeField] float rotationDuration = 3f;
    [SerializeField] float pulseAmount = 1.05f;
    [SerializeField] float pulseDuration = 1f;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        CanvasGroup group = GetComponent<CanvasGroup>();
        group.alpha = 0;
        group.DOFade(1, fadeDuration);

        int i = 0;
        foreach (Transform child in transform)
        {
            Vector3 startPos = child.localPosition;
            child.localPosition = startPos + new Vector3(0, -moveOffsetY, 0);

            Sequence seq = DOTween.Sequence();

            // Entrada com fade + movimento
            seq.Append(child.DOLocalMove(startPos, fadeDuration)
                        .SetEase(Ease.OutBack)
                        .SetDelay(i * staggerDelay));

            CanvasGroup childCanvas = child.GetComponent<CanvasGroup>();
            if (childCanvas != null)
            {
                childCanvas.alpha = 0;
                seq.Join(childCanvas.DOFade(1, fadeDuration).SetDelay(i * staggerDelay));
            }

            // Se for logo ou imagem de destaque: adiciona rotação + float contínuo
            if (child.CompareTag("Logo") || child.CompareTag("Image"))
            {
                // Float contínuo
                child.DOLocalMoveY(startPos.y + floatAmount, floatDuration)
                     .SetLoops(-1, LoopType.Yoyo)
                     .SetEase(Ease.InOutSine);

                // Rotação leve contínua
                child.DORotate(new Vector3(0, 0, rotationAmount), rotationDuration, RotateMode.FastBeyond360)
                     .SetLoops(-1, LoopType.Yoyo)
                     .SetEase(Ease.InOutSine);
            }

            // Se for botão: adiciona pulso sutil
            if (child.CompareTag("Botao"))
            {
                child.DOScale(pulseAmount, pulseDuration)
                     .SetLoops(-1, LoopType.Yoyo)
                     .SetEase(Ease.InOutSine);
            }

            i++;
        }
    }

    public void RegisterButton(ButtonEffect button)
    {
        if (!buttons.Contains(button))
            buttons.Add(button);
    }

    public void FocusButton(ButtonEffect focusedButton)
    {
        foreach (var btn in buttons)
        {
            if (btn == focusedButton)
                btn.Focus();
            else
                btn.ResetSize();
        }
    }
}