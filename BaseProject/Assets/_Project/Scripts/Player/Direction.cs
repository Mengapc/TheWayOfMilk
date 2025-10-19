using UnityEngine;

public class Direction : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] public Vector3 directionVector;
    [SerializeField] private float distance;
    [SerializeField] private Camera mainCamera;

    private void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
    }

    private void Update()
    {
        Plane playerPlane = new Plane(Vector3.up, player.position);
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (playerPlane.Raycast(ray, out float enter))
        {
            Vector3 hitPoint = ray.GetPoint(enter);
            directionVector = hitPoint - player.position;
            directionVector.y = 0;
            distance = directionVector.magnitude;
            directionVector.Normalize();
            Debug.DrawRay(player.position, directionVector * distance, Color.blue);
        }
    }

}
