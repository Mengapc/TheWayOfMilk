using UnityEngine;
using System.Collections;

public class OpenCloseDoor : MonoBehaviour
{
    #region Singleton
    [SerializeField] private SkinnedMeshRenderer doorRenderer;
    [SerializeField] private AnimationCurve doorAnimationCurve;
    [SerializeField] private Collider doorColliderOpened;
    [SerializeField] private Collider doorColliderClosed;
    [SerializeField] private float animationDuration = 2f;
    [Tooltip("Som de abertura.")]
    [SerializeField] private AudioClip somPorta; 
    private bool isOpen = false;
    #endregion

    #region Unity Methods
    public void Start()
    {
        ToggleDoor();
    }
    public void ToggleDoor()
    {
        // Alterna o estado da porta
        if (!isOpen)
        {
            doorColliderClosed.enabled = false;
            doorColliderOpened.enabled = true;
            StartCoroutine(AnimateDoor(0f, 1f));
        }
        // Fecha a porta
        else
        {
            doorColliderClosed.enabled = true;
            doorColliderOpened.enabled = false;
            StartCoroutine(AnimateDoor(1f, 0f));
        }
        isOpen = !isOpen;
    }
    private IEnumerator AnimateDoor(float startValue, float endValue)
    {
        
        float elapsedTime = 0f;
        SoundFXManager.instance.PlaySoundFXClip(somPorta, transform, 1f);
        while (elapsedTime < animationDuration)
        {
            float t = elapsedTime / animationDuration;
            float curveValue = doorAnimationCurve.Evaluate(t);
            float blendShapeValue = Mathf.Lerp(startValue, endValue, curveValue) * 100f;
            doorRenderer.SetBlendShapeWeight(0, blendShapeValue);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        doorRenderer.SetBlendShapeWeight(0, endValue * 100f);

    }
    #endregion
}
