using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeGravity : MonoBehaviour
{
    public float m_fakeGravity;
    public Rigidbody m_rb;

    void Start()
    {
        m_rb.useGravity = false;
    }

    void FixedUpdate()
    {
        if (!m_rb.isKinematic )
        {
            m_rb.AddForce(m_fakeGravity * Vector3.down, ForceMode.Acceleration);
        }
    }
}
