using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Precisamos disso para controlar a cor da Imagem

#if UNITY_EDITOR
using UnityEditor;
#endif

public class ButtonEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("Configuração Geral")]
    [SerializeField] float effectTime = 0.2f;

    [Header("Efeito Hover")]
    [SerializeField] float focusedSize = 1.1f;
    [SerializeField] Color hoverColor = Color.white; // Cor do botão no hover (se branco, não muda)

    [Header("Efeito Click")]
    [SerializeField] Vector3 punchScale = new Vector3(0.1f, 0.1f, 0.1f); // Força do "soco"
    [SerializeField] int punchVibrato = 10;
    [SerializeField] float punchElasticity = 1f;

    [Header("Sons")]
    [SerializeField] private AudioClip[] onClickSounds;
    [SerializeField] private AudioClip onHoverSound;

    public Action OnClickAction;

    // ----- Scene selection -----
#if UNITY_EDITOR
    public SceneAsset sceneAsset;
#endif
    [SerializeField, HideInInspector] string sceneName;
    // ---------------------------

    // --- Componentes e Valores Originais ---
    private Image buttonImage;
    private Color originalColor;
    private float normalSize;
    private Sequence currentSequence; // Para controlar animações ativas

    private void Awake()
    {
        // Tenta pegar a imagem e guardar a cor original
        buttonImage = GetComponent<Image>();
        if (buttonImage != null)
        {
            originalColor = buttonImage.color;
        }

        // Guarda o tamanho original
        normalSize = transform.localScale.x;
    }

    private void Start()
    {
        if (MenuManager.Instance != null)
            MenuManager.Instance.RegisterButton(this);
    }

    private void OnValidate()
    {
#if UNITY_EDITOR
        if (sceneAsset != null)
        {
            sceneName = sceneAsset.name;
        }
#endif
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (MenuManager.Instance != null)
            MenuManager.Instance.FocusButton(this);
        SoundFXManager.instance.PlaySoundFXClip(onHoverSound, transform, 1f);
        Focus();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ResetSize();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // Mata a sequência anterior se houver (para evitar cliques duplos)
        if (currentSequence != null && currentSequence.IsActive())
        {
            currentSequence.Kill();
        }

        // Toca o som primeiro
        SoundFXManager.instance.PlayRandomSoundFXClip(onClickSounds, transform, 1f);

        // Cria uma nova sequência de animação
        currentSequence = DOTween.Sequence();

        // 1. Adiciona o efeito de "punch"
        currentSequence.Append(transform.DOPunchScale(
            punchScale,
            effectTime,
            punchVibrato,
            punchElasticity
        ));

        // 2. QUANDO TERMINAR (OnComplete), carrega a cena ou ação
        currentSequence.OnComplete(LoadSceneOrAction);
    }

    // Ação de carregar cena/evento (agora separada)
    private void LoadSceneOrAction()
    {
        // Prioriza cena configurada (se houver)
        if (!string.IsNullOrEmpty(sceneName))
        {
            Debug.Log($"ButtonEffect: '{gameObject.name}' -> carregando cena '{sceneName}'");
            if (MenuManager.Instance != null)
                MenuManager.Instance.LoadSceneWithFade(sceneName);
            else
                SceneManager.LoadScene(sceneName);
            return;
        }

        // Senão, fallback para ação custom (se existir)
        OnClickAction?.Invoke();
    }


    public void Focus()
    {
        // Mata tweens anteriores para evitar bugs
        transform.DOKill();
        if (buttonImage != null) buttonImage.DOKill();

        // Animação de escala E cor
        transform.DOScale(focusedSize, effectTime).SetEase(Ease.OutBack);
        if (buttonImage != null && hoverColor != Color.white)
        {
            buttonImage.DOColor(hoverColor, effectTime);
        }
    }

    public void ResetSize()
    {
        // Mata tweens anteriores
        transform.DOKill();
        if (buttonImage != null) buttonImage.DOKill();

        // Reseta escala E cor
        transform.DOScale(normalSize, effectTime);
        if (buttonImage != null && hoverColor != Color.white)
        {
            buttonImage.DOColor(originalColor, effectTime);
        }
    }

    // Mantive este método caso o MenuManager chame ele por nome
    public void JumpEffect()
    {
        transform.DOPunchScale(punchScale, effectTime, punchVibrato, punchElasticity);
    }
}