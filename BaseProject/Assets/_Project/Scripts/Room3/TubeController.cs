using UnityEngine;

public class TubeController : MonoBehaviour
{
    [Tooltip("O ID que este tubo representa (ex: 5, 6 ou 2)")]
    [SerializeField] private int tubeValue;

    public static PuzzleManager puzzleManager;

    private bool isFilled = false;
    private GameObject currentBall = null;

    private void OnTriggerEnter(Collider other)
    {
        if (!isFilled && other.CompareTag("Ball"))
        {
            Debug.Log("add bola");
            isFilled = true;
            currentBall = other.gameObject;

            Rigidbody ballRb = currentBall.GetComponent<Rigidbody>();
            if (ballRb != null)
            {
                ballRb.isKinematic = true;
            }

            currentBall.transform.position = transform.position;

            if (puzzleManager != null)
            {
                puzzleManager.TubeFilled(tubeValue);
            }

            Debug.Log("Tubo " + tubeValue + " foi preenchido!");
        }
    }

    // Reseta o estado do tubo para que possa aceitar uma nova bola
    public void ResetTube()
    {
        isFilled = false;
        currentBall = null;
    }
}

