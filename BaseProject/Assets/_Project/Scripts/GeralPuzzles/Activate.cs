using System.Collections;
using UnityEngine;
using UnityEngine.Events; // Adicionei este using, pois voc� tem um UnityEvent

public class Activate : MonoBehaviour
{
    [Header("Componentes")]
    [SerializeField] private UnityEvent _evento;
    [SerializeField] private Transform pont1;
    [SerializeField] private Transform pont2;

    [Header("Anima��o")]
    [Tooltip("Curva de anima��o para o movimento do objeto.")]
    [SerializeField] private AnimationCurve _animation1;
    [Tooltip("Dura��o da anima��o do objeto.")]
    [SerializeField] private float durationAnimation1 = 2f;
    [Tooltip("Curva de anima��o para o movimento do objeto na segunda fase.")]
    [SerializeField] private AnimationCurve _animation2;
    [Tooltip("Dura��o da anima��o do objeto na segunda fase.")]
    [SerializeField] private float durationAnimation2 = 2f;
    [Tooltip("Velocidade de rota��o constante do objeto.")]
    [SerializeField] private float rotationSpeed = 30f;

    private Transform objectTarget;

    



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

        while (tempoDecorrido < durationAnimation1)
        {
            //informa��es da curva de anima��o
            tempoDecorrido += Time.deltaTime;
            float porcentagemTempo = tempoDecorrido / durationAnimation1;
            float curveValue = _animation1.Evaluate(porcentagemTempo);

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
        yield return StartCoroutine(PosicaoFinal(other.transform, other.transform.position, pont2.position));
    }

    private IEnumerator PosicaoFinal(Transform other, Vector3 startPos, Vector3 finalPos)
    {
        float tempoDecorrido = 0f;
        other.GetComponent<Rigidbody>().isKinematic = true;
        // Captura a rota��o inicial no come�o da corrotina
        Quaternion startRotation = other.transform.rotation;
        // Define a rota��o final (assumindo que � a rota��o do pont1)
        Quaternion finalRotation = pont1.rotation;

        while (tempoDecorrido < durationAnimation2)
        {
            //informa��es da curva de anima��o
            tempoDecorrido += Time.deltaTime;
            float porcentagemTempo = tempoDecorrido / durationAnimation2;
            float curveValue = _animation2.Evaluate(porcentagemTempo);

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
        other.position = finalPos;
        other.rotation = finalRotation;
        _evento.Invoke();
        yield return StartCoroutine(ConstanteRotation(other));
    }
    
    private IEnumerator ConstanteRotation(Transform other)
    {
        while (true)
        {
            other.Rotate(new Vector3(0, rotationSpeed, 0) * Time.deltaTime);
            yield return null;
        }
    }
}