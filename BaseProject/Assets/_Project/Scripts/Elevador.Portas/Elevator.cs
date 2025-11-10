using System.Collections;
using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.InputSystem;

public class Elevator : MonoBehaviour
{
    #region Variáveis
    [Header("Componentes")]
    [Tooltip("A cabine do elevador que se move entre os andares.")]
    [SerializeField] private Transform cabine;
    [Tooltip("Porta da cabine.")]
    [SerializeField] private OpenCloseDoor cabineDoor;
    [Tooltip("O ponto do primeiro andar onde a cabine deve parar.")]
    [SerializeField] private Transform pontoPrimeiroAndar;
    [Tooltip("A porta do primeiro andar.")]
    [SerializeField] private OpenCloseDoor portaPrimeiroAndar;
    [Tooltip("O ponto do segundo andar onde a cabine deve parar.")]
    [SerializeField] private Transform pontoSegundoAndar;
    [Tooltip("A porta do segundo andar.")]
    [SerializeField] private OpenCloseDoor portaSegundoAndar;
    [Tooltip("Transform do jogador.")]
    [SerializeField] private Transform player;
    [Tooltip("Indica se o elevador está no primeiro andar.")]
    private bool primeiroAndar = true;
    [Space]

    [Header("Váriaveis de animação")]
    [Tooltip("Curva de movimento do elevador")]
    [SerializeField] private AnimationCurve movementAanimation;
    [Tooltip("Tempo de animação")]
    [Range(1f, 5f)]
    [SerializeField] private float durationAnimation = 2f;
    [Tooltip("Porcentagem da distância percorrida pelo elevador")]
    [Range(0f, 1f)]
    [SerializeField] private float porcentagemDistancia;
    [Space]

    [Header("Câmeras")]
    [Tooltip("Câmera principal do jogo.")]
    [SerializeField] private CinemachineCamera mainCamera;
    [Tooltip("Câmera usada durante o movimento do elevador.")]
    [SerializeField] private CinemachineCamera shakeCamera;
    [Tooltip("Câmera do elevador.")]
    [SerializeField] private CinemachineClearShot elevatorCamera;
    [Tooltip("Tempo de espera antes de trocar a câmera.")]
    [SerializeField] private float waitSwithCamera;
    [Space]

    [Header("Camera Shake")]
    [Tooltip("Amplitude do shake da câmera (intensidade).")]
    [SerializeField] private float shakeAmplitude = 0.5f;
    [Tooltip("Frequência do shake da câmera (velocidade).")]
    [SerializeField] private float shakeFrequency = 2.0f;
    [Tooltip("Curva de intensidade do shake durante o movimento (0=sem shake, 1=shake máximo).")]
    [SerializeField] private AnimationCurve shakeIntensityCurve;

    [Header("Áudio")]
    [Tooltip("AudioSource que tocará o som de movimento. Deve estar neste GameObject e com 'Loop' marcado.")]
    [SerializeField] private AudioSource movingSoundSource;
    [Tooltip("Som que toca em loop ENQUANTO o elevador está se movendo.")]
    [SerializeField] private AudioClip movingSoundClip;
    [Tooltip("Som que toca quando o elevador chega ao destino.")]
    [SerializeField] private AudioClip arriveSound;
    [Tooltip("Volume dos efeitos sonoros do elevador.")]
    [Range(0f, 1f)]
    [SerializeField] private float soundVolume = 1f;
    [Space]

    #endregion


    private bool movendo = false;
    public bool colliderPlayer = false;


    #region Lógica Principal do Elevador

    // Inicia o movimento do elevador, decidindo para qual andar ir.
    public void ElevatorActivation()
    {
        if (movendo) return; // <-- ADIÇÃO SUGERIDA: Impede reativação se já estiver em movimento

        if (primeiroAndar)
        {
            // Se está no primeiro andar, move para o segundo
            StartCoroutine(MoverCabine(pontoPrimeiroAndar.position, pontoSegundoAndar.position));
        }
        else
        {
            // Se está no segundo andar, move para o primeiro
            StartCoroutine(MoverCabine(pontoSegundoAndar.position, pontoPrimeiroAndar.position));
        }
    }

    // Corrotina principal: move o elevador, toca o "shake" da câmera e gerencia o jogador.
    private IEnumerator MoverCabine(Vector3 startPos, Vector3 finalPos)
    {
        movendo = true; // <-- MOVIDO PARA CIMA: Garante que o estado 'movendo' seja definido imediatamente

        // 1. Inicia a troca para a câmera do elevador
        StartCoroutine(SwitchToElevatorCamera());

        // 2. Fecha a porta da cabine
        cabineDoor.ToggleDoor();
        if (primeiroAndar)
        {
            portaPrimeiroAndar.ToggleDoor();
        }
        else
        {
            portaSegundoAndar.ToggleDoor();
        }
        yield return new WaitForSeconds(waitSwithCamera); // Espera a porta fechar

        // 3. Prende o jogador ao elevador para que ele se mova junto
        player.SetParent(cabine);
        player.transform.GetComponent<Movement>().canMove = false; // Desabilita o movimento do jogador durante o trajeto
        float tempoDecorrido = 0f;

        // 4. Configura o "shake" (tremor) da câmera
        CinemachineBasicMultiChannelPerlin noise =
            shakeCamera.GetCinemachineComponent(CinemachineCore.Stage.Noise)
            as CinemachineBasicMultiChannelPerlin;

        if (noise != null)
        {
            noise.FrequencyGain = shakeFrequency;
        }

        // 5. Toca o som de INÍCIO de movimento
        if (movingSoundSource != null && movingSoundClip != null)
        {
            movingSoundSource.clip = movingSoundClip;
            movingSoundSource.volume = soundVolume;
            // movingSoundSource.loop = true; // Já garantimos isso pelo Inspector, mas podemos forçar aqui
            movingSoundSource.Play();
        }
        else
        {
            Debug.LogWarning("MovingSoundSource ou MovingSoundClip não assignados no Elevador."); // <-- NOVO
        }

        // 6. Loop de movimento (enquanto o tempo decorrido for menor que a duração)
        while (tempoDecorrido < durationAnimation)
        {
            tempoDecorrido += Time.deltaTime;
            float porcentagemTempo = tempoDecorrido / durationAnimation;

            // Usa a Curva de Animação para suavizar o movimento (aceleração e desaceleração)
            porcentagemDistancia = movementAanimation.Evaluate(porcentagemTempo);

            // Move a cabine usando Lerp (interpolação linear) baseado na curva
            cabine.position = Vector3.Lerp(startPos, finalPos, porcentagemDistancia);

            // Aplica o shake da câmera baseado na curva de intensidade
            if (noise != null)
            {
                float shakeIntensity = shakeIntensityCurve.Evaluate(porcentagemTempo);
                noise.AmplitudeGain = shakeAmplitude * shakeIntensity;
            }

            yield return null; // Espera até o próximo frame
        }

        {
            movingSoundSource.Stop();
        }

        // 7. Finalização do movimento
        if (noise != null)
        {
            // Reseta os valores de shake para parar o efeito.
            noise.AmplitudeGain = 0f;
            noise.FrequencyGain = 0f;
        }

        cabine.position = finalPos; // Garante que a cabine chegue exatamente à posição final

        // 8. Toca o som de CHEGADA
        if (arriveSound != null && SoundFXManager.instance != null) // <-- NOVO
        {
            SoundFXManager.instance.PlaySoundFXClip(arriveSound, cabine.transform, soundVolume); // <-- NOVO
        }

        player.SetParent(null); // Libera o jogador do elevador
        ChangeFloor(); // Atualiza o estado do andar
        player.transform.GetComponent<Movement>().canMove = true; // Habilita o movimento do jogador novamente

        // 9. Troca de volta para a câmera principal e abre a porta
        StartCoroutine(SwitchToMainCamera());
        cabineDoor.ToggleDoor();
        if (primeiroAndar)
        {
            portaPrimeiroAndar.ToggleDoor();
        }
        else
        {
            portaSegundoAndar.ToggleDoor();
        }

        // Espera um pouco antes de permitir o próximo movimento (opcional, mas bom para o áudio terminar)
        yield return new WaitForSeconds(0.5f); // <-- ADIÇÃO SUGERIDA
        movendo = false; // Permite que o elevador seja ativado novamente
    }

    // Atualiza o estado do andar (inverte de primeiro para segundo, ou vice-versa).
    private void ChangeFloor()
    {
        primeiroAndar = !primeiroAndar;
    }

    #endregion

    #region Gerenciamento de Câmera

    // Ativa a câmera do elevador.
    private IEnumerator SwitchToElevatorCamera()
    {
        if (mainCamera != null && elevatorCamera != null)
        {
            Debug.Log("Trocando para a câmera do elevador.");
            // Ativa o GameObject da câmera do elevador (que pode ser um ClearShot ou virtual camera)
            elevatorCamera.gameObject.SetActive(true);
            mainCamera.gameObject.SetActive(false);
            // A mainCamera será desativada automaticamente se tiver prioridade menor
        }
        else
        {
            Debug.LogWarning("Referências de câmera não configuradas no Inspector.");
        }
        yield return null;
    }

    // Desativa a câmera do elevador e retorna para a câmera principal.
    private IEnumerator SwitchToMainCamera()
    {
        if (mainCamera != null && elevatorCamera != null)
        {
            Debug.Log("Voltando para a câmera principal.");
            // Desativa a câmera do elevador, fazendo o Cinemachine voltar para a mainCamera
            mainCamera.gameObject.SetActive(true);
            elevatorCamera.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Referências de câmera não configuradas no Inspector.");
        }
        yield return null;
    }

    #endregion
}