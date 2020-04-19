using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    public Animator m_animator;
    public PlayerControl m_control;
    public Rigidbody m_rb;
    public TreeSensor m_treeSensor;
    public Damageable m_damageable;


    private void Start()
    {
        m_treeSensor.m_chopEvent.AddListener(Chop);
    }

    void Update()
    {
        m_animator.SetBool("IsWalking", m_rb.velocity.sqrMagnitude <= 0);
        m_animator.SetBool("IsSawing", m_treeSensor.IsChoppingTree());
        m_animator.SetBool("IsStun", m_damageable.IsStunned());
    }

    void Chop()
    {
        if (!m_animator.GetBool("IsSawing"))
        {
            m_animator.SetTrigger("TSawing");
            m_animator.SetBool("IsSawing", true);
        }
    }
}
