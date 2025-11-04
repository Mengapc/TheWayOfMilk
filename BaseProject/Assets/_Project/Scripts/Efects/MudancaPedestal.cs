using UnityEngine;

public class MudancaPedestal : MonoBehaviour
{
    public Renderer rend;
    public Activate other;
    public int materialIndex = 0;

    [Header("Cores")]
    public Color colorOff = Color.red;
    public Color colorOn = Color.green;

    [Header("Velocidade e Intensidade")]
    public float lerpSpeed = 1f;
    public float emissionIntensity = 3f;

    Material mat;
    Color currentBase;
    Color currentEmission;

    void Start()
    {
        if (rend == null) rend = GetComponent<Renderer>();
        mat = rend.materials[materialIndex];

        currentBase = colorOff;
        currentEmission = colorOff;
    }

    // Update is called once per frame
    void Update()
    {
        Color target = other.Ativado ? colorOn : colorOff;

        // lerp base
        currentBase = Color.Lerp(currentBase, target, Time.deltaTime * lerpSpeed);
        mat.color = currentBase;

        // lerp emissive
        currentEmission = Color.Lerp(currentEmission, target, Time.deltaTime * lerpSpeed);
        mat.SetColor("_EmissionColor", currentEmission * emissionIntensity);
    }
}
