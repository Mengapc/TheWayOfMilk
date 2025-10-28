using System.Collections;
using UnityEngine;
using UnityEngine.Events; // Adicionei este using, pois você tem um UnityEvent

public class Activate : MonoBehaviour
{
    [Header("Componentes")]
    [SerializeField] private UnityEvent _evento;
    [SerializeField] private Transform pont1;
    [SerializeField] private Transform pont2;

    [Header("Animação")]
    [Tooltip("Curva de animação para o movimento do objeto.")]
    [SerializeField] private AnimationCurve _animation1;
    [Tooltip("Duração da animação do objeto.")]
    [SerializeField] private float durationAnimation1 = 2f;
    [Tooltip("Curva de animação para o movimento do objeto na segunda fase.")]
    [SerializeField] private AnimationCurve _animation2;
    [Tooltip("Duração da animação do objeto na segunda fase.")]
    [SerializeField] private float durationAnimation2 = 2f;
    [Tooltip("Velocidade de rotação constante do objeto.")]
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
        // Captura a rotação inicial no começo da corrotina
        Quaternion startRotation = other.transform.rotation;
        // Define a rotação final (assumindo que é a rotação do pont1)
        Quaternion finalRotation = pont1.rotation;

        while (tempoDecorrido < durationAnimation1)
        {
            //informações da curva de animação
            tempoDecorrido += Time.deltaTime;
            float porcentagemTempo = tempoDecorrido / durationAnimation1;
            float curveValue = _animation1.Evaluate(porcentagemTempo);

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
        yield return StartCoroutine(PosicaoFinal(other.transform, other.transform.position, pont2.position));
    }

    private IEnumerator PosicaoFinal(Transform other, Vector3 startPos, Vector3 finalPos)
    {
        float tempoDecorrido = 0f;
        other.GetComponent<Rigidbody>().isKinematic = true;
        // Captura a rotação inicial no começo da corrotina
        Quaternion startRotation = other.transform.rotation;
        // Define a rotação final (assumindo que é a rotação do pont1)
        Quaternion finalRotation = pont1.rotation;

        while (tempoDecorrido < durationAnimation2)
        {
            //informações da curva de animação
            tempoDecorrido += Time.deltaTime;
            float porcentagemTempo = tempoDecorrido / durationAnimation2;
            float curveValue = _animation2.Evaluate(porcentagemTempo);

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