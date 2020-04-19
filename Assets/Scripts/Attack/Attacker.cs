using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Attacker : MonoBehaviour
{
    public List<string> m_attackableTags;
    public bool m_enableAttack = true;

    public float m_stunDamages;
    public int m_healthDamages;

    public UnityEvent m_attackEvent;


    public void Attack(Damageable _damageable)
    {
        m_attackEvent.Invoke();
        _damageable.Damage(m_healthDamages, m_stunDamages);
    }

    public void SetEnableAttack(bool _enabled)
    {
        m_enableAttack = _enabled;
    }

    void Awake()
    {
        m_attackEvent = new UnityEvent();
    }

    void OnDisable()
    {
        m_attackEvent.RemoveAllListeners();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (m_enableAttack && m_attackableTags.Contains(other.tag))
        {
            Damageable damageable = other.GetComponent<Damageable>();
            if (damageable != null)
            {
                   Attack(damageable);
            }
        }
    }
}
