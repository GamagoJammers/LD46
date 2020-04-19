using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoppableTree : MonoBehaviour
{
    public float m_maxChopTimer;
    public WoodenTree m_tree;

    private float m_chopTimer;
    private bool m_flagIsChopped;

    public float GetChopInversionPregression()
    {
        return Mathf.Max(m_chopTimer / m_maxChopTimer, 0);
    }

    public bool CanBeChopped()
    {
       return m_chopTimer > 0 && m_tree.actualState.growthStatus != WoodenTreeGrowthStatus.SPROUT;
    }

    public bool IsBeingChopped()
    {
        return m_flagIsChopped;
    }

    public void StartChop()
    {
        m_flagIsChopped = true;
    }

    public void StopChop()
    {
        m_flagIsChopped = false;
    }
    
    void Start()
    {
        m_flagIsChopped = false;
        m_chopTimer = m_maxChopTimer;
    }

    
    void Update()
    {
        if (m_flagIsChopped)
        {
            m_chopTimer -= Time.deltaTime;
        }

        if (m_chopTimer <= 0)
        {
            m_tree.Die();
        }
    }
}
