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
    public PickerSensor m_picker;

    void Start()
    {
        m_input.m_mainActionReleaseEvent.AddListener(TryPickUp);
        m_input.m_mainActionDownEvent.AddListener(TryThrow);
        m_input.m_secoundaryActionDownEvent.AddListener(TryDrop);
    }
    private void OnDisable()
    {
        if(m_input != null)
        {
            m_input.m_mainActionReleaseEvent.RemoveListener(TryPickUp);
            m_input.m_mainActionDownEvent.RemoveListener(TryThrow);
            m_input.m_secoundaryActionDownEvent.RemoveListener(TryDrop);
        }
    }

    void FixedUpdate()
    {
        Movement();
    }

    void Movement()
    {
        if (!m_damageable.CanMove())
        {
            m_rb.AddForce(-m_rb.velocity, ForceMode.VelocityChange);
            m_rb.transform.LookAt(transform.position + transform.forward);

        }
        else
        {
            float forward = Vector3.Dot(transform.forward, m_input.m_movementVector);
            float right = Vector3.Dot(transform.right, m_input.m_movementVector);

            float maxForwardSpeed = forward >= 0 ? m_maxForwardSpeed : m_maxBackwardSpeed;

            float speedFactor = Mathf.Sqrt(Mathf.Pow(forward * maxForwardSpeed, 2) + Mathf.Pow(right * m_maxSideSpeed, 2));
            m_rb.AddForce(m_input.m_movementVector * speedFactor - m_rb.velocity, ForceMode.VelocityChange);

            if (m_input.m_mainAction)
            {
                m_rb.transform.LookAt(transform.position + m_input.m_aimVector);
            }
            else
            {
                m_rb.transform.LookAt(transform.position + m_input.m_movementVector);
            }

        }
    }

    public void TryPickUp()
    {
        if (m_damageable.CanMove() && m_picker.CanPickUp())
        {
            m_picker.PickUp();
        }
    }

    public void TryDrop()
    {
        if (m_damageable.CanMove() && m_picker.IsCarryingPickable())
        {
            m_picker.Drop();
        }
    }    
    
    public void TryThrow()
    {
        if (m_damageable.CanMove() && m_picker.IsCarryingPickable())
        {
            m_picker.Throw();
        }
    }
}
