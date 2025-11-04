using Unity.VisualScripting;
using UnityEngine;

public class ElavatorCollider : MonoBehaviour
{
    [Header("Componentes.")]
    // REMOVIDO: [SerializeField] private GameObject player; (Vamos pegar pelo trigger)

    [Tooltip("Referência ao script principal do elevador (este trigger pertence a ele).")]
    [SerializeField] private Elevator elevador; // Continua necessário

    private void OnTriggerEnter(Collider other)
    {
        // Verifica se quem entrou é o Player
        if (other.CompareTag("Player"))
        {
            // Tenta pegar o script de interação no Player
            Movement playerInteraction = other.GetComponent<Movement>();
            if (playerInteraction != null)
            {
                // Informa ao Player qual elevador ele pode usar
                playerInteraction.SetCurrentElevator(elevador);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Verifica se quem saiu é o Player
        if (other.CompareTag("Player"))
        {
            // Tenta pegar o script de interação no Player
            Movement playerInteraction = other.GetComponent<Movement>();
            if (playerInteraction != null)
            {
                // Limpa a referência do elevador, pois o Player saiu
                playerInteraction.ClearCurrentElevator();
            }
        }
    }
}
