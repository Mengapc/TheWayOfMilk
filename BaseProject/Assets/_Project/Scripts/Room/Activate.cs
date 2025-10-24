using System.Collections;
using UnityEngine;
using UnityEngine.Events; // Adicionei este using, pois voc� tem um UnityEvent

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
        // Captura a rota��o inicial no come�o da corrotina
        Quaternion startRotation = other.transform.rotation;
        // Define a rota��o final (assumindo que � a rota��o do pont1)
        Quaternion finalRotation = pont1.rotation;

        while (tempoDecorrido < durationAnimation)
        {
            //informa��es da curva de anima��o
            tempoDecorrido += Time.deltaTime;
            float porcentagemTempo = tempoDecorrido / durationAnimation;
            float curveValue = _animation.Evaluate(porcentagemTempo);

            // --- L�GICA DE ROTA��O CORRIGIDA ---
            // Usamos Quaternion.Slerp para uma interpola��o de rota��o suave
            // e aplicamos diretamente ao transform do objeto
            other.rotation = Quaternion.Slerp(startRotation, finalRotation, curveValue);


            //anima��o de posi��o (seu c�digo j� estava correto)
            other.position = Vector3.Lerp(startPos, finalPos, curveValue);

            // --- CORRE��O DE L�GICA ---
            // O yield return null deve estar DENTRO do loop while
            yield return null;
        }

        // --- MELHORIA ---
        // Garante que o objeto termine exatamente na posi��o e rota��o final
        other.position = finalPos;
        other.rotation = finalRotation;
        _evento.Invoke();
    }
}