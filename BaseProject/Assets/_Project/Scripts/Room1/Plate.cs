using UnityEngine;

public class Plate : MonoBehaviour
{
    [SerializeField] private Transform plate;
    [SerializeField] private GameObject door;
    [SerializeField] private Vector3 platePress = new Vector3(0f, 0.75f, 0f);
    private Vector3 initialPlatePosition;

    private bool isBoxOnPlate = false;

    void Start()
    {
        initialPlatePosition = plate.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            isBoxOnPlate = true;
            plate.position = platePress;
            CheckPlate();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            isBoxOnPlate = false;
            plate.position = initialPlatePosition;
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
