using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class TubesPuzzleManager : MonoBehaviour
{
    [Tooltip("Refer�ncia para o controlador da porta que ser� aberta ao completar o puzzle")]
    [SerializeField] private DoorController doorController;

    [Header("Configura��es do Puzzle")]
    [Tooltip("Lista de tubos no puzzle e a quantidade de bolas necess�rias em cada um")]
    [SerializeField] private List<Tubes> tubes;
    [SerializeField] private List<GameObject> balls;

    private bool isPlayerNear = false;
    
    private BallController[] allBalls;

    // Fun��o para checar se o puzzle foi conclu�do
    public void CheckPuzzleCompletion()
    {
        // Percorre cada tubo na lista de configura��o do puzzle
        foreach (var tubeConfig in tubes)
        {
            if (tubeConfig.tube != null)
            {
                TubeController tubeController = tubeConfig.tube.GetComponent<TubeController>();

                // Se algum tubo n�o tiver o n�mero correto de bolas, o puzzle n�o est� resolvido
                if (tubeController == null || tubeController.currentBallCount != tubeConfig.valor)
                {
                    Debug.Log("Puzzle ainda n�o resolvido.");
                    return; // Sai da fun��o mais cedo
                }
            }
        }

        // Se o loop terminar, significa que todos os tubos est�o corretos
        Debug.Log("Puzzle Resolvido! Abrindo a porta.");
        if (doorController != null)
        {
            doorController.OpenDoor(); // Supondo que exista um m�todo OpenDoor()
        }
    }

    //Fun��es para o reset do puzzle
    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed && isPlayerNear)
        {
            Debug.Log("Bot�o de reset pressionado!");

            // 1. Resetar a posi��o de todas as bolas
            foreach (var ballObject in balls)
            {
                if (ballObject != null)
                {
                    BallController ballController = ballObject.GetComponent<BallController>();
                    if (ballController != null)
                    {
                        ballController.ResetPosition();
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

            // 3. Opcional, mas recomendado: verificar o estado do puzzle ap�s o reset
            CheckPuzzleCompletion();
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