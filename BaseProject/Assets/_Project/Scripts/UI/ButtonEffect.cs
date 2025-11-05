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
    // <<< NOVO >>>
    // Marque esta opção no Inspetor APENAS para o botão "Jogar"
    [SerializeField] bool destruirMenuAudioAoCarregar = false;

    [Header("Efeito Hover")]
    [SerializeField] float focusedSize = 1.1f;
    [SerializeField] Color hoverColor = Color.white;

    [Header("Efeito Click")]
    [SerializeField] Vector3 punchScale = new Vector3(0.1f, 0.1f, 0.1f);
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

    private Image buttonImage;
    private Color originalColor;
    private float normalSize;
    private Sequence currentSequence;

    private void Awake()
    {
        buttonImage = GetComponent<Image>();
        if (buttonImage != null)
        {
            originalColor = buttonImage.color;
        }
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
        if (currentSequence != null && currentSequence.IsActive())
        {
            currentSequence.Kill();
        }

        SoundFXManager.instance.PlayRandomSoundFXClip(onClickSounds, transform, 1f);

        currentSequence = DOTween.Sequence();
        currentSequence.Append(transform.DOPunchScale(
            punchScale,
            effectTime,
            punchVibrato,
            punchElasticity
        ));

        // <<< ALTERADO >>>
        // Agora chamamos o método que contém a nossa nova lógica
        currentSequence.OnComplete(LoadSceneOrAction);
    }

    // <<< MÉTODO ALTERADO >>>
    private void LoadSceneOrAction()
    {
        // Prioriza cena configurada (se houver)
        if (!string.IsNullOrEmpty(sceneName))
        {
            // --- INÍCIO DA LÓGICA NOVA ---
            // Se este botão está marcado para destruir o áudio, fazemos isso agora.
            if (destruirMenuAudioAoCarregar)
            {
                // Você precisa marcar seu AudioManager do Menu com esta tag!
                GameObject menuAudio = GameObject.FindGameObjectWithTag("MenuAudioManager");
                if (menuAudio != null)
                {
                    Destroy(menuAudio);
                    Debug.Log("MenuAudioManager destruído pelo botão.");
                }
                else
                {
                    Debug.LogWarning("Botão 'Jogar' tentou destruir o MenuAudioManager, mas não o encontrou pela tag 'MenuAudioManager'.");
                }
            }
            // --- FIM DA LÓGICA NOVA ---

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
        transform.DOKill();
        if (buttonImage != null) buttonImage.DOKill();

        transform.DOScale(focusedSize, effectTime).SetEase(Ease.OutBack);
        if (buttonImage != null && hoverColor != Color.white)
        {
            buttonImage.DOColor(hoverColor, effectTime);
        }
    }

    public void ResetSize()
    {
        transform.DOKill();
        if (buttonImage != null) buttonImage.DOKill();

        transform.DOScale(normalSize, effectTime);
        if (buttonImage != null && hoverColor != Color.white)
        {
            buttonImage.DOColor(originalColor, effectTime);
        }
    }

    public void JumpEffect()
    {
        transform.DOPunchScale(punchScale, effectTime, punchVibrato, punchElasticity);
    }
}