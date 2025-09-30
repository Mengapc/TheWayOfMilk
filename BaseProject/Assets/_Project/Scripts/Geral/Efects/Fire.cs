using System.Collections;
using UnityEngine;

public class Fire : MonoBehaviour
{
    [SerializeField] private ParticleSystem particle;
    [SerializeField] private AnimationCurve intensityOverTime;
    [SerializeField] private float force;
    [SerializeField] private float intensy;
    [SerializeField] private float size;
    [SerializeField] private Vector3 direction;

    void Start()
    {
        if (particle != null) StartCoroutine(FlickerFire(particle));

    }

    private IEnumerator FlickerFire(ParticleSystem targetParticle)
    {

        var forceModule = targetParticle.forceOverLifetime;
        var mainModule = targetParticle.sizeOverLifetime;

        // Garante que o módulo está ativo antes de começar.
        forceModule.enabled = true;

        while (true)
        {
            float duration = intensityOverTime.keys[intensityOverTime.length - 1].time;
            float time = 0f;

            while (time < duration)
            {
                time += Time.deltaTime;

                float intensity = intensy *intensityOverTime.Evaluate(time);
                float size = this.size * intensityOverTime.Evaluate(time);



                // Modifica diretamente o módulo, pois forceOverLifetime é somente leitura.
                forceModule.x = new ParticleSystem.MinMaxCurve(direction.x * force * intensity);
                forceModule.y = new ParticleSystem.MinMaxCurve(direction.y * force * intensity);
                forceModule.z = new ParticleSystem.MinMaxCurve(direction.z * force * intensity);

                mainModule.size = new ParticleSystem.MinMaxCurve(size);


                yield return null;
            }
        }
    }
}