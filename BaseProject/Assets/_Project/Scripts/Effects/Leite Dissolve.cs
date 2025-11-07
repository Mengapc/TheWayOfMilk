using UnityEngine;
using System.Collections;

public class LeiteDissolve : MonoBehaviour
{
    [Tooltip("Nome do float no shader que controla o dissolve (ex: \"_Cutoff\", \"_Dissolve\", \"_Amount\")")]
    public string propertyName = "_Cutoff";

    public float from = 0f;
    public float to = 1f;
    [Tooltip("Tempo em segundos para ir de 'from' para 'to'")]
    public float duration = 1f;

    [Tooltip("Se true inicia automaticamente no Start()")]
    public bool autoStart = true;

    [Tooltip("Se true instancia o material (Renderer.material). Se false usará sharedMaterial (não recomendado se houver múltiplos objetos usando o mesmo material).")]
    public bool instantiateMaterial = true;

    private Renderer rend;
    private Material instancedMaterial;
    private Coroutine currentCoroutine;

    void Start()
    {
        rend = GetComponent<Renderer>();
        if (rend == null)
        {
            Debug.LogWarning("DissolveAndDestroy: nenhum Renderer encontrado no GameObject.");
            return;
        }

        if (instantiateMaterial)
            instancedMaterial = rend.material; // cria instância do material para este objeto
        else
            instancedMaterial = rend.sharedMaterial; // altera material compartilhado (cuidado)

        // define valor inicial imediatamente
        if (instancedMaterial.HasProperty(propertyName))
            instancedMaterial.SetFloat(propertyName, from);
        else
            Debug.LogWarning($"DissolveAndDestroy: material não tem a propriedade '{propertyName}'.");

        if (autoStart)
            StartDissolve();
    }
    public void StartDissolve()
    {
        if (rend == null) return;
        // evita múltiplas coroutines
        if (currentCoroutine != null) StopCoroutine(currentCoroutine);
        currentCoroutine = StartCoroutine(DissolveRoutine());
    }
    private IEnumerator DissolveRoutine()
    {
        if (!instancedMaterial.HasProperty(propertyName))
            yield break;

        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(duration > 0f ? (elapsed / duration) : 1f);
            float value = Mathf.Lerp(from, to, t);
            instancedMaterial.SetFloat(propertyName, value);
            yield return null;
        }

        // garante o valor final exato
        instancedMaterial.SetFloat(propertyName, to);

        // opcional: esperar um frame (não obrigatório)
        yield return null;

        Destroy(gameObject);
    }

    // método extra se quiser disparar com UI (por exemplo: um botão)
    public void StartDissolveFrom(float startValue)
    {
        from = startValue;
        StartDissolve();
    }
}
