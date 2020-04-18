using UnityEngine;
using UnityEngine.Events;

public class PlayerInput : MonoBehaviour
{
    // Status
    public bool m_modeGamepad;
    public Vector3 m_movementVector;
    public Vector3 m_aimVector;
    public bool m_interact;
    public bool m_drop;

    // Helper
    public Vector3 m_cameraForward;

    //Events
    public UnityEvent m_interactDownEvent;
    public UnityEvent m_interactReleaseEvent;
    public UnityEvent m_dropEvent;

    // Components
    private GameObject m_player;
    private GameObject m_camera;

    void Awake()
    {
        if (m_interactDownEvent == null)
        {
            m_interactDownEvent = new UnityEvent();
        }        
        
        if (m_interactReleaseEvent == null)
        {
            m_interactReleaseEvent = new UnityEvent();
        }        

        if (m_dropEvent == null)
        {
            m_dropEvent = new UnityEvent();
        }
    }

    void Start()
    {
        m_modeGamepad = false;
        m_aimVector = -Vector3.forward;
        m_movementVector = Vector3.zero;
        m_player = GameObject.FindGameObjectWithTag("player");
        m_camera = GameObject.FindGameObjectWithTag("MainCamera");
    }

    void Update()
    {
        m_cameraForward = m_camera.transform.forward;
        m_cameraForward.y = 0;
        m_cameraForward.Normalize();

        bool interactKeypad = Input.GetKey(KeyCode.Mouse0);  // Left Click
        bool dropKeypad = Input.GetKey(KeyCode.Mouse1);  // Right Click
        bool interactGamepad = Input.GetKey(KeyCode.JoystickButton5) || Input.GetAxis("JoystickRightTrigger") != 0; // Right Bumper or Right Trigger
        bool dropGamepad = Input.GetKey(KeyCode.JoystickButton4) || Input.GetAxis("JoystickLeftTrigger") != 0; // Left Bumper or Left Trigger

        bool interact = interactKeypad || interactGamepad;
        bool drop = dropKeypad || dropGamepad;

        if (m_modeGamepad)
        {
            m_modeGamepad = !(Input.GetKey(KeyCode.KeypadEnter) || Input.GetKey(KeyCode.Space) || interactKeypad || dropKeypad);// Button Enter / Space or any interact/drop keypad button
        }
        else
        {
            m_modeGamepad = Input.GetKey(KeyCode.JoystickButton0) || interactGamepad || dropGamepad; // Button A or any interact/drop gamepad button
        }


        Vector3 direction = Vector3.zero;
        if (m_modeGamepad)
        {
            direction = new Vector3(Input.GetAxis("JoystickRightHorizontal"), 0, Input.GetAxis("JoystickRightVertical"));
            direction = Quaternion.FromToRotation(Vector3.forward, m_cameraForward) * direction;
        }
        else
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                direction = hit.point - m_player.transform.position;
                direction.y = 0;
            }
        }

        m_movementVector = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        m_movementVector = Quaternion.FromToRotation(Vector3.forward, m_cameraForward) * m_movementVector;

        if (m_movementVector != Vector3.zero)
        {
            m_movementVector = m_movementVector.normalized;
        }

        if (direction != Vector3.zero)
        {
            // If null vector, keep the previous one
            m_aimVector = direction.normalized;
        } else
        {
            m_aimVector = m_player.transform.forward;
        }

        if (!m_interact && interact)
        {
            m_interact = true;
            m_interactDownEvent.Invoke();
        } else if (m_interact && !interact)
        {
            m_interact = false;
            m_interactReleaseEvent.Invoke();
        }

        if (!m_drop && drop)
        {
            m_dropEvent.Invoke();
        }
        m_drop = drop;
    }
}
