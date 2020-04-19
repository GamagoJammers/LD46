using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    public Animator m_animator;
    public PlayerControl m_control;
    public Rigidbody m_rb;


    void Start()
    {
        
    }

    
    void Update()
    {
        m_animator.SetBool("IsWalking", m_rb.velocity.sqrMagnitude > 0);
    }
}
