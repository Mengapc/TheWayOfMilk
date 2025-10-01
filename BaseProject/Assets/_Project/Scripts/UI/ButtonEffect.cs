using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using System;
using UnityEngine.SceneManagement;
using UnityEditor.Experimental.GraphView;


#if UNITY_EDITOR
using UnityEditor; // para SceneAsset
#endif

public class ButtonEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("Configuração do botão")]
    [SerializeField] float effectTime = 0.2f;
    [SerializeField] float focusedSize = 1.2f;
    [SerializeField] float normalSize = 1f;
    [SerializeField] float jumpHeight = 1f;
    [SerializeField] float jumpForce = 1f;

    [SerializeField] private AudioClip[] onClickSounds;
    [SerializeField] private AudioClip onHoverSound;


    public Action OnClickAction;

    // ----- Scene selection (arrastar a cena aqui no Inspector) -----
#if UNITY_EDITOR
    public SceneAsset sceneAsset; // arraste a cena .unity aqui (editor only)
#endif
    [SerializeField, HideInInspector] string sceneName; // name salvo para runtime

    // ---------------------------------------------------------------

    private void Start()
    {
        // registra no MenuManager para hover/focus
        if (MenuManager.Instance != null)
            MenuManager.Instance.RegisterButton(this);
    }

    // Executa no Editor quando você altera o SceneAsset no Inspector:
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
        SoundFXManager.instance.PlayRandomSoundFXClip(onClickSounds, transform, 1f);
        JumpEffect();
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
        transform.DOScale(focusedSize, effectTime);
    }

    public void ResetSize()
    {
        transform.DOScale(normalSize, effectTime);
    }

    // opcional: efeito de pulo (caso queira usar em clique)
    public void JumpEffect()
    {
        transform.DOJump(
            new Vector3(transform.position.x, transform.position.y + jumpHeight, transform.position.z),
            jumpForce, 1, effectTime, false
        );
    }
}