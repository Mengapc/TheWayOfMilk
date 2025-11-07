using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreenController : MonoBehaviour
{
    [Header("Opcional: Barra de Progresso")]
    [SerializeField]
    private Slider progressBar;

    [Header("Tempo de Carregamento (em segundos)")]
    [Tooltip("O tempo MÍNIMO que a tela ficará visível.")]
    [SerializeField] private float minLoadTime = 1.5f;

    [Tooltip("O tempo MÁXIMO que a tela ficará visível (para dar variação).")]
    [SerializeField] private float maxLoadTime = 3.0f;

    void Start()
    {
        if (progressBar != null)
        {
            progressBar.value = 0;
        }

        StartCoroutine(LoadSceneAsync());
    }

    private IEnumerator LoadSceneAsync()
    {
        yield return null; // Espera um frame

        // Pega o nome da cena que o SceneLoader guardou
        string sceneToLoad = SceneLoader.nextSceneName;

        if (string.IsNullOrEmpty(sceneToLoad))
        {
            sceneToLoad = "Menu"; // Nome da sua cena de menu principal
            Debug.LogWarning("O nome da cena estava vazio! Voltando ao Menu.");
        }

        // 1. Inicia o carregamento em background
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneToLoad);
        asyncLoad.allowSceneActivation = false;

        // 2. Sorteia o tempo de espera dessa vez
        float randomLoadTime = Random.Range(minLoadTime, maxLoadTime);
        progressBar.maxValue = randomLoadTime;
        float timer = 0f;

        // O loop agora continua até a cena ser ativada
        while (asyncLoad.allowSceneActivation == false)
        {
            // Atualiza o timer
            timer += Time.deltaTime;

            // Atualiza a barra de progresso (baseado no carregamento REAL)
            float realProgress = randomLoadTime;
            if (progressBar != null)
            {
                progressBar.value = timer;
            }

            if (asyncLoad.progress >= 0.9f && timer >= randomLoadTime)
            {
                // Permite a cena ser ativada
                asyncLoad.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}