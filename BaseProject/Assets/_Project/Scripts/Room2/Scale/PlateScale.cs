using System.Runtime.CompilerServices;
using UnityEngine;

public class PlateScale : MonoBehaviour
{
    private ScaleManager scaleManager;
    private BallController ballController;
    public float weightBall;

    private void Start()
    {
        scaleManager = GetComponentInParent<ScaleManager>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            ballController = other.GetComponent<BallController>();
            weightBall += ballController.weight;
            scaleManager.objectsOnPlate.Add(other.gameObject);
            ballController = null;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            ballController = other.GetComponent<BallController>();
            weightBall -= ballController.weight;
            ballController = null;
            scaleManager.objectsOnPlate.Remove(other.gameObject);
        }
    }
}
