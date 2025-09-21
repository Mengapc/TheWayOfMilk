using System.Collections;
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
    [SerializeField] private GameObject painel;

    // A referência ao jogador será pega quando ele entrar no elevador
    private Movement playerMovement;
    private Transform playerTransform;

    private bool movendo = false;
    private bool playerDentro = false;
    private bool primeiroAndar = false;
    private bool segundoAndar = false;

    public void MoverElevador()
    {
        if (playerDentro && !movendo)
        {
            // Se está no primeiro andar, vai para o segundo
            if (!segundoAndar & primeiroAndar)
            {
                MoverParaAndar(pontoSegundoAndar);
            }
            // Se está no segundo andar, vai para o primeiro
            else if (segundoAndar & !primeiroAndar)
            {
                MoverParaAndar(pontoPrimeiroAndar);
            }
        }
    }
    // Função pública para iniciar o movimento do elevador
    public void MoverParaAndar(Transform destino)
    {
        if (!movendo)
        {
            StartCoroutine(MoverCabine(destino));
        }
    }

    // Coroutine que executa o movimento suave da cabine
    private IEnumerator MoverCabine(Transform destino)
    {
        movendo = true;

        // Se o jogador estiver dentro, desativa seu movimento e o anexa ao elevador
        if (playerMovement != null)
        {
            playerMovement.ToggleMovement(false);
            playerTransform.SetParent(cabine);
        }

        // Move a cabine até chegar ao destino
        while (Vector3.Distance(cabine.position, destino.position) > 0.01f)
        {
            cabine.position = Vector3.MoveTowards(cabine.position, destino.position, velocidade * Time.deltaTime);
            yield return null; // Espera até o próximo frame
        }

        // Garante que a posição final seja exata
        cabine.position = destino.position;

        // Se o jogador estava dentro, reativa seu movimento e o desanexa do elevador
        if (playerMovement != null)
        {
            playerTransform.SetParent(null);
            playerMovement.ToggleMovement(true);
        }

        movendo = false;
    }

    // Detecta quando o jogador entra na área do elevador
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(!movendo)
            {
                painel.SetActive(true);
            }
            else
            {
                painel.SetActive(false);
            }
            playerDentro = true;
            playerMovement = other.GetComponent<Movement>();
            playerTransform = other.transform;
            Debug.Log("Jogador entrou no elevador. Pressione 'E' para usar.");
        }
    }

    // Detecta quando o jogador sai da área do elevador
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (painel.activeSelf)
            {
                painel.SetActive(false);
            }
            playerDentro = false;
            playerMovement = null;
            playerTransform = null;
        }
    }
}