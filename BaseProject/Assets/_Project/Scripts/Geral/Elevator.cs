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
    [SerializeField] private AnimationCurve movementAanimation;
    [Tooltip("Tempo de animação")]
    [Range(1f, 3f)]
    [SerializeField] private float durationAnimation;
    [Space]
    [Range(0f,1f)]
    [SerializeField] private float porcentagemDistancia;
    //[SerializeField] private GameObject painel;

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
        distance = Vector3.Distance(pontoPrimeiroAndar.position, pontoSegundoAndar.position);
    }
    private void Update()
    {
        cabine.transform.position = Vector3.Lerp(pontoPrimeiroAndar.position, pontoSegundoAndar.position, porcentagemDistancia);
    }

    private IEnumerator IniciarPercurso(Vector3 startPos, Vector3 endPos)
    {


        yield return null;
    }
}