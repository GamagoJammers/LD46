using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoppableTree : MonoBehaviour
{
    public float m_maxChopTimer;
    public WoodenTree m_tree;

    private float m_chopTimer;
    private bool m_flagIsChopped;

    public void Chop()
    {
        m_flagIsChopped = true;
    }

    public float GetChopInversionPregression()
    {
        return m_chopTimer / m_maxChopTimer;
    }

    public bool CanBeChopped()
    {
       return GetChopInversionPregression() > 0 && m_tree.actualState.growthStatus != WoodenTreeGrowthStatus.SPROUT;
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
        m_flagIsChopped = false;

        if (!CanBeChopped())
        {
            m_tree.Die();
        }
    }
}
