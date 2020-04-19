using UnityEngine;

public class ThrowableAttackControl : MonoBehaviour
{
    public Rigidbody m_rb;
    public Pickable m_pickable;
    public Attacker m_attack;

    public float m_minHitVelocity;

    void Update()
    {
        if (m_pickable.IsPickedUp())
        {
            m_attack.SetEnableAttack(false);
        }
        else
        {
            Vector3 velocity2D = m_rb.velocity;
            velocity2D.y = 0;
            m_attack.SetEnableAttack(velocity2D.magnitude > m_minHitVelocity);
        }
    }
}
