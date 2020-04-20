using UnityEngine;
using UnityEngine.Events;

public class Damageable : MonoBehaviour
{
    // Parameters
    public bool m_canBeKilled;
    public int m_maxHealth;

    public bool m_canBeStunned;
    public bool m_additiveStun;
    public float m_maxStunnedTime;

    // Event
    public UnityEvent m_deathEvent;
    public UnityEvent m_damageEvent;
    public UnityEvent m_startStunEvent;

    // State
    public int m_healthPoints;
    public float m_stunTimer;


    // Interface
    public void Damage(int _healthDamages, float _stunDamage)
    {
        if (!IsAlive())
        {
            return;
        }

        bool damage = false;
        if (m_canBeKilled)
        {
            damage = true;
            m_healthPoints = Mathf.Max(m_healthPoints - _healthDamages, 0);

            if (!IsAlive())
            {
                m_deathEvent.Invoke();
            }
        }

        if (m_canBeStunned)
        {
            if (!IsStunned() || m_additiveStun)
            {
                if (!IsStunned())
                {
                    m_startStunEvent.Invoke();
                }
                m_stunTimer = Mathf.Min(m_stunTimer + _stunDamage, m_maxStunnedTime);
                damage = true;
            }
        }
        if (damage)
        {
            m_damageEvent.Invoke();
        }
    }

    public void Heal(int _healthHeal)
    {
        if (IsAlive())
        {
            m_healthPoints = Mathf.Min(m_healthPoints + _healthHeal, m_maxHealth);
        }
    }

    public bool IsAlive()
    {
        return m_healthPoints > 0;
    }

    public bool IsStunned()
    {
        return m_stunTimer > 0;
    }

    public float GetStunnedTimer()
    {
        return m_stunTimer;
    }

    public bool CanPerformActions()
    {
        return IsAlive() && !IsStunned();
    }


    //Internal methods
    private void Awake()
    {
        if (m_deathEvent == null)
        {
            m_deathEvent = new UnityEvent();
        }

        if (m_startStunEvent == null)
        {
            m_startStunEvent = new UnityEvent();
        }

        if (m_damageEvent == null)
        {
            m_damageEvent = new UnityEvent();
        }
    }

    private void OnDisable()
    {
        m_deathEvent.RemoveAllListeners();
        m_startStunEvent.RemoveAllListeners();
        m_damageEvent.RemoveAllListeners();
    }

    void Start()
    {
        m_healthPoints = m_maxHealth;
        m_stunTimer = 0;
    }

    void FixedUpdate()
    {
        if (!IsAlive())
        {
            return;
        }

        if (IsStunned())
        {
            m_stunTimer = Mathf.Max(0, m_stunTimer - Time.deltaTime);
        }
    }
}