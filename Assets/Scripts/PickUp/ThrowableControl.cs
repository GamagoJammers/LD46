using UnityEngine;

public class ThrowableControl : MonoBehaviour
{
    public Rigidbody m_rb;
    public Pickable m_pickable;
    public Attacker m_attack;

    public float m_minHitVelocity;

    void Update()
    {
		if (!GameManager.instance.isPaused)
		{
			if (m_pickable.IsPickedUp())
			{
				m_attack.SetEnableAttack(false);
			}
			else
			{
				Vector3 velocity2D = m_rb.velocity;
				velocity2D.y = 0;
				bool condition = velocity2D.magnitude > m_minHitVelocity;
				m_pickable.m_isPickable = !condition;
				m_attack.SetEnableAttack(condition);

			}
		}
    }
}
