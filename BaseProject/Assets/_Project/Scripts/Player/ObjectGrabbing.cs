using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

// O nome da sua classe é ObjectGrabbing
public class ObjectGrabbing : MonoBehaviour
{
    #region Variaveis
    [Header("Configurações de Pegar Objeto")]
    [Tooltip("O ponto na 'mão' do personagem onde o objeto ficará preso.")]
    [SerializeField] private Transform handPoint;
    [Tooltip("A camada (Layer) dos objetos que podem ser pegos.")]
    [SerializeField] private LayerMask grabbingLayer;
    [Tooltip("Referência ao SphereCollider que define a área de 'pegar'.")]
    [SerializeField] private SphereCollider grabCollider;
    [Tooltip("A distância máxima para pegar um objeto.")]
    [SerializeField] private float grabRange = 2f;

    [Header("Configurações de Arremesso")]
    [Tooltip("A força horizontal MÍNIMA do arremesso (distância).")]
    [SerializeField] private float horizontalForceMin = 7f;
    [Tooltip("A força horizontal MÁXIMA do arremesso (distância).")]
    [SerializeField] private float horizontalForceMax = 25f;
    [Tooltip("A força vertical MÍNIMA do arremesso (altura).")]
    [SerializeField] private float verticallForceMin = 3f; // Mantendo 'verticall' para não quebrar a referência
    [Tooltip("A força vertical MÁXIMA do arremesso (altura).")]
    [SerializeField] private float verticallForceMax = 8f; // Mantendo 'verticall'
    [Tooltip("O tempo em segundos segurando o botão para atingir a força máxima.")]
    [SerializeField] private float tempoMaximoDeCarga = 2f;

    [Header("Áudio")] 
    [Tooltip("AudioSource para o som de 'canalizar' (deve ter Loop=true e PlayOnAwake=false).")] 
    [SerializeField] private AudioSource chargingSoundSource; 
    [Tooltip("Som que toca ao PEGAR um objeto.")] 
    [SerializeField] private AudioClip grabSound; 
    [Tooltip("Array de sons de 'canalizando' (um será escolhido aleatoriamente e tocará em loop).")] 
    [SerializeField] private AudioClip[] chargingSounds; 
    [Tooltip("Array de sons de ARREMESSO (um será escolhido aleatoriamente).")] 
    [SerializeField] private AudioClip[] throwSounds; 
    [Tooltip("Volume dos efeitos sonoros.")] 
    [Range(0f, 1f)] 
    [SerializeField] private float soundVolume = 1f; 
    [Space]

    [Header("Referências (Auto-Buscadas)")]
    [Tooltip("Referência ao script de animação do jogador.")]
    [SerializeField] private PlayerAnimationController animController;
    [Tooltip("Referência ao script de movimento do jogador.")]
    [SerializeField] private Movement movementScript;
    [Tooltip("Referência ao script que calcula a direção da mira.")]
    [SerializeField] private Direction direction;
    [Tooltip("Referência à câmera principal.")]
    [SerializeField] private Camera cam;

    // --- Variáveis de Estado (Privadas) ---
    private GameObject currentGrabbableObject = null; // Objeto no trigger (alvo)
    private GameObject grabObject = null;             // Objeto segurado
    private Rigidbody grabObjectRb = null;
    private bool grabbingObject = false;              // True se estamos segurando algo
    private bool isCharging = false;                  // True se carregando arremesso
    private float currentChargeTime = 0f;
    private bool isNearGrabbableDistance = false;

    // --- Propriedades Públicas (para a UI) ---
    public bool IsNearGrabbableDistance { get { return isNearGrabbableDistance; } }
    public bool GrabbingObject { get { return grabbingObject; } }
    public bool IsNearGrabbable { get; private set; }
    public bool IsCharging => isCharging;
    public float CurrentChargeTime => currentChargeTime;
    public float MaxChargeTime => tempoMaximoDeCarga;
    #endregion

    #region Metodos da Unity
    private void Awake()
    {
        // Pega referências que estão no mesmo objeto
        if (animController == null)
            animController = GetComponent<PlayerAnimationController>();

        if (movementScript == null)
            movementScript = GetComponent<Movement>();

        // Tenta encontrar a câmera principal se não foi definida
        if (cam == null)
            cam = Camera.main;
    }

    // Update é chamado a cada frame
    private void Update()
    {
        // Se o trigger detectou um "alvo"...
        if (currentGrabbableObject != null)
        {
            // ...checa a distância dele a cada frame.
            isNearGrabbableDistance = IsWithinGrabRange(currentGrabbableObject.transform.position);
        }
        else
        {
            // Se não há alvo, não podemos estar perto.
            isNearGrabbableDistance = false;
        }

        // --- LÓGICA DE CARREGAMENTO E ROTAÇÃO ---
        if (isCharging)
        {
            // Incrementa o tempo de carregamento
            currentChargeTime = Mathf.Min(currentChargeTime + Time.deltaTime, tempoMaximoDeCarga);

            // Força o jogador a olhar na direção da mira (do script Direction.cs)
            if (direction != null && movementScript != null && direction.directionVector != Vector3.zero)
            {
                // Chama a função de rotação no script de movimento
                movementScript.RotateTowards(direction.directionVector);
            }
        }
    }

    // Chamado UMA VEZ quando algo ENTRA no trigger
    private void OnTriggerEnter(Collider other)
    {
        // Checa se:
        // 1. Já não temos um alvo (currentGrabbableObject == null)
        // 2. Não estamos segurando nada (!grabbingObject)
        // 3. O objeto tem a tag "Ball"
        if (currentGrabbableObject == null && !grabbingObject && other.CompareTag("Ball"))
        {
            // Define este como o nosso novo "alvo"
            currentGrabbableObject = other.gameObject;
            IsNearGrabbable = true; // Ativa a UI ("Chegar Perto")
        }
    }

    // Chamado UMA VEZ quando algo SAI do trigger
    private void OnTriggerExit(Collider other)
    {
        // Se o objeto que saiu é o nosso "alvo" atual...
        if (other.gameObject == currentGrabbableObject)
        {
            // ...nós o perdemos. Limpa tudo.
            currentGrabbableObject = null;
            IsNearGrabbable = false;
            isNearGrabbableDistance = false; // Garante que a UI "Pegar" também apague
        }
    }
    #endregion

    #region Funções de Input
    // Chamado pelo Input System (Botão de Interação)
    public void InteractionGrabbing(InputAction.CallbackContext context)
    {
        // Se apertou e NÃO está segurando nada -> Tenta Pegar
        if (context.started && !grabbingObject)
        {
            // Checa se temos um "alvo" e se estamos perto
            if (currentGrabbableObject != null && isNearGrabbableDistance)
            {
                // Pega o "alvo" que o trigger detectou
                GrabObject_DisgrabObject(currentGrabbableObject);
            }
        }
        // Se apertou e JÁ está segurando -> Solta (Drop)
        else if (context.started && grabbingObject && !isCharging)
        {
            GrabObject_DisgrabObject(grabObject);
        }
    }

    // Chamado pelo Input System (Botão de Arremesso)
    public void ThrowObject(InputAction.CallbackContext context)
    {
        // Só funciona se estiver segurando um objeto
        if (!grabbingObject) return;

        // Se o botão foi PRESSIONADO (Started) -> Começa a carregar
        if (context.started)
        {
            isCharging = true;
            animController?.SetCharging(true);

            // Trava a rotação de movimento e ativa a rotação da mira
            if (movementScript != null)
                movementScript.overrideRotation = true;

            if (chargingSoundSource != null && chargingSounds != null && chargingSounds.Length > 0)
            {
                // Escolhe um som aleatório do array
                int randIndex = Random.Range(0, chargingSounds.Length);

                // Configura e toca o AudioSource
                chargingSoundSource.clip = chargingSounds[randIndex];
                chargingSoundSource.volume = soundVolume;
                chargingSoundSource.loop = true; // Garante que está em loop
                chargingSoundSource.Play();
            }
        }
        // Se o botão foi SOLTO (Canceled) -> Arremessa
        else if (context.canceled && isCharging)
        {
            isCharging = false;
            animController?.SetCharging(false);

            // Devolve o controle da rotação para o movimento
            if (movementScript != null)
                movementScript.overrideRotation = false;

            if (chargingSoundSource != null)
            {
                chargingSoundSource.Stop();
            }

            // Apenas inicia a corrotina.
            if (grabObject != null)
            {
                StartCoroutine(Arremessar(grabObject));
            }
        }
    }
    #endregion

    #region Corrotinas (Pegar / Largar / Arremessar)

    // Decide se deve pegar ou largar
    private void GrabObject_DisgrabObject(GameObject gameObject)
    {
        if (!grabbingObject) // Se NÃO está segurando -> PEGAR
        {
            // Checagem de segurança: não tenta pegar um objeto nulo
            if (gameObject == null)
            {
                Debug.LogWarning("Tentei pegar um objeto nulo. Ignorando.");
                return;
            }
            StartCoroutine(Pegar(gameObject));
        }
        else // Se ESTÁ segurando -> LARGAR
        {
            // Checagem de segurança para estado quebrado
            if (gameObject == null)
            {
                Debug.LogWarning("Estado de 'Grab' quebrado detectado. Forçando reset.");
                grabbingObject = false;
                grabObject = null;
                grabObjectRb = null;
                animController?.SetHolding(false);
                return; // Não chama a corrotina 'Disgrab' com um nulo
            }

            // Se o objeto não é nulo, larga normalmente.
            StartCoroutine(Disgrab(gameObject));
        }
    }

    // Corrotina para Pegar
    private IEnumerator Pegar(GameObject gameObject)
    {
        // Agora que pegamos o objeto, limpa o "alvo" do trigger
        currentGrabbableObject = null;
        IsNearGrabbable = false;
        isNearGrabbableDistance = false;

        // Configura o objeto
        grabObject = gameObject;

        BallController ball = grabObject.GetComponent<BallController>();
        if (ball != null)
        {
            // Diz ao galão para "resetar" sua trava de som
            ball.ResetHitSound();
        }

        grabObject.transform.SetParent(handPoint);
        grabObject.transform.position = handPoint.transform.position;
        grabObject.transform.rotation = handPoint.transform.rotation;
        grabObjectRb = grabObject.GetComponent<Rigidbody>();
        grabObjectRb.isKinematic = true;
        grabbingObject = true;

        // Animação
        animController?.SetHolding(true);
        animController?.TriggerCollect();

        if (grabSound != null && SoundFXManager.instance != null)
        {
            // 'transform' fará o som sair do jogador
            SoundFXManager.instance.PlaySoundFXClip(grabSound, transform, soundVolume);
        }

        yield return null;
    }

    // Corrotina para Largar (Drop)
    private IEnumerator Disgrab(GameObject gameObject)
    {
        // Pega o Rigidbody DO PARÂMETRO 'gameObject'
        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("O objeto largado não tem Rigidbody!", gameObject);
            yield break; // Para a corrotina se houver um problema
        }

        // Usa o 'gameObject' do parâmetro
        gameObject.transform.SetParent(null);
        rb.isKinematic = false;

        // Limpa as variáveis de estado
        grabbingObject = false;
        grabObject = null;
        grabObjectRb = null; // Limpa a referência do Rigidbody

        // Animação
        animController?.SetHolding(false);
        animController?.TriggerDrop();

        yield return null;
    }

    // Corrotina para Arremessar
    private IEnumerator Arremessar(GameObject gameObject)
    {
        // Pega o Rigidbody DO PARÂMETRO 'gameObject'
        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("O objeto arremessado não tem Rigidbody!", gameObject);
            yield break; // Para a corrotina
        }

        // Solta o objeto
        gameObject.transform.SetParent(null);
        rb.isKinematic = false;

        // --- LÓGICA DE FORÇA ATUALIZADA ---

        // 1. Calcula a porcentagem de carga
        float chargePercent = currentChargeTime / tempoMaximoDeCarga;

        // 2. Calcula a força horizontal (baseada na % de carga)
        float currentHorizontalForce = Mathf.Lerp(horizontalForceMin, horizontalForceMax, chargePercent);

        // 3. Calcula a força vertical (baseada na % de carga)
        // (Usando 'verticall' para bater com sua variável)
        float currentVerticalForce = Mathf.Lerp(verticallForceMin, verticallForceMax, chargePercent);

        // 4. Combina as forças
        Vector3 horizontalForceVec = direction.directionVector * currentHorizontalForce;
        Vector3 verticalForceVec = Vector3.up * currentVerticalForce;
        Vector3 finalForce = horizontalForceVec + verticalForceVec;

        // 5. Aplica a força combinada
        rb.AddForce(finalForce, ForceMode.Impulse);

        // --- FIM DA ATUALIZAÇÃO ---

        // Animação
        animController?.SetHolding(false);
        animController?.TriggerThrow();

        if (throwSounds != null && throwSounds.Length > 0 && SoundFXManager.instance != null)
        {
            SoundFXManager.instance.PlayRandomSoundFXClip(throwSounds, transform, soundVolume);
        }

        // Reseta
        currentChargeTime = 0;

        // Limpa as variáveis de estado AQUI, no final
        grabObject = null;
        grabObjectRb = null;
        grabbingObject = false;

        yield return null;
    }

    #endregion

    #region Funções Auxiliares
    // Checa se o ponto está dentro do raio do collider (distância de "pegar")
    private bool IsWithinGrabRange(Vector3 objectPosition)
    {
        if (grabCollider == null) return false;

        // Calcula a distância do centro do collider (jogador) até o objeto
        float distance = Vector3.Distance(transform.position + grabCollider.center, objectPosition);

        // Retorna true se a distância for MENOR que o raio
        // (Usando a variável grabRange agora)
        return distance <= grabRange;
    }
    #endregion
}