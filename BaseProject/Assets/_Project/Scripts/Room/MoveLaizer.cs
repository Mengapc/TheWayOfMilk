using UnityEngine;
using System.Collections;

// O nome do arquivo DEVE ser MoveLaizer.cs para corresponder ao nome da classe
public class MoveLaizer : MonoBehaviour
{
    [Header("Componentes")]
    [Tooltip("O obst�culo que se move.")]
    [SerializeField] private Transform taizer;
    [Tooltip("O ponto superior para onde o obst�culo se move.")]
    [SerializeField] private Transform pontoSuperior;
    [Tooltip("O ponto inferior para onde o obst�culo se move.")]
    [SerializeField] private Transform pontoInferior;
    [Space]
    [Header("V�riaveis de Anima��o")]
    [Tooltip("Curva que define a acelera��o e desacelera��o do movimento.")]
    [SerializeField] private AnimationCurve movementAnimation;
    [Tooltip("Tempo em segundos para completar um trecho do movimento (ida ou volta).")]
    [Range(1f, 10f)]
    [SerializeField] private float durationAnimation = 2f;

    private void Start()
    {
        StartCoroutine(MoveBackAndForth());
    }


    private IEnumerator MoveBackAndForth()
    {
        while (true)
        {
            yield return StartCoroutine(MoveToPosition(pontoInferior.position, pontoSuperior.position));

            yield return StartCoroutine(MoveToPosition(pontoSuperior.position, pontoInferior.position));
        }
    }


    private IEnumerator MoveToPosition(Vector3 startPos, Vector3 finalPos)
    {
        float tempoDecorrido = 0f;
        while (tempoDecorrido < durationAnimation)
        {
            tempoDecorrido += Time.deltaTime;
            float porcentagemTempo = tempoDecorrido / durationAnimation;

            float porcentagemDistancia = movementAnimation.Evaluate(porcentagemTempo);

            taizer.position = Vector3.Lerp(startPos, finalPos, porcentagemDistancia);

            yield return null;
        }

        taizer.position = finalPos;
    }
}