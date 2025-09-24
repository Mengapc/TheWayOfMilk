using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem; 

public class MovimentoSuave : MonoBehaviour
{
    [Header("Componentes")]
    [Tooltip("A cabine do elevador que se move entre os andares.")]
    [SerializeField] private Transform cabine;
    [Tooltip("O ponto do primeiro andar onde a cabine deve parar.")]
    [SerializeField] private Transform pontoPrimeiroAndar;
    [Tooltip("O ponto do segundo andar onde a cabine deve parar.")]
    [SerializeField] private Transform pontoSegundoAndar;
    [Tooltip("Curva de movimento do elevador")]
    [SerializeField] private AnimationCurve movementAanimation;
    [Tooltip("Tempo de animação")]
    [Range(1f, 5f)] 
    [SerializeField] private float durationAnimation = 2f;
    [Space]
    [Range(0f, 1f)]
    [SerializeField] private float porcentagemDistancia; 

    private Movement playerMovement;
    private Transform playerTransform;

    private bool movendo = false;
    private bool primeiroAndar = true;

    private void Update()
    {
        ElevatorActivation();
    }

    private void ElevatorActivation()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame && !movendo)
        {
            if (primeiroAndar)
            {
                StartCoroutine(MoverCabine(pontoPrimeiroAndar.position, pontoSegundoAndar.position));
            }
            else
            {
                StartCoroutine(MoverCabine(pontoSegundoAndar.position, pontoPrimeiroAndar.position));
            }
        }
    }


    private IEnumerator MoverCabine(Vector3 startPos, Vector3 finalPos)
    {
        movendo = true;
        float tempoDecorrido = 0f;

        while (tempoDecorrido < durationAnimation)
        {
            tempoDecorrido += Time.deltaTime;
            float porcentagemTempo = tempoDecorrido / durationAnimation;

            porcentagemDistancia = movementAanimation.Evaluate(porcentagemTempo);

            cabine.position = Vector3.Lerp(startPos, finalPos, porcentagemDistancia);

            yield return null; 
        }

        cabine.position = finalPos;

        ChangeFloor(); 
        movendo = false;
    }


    private void ChangeFloor()
    {
        // Simplesmente inverte o valor booleano
        primeiroAndar = !primeiroAndar;
    }
}

