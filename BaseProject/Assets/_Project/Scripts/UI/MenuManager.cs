using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    bool isLoadingScene = false; // evita double-load

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        CanvasGroup group = GetComponent<CanvasGroup>();
        if (group != null)
        {
            group.alpha = 0;
            group.DOFade(1, fadeDuration);
        }

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
                // atenção: floatAmount pode ser pequeno (ex: 1-2) para não sair da tela
                child.DOLocalMoveY(startPos.y + floatAmount, floatDuration)
                     .SetLoops(-1, LoopType.Yoyo)
                     .SetEase(Ease.InOutSine);

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

    // Método público para o ButtonEffect chamar quando quiser trocar de cena
    public void LoadSceneWithFade(string sceneName)
    {
        if (isLoadingScene)
        {
            Debug.Log("Já estamos carregando uma cena...");
            return;
        }

        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogWarning("MenuManager: sceneName vazio.");
            return;
        }

        isLoadingScene = true;

        CanvasGroup group = GetComponent<CanvasGroup>();
        if (group != null)
        {
            // desabilita interações para evitar múltiplos cliques
            group.interactable = false;
            group.blocksRaycasts = false;

            group.DOFade(0f, fadeDuration).OnComplete(() =>
            {
                SceneManager.LoadScene(sceneName);
            });
        }
        else
        {
            // se não tiver CanvasGroup, carrega direto
            SceneManager.LoadScene(sceneName);
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