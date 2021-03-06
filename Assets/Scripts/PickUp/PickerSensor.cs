﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class PickerSensor : MonoBehaviour
{
    // Parameters
    public List<string> m_pickableTags;
    public GameObject m_carryingTag;
    public GameObject m_thrower;
    public bool m_canThrow;
    public bool m_shouldFlagOutline;
	public bool m_isPlayer;

	public Pickable m_selectedPickable;

	public Damageable m_damageable;

	public UnityEvent m_pickEvent;
	public UnityEvent m_throwEvent;

	private List<Pickable> m_sensedPickables;
    private bool m_isCarrying;


    public bool CanPickUp()
    {
        return !m_isCarrying && m_selectedPickable != null;
    }

    public bool IsCarryingPickable()
    {
        return m_isCarrying && m_selectedPickable != null;
    }

    public void PickUp()
    {
        if (CanPickUp())
        {
            m_isCarrying = true;
            m_selectedPickable.PickUp(m_carryingTag, m_thrower, m_isPlayer);
			m_pickEvent.Invoke();
        }
    }

    public void Drop()
    {
        if (IsCarryingPickable())
        {
            m_isCarrying = false;
            m_selectedPickable.Drop();
        }
    }

    public void Throw()
    {
        if (IsCarryingPickable() && m_canThrow)
        {
            m_isCarrying = false;
            if (m_canThrow)
            {
                m_selectedPickable.Throw();
                m_throwEvent.Invoke();
            }
            else
            {
                m_selectedPickable.Drop();
            }
        }
    }

    private void SelectPickable()
    {
        Pickable returnedPickable = null;
        float returnedProximity = float.MaxValue;

        List<int> toDelete = new List<int>(); ;

        for (int i = 0; i < m_sensedPickables.Count; ++i)
        {
            if (m_sensedPickables[i] == null)
            {
                toDelete.Add(i);
                continue;
			}

			if (!m_sensedPickables[i].IsPickable())
			{
				continue;
			}

			if (m_sensedPickables[i].IsPickedUp())
            {
                continue;
            }

            Vector3 position = m_sensedPickables[i].gameObject.transform.position;

            float proximity = (transform.position - position).sqrMagnitude + Vector3.Angle(transform.position, position) / 90;
            if (returnedProximity > proximity)
            {
                returnedProximity = proximity;
                returnedPickable = m_sensedPickables[i];
            }

        }

        // Clean (inverse browse, to keep the order, and the right index)
        for (int i = toDelete.Count -1; i >= 0; --i)
        {
            toDelete.RemoveAt(i);
        }

        m_selectedPickable = returnedPickable;
    }

    private void Awake()
    {
		m_pickEvent = new UnityEvent();
        m_throwEvent = new UnityEvent();
    }

    void Start()
    {
        m_sensedPickables = new List<Pickable>();
        m_selectedPickable = null;

        m_damageable.m_startStunEvent.AddListener(Drop);
        m_damageable.m_deathEvent.AddListener(Drop);
    }

    private void OnDisable()
    {
        if(m_damageable != null)
        {
            m_damageable.m_startStunEvent.RemoveListener(Drop);
            m_damageable.m_deathEvent.RemoveListener(Drop);
		}
		m_pickEvent.RemoveAllListeners();
		m_throwEvent.RemoveAllListeners();
	}

    private void FixedUpdate()
	{
		if (!GameManager.instance.isPaused)
		{
			// Fix state
			if (m_isCarrying && m_selectedPickable == null)
			{
				m_isCarrying = false;
			}

			if (!m_isCarrying)
			{
				SelectPickable();
			}
		}
    }

    private void Update()
    {
		if (!GameManager.instance.isPaused)
		{
			if (m_shouldFlagOutline && CanPickUp())
			{
				m_selectedPickable.FlagOutline();
			}
		}
    }

    private void OnTriggerEnter(Collider other)
    {
        if (m_pickableTags.Contains(other.tag))
        {
            Pickable pickable = other.GetComponent<Pickable>();
            if (pickable != null && !m_sensedPickables.Contains(pickable))
            {
                m_sensedPickables.Add(pickable);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Pickable pickable = other.GetComponent<Pickable>();
        if (pickable != null)
        {
            m_sensedPickables.Remove(pickable);
        }
    }
}
