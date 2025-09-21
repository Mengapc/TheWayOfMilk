using UnityEngine;

public class TubeController : PuzzleManager
{
    private PuzzleManager puzzleManager;
    private GameObject currentBall = null;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            
        }
    }

    // Reseta o estado do tubo para que possa aceitar uma nova bola
    public void ResetTube()
    {
        currentBall = null;
    }
}

