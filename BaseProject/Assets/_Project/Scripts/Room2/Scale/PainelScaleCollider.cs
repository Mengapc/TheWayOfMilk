using UnityEngine;

public class PainelScaleCollider : MonoBehaviour
{
    [SerializeField] private MovementScale movementScale;
    private bool isColliding = false;
    public bool IsColliding => isColliding;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isColliding = true;
            movementScale.enabled = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isColliding = false;
            movementScale.enabled = false;
        }
    }


}
