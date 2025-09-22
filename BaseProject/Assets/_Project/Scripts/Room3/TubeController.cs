using UnityEngine;

public class TubeController : MonoBehaviour // Remova a heran�a de PuzzleManager
{
    [Tooltip("Refer�ncia para o PuzzleManager da cena")]
    public TubesPuzzleManager tubesPuzzle;

    // Vari�vel para contar as bolas dentro do tubo
    public int currentBallCount = 0;

    private void OnTriggerEnter(Collider other)
    {
        // Verifica se o objeto que entrou tem a tag "Ball"
        if (other.CompareTag("Ball"))
        {
            // Opcional: Se voc� tiver um BallController, pode verificar o tipo da bola
            // BallController ball = other.GetComponent<BallController>();
            // if (ball != null && ball.ballType == this.tubeType) {

            currentBallCount++;
            Debug.Log("Bola entrou no tubo ,Total: " + currentBallCount);

            // Notifica o PuzzleManager para checar o estado do puzzle
            if (tubesPuzzle != null)
            {
                tubesPuzzle.CheckPuzzleCompletion();
            }
            // }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Verifica se o objeto que saiu tem a tag "Ball"
        if (other.CompareTag("Ball"))
        {
            // Opcional: Se voc� tiver um BallController, pode verificar o tipo da bola
            // BallController ball = other.GetComponent<BallController>();
            // if (ball != null && ball.ballType == this.tubeType) {

            currentBallCount--;
            Debug.Log("Bola saiu do tubo ,Total: " + currentBallCount);

            // Garante que a contagem n�o seja negativa
            if (currentBallCount < 0)
            {
                currentBallCount = 0;
            }

            // Notifica o PuzzleManager para checar o estado do puzzle
            if (tubesPuzzle != null)
            {
                tubesPuzzle.CheckPuzzleCompletion();
            }
            // }
        }
    }

    // Reseta o estado do tubo para que possa aceitar uma nova bola
    public void ResetBallCount()
    {
        currentBallCount = 0;
        Debug.Log("Contagem do tubo resetada.");
    }
}