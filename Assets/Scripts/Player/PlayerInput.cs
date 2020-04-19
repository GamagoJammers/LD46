using UnityEngine;
using UnityEngine.Events;

public class PlayerInput : MonoBehaviour
{
    // Status
    public bool m_modeGamepad;
    public Vector3 m_movementVector;
    public Vector3 m_aimVector;
    public bool m_mainAction;
    public bool m_secoundaryAction;

    // Helper
    public Vector3 m_cameraForward;

    //Events
    public UnityEvent m_mainActionDownEvent;
    public UnityEvent m_mainActionReleaseEvent;
    public UnityEvent m_secoundaryActionDownEvent;
    public UnityEvent m_secoundaryActionReleaseEvent;

    // Components
    private GameObject m_player;
    private GameObject m_camera;

    private void OnDisable()
    {
        m_mainActionDownEvent.RemoveAllListeners();
        m_mainActionReleaseEvent.RemoveAllListeners();
        m_secoundaryActionDownEvent.RemoveAllListeners();
        m_secoundaryActionReleaseEvent.RemoveAllListeners();
    }

    void Awake()
    {
        if (m_mainActionDownEvent == null)
        {
            m_mainActionDownEvent = new UnityEvent();
        }

        if (m_mainActionReleaseEvent == null)
        {
            m_mainActionReleaseEvent = new UnityEvent();
        }

        if (m_secoundaryActionReleaseEvent == null)
        {
            m_secoundaryActionDownEvent = new UnityEvent();
        }

        if (m_secoundaryActionDownEvent == null)
        {
            m_secoundaryActionDownEvent = new UnityEvent();
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

        bool mainAction = interactKeypad || interactGamepad;
        bool secoundaryAction = dropKeypad || dropGamepad;

        if (m_modeGamepad)
        {
            m_modeGamepad = !(Input.GetKey(KeyCode.KeypadEnter) || Input.GetKey(KeyCode.Space) || interactKeypad || dropKeypad);// Button Enter / Space or any action keypad button
        }
        else
        {
            m_modeGamepad = Input.GetKey(KeyCode.JoystickButton0) || interactGamepad || dropGamepad; // Button A or any action gamepad button
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
            if (Physics.Raycast(ray, out hit, float.MaxValue,LayerMask.GetMask("Default")))
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
        }
        else
        {
            m_aimVector = m_player.transform.forward;
        }

        if (!m_mainAction && mainAction)
        {
            m_mainAction = true;
            m_mainActionDownEvent.Invoke();
        }
        else if (m_mainAction && !mainAction)
        {
            m_mainAction = false;
            m_mainActionReleaseEvent.Invoke();
        }

        if (!m_secoundaryAction && secoundaryAction)
        {
            m_secoundaryAction = true;
            m_secoundaryActionDownEvent.Invoke();
        }
        else if (m_secoundaryAction && !secoundaryAction)
        {
            m_secoundaryAction = false;
            m_secoundaryActionReleaseEvent.Invoke();
        }
    }
}
