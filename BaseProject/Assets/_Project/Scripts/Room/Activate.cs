using System.Collections;
using UnityEngine;
using UnityEngine.Events; // Adicionei este using, pois você tem um UnityEvent

public class Activate : MonoBehaviour
{

    [SerializeField] private DoorController door;
    [SerializeField] private UnityEvent _evento;
    [SerializeField] private AnimationCurve _animation;
    [SerializeField] private Transform pont1;
    [SerializeField] private Transform pont2;
    private Transform objectTarget;

    float durationAnimation = 2f;



    [SerializeField]

    private bool isBoxOnPlate = false;



    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {

            if (other.GetComponent<Rigidbody>().isKinematic == false)
            {
                  StartCoroutine(AjustLeite(other.transform, other.transform.position, pont1.position));
            }
        }
    }

    private IEnumerator AjustLeite(Transform other, Vector3 startPos, Vector3 finalPos)
    {
        float tempoDecorrido = 0f;
        other.GetComponent<Rigidbody>().isKinematic = true;
        // Captura a rotação inicial no começo da corrotina
        Quaternion startRotation = other.transform.rotation;
        // Define a rotação final (assumindo que é a rotação do pont1)
        Quaternion finalRotation = pont1.rotation;

        while (tempoDecorrido < durationAnimation)
        {
            //informações da curva de animação
            tempoDecorrido += Time.deltaTime;
            float porcentagemTempo = tempoDecorrido / durationAnimation;
            float curveValue = _animation.Evaluate(porcentagemTempo);

            // --- LÓGICA DE ROTAÇÃO CORRIGIDA ---
            // Usamos Quaternion.Slerp para uma interpolação de rotação suave
            // e aplicamos diretamente ao transform do objeto
            other.rotation = Quaternion.Slerp(startRotation, finalRotation, curveValue);


            //animação de posição (seu código já estava correto)
            other.position = Vector3.Lerp(startPos, finalPos, curveValue);

            // --- CORREÇÃO DE LÓGICA ---
            // O yield return null deve estar DENTRO do loop while
            yield return null;
        }

        // --- MELHORIA ---
        // Garante que o objeto termine exatamente na posição e rotação final
        other.position = finalPos;
        other.rotation = finalRotation;
        _evento.Invoke();
    }
}