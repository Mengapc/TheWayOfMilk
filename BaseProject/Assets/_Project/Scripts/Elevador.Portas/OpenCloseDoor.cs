using UnityEngine;
using System.Collections;

public class OpenCloseDoor : MonoBehaviour
{
    #region V�riaveis
    [SerializeField] private bool startOpen = true;
    [Tooltip("Renderizador da porta com blend shapes.")]
    [SerializeField] private SkinnedMeshRenderer doorRenderer;
    [Tooltip("Curva de anima��o da porta.")]
    [SerializeField] private AnimationCurve doorAnimationCurve;
    [Tooltip("Colisor da porta quando aberta.")]
    [SerializeField] private Collider doorColliderOpened;
    [Tooltip("Colisor da porta quando fechada.")]
    [SerializeField] private Collider doorColliderClosed;
    [Tooltip("Dura��o da anima��o de abertura/fechamento da porta.")]
    [SerializeField] private float animationDuration = 2f;
    [Tooltip("Som de abertura.")]
    [SerializeField] private AudioClip somPortaAbrindo;
    [Tooltip("Som de fechamento.")]
    [SerializeField] private AudioClip somPortaFechando;
    private bool isOpen = false;
    #endregion

    #region Unity Methods
    public void Start()
    {
        // Valida��o de seguran�a para ajudar a encontrar erros
        if (doorRenderer == null) Debug.LogError("Door Renderer n�o atribu�do em " + gameObject.name);
        if (doorColliderOpened == null) Debug.LogError("Door Collider Opened n�o atribu�do em " + gameObject.name);
        if (doorColliderClosed == null) Debug.LogError("Door Collider Closed n�o atribu�do em " + gameObject.name);

        if (startOpen)
        {
            isOpen = false; /// Garante que a porta ser� aberta
            ToggleDoor();   /// Abre a porta no in�cio
        }
        else
        {
            // Define o estado inicial como fechado
            if (doorColliderClosed != null) doorColliderClosed.enabled = true;
            if (doorColliderOpened != null) doorColliderOpened.enabled = false;
            if (doorRenderer != null) doorRenderer.SetBlendShapeWeight(0, 0f); /// Porta fechada
        }
    }
    public void ToggleDoor()
    {
        // Abre a porta
        if (!isOpen)
        {
            StartCoroutine(AnimateDoor(0f, 1f));
        }
        // Fecha a porta
        else
        {
            StartCoroutine(AnimateDoor(1f, 0f));
        }
        // NOTA: A l�gica 'isOpen = !isOpen;' foi movida para dentro da corrotina.
    }

    private IEnumerator AnimateDoor(float startValue, float endValue)
    {
        float elapsedTime = 0f; /// Tempo decorrido desde o in�cio da anima��o

        // --- CORRE��O DE BUG DO SOM ---
        // Determina o som ANTES de inverter o estado 'isOpen'
        AudioClip somPorta = isOpen ? somPortaFechando : somPortaAbrindo;/// Escolhe o som correto
        if (SoundFXManager.instance != null && somPorta != null)
        {
            SoundFXManager.instance.PlaySoundFXClip(somPorta, transform, 1f);/// Toca o som da porta
        }

        // Ativa o colisor correto no in�cio da anima��o
        if (startValue > 0) /// Porta est� fechando (endValue � 0)
        {
            if (doorColliderClosed != null) doorColliderClosed.enabled = true;
            if (doorColliderOpened != null) doorColliderOpened.enabled = false;
        }
        else // Porta est� abrindo (endValue � 1)
        {
            if (doorColliderClosed != null) doorColliderClosed.enabled = false;
            if (doorColliderOpened != null) doorColliderOpened.enabled = true;
        }

        // Anima a porta ao longo do tempo
        while (elapsedTime < animationDuration)
        {
            float t = elapsedTime / animationDuration;
            float curveValue = doorAnimationCurve.Evaluate(t);
            float blendShapeValue = Mathf.Lerp(startValue, endValue, curveValue) * 100f;

            if (doorRenderer != null) doorRenderer.SetBlendShapeWeight(0, blendShapeValue);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Garante que a porta atinja o valor final exato
        if (doorRenderer != null) doorRenderer.SetBlendShapeWeight(0, endValue * 100f);

        // --- CORRE��O DE BUG DO SOM ---
        // Inverte o estado APENAS no final da anima��o
        isOpen = !isOpen;
    }
    #endregion
}