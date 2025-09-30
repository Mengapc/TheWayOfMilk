using Unity.VisualScripting;
using UnityEngine;

public class ElavatorCollider : MonoBehaviour
{
    [Header("Componentes.")]
    [Tooltip("Prefab do player.")]
    [SerializeField] private GameObject player;
    [Tooltip("Prefab do elevador.")]
    [SerializeField] private Elevator elevador;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            elevador.colliderPlayer = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            elevador.colliderPlayer = false;
        }
    }
}
