using UnityEngine;

// Este script gerencia o Animator do jogador.
// Ele recebe comandos de outros scripts (como Movement e ObjectGrabbing)
// e atualiza os parâmetros do Animator.
[RequireComponent(typeof(Animator))]
public class PlayerAnimationController : MonoBehaviour
{
    private Animator animator;

    // Hashes dos parâmetros (para otimização, mais rápido que usar strings)
    private readonly int hashMoveSpeed = Animator.StringToHash("moveSpeed");
    private readonly int hashIsHolding = Animator.StringToHash("isHolding");
    private readonly int hashIsCharging = Animator.StringToHash("isCharging");
    private readonly int hashOnCollect = Animator.StringToHash("onCollect");
    private readonly int hashOnDrop = Animator.StringToHash("onDrop");
    private readonly int hashOnThrow = Animator.StringToHash("onThrow");
    private readonly int hasIshMoveMilk = Animator.StringToHash("isMoveMilk");

    private void Awake()
    {
        animator = GetComponent<Animator>(); // Pega o Animator
    }

    // --- Métodos Públicos para outros scripts chamarem ---

    // Chamado por Movement.cs no Update()
    public void SetMoveSpeed(float speed)
    {
        // Atualiza o float 'moveSpeed' no Animator
        animator.SetFloat(hashMoveSpeed, speed);
    }

    // Chamado por ObjectGrabbing.cs quando pega/solta
    public void SetHolding(bool holding)
    {
        // Atualiza o bool 'isHolding' (controla Walk_Arny vs Walk_Leite)
        animator.SetBool(hashIsHolding, holding);
    }
    // Chamado por Movement.cs no Update() quando está movendo o leite
    public void SetMoveMilk(bool moving)
    {
        // Atualiza o bool 'isMoveMilk' no Animator
        animator.SetBool(hasIshMoveMilk, moving);
    }

    // Chamado por ObjectGrabbing.cs quando começa/para de carregar
    public void SetCharging(bool charging)
    {
        // Atualiza o bool 'isCharging' (controla a transição para 'Canalizando')
        animator.SetBool(hashIsCharging, charging);
    }

    // Chamado por ObjectGrabbing.cs quando coleta
    public void TriggerCollect()
    {
        // Dispara o trigger 'onCollect' (para 'Collect_Arny')
        animator.SetTrigger(hashOnCollect);
    }

    // Chamado por ObjectGrabbing.cs quando solta (sem arremessar)
    public void TriggerDrop()
    {
        // Dispara o trigger 'onDrop' (para 'Drop_Arny')
        animator.SetTrigger(hashOnDrop);
    }

    // Chamado por ObjectGrabbing.cs quando arremessa
    public void TriggerThrow()
    {
        // Dispara o trigger 'onThrow' (para 'Arremesso')
        animator.SetTrigger(hashOnThrow);
    }



}