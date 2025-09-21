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

}

