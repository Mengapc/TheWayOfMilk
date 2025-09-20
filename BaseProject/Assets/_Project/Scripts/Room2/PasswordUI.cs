using UnityEngine;
using TMPro; // Necessário para os componentes de TextMeshPro
using System.Linq;

public class PasswordUI : MonoBehaviour
{
    [SerializeField] private string correctPassword = "1234"; // Defina a senha correta aqui
    [SerializeField] private TMP_InputField[] inputFields; // Arraste os 4 InputFields aqui, na ordem
    [SerializeField] private GameObject doorToOpen; // Arraste o objeto da porta aqui

    // A referência para o GameObject 'panel' foi removida por ser desnecessária

    private void OnEnable()
    {
        // Limpa os campos e foca no primeiro quando o painel é ativado
        ClearFields();
        if (inputFields.Length > 0 && inputFields[0] != null)
        {
            inputFields[0].Select();
            inputFields[0].ActivateInputField();
        }

        // Adiciona "ouvintes" de eventos para pular para o próximo campo automaticamente
        for (int i = 0; i < inputFields.Length; i++)
        {
            int index = i;
            inputFields[i].onValueChanged.AddListener(delegate { OnInputValueChanged(index); });
        }
    }

    private void OnDisable()
    {
        // É uma boa prática remover os "ouvintes" quando o objeto é desativado
        foreach (var field in inputFields)
        {
            if (field != null) field.onValueChanged.RemoveAllListeners();
        }
    }

    // Chamado toda vez que o valor de um campo de input muda
    private void OnInputValueChanged(int index)
    {
        // Se um caractere foi digitado, pula para o próximo campo
        if (inputFields[index].text.Length >= 1)
        {
            if (index + 1 < inputFields.Length)
            {
                inputFields[index + 1].Select();
                inputFields[index + 1].ActivateInputField();
            }
            else // Se for o último campo, verifica a senha
            {
                CheckPassword();
            }
        }
    }

    public void CheckPassword()
    {
        // Junta o texto de todos os campos para formar a senha digitada
        string enteredPassword = string.Concat(inputFields.Select(field => field.text));

        if (enteredPassword.ToUpper() == correctPassword.ToUpper())
        {
            Debug.Log("Senha correta! Abrindo a porta.");

            // Passo 1: Tentar abrir a porta (se ela existir e tiver o script)
            if (doorToOpen != null)
            {
                DoorController doorController = doorToOpen.GetComponent<DoorController>();
                if (doorController != null)
                {
                    doorController.OpenDoor();
                }
                else
                {
                    Debug.LogWarning("O GameObject da porta está atribuído, mas não tem o script 'DoorController'.");
                }
            }

            // Passo 2: Fechar a UI do painel. Esta ação agora é independente da porta.
            PanelController panelController = GetComponent<PanelController>();
            if (panelController != null)
            {
                panelController.ClosePanel();
            }
            else
            {
                Debug.LogError("O script 'PanelController' não foi encontrado no mesmo objeto do 'PasswordUI'. A UI não pode ser fechada.");
            }
        }
        else
        {
            Debug.Log("Senha incorreta! Tente novamente.");
            ClearFields();
            if (inputFields.Length > 0 && inputFields[0] != null)
            {
                inputFields[0].Select();
            }
        }
    }

    private void ClearFields()
    {
        foreach (var field in inputFields)
        {
            if (field != null) field.text = "";
        }
    }
}

