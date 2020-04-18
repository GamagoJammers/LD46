using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    // Parameters
    public float m_maxForwardSpeed;
    public float m_maxBackwardSpeed;
    public float m_maxSideSpeed;

    // Components
    public Damageable m_damageable;
    public Rigidbody m_rb;
    public PlayerInput m_input;

    void Start()
    {

    }

    void FixedUpdate()
    {
        if (m_damageable.CanMove())
        {
            float forward = Vector3.Dot(transform.forward, m_input.m_movementVector);
            float right = Vector3.Dot(transform.right, m_input.m_movementVector);

            float maxForwardSpeed = forward >= 0 ? m_maxForwardSpeed : m_maxBackwardSpeed;

            float speedFactor = Mathf.Sqrt(Mathf.Pow(forward * maxForwardSpeed, 2) + Mathf.Pow(right * m_maxSideSpeed, 2));
            m_rb.AddForce(m_input.m_movementVector * speedFactor - m_rb.velocity, ForceMode.VelocityChange);

            if (m_input.m_interact)
            {
                m_rb.transform.LookAt(transform.position + m_input.m_aimVector);
            } else
            {
                m_rb.transform.LookAt(transform.position + m_input.m_movementVector);
            }
        }
        else
        {
            m_rb.AddForce(-m_rb.velocity, ForceMode.VelocityChange);
            m_rb.AddTorque(-m_rb.angularVelocity, ForceMode.VelocityChange);
        }

    }
}
