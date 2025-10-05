using System.Runtime.CompilerServices;
using UnityEngine;

public class PlateScale : MonoBehaviour
{
    private ScaleManager scaleManager;
    private BallController ballController;
    [SerializeField] private float weightBall;
    public float WeightBall { get { return weightBall; } }

    private void Start()
    {
        scaleManager = GetComponentInParent<ScaleManager>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            object ball = other.GetComponent<BallController>();
            ballController = (BallController)ball;
            weightBall =+ ballController.weight;
            if (ballController.resetavel)
            {
                scaleManager.objectsOnPlate.Add(other.gameObject);
            }
            ballController = null;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            object ball = other.GetComponent<BallController>();
            ballController = (BallController)ball;
            weightBall = weightBall - ballController.weight;
            ballController = null;
        }
    }

}
