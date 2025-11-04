using UnityEngine;

public class AtivacaoPedestal : MonoBehaviour
{
    public ParticleSystem ps; // arrasta o particle aqui
    public Activate other; // o script com a variável
    public Light targetLight;

    [Header("Cores")]
    public Color colorOff = Color.red;
    public Color colorOn = Color.green;

    [Header("Velocidade da transição")]
    public float lerpSpeed = 3f;

    private Color currentColor;
    private ParticleSystem.MainModule main;
    void Start()
    {
        if (ps == null) ps = GetComponent<ParticleSystem>();
        main = ps.main;
        currentColor = colorOff;
        main.startColor = currentColor;

        if (targetLight != null)
            targetLight.color = currentColor;
    }

    // Update is called once per frame
    void Update()
    {
        // define a cor alvo com base na variável
        Color targetColor = other.Ativado ? colorOn : colorOff;

        // faz o LERP suave entre a cor atual e a alvo
        currentColor = Color.Lerp(currentColor, targetColor, Time.deltaTime * lerpSpeed);

        // aplica ao particle system
        main.startColor = currentColor;

        if (targetLight != null)
            targetLight.color = currentColor;
    }
}
