using UnityEngine;
using UnityEngine.InputSystem;

public class ResetButton : MonoBehaviour
{
    [SerializeField] private PuzzleManager puzzleManager;
    private bool isPlayerNear = false;

    private void Awake()
    {
        // Tenta encontrar o PuzzleManager automaticamente se não for atribuído no Inspector
        if (puzzleManager == null)
        {
            puzzleManager = FindObjectOfType<PuzzleManager>();
        }
    }

    // Este método será chamado pela sua Ação de Input 'Interact'
    public void OnInteract(InputAction.CallbackContext context)
    {
        // Só executa se o botão for pressionado e o jogador estiver perto
        if (context.performed && isPlayerNear)
        {
            if (puzzleManager != null)
            {
                Debug.Log("Botão de reset ativado pelo jogador.");
                puzzleManager.ResetPuzzle();
            }
            else
            {
                Debug.LogError("A referência para o PuzzleManager não foi encontrada no ResetButton!");
            }
        }
    }

    // Detecta se o jogador entrou na área de interação
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = true;
        }
    }

    // Detecta se o jogador saiu da área de interação
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;
        }
    }
}

