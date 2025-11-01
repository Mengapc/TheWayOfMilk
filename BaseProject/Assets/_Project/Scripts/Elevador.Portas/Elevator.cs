using System.Collections;
using UnityEngine;
using Unity.Cinemachine; // Corrigido de "Unity.Cinemachine" para o namespace correto (se for o caso, ou manter)
using UnityEngine.InputSystem;

public class Elevator : MonoBehaviour
{
    #region Vari�veis
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
    [Tooltip("Indica se o elevador est� no primeiro andar.")]
    private bool primeiroAndar = true;
    [Space]

    [Header("V�riaveis de anima��o")]
    [Tooltip("Curva de movimento do elevador")]
    [SerializeField] private AnimationCurve movementAanimation;
    [Tooltip("Tempo de anima��o")]
    [Range(1f, 5f)]
    [SerializeField] private float durationAnimation = 2f;
    [Tooltip("Porcentagem da dist�ncia percorrida pelo elevador")]
    [Range(0f, 1f)]
    [SerializeField] private float porcentagemDistancia;
    [Space]

    [Header("C�meras")]
    [Tooltip("C�mera principal do jogo.")]
    [SerializeField] private CinemachineCamera mainCamera;
    [Tooltip("C�mera usada durante o movimento do elevador.")]
    [SerializeField] private CinemachineCamera shakeCamera;
    [Tooltip("C�mera do elevador.")]
    [SerializeField] private CinemachineClearShot elevatorCamera;
    [Tooltip("Tempo de espera antes de trocar a c�mera.")]
    [SerializeField] private float waitSwithCamera;
    [Space]

    [Header("Camera Shake")]
    [Tooltip("Amplitude do shake da c�mera (intensidade).")]
    [SerializeField] private float shakeAmplitude = 0.5f;
    [Tooltip("Frequ�ncia do shake da c�mera (velocidade).")]
    [SerializeField] private float shakeFrequency = 2.0f;
    [Tooltip("Curva de intensidade do shake durante o movimento (0=sem shake, 1=shake m�ximo).")]
    [SerializeField] private AnimationCurve shakeIntensityCurve;
    #endregion


    private bool movendo = false;
    public bool colliderPlayer = false;

    #region Fun��es de Evento Unity

    // Chamado a cada frame, verifica o input do jogador para ativar o elevador.
    private void Update()
    {
        // Se a tecla 'Espa�o' foi pressionada, o elevador n�o est� se movendo, e o jogador est� na �rea de colis�o
        if (Keyboard.current.spaceKey.wasPressedThisFrame && !movendo && colliderPlayer)
        {
            ElevatorActivation();
        }
    }

    #endregion

    #region L�gica Principal do Elevador

    // Inicia o movimento do elevador, decidindo para qual andar ir.
    private void ElevatorActivation()
    {
        if (primeiroAndar)
        {
            // Se est� no primeiro andar, move para o segundo
            StartCoroutine(MoverCabine(pontoPrimeiroAndar.position, pontoSegundoAndar.position));
        }
        else
        {
            // Se est� no segundo andar, move para o primeiro
            StartCoroutine(MoverCabine(pontoSegundoAndar.position, pontoPrimeiroAndar.position));
        }
    }

    // Corrotina principal: move o elevador, toca o "shake" da c�mera e gerencia o jogador.
    private IEnumerator MoverCabine(Vector3 startPos, Vector3 finalPos)
    {
        // 1. Inicia a troca para a c�mera do elevador
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
        movendo = true;
        player.transform.GetComponent<Movement>().canMove = false; // Desabilita o movimento do jogador durante o trajeto
        float tempoDecorrido = 0f;

        // 4. Configura o "shake" (tremor) da c�mera
        CinemachineBasicMultiChannelPerlin noise =
            shakeCamera.GetCinemachineComponent(CinemachineCore.Stage.Noise)
            as CinemachineBasicMultiChannelPerlin;

        if (noise != null)
        {
            noise.FrequencyGain = shakeFrequency;
        }

        // 5. Loop de movimento (enquanto o tempo decorrido for menor que a dura��o)
        while (tempoDecorrido < durationAnimation)
        {
            tempoDecorrido += Time.deltaTime;
            float porcentagemTempo = tempoDecorrido / durationAnimation;

            // Usa a Curva de Anima��o para suavizar o movimento (acelera��o e desacelera��o)
            porcentagemDistancia = movementAanimation.Evaluate(porcentagemTempo);

            // Move a cabine usando Lerp (interpola��o linear) baseado na curva
            cabine.position = Vector3.Lerp(startPos, finalPos, porcentagemDistancia);

            // Aplica o shake da c�mera baseado na curva de intensidade
            if (noise != null)
            {
                float shakeIntensity = shakeIntensityCurve.Evaluate(porcentagemTempo);
                noise.AmplitudeGain = shakeAmplitude * shakeIntensity;
            }

            yield return null; // Espera at� o pr�ximo frame
        }

        // 6. Finaliza��o do movimento
        if (noise != null)
        {
            // Reseta os valores de shake para parar o efeito.
            noise.AmplitudeGain = 0f;
            noise.FrequencyGain = 0f;
        }

        cabine.position = finalPos; // Garante que a cabine chegue exatamente � posi��o final
        player.SetParent(null); // Libera o jogador do elevador
        ChangeFloor(); // Atualiza o estado do andar
        movendo = false; // Permite que o elevador seja ativado novamente
        player.transform.GetComponent<Movement>().canMove = true; // Habilita o movimento do jogador novamente

        // 7. Troca de volta para a c�mera principal e abre a porta
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
        // (Opcional: abrir a porta do novo andar, ex: portaSegundoAndar.ToggleDoor())
    }

    // Atualiza o estado do andar (inverte de primeiro para segundo, ou vice-versa).
    private void ChangeFloor()
    {
        primeiroAndar = !primeiroAndar;
    }

    #endregion

    #region Gerenciamento de C�mera

    // Ativa a c�mera do elevador.
    private IEnumerator SwitchToElevatorCamera()
    {
        if (mainCamera != null && elevatorCamera != null)
        {
            Debug.Log("Trocando para a c�mera do elevador.");
            // Ativa o GameObject da c�mera do elevador (que pode ser um ClearShot ou virtual camera)
            elevatorCamera.gameObject.SetActive(true);
            // A mainCamera ser� desativada automaticamente se tiver prioridade menor
        }
        else
        {
            Debug.LogWarning("Refer�ncias de c�mera n�o configuradas no Inspector.");
        }
        yield return null;
    }

    // Desativa a c�mera do elevador e retorna para a c�mera principal.
    private IEnumerator SwitchToMainCamera()
    {
        if (mainCamera != null && elevatorCamera != null)
        {
            Debug.Log("Voltando para a c�mera principal.");
            // Desativa a c�mera do elevador, fazendo o Cinemachine voltar para a mainCamera
            elevatorCamera.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Refer�ncias de c�mera n�o configuradas no Inspector.");
        }
        yield return null;
    }

    #endregion
}