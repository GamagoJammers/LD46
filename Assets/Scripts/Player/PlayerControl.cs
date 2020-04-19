﻿using UnityEngine;

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
    public TreeSensor m_chopper;

    public bool m_flagInteractAction;
    public bool m_flagChopAction;

    void Start()
    {
        m_input.m_mainActionDownEvent.AddListener(TryStartChop);
        m_input.m_mainActionReleaseEvent.AddListener(StopChop);

        m_input.m_mainActionDownEvent.AddListener(TryPickUp);
        m_input.m_mainActionReleaseEvent.AddListener(TryPickUp);

        m_input.m_mainActionDownEvent.AddListener(TryThrow);
        m_input.m_secoundaryActionDownEvent.AddListener(TryDrop);
    }
    private void OnDisable()
    {
        if (m_input != null)
        {
            m_input.m_mainActionDownEvent.RemoveListener(TryStartChop);
            m_input.m_mainActionReleaseEvent.RemoveListener(StopChop);

            m_input.m_mainActionDownEvent.RemoveListener(TryPickUp);
            m_input.m_mainActionReleaseEvent.RemoveListener(TryPickUp);

            m_input.m_mainActionDownEvent.RemoveListener(TryThrow);
            m_input.m_secoundaryActionDownEvent.RemoveListener(TryDrop);
        }
    }

    void FixedUpdate()
    {
        ProcessMovement();
        ProcessChop();
        
        m_flagInteractAction = false;

    }

    void ProcessMovement()
    {
        if (!CanMove())
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

            if (m_input.m_mainAction || m_picker.IsCarryingPickable())
            {
                m_rb.transform.LookAt(transform.position + m_input.m_aimVector);
            }
            else
            {
                m_rb.transform.LookAt(transform.position + m_input.m_movementVector);
            }

        }
    }

    void ProcessChop()
    {
        if (IsChoppingTree())
        {
            m_chopper.ChopTree();
        } else
        {
            m_flagChopAction = false;
        }
    }

    bool CanMove()
    {
        return m_damageable.CanPerformActions() && !IsChoppingTree();
    }

    bool CanPickupItem()
    {
        return m_damageable.CanPerformActions() && m_picker.CanPickUp() && !IsChoppingTree();
    }

    bool CanDrop()
    {
        return m_damageable.CanPerformActions() && m_picker.IsCarryingPickable();
    }

    bool IsChoppingTree()
    {
        return m_flagChopAction && CanChopTree();
    }

    bool CanChopTree()
    {
        return m_damageable.CanPerformActions() && !m_picker.IsCarryingPickable() && m_chopper.CanChopTree();
    }

    public void TryStartChop()
    {
        m_flagChopAction = true;
    }

    public void StopChop()
    {
        m_flagChopAction = false;
    }

    public void TryPickUp()
    {
        if (!m_flagInteractAction && CanPickupItem())
        {
            m_flagInteractAction = true;
            m_picker.PickUp();
        }
    }

    public void TryDrop()
    {
        if (!m_flagInteractAction && CanDrop())
        {
            m_flagInteractAction = true;
            m_picker.Drop();
        }
    }

    public void TryThrow()
    {
        if (!m_flagInteractAction && CanDrop())
        {
            m_flagInteractAction = true;
            m_picker.Throw();
        }
    }
}
