using UnityEngine;
using System.Collections;

// O nome do arquivo � Bomb.cs, ent�o a classe deve se chamar Bomb
// Se quiser que se chame DoorController, precisa renomear o arquivo para DoorController.cs
public class DoorController : MonoBehaviour
{
    [Header("Configura��es de Anima��o da Porta")]
    [Tooltip("Curva de anima��o para abrir a porta.")]
    [SerializeField] private AnimationCurve _animationCurveOpen;
    [Tooltip("Rota��o inicial da porta.")]
    [SerializeField] float initialRotation; 
    [Tooltip("Rota��o atual da porta.")]
    [SerializeField] float atualRotation;
    [Tooltip("Rota��o final da porta.")]
    [SerializeField] float endRotation;
    [Tooltip("Dura��o da anima��o de abertura da porta.")][Range(0.1f, 10f)]
    [SerializeField] private float duration;

    [Header("Comfigura��es de Anima��o de Destrava")]
    [Tooltip("Skinned Mesh Renderer da porta.")]
    [SerializeField] private SkinnedMeshRenderer doorRenderer;
    [Tooltip("Curva de anima��o para destravar a porta.")]
    [SerializeField] private AnimationCurve _animationCurveDestrava;
    [Tooltip("Dura��o da anima��o de destrava da porta.")][Range(0.1f, 10f)]
    [SerializeField] private float durationDestrava;

    private bool isOpen = false;

    public void OpenDoor()
    {
        // Checa se a porta J� N�O EST� aberta ou se movendo
        if (!isOpen)
        {
            // Marca a porta como "aberta" imediatamente para evitar
            // que a coroutine seja chamada v�rias vezes
            isOpen = true;
            
            StartCoroutine(AnimationOpen(initialRotation, endRotation));
        }

    }

    private IEnumerator AnimationOpen(float startValue, float endValue)
    {
        yield return StartCoroutine(AnimationDestrava(1f, 0f));
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            float curveValue = _animationCurveOpen.Evaluate(t);

            // --- CORRE��O ---
            // 1. Removido o "* 100f"
            atualRotation = Mathf.Lerp(startValue, endValue, curveValue);

            // 2. Aplicando a rota��o no eixo Y (como uma porta normal)
            // Quaternion.Euler converte �ngulos (0, 90, etc.) para a rota��o correta
            transform.rotation = Quaternion.Euler(0, atualRotation, 0);
            // --- FIM DA CORRE��O ---

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Garante que a rota��o final seja exata
        transform.rotation = Quaternion.Euler(0, endValue, 0);
    }

    private IEnumerator AnimationDestrava(float startValue, float endValue)
    {
        float elapsedTime = 0f;
        while (elapsedTime < durationDestrava)
        {
            float t = elapsedTime / durationDestrava;
            float curveValue = _animationCurveDestrava.Evaluate(t);
            float blendShapeValue = Mathf.Lerp(startValue, endValue, curveValue) * 100f;
            doorRenderer.SetBlendShapeWeight(0, blendShapeValue);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        doorRenderer.SetBlendShapeWeight(0, endValue * 100f);
    }
}

