using UnityEngine;

public class Reset : MonoBehaviour
{

    //reseta a cena atual
    public void ResetScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }
    //reseta o jogo
    public void ResetGame() 
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
    //reseta o leite
    public void RespanwLeite()
    {
        BallController respawnLeite = FindFirstObjectByType<BallController>();
        if (respawnLeite != null)
        {
            respawnLeite.Reset();
        }
    }
}
