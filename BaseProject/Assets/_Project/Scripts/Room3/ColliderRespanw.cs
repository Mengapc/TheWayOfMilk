using UnityEngine;

public class ColliderRespanw : MonoBehaviour
{
    public bool isBall = false; // Define se o objeto que colidiu é uma bola

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            isBall = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            isBall = false;
        }
    }
}
