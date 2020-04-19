﻿using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    public float m_timeToGetUp;

    public AnimationClip m_throwAnimationClip;

    public Animator m_animator;
    public Rigidbody m_rb;
    public TreeSensor m_treeSensor;
    public PickerSensor m_pickerSensor;
    public Damageable m_damageable;

    private float m_throwTimer;

    private void Start()
    {
        m_treeSensor.m_chopEvent.AddListener(Chop);
        m_damageable.m_startStunEvent.AddListener(Stun);
        m_pickerSensor.m_throwEvent.AddListener(Throw);
        m_throwTimer = 0;
    }
    private void OnDisable()
    {
        if (m_treeSensor!=null)
        {
            m_treeSensor.m_chopEvent.RemoveListener(Chop);
        }

        if (m_damageable != null)
        {
            m_damageable.m_startStunEvent.RemoveListener(Stun);
        }

        if (m_pickerSensor != null)
        {
            m_pickerSensor.m_throwEvent.RemoveListener(Throw);
        }
    }

    void Update()
    {
        m_animator.SetBool("IsWalking", m_rb.velocity.sqrMagnitude > 0);
        m_animator.SetBool("IsSawing", m_treeSensor.IsChoppingTree());
        m_animator.SetBool("IsStun", m_damageable.IsStunned());
        m_animator.SetBool("IsThrowing", m_throwTimer >0);
        if (m_throwTimer>0)
        {
            m_throwTimer -= Time.deltaTime;
        }
    }

    void Chop()
    {
        if (!m_animator.GetBool("IsSawing"))
        {
            m_animator.SetBool("IsSawing", true);
            m_animator.SetTrigger("TSawing");
        }
    }

    void Stun()
    {
        m_animator.SetTrigger("TStun");
    }

    void Throw()
    {
        m_animator.SetBool("IsThrowing", true);
        m_animator.SetTrigger("TThrowing");
        m_throwTimer = m_throwAnimationClip.averageDuration;
    }
}
