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

    bool isLoadingScene = false; // evita double-load

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        // Garante que o CanvasGroup principal esteja visível
        CanvasGroup mainGroup = GetComponent<CanvasGroup>();
        if (mainGroup != null)
        {
            mainGroup.alpha = 1;
            mainGroup.interactable = true;
            mainGroup.blocksRaycasts = true;
        }

        foreach (Transform child in transform)
        {
            // Procuramos o CanvasGroup no filho (ou nos filhos dele)
            CanvasGroup childCanvas = child.GetComponentInChildren<CanvasGroup>();

            if (childCanvas == null)
            {
                // Pula este item se não tiver CanvasGroup
                continue;
            }

            Sequence seq = DOTween.Sequence();

            // 1. Preparar o Fade (para TODOS os filhos com CanvasGroup)
            childCanvas.alpha = 0; // Define o alpha inicial
            var fadeTween = childCanvas.DOFade(1, fadeDuration);

            seq.Append(fadeTween);

            // 2. Preparar o Movimento (APENAS para tag "Botao")
            if (child.CompareTag("Botao"))
            {
                Vector3 startPos = child.localPosition;
                // Define a posição inicial "para baixo"
                child.localPosition = startPos + new Vector3(0, -moveOffsetY, 0);

                var moveTween = child.DOLocalMove(startPos, fadeDuration)
                                    .SetEase(Ease.OutBack);

                // Junta o movimento ao fade
                seq.Join(moveTween);
            }
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
                SceneLoader.LoadScene(sceneName);
            });
        }
        else
        {
            // se não tiver CanvasGroup, carrega direto
            SceneLoader.LoadScene(sceneName);
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