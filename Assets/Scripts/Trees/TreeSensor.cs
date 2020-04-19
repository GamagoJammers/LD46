using System.Collections.Generic;
using UnityEngine;

public class TreeSensor : MonoBehaviour
{
    // Parameters
    public string m_treeTag;
    public GameObject m_choppedTree;
    public bool m_isChoppingFlag;

    private List<ChoppableTree> m_sensedTrees;
    private ChoppableTree m_selectedTree;

    public bool CanChopTree()
    {
        return m_selectedTree != null;
    }

    public bool IsChoppingTree()
    {
        return m_isChoppingFlag && m_selectedTree != null;
    }

    public void ChopTree()
    {
        if (m_selectedTree != null)
        {
            m_isChoppingFlag = true;
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

            //if (m_sensedTrees[i].IsPickedUp())
            //{
            //    continue;
            //}

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


    void Start()
    {
        m_sensedTrees = new List<ChoppableTree>();
        m_selectedTree = null;
    }

    private void Update()
    {
        if (m_isChoppingFlag)
        {
            //m_selectedTree -> Chop
            m_isChoppingFlag = false;
        }
    }

    private void FixedUpdate()
    {
        // Fix state
        if (m_isChoppingFlag && m_selectedTree == null)
        {
            m_isChoppingFlag = false;
        }

        if (m_isChoppingFlag)
        {
            SelectTree();
        }
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
        }
    }
}
