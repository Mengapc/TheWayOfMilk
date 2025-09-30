using UnityEngine;
using System.Collections;

public class LightControler : MonoBehaviour
{

    [SerializeField] private Light lightToControl;
    [SerializeField] private float Intensity = 1f;
    [SerializeField] private AnimationCurve intensityCurve;
    [SerializeField] private float duration = 2f;
    [SerializeField] private float flacker;
    private bool isOn = false;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            if (!isOn)
            {
                StartCoroutine(TurnOnLight());
            }
            else
            {
                StartCoroutine(TurnOffLight());
            }
        }
    }

    private IEnumerator TurnOnLight()
    {
        isOn = true;
        float elapsedTime = 0f;
        lightToControl.enabled = true;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float percentage = elapsedTime / duration;
            lightToControl.intensity = intensityCurve.Evaluate(percentage) * Intensity + Random.Range(-flacker, flacker);
            yield return null;
        }
        lightToControl.intensity = Intensity;
    }
    private IEnumerator TurnOffLight()
    {
        isOn = false;
        float elapsedTime = 0f;
        float initialIntensity = lightToControl.intensity;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float percentage = elapsedTime / duration;
            lightToControl.intensity = (1 - intensityCurve.Evaluate(percentage)) * initialIntensity + Random.Range(-flacker, flacker);
            yield return null;
        }
        lightToControl.intensity = 0f;
        lightToControl.enabled = false;
    }

}
