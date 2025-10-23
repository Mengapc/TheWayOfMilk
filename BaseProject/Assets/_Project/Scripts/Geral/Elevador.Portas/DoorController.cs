using UnityEngine;
using System.Collections;

public class DoorController : MonoBehaviour
{
    [SerializeField] private GameObject door;
    [SerializeField] private AnimationCurve _animationCurve;
    [SerializeField] float initialRotation;
    [SerializeField] float atualRotation;
    [SerializeField] float endRotation;
    [SerializeField] private float duration;
    [SerializeField] private float speed;
    [SerializeField] private SkinnedMeshRenderer doorRenderer;
    private bool isOpen = false;


    public void OpenDoor()
    {
        if (!isOpen)
        {
            StartCoroutine(AnimationOpen(initialRotation,endRotation));
        }
        else
        {
            star
        }
    }
    private IEnumerator AnimationOpen(float startValue, float endValue)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            float curveValue = _animationCurve.Evaluate(t);
            atualRotation = Mathf.Lerp(startValue, endValue, curveValue) * 100f;
            transform.rotation = new Quaternion(ne);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}
