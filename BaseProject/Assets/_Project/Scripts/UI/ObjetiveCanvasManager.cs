using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using TMPro;

// 1. Crie uma classe pequena para guardar os dados de CADA cena
[System.Serializable] // Isso faz ela aparecer no Inspector
public class SceneObjectives
{
    [Tooltip("O nome exato do arquivo da cena")]
    public string sceneName;

    [Tooltip("A lista de objetivos para esta cena")]
    [TextArea(3, 10)]
    public List<string> objectives;
}

public class ObjetiveCanvasManager : MonoBehaviour
{
    [Header("UI Elements")]
    [Tooltip("Lista dos campos de texto da UI (ex: objetivo_1, objetivo_2, ...).")]
    [SerializeField] private List<TextMeshProUGUI> areaInfo;

    [Header("Configuração dos Objetivos")]
    [Tooltip("Configure os objetivos para cada cena aqui.")]
    [SerializeField] private List<SceneObjectives> allSceneObjectives;

    private void Start()
    {
        // 1. Pega o nome da cena atual (COM A CORREÇÃO)
        string nomeDaCenaAtual = SceneManager.GetActiveScene().name;

        // 2. Encontra os objetivos para a cena atual
        List<string> listaDeObjetivosDaCenaAtual = null;

        // Loop mais limpo: Procura na lista de "allSceneObjectives"
        foreach (SceneObjectives sceneData in allSceneObjectives)
        {
            if (sceneData.sceneName == nomeDaCenaAtual)
            {
                listaDeObjetivosDaCenaAtual = sceneData.objectives;
                break; // Encontrou, pode parar o loop
            }
        }

        // 3. Atualiza os textos na UI
        UpdateObjectiveText(listaDeObjetivosDaCenaAtual);
    }

    /// <summary>
    /// Preenche os campos de texto da UI com a lista de objetivos fornecida.
    /// (Esta função é a mesma que você já tinha)
    /// </summary>
    private void UpdateObjectiveText(List<string> objetivos)
    {
        // Limpa todos os campos
        foreach (TextMeshProUGUI campoTexto in areaInfo)
        {
            if (campoTexto != null)
            {
                campoTexto.text = "";
            }
        }

        // Se não tiver objetivos, sai
        if (objetivos == null || objetivos.Count == 0)
        {
            if (areaInfo.Count > 0 && areaInfo[0] != null)
            {
                areaInfo[0].text = "";
            }
            return;
        }

        // Preenche os campos
        for (int i = 0; i < areaInfo.Count; i++)
        {
            if (areaInfo[i] == null)
            {
                continue;
            }

            if (i < objetivos.Count)
            {
                areaInfo[i].text = objetivos[i];
            }
            else
            {
                areaInfo[i].text = "";
            }
        }
    }
}
