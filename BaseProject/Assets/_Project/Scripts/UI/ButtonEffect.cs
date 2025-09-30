using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class ButtonEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("Configuração do botão")]
    [SerializeField] float effectTime = 0.2f;
    [SerializeField] float focusedSize = 1.2f;
    [SerializeField] float normalSize = 1f;
    [SerializeField] float jumpHeight = 1f;
    [SerializeField] float jumpForce = 1f;

    private void Start()
    {
        // Se registrar no gerenciador
        MenuManager.Instance.RegisterButton(this);
    }

    // Quando o mouse entra no botão
    public void OnPointerEnter(PointerEventData eventData)
    {
        MenuManager.Instance.FocusButton(this);
    }

    // Quando o mouse sai do botão
    public void OnPointerExit(PointerEventData eventData)
    {
        ResetSize();
    }

    // Quando o botão é clicado
    public void OnPointerClick(PointerEventData eventData)
    {
        switch (eventData.button)
        {
            case PointerEventData.InputButton.Left:
                Debug.Log("Clique Esquerdo no botão " + gameObject.name);
                JumpEffect();
                break;

            case PointerEventData.InputButton.Right:
                Debug.Log("Clique Direito no botão " + gameObject.name);
                break;

            case PointerEventData.InputButton.Middle:
                Debug.Log("Clique Scroll/Meio no botão " + gameObject.name);
                break;
        }
    }

    // Foca este botão (usado pelo gerenciador)
    public void Focus()
    {
        transform.DOScale(focusedSize, effectTime);
    }

    // Reseta o tamanho do botão
    public void ResetSize()
    {
        transform.DOScale(normalSize, effectTime);
    }

    // Efeito de pulo ao clicar
    private void JumpEffect()
    {
        transform.DOJump(
            new Vector3(transform.position.x, transform.position.y + jumpHeight, transform.position.z),
            jumpForce, 1, effectTime, false
        );
    }
}