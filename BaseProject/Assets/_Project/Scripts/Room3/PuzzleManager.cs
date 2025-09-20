using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class PuzzleManager : MonoBehaviour
{
    [SerializeField] private DoorController doorController; // Arraste a porta que este puzzle controla
    [SerializeField] private List<int> correctSequence; // Defina a sequência correta de números (ex: 5, 6, 2)

    private List<int> currentSequence = new List<int>();

    private void Awake()
    {
        TubeController.puzzleManager = this;
    }

    public void TubeFilled(int value)
    {
        if (!currentSequence.Contains(value)) // Evita adicionar o mesmo tubo duas vezes
        {
            currentSequence.Add(value);
        }

        // Verifica se a sequência está correta a cada bola inserida
        if (currentSequence.Count == correctSequence.Count)
        {
            // A função SequenceEqual verifica se as duas listas são idênticas na mesma ordem
            if (currentSequence.SequenceEqual(correctSequence))
            {
                Debug.Log("Sequência correta! Abrindo a porta.");
                if (doorController != null)
                {
                    doorController.OpenDoor();
                }
            }
            else // Se a contagem é a mesma mas a sequência está errada, avisa o jogador.
            {
                Debug.Log("Sequência incorreta. Interaja com o botão de reset para tentar novamente.");
            }
        }
    }

    // Função pública para resetar o estado do puzzle
    public void ResetPuzzle()
    {
        Debug.Log("Resetando o puzzle...");
        currentSequence.Clear();

        // Encontra todos os tubos na cena e chama a função para resetá-los
        TubeController[] tubes = FindObjectsOfType<TubeController>();
        foreach (TubeController tube in tubes)
        {
            tube.ResetTube();
        }

        // Encontra todas as bolas na cena e chama a função para resetá-las
        BallController[] balls = FindObjectsOfType<BallController>();
        foreach (BallController ball in balls)
        {
            ball.ResetPosition();
        }
    }
}

