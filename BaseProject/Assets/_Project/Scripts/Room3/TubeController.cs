using UnityEngine;

public class TubeController : MonoBehaviour // Remova a herança de PuzzleManager
{
    [Tooltip("Referência para o PuzzleManager da cena")]
    public TubesPuzzleManager tubesPuzzle;

    // Variável para contar as bolas dentro do tubo
    public int currentBallCount = 0;

    private void OnTriggerEnter(Collider other)
    {
        // Verifica se o objeto que entrou tem a tag "Ball"
        if (other.CompareTag("Ball"))
        {

            currentBallCount++;
            Debug.Log("Bola entrou no tubo ,Total: " + currentBallCount);

            // Notifica o PuzzleManager para checar o estado do puzzle
            if (tubesPuzzle != null)
            {
                tubesPuzzle.CheckPuzzleCompletion();
            }
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Verifica se o objeto que saiu tem a tag "Ball"
        if (other.CompareTag("Ball"))
        {

            currentBallCount--;
            Debug.Log("Bola saiu do tubo ,Total: " + currentBallCount);

            // Garante que a contagem não seja negativa
            if (currentBallCount < 0)
            {
                currentBallCount = 0;
            }

            // Notifica o PuzzleManager para checar o estado do puzzle
            if (tubesPuzzle != null)
            {
                tubesPuzzle.CheckPuzzleCompletion();
            }
            
        }
    }

    // Reseta o estado do tubo para que possa aceitar uma nova bola
    public void ResetBallCount()
    {
        currentBallCount = 0;
        Debug.Log("Contagem do tubo resetada.");
    }
}