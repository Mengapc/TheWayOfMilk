using UnityEngine.SceneManagement;

public static class SceneLoader
{
    // A variável estática que "sobrevive" à troca de cenas
    public static string nextSceneName;

    /// <summary>
    /// O nome da sua cena de carregamento
    /// </summary>
    private const string LOADING_SCENE_NAME = "TelaCarregamento";

    /// <summary>
    /// Use esta função para carregar qualquer cena do seu jogo.
    /// </summary>
    /// <param name="sceneName">O nome da cena que você quer carregar (ex: "Puzzle_2" ou "MainMenu")</param>
    public static void LoadScene(string sceneName)
    {
        // 1. Armazena o nome da cena que queremos carregar
        nextSceneName = sceneName;

        // 2. Carrega a cena de carregamento
        SceneManager.LoadScene(LOADING_SCENE_NAME);
    }
}