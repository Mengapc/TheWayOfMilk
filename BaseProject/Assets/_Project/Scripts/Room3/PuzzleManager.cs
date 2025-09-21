using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PuzzleManager : MonoBehaviour
{
    [Tooltip("Referência para o controlador da porta que será aberta ao completar o puzzle")]
    [SerializeField] private DoorController doorController; 

    [Header("Configurações do Puzzle")]
    [Tooltip("Lista de tubos no puzzle")]
    [SerializeField] private List<Tubes> tubes; 

    private bool isPlayerNear = false;



    //Funções para o reset do puzzle
    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed && isPlayerNear)
        {

        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;
        }
    }
}

