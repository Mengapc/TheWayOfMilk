using UnityEngine;
using UnityEngine.SceneManagement; // <-- Adicionado
using System.Collections.Generic; // <-- Adicionado
using TMPro; // <-- Adicionado

public class CanvasManager : MonoBehaviour
{
    [Header("UI Elements")]
    [Tooltip("O campo de texto (TMP) para exibir as informa��es.")]
    [SerializeField] private TextMeshProUGUI objectiveText;

    [Header("Scene Information")]
    [Tooltip("Lista dos NOMES das cenas.")]
    [SerializeField] private List<string> sceneNames;

    [Tooltip("Lista de informa��es. DEVE ter a mesma ordem e tamanho da lista de nomes de cenas.")]
    [TextArea(3, 10)]
    [SerializeField] private List<string> detalhesScene1;
    [TextArea(3, 10)]
    [SerializeField] private List<string> detalhesScene2;
    [TextArea(3, 10)]
    [SerializeField] private List<string> detalhesScene3;

    // --- MUDAN�AS ---
    // Vari�veis para rastrear o �ndice atual de cada lista
    private int indiceDetalhes1 = 0;
    private int indiceDetalhes2 = 0;
    private int indiceDetalhes3 = 0;

    private void Start()
    {
        // Reseta todos os contadores e exibe o primeiro texto
        indiceDetalhes1 = 0;
        indiceDetalhes2 = 0;
        indiceDetalhes3 = 0;

        // Chama a fun��o para exibir o texto inicial (�ndice 0)
        UpdateObjectiveText(true); // 'true' para indicar que � o reset
    }

    // Fun��o agora � p�blica e pode ser chamada por um bot�o, etc.
    public void DisplayNextDetail()
    {
        // Chama a fun��o para avan�ar o texto
        UpdateObjectiveText(false); // 'false' para indicar que � para avan�ar
    }

    // Fun��o principal foi atualizada
    private void UpdateObjectiveText(bool resetAndShowFirst)
    {
        if (objectiveText == null) return; // Seguran�a

        string sceneName = SceneManager.GetActiveScene().name;

        switch (sceneName)
        {
            // --- CENA 1 ---
            case var name when sceneNames.Count > 0 && name == sceneNames[0]:
                if (resetAndShowFirst)
                {
                    indiceDetalhes1 = 0; // Reseta
                }
                else
                {
                    indiceDetalhes1++; // Avan�a
                }

                if (detalhesScene1.Count > 0)
                {
                    // Garante que o �ndice n�o saia do limite
                    if (indiceDetalhes1 >= detalhesScene1.Count)
                    {
                        indiceDetalhes1 = detalhesScene1.Count - 1;
                    }
                    objectiveText.text = detalhesScene1[indiceDetalhes1];
                }
                else
                {
                    objectiveText.text = "Detalhes n�o dispon�veis.";
                }
                break;

            // --- CENA 2 ---
            case var name when sceneNames.Count > 1 && name == sceneNames[1]:
                if (resetAndShowFirst)
                {
                    indiceDetalhes2 = 0; // Reseta
                }
                else
                {
                    indiceDetalhes2++; // Avan�a
                }

                if (detalhesScene2.Count > 0)
                {
                    if (indiceDetalhes2 >= detalhesScene2.Count)
                    {
                        indiceDetalhes2 = detalhesScene2.Count - 1;
                    }
                    objectiveText.text = detalhesScene2[indiceDetalhes2];
                }
                else
                {
                    objectiveText.text = "Detalhes n�o dispon�veis.";
                }
                break;

            // --- CENA 3 ---
            case var name when sceneNames.Count > 2 && name == sceneNames[2]:
                if (resetAndShowFirst)
                {
                    indiceDetalhes3 = 0; // Reseta
                }
                else
                {
                    indiceDetalhes3++; // Avan�a
                }

                if (detalhesScene3.Count > 0)
                {
                    if (indiceDetalhes3 >= detalhesScene3.Count)
                    {
                        indiceDetalhes3 = detalhesScene3.Count - 1;
                    }
                    objectiveText.text = detalhesScene3[indiceDetalhes3];
                }
                else
                {
                    objectiveText.text = "Detalhes n�o dispon�veis.";
                }
                break;

            // --- DEFAULT ---
            default:
                objectiveText.text = "Cena desconhecida. Detalhes n�o dispon�veis.";
                break;
        }
    }
}

