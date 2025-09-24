using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Elevator : MonoBehaviour
{
    [Header("Componentes")]
    [Tooltip("A cabine do elevador que se move entre os andares.")]
    [SerializeField] private Transform cabine;
    [Tooltip("O ponto do primeiro andar onde a cabine deve parar.")]
    [SerializeField] private Transform pontoPrimeiroAndar;
    [Tooltip("O ponto do segundo andar onde a cabine deve parar.")]
    [SerializeField] private Transform pontoSegundoAndar;
    [Tooltip("Transform do jogador.")]
    [SerializeField] private Transform player;
     [Space]
    [Header("Váriaveis")]
    [Tooltip("Curva de movimento do elevador")]
    [SerializeField] private AnimationCurve movementAanimation;
    [Tooltip("Tempo de animação")]
    [Range(1f, 5f)] 
    [SerializeField] private float durationAnimation = 2f;
    [Range(0f, 1f)]
    [SerializeField] private float porcentagemDistancia; 

    private bool movendo = false;
    private bool primeiroAndar = true;
    public bool colliderPlayer = false;

    private void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame && !movendo && colliderPlayer)
        {
            ElevatorActivation();
        }
    }

    private void ElevatorActivation()
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


    private IEnumerator MoverCabine(Vector3 startPos, Vector3 finalPos)
    {
        movendo = true;
        float tempoDecorrido = 0f;
        cabine.SetParent(player.transform,true);

        while (tempoDecorrido < durationAnimation)
        {
            tempoDecorrido += Time.deltaTime;
            float porcentagemTempo = tempoDecorrido / durationAnimation;

            porcentagemDistancia = movementAanimation.Evaluate(porcentagemTempo);

            cabine.position = Vector3.Lerp(startPos, finalPos, porcentagemDistancia);

            yield return null; 
        }

        cabine.position = finalPos;
        cabine.SetParent(player.transform, false);
        ChangeFloor(); 
        movendo = false;
    }


    private void ChangeFloor()
    {
        // Simplesmente inverte o valor booleano
        primeiroAndar = !primeiroAndar;
    }
}