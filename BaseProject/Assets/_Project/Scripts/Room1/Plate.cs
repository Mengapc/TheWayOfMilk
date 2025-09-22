using UnityEngine;

public class Plate : MonoBehaviour
{
    [SerializeField] private Transform box;
    [SerializeField] private GameObject door;

    private bool isBoxOnPlate = false;
     
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform == box)
        {
            isBoxOnPlate = true;
            CheckPlate();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.transform == box)
        {
            isBoxOnPlate = false;
            CheckPlate();
        }
    }
    private void CheckPlate()
    {
        if (isBoxOnPlate)
        {
            door.SetActive(false); // Abre a porta
            Debug.Log("Porta aberta!");
        }
        else
        {
            door.SetActive(true); // Fecha a porta
            Debug.Log("Porta fechada!");
        }
    }
}
