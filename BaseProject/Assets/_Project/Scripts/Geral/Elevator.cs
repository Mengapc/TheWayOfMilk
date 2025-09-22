using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem; // Adicionado para interagir com o elevador

public class Elevator : MonoBehaviour
{
    [Header("Componentes")]
    [Tooltip("A cabine do elevador que se move entre os andares.")]
    [SerializeField] private Transform cabine;
    [Tooltip("O ponto do primeiro andar onde a cabine deve parar.")]
    [SerializeField] private Transform pontoPrimeiroAndar;
    [Tooltip("O ponto do segundo andar onde a cabine deve parar.")]
    [SerializeField] private Transform pontoSegundoAndar;
    [Tooltip("Velocidade de movimento da cabine.")]
    [SerializeField] private float velocidade = 2f;
    [Tooltip("Curva de movimento do elevador")]
    [Range(0f, 1f)]
    [SerializeField] private AnimationCurve movementAanimation;
    [SerializeField] private GameObject painel;

    // A referência ao jogador será pega quando ele entrar no elevador
    private Movement playerMovement;
    private Transform playerTransform;
    private float distance;

    private bool movendo = false;
    private bool playerDentro = false;
    private bool primeiroAndar = false;
    private bool segundoAndar = false;

    private void Start()
    {
        playerMovement = GetComponent<Movement>();
        playerTransform = GetComponent<Transform>();
    }

}