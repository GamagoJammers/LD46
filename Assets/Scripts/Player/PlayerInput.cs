using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public bool m_modeGamepad;

    public Vector3 m_movementVector;
    public Vector3 m_aimVector;
    public bool m_interact;

    public Vector3 m_cameraForward;

    private GameObject m_player;
    private GameObject m_camera;


    private void Start()
    {
        m_modeGamepad = false;
        m_aimVector = Vector2.down;
        m_player = GameObject.FindGameObjectWithTag("player");
        m_camera = GameObject.FindGameObjectWithTag("MainCamera");
    }

    void Update()
    {
        m_cameraForward = m_camera.transform.forward;
        m_cameraForward.y = 0;
        m_cameraForward.Normalize();

        bool interactKeypad = Input.GetKey(KeyCode.Mouse0) || Input.GetKey(KeyCode.Mouse1);  // Left or Right Click
        bool interactGamepad = Input.GetKey(KeyCode.JoystickButton5) || Input.GetAxis("JoystickRightTrigger") != 0; // RightBumper or Right Trigger

        if (m_modeGamepad)
        {
            m_modeGamepad = !(Input.GetKey(KeyCode.KeypadEnter) || Input.GetKey(KeyCode.Space) || interactKeypad);// Button Enter / Space or any interact keypad button
        }
        else
        {
            m_modeGamepad = Input.GetKey(KeyCode.JoystickButton0) || interactGamepad; // Button A or any interact gamepad button
        }

        m_interact = interactGamepad || interactKeypad;

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

        if (direction != Vector3.zero)
        {
            // If null vector, keep the previous one
            m_aimVector = direction.normalized;
        }
    }
}
