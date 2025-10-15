using Unity.VisualScripting;
using UnityEngine;

public class ColliderArmadilha : MonoBehaviour
{
    [SerializeField] private Transform spaanwPoint;
    [SerializeField] private GameObject ballPrefeb;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            Destroy(ballPrefeb);
            Instantiate(ballPrefeb,spaanwPoint.position, spaanwPoint.rotation);
        }
    }
}
