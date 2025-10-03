using System.Runtime.CompilerServices;
using UnityEngine;

public class PlateScale : MonoBehaviour
{
    private ScaleManager scaleManager;
    private BallController ballController;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            scaleManager
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            
        }
    }

}
