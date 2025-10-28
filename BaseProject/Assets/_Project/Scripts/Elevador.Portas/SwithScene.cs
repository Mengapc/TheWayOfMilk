using UnityEditor;
using UnityEngine;

public class SwithScene : MonoBehaviour
{
    [SerializeField] private SceneAsset sceneAsset;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (sceneAsset != null)
            {
                string sceneName = sceneAsset.name;
                UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
            }
            else
            {
                Debug.LogWarning("Nenhuma cena atribuída ao SwithScene.");
            }
        }
    }
}
