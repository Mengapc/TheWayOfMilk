using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class TubesPuzzleManager : MonoBehaviour
{
    [Tooltip("Referência para o controlador da porta que será aberta ao completar o puzzle")]
    [SerializeField] private DoorController doorController;

    [Header("Configurações do Puzzle")]
    [Tooltip("Lista de tubos no puzzle e a quantidade de bolas necessárias em cada um")]
    [SerializeField] private List<Tubes> tubes;
    [SerializeField] private List<GameObject> balls;
    [SerializeField] private BallController bc;
    [SerializeField] private ColliderRespanw colliderRespawn;
    [SerializeField] private Transform respawnPoint;
    [SerializeField] private float resetDelay = 2f;

    [SerializeField] private bool isPlayerNear = false;

    private void Update()
    {
        if (colliderRespawn != null  && !colliderRespawn.isBall)
        {
            StartCoroutine(ResetBall(resetDelay));
        }
    }

    // Função para checar se o puzzle foi concluído
    public void CheckPuzzleCompletion()
    {
        // Percorre cada tubo na lista de configuração do puzzle
        foreach (var tubeConfig in tubes)
        {
            if (tubeConfig.tube != null)
            {
                TubeController tubeController = tubeConfig.tube.GetComponent<TubeController>();

                // Se algum tubo não tiver o número correto de bolas, o puzzle não está resolvido
                if (tubeController == null || tubeController.currentBallCount != tubeConfig.valor)
                {
                    Debug.Log("Puzzle ainda não resolvido.");
                    return; // Sai da função mais cedo
                }
            }
        }

        // Se o loop terminar, significa que todos os tubos estão corretos
        Debug.Log("Puzzle Resolvido! Abrindo a porta.");
        if (doorController != null)
        {
            doorController.OpenDoor(); // Supondo que exista um método OpenDoor()
        }
    }

    //Funções para o reset do puzzle
    public void ResetPuzzle(InputAction.CallbackContext context)
    {
        if (context.performed && isPlayerNear)
        {
            Debug.Log("Botão de reset pressionado!");

            // 1. Resetar a posição de todas as bolas
            foreach (var ballObject in balls)
            {
                if (ballObject != null)
                {
                    BallController ballController = ballObject.GetComponent<BallController>();
                    if (ballController != null)
                    {
                        ballController.Destroy();
                    }
                }
                
            }

            // 2. Resetar a contagem de bolas em cada tubo (PASSO ADICIONADO)
            foreach (var tubeConfig in tubes)
            {
                if (tubeConfig.tube != null)
                {
                    TubeController tubeController = tubeConfig.tube.GetComponent<TubeController>();
                    if (tubeController != null)
                    {
                        tubeController.ResetBallCount();
                    }
                }
            }

            // 3. Opcional, mas recomendado: verificar o estado do puzzle após o reset
            CheckPuzzleCompletion();
            Instantiate(bc.ballPrefab, bc.spawnPoint.position, bc.spawnPoint.rotation);
        }
    }

    private IEnumerator ResetBall(float delay)
    {
        yield return new WaitForSeconds(delay);
        GameObject addGameObject = Instantiate(bc.ballPrefab, respawnPoint.position, respawnPoint.rotation);
        balls.Add(addGameObject);
        yield return new WaitForSeconds(delay);
        if (!colliderRespawn.isBall)
        {
            StartCoroutine(ResetBall(delay));
            yield break;
        }
        else
        {
            yield break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;
        }
    }
}