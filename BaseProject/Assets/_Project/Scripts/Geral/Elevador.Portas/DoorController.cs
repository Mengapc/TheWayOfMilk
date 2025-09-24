using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField] private GameObject door; 
    private bool isOpen = false;

    public void OpenDoor()
    {
        if (!isOpen)
        {
            isOpen = true;
            door.SetActive(false); 
        }
    }
}
