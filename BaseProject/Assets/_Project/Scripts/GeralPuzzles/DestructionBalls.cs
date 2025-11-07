using Unity.VisualScripting;
using UnityEngine;

public class DestructionBalls : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            other.GetComponent<BallController>().InstantiateEfect();
            Destroy(other.gameObject);
        }
    }
}