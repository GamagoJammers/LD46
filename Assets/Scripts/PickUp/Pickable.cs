﻿using UnityEngine;

public class Pickable : MonoBehaviour
{
    public float m_dropImpulse;
    public float m_throwImpulse;
    public Rigidbody m_rb;
    public GameObject m_outline;

    public bool m_pickedUp { get; private set; }
    private bool m_flagOutline;

    public void PickUp(GameObject _picker)
    {
        if (!m_pickedUp)
        {
            m_pickedUp = true;
            m_rb.detectCollisions = false;
            m_rb.isKinematic = true;
            gameObject.transform.SetPositionAndRotation(_picker.transform.position, _picker.transform.rotation);
            gameObject.transform.SetParent(_picker.transform);
        }
    }

    public void Drop()
    {
        Detach(m_dropImpulse);
    }

    public void Throw()
    {
        Detach(m_throwImpulse);
    }

    public void FlagOutline()
    {
        m_flagOutline = true;
    }

    void Detach(float _impulse)
    {
        if (m_pickedUp)
        {
            m_rb.detectCollisions = false;
            m_rb.isKinematic = false;
            gameObject.transform.parent = null;

            m_rb.AddForce(transform.forward * _impulse, ForceMode.Impulse);
        }
    }

    void Start()
    {
        m_pickedUp = false;
        m_flagOutline = false;
    }

    void Update()
    {
        if(m_outline != null)
        {
            m_outline.SetActive(m_flagOutline);
        }
        m_flagOutline = false;
    }
}