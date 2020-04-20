using UnityEngine;

public class Pickable : MonoBehaviour
{
	[HideInInspector]
	public bool m_isPickable = true;

	public float m_dropImpulse;
    public float m_throwImpulse;
    public Rigidbody m_rb;
    public GameObject m_outline;

	private bool m_pickedUp;
    private bool m_playerPickedUp;
    private bool m_flagOutline;
	
	public bool IsPickable()
	{
		return m_isPickable;
	}

	public bool IsPickedUp()
	{
		return m_pickedUp;
	}

	public bool IsPickedButNotByPlayer()
    {
        return m_pickedUp && !m_playerPickedUp;
    }

    public void PickUp(GameObject _picker, bool _byPlayer = false)
    {
        if (!m_pickedUp)
        {
            m_pickedUp = true;
            m_rb.detectCollisions = false;
            m_rb.isKinematic = true;
            m_rb.velocity = Vector3.zero;
            m_rb.angularVelocity = Vector3.zero;
            gameObject.transform.SetPositionAndRotation(_picker.transform.position, _picker.transform.rotation);
            gameObject.transform.SetParent(_picker.transform);

            m_playerPickedUp = _byPlayer;
        }
    }

    public void Drop( bool _bypassCheck = false )
    {
        Detach(m_dropImpulse, _bypassCheck);
    }

    public void Throw()
    {
        Detach(m_throwImpulse, false);
    }

    public void FlagOutline()
    {
        m_flagOutline = true;
    }

    void Detach(float _impulse, bool _bypassCheck)
    {
        if (_bypassCheck || m_pickedUp)
        {
            m_pickedUp = false;
            m_rb.detectCollisions = true;
            m_rb.isKinematic = false;
            gameObject.transform.parent = null;

            m_playerPickedUp = false;

            m_rb.AddForce(transform.forward * _impulse, ForceMode.Impulse);
        }
    }

    void Start()
    {
        m_pickedUp = false;
        m_flagOutline = false;
        m_playerPickedUp = false;
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