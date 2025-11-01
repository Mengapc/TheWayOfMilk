using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using TMPro;

public class ObjetiveCanvasManager : MonoBehaviour
{
    [Header("Scene Information")]
    [Tooltip("Lista dos NOMES das cenas. Deve ter a mesma ordem das listas de objetivos.")]
    [SerializeField] private List<string> sceneNames;

    [Header("UI Elements")]
    [Tooltip("Lista dos campos de texto da UI (ex: objetivo_1, objetivo_2, ...).")]
    [SerializeField] private List<TextMeshProUGUI> areaInfo;

    [Header("Lista de Objetivos por Cena")]
    [Tooltip("Lista de objetivos para a Cena 1 (corresponde ao sceneNames[0]).")]
    [TextArea(3, 10)]
    [SerializeField] private List<string> objetivosScene1;

    [Tooltip("Lista de objetivos para a Cena 2 (corresponde ao sceneNames[1]).")]
    [TextArea(3, 10)]
    [SerializeField] private List<string> objetivosScene2;

    [Tooltip("Lista de objetivos para a Cena 3 (corresponde ao sceneNames[2]).")]
    [TextArea(3, 10)]
    [SerializeField] private List<string> objetivosScene3;

    private void Start()
    {
        // 1. Pega o nome da cena atual
        string nomeDaCenaAtual = SceneManager.GetActiveScene().name;

        // 2. Cria uma lista temporária para guardar os objetivos corretos
        List<string> listaDeObjetivosDaCenaAtual = null;

        // 3. Descobre qual lista de objetivos usar com base no nome da cena
        // Esta parte assume que a ordem em 'sceneNames' é a mesma das listas de objetivos
        if (nomeDaCenaAtual == sceneNames[0])
        {
            listaDeObjetivosDaCenaAtual = objetivosScene1;
        }
        else if (nomeDaCenaAtual == sceneNames[1])
        {
            listaDeObjetivosDaCenaAtual = objetivosScene2;
        }
        else if (nomeDaCenaAtual == sceneNames[2])
        {
            listaDeObjetivosDaCenaAtual = objetivosScene3;
        }

        // 4. Atualiza os textos na UI
        UpdateObjectiveText(listaDeObjetivosDaCenaAtual);
    }

    /// <summary>
    /// Preenche os campos de texto da UI com a lista de objetivos fornecida.
    /// </summary>
    private void UpdateObjectiveText(List<string> objetivos)
    {
        // Primeiro, limpa todos os campos de texto da UI
        foreach (TextMeshProUGUI campoTexto in areaInfo)
        {
            if (campoTexto != null)
            {
                campoTexto.text = ""; // Limpa o texto
            }
        }

        // Verifica se temos objetivos para esta cena
        if (objetivos == null || objetivos.Count == 0)
        {
            // Se não houver objetivos, mostra uma mensagem no primeiro campo (opcional)
            if (areaInfo.Count > 0 && areaInfo[0] != null)
            {
                areaInfo[0].text = "";
            }
            return; // Sai da função
        }

        // 5. Preenche os campos de texto da UI com os objetivos
        // Itera pelo número de campos de texto que você tem na UI
        for (int i = 0; i < areaInfo.Count; i++)
        {
            // Verifica se o campo de texto atual existe
            if (areaInfo[i] == null)
            {
                continue; // Pula para o próximo se este campo for nulo
            }

            // Verifica se ainda existem objetivos na lista de objetivos
            if (i < objetivos.Count)
            {
                // Se sim, preenche o texto
                areaInfo[i].text = objetivos[i];
            }
            else
            {
                // Se não houver mais objetivos, deixa o campo de texto vazio
                areaInfo[i].text = "";
            }
        }
    }
}
