using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TreeSensor : MonoBehaviour
{
    // Parameters
    public string m_treeTag;

    public Damageable m_damageable;

    public UnityEvent m_chopEvent;

    private List<ChoppableTree> m_sensedTrees;
    private ChoppableTree m_selectedTree;

    public bool IsChoppingTree()
    {
        return m_selectedTree != null && m_selectedTree.IsBeingChopped();
    }

    public bool CanChopTree()
    {
        return m_selectedTree != null && m_selectedTree.CanBeChopped();
    }
    public Vector3 GetSelectedTreePosition()
    {
        return m_selectedTree != null ? m_selectedTree.transform.position : transform.position;
    }

    public void TryStartChop()
    {
        if (m_selectedTree != null)
        {
            m_selectedTree.StartChop();
            m_chopEvent.Invoke();
        }
    }

    public void StopChop()
    {
        if (m_selectedTree != null)
        {
            m_selectedTree.StopChop();
        }
    }

    private void SelectTree()
    {
        ChoppableTree returnedTree = null;
        float returnedProximity = float.MaxValue;

        List<int> toDelete = new List<int>(); ;

        for (int i = 0; i < m_sensedTrees.Count; ++i)
        {
            if (m_sensedTrees[i] == null)
            {
                toDelete.Add(i);
                continue;
            }

            if (!m_sensedTrees[i].CanBeChopped())
            {
                continue;
            }

            Vector3 position = m_sensedTrees[i].gameObject.transform.position;

            float proximity = (transform.position - position).sqrMagnitude + Vector3.Angle(transform.position, position) / 90;
            if (returnedProximity > proximity)
            {
                returnedProximity = proximity;
                returnedTree = m_sensedTrees[i];
            }

        }

        // Clean (inverse browse, to keep the order, and the right index)
        for (int i = toDelete.Count - 1; i >= 0; --i)
        {
            toDelete.RemoveAt(i);
        }

        m_selectedTree = returnedTree;
    }

    void Awake()
    {
        m_chopEvent = new UnityEvent();
    }
    void Start()
    {
        m_sensedTrees = new List<ChoppableTree>();
        m_selectedTree = null;

        m_damageable.m_startStunEvent.AddListener(StopChop);
        m_damageable.m_deathEvent.AddListener(StopChop);
    }


    private void OnDisable()
    {
        if (m_damageable != null)
        {
            m_damageable.m_startStunEvent.RemoveListener(StopChop);
            m_damageable.m_deathEvent.RemoveListener(StopChop);
        }
        m_chopEvent.RemoveAllListeners();
    }

    private void FixedUpdate()
    {
         SelectTree();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(m_treeTag))
        {
            ChoppableTree tree = other.GetComponent<ChoppableTree>();
            if (tree != null && !m_sensedTrees.Contains(tree))
            {
                m_sensedTrees.Add(tree);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        ChoppableTree tree = other.GetComponent<ChoppableTree>();
        if (tree != null)
        {
            m_sensedTrees.Remove(tree);
            tree.StopChop();
        }
    }
}
