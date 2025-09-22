using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField] private GameObject door; // Arraste o objeto da porta aqui no Inspector
    private bool isOpen = false;


    // M�todo p�blico que ser� chamado pelo script da UI de senha para iniciar a abertura
    public void OpenDoor()
    {
        if (!isOpen)
        {
            isOpen = true;
            door.SetActive(false); // Desativa a porta para "abrir"
        }
    }
}
