using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowAim : MonoBehaviour
{

    public GameObject arrow;
    public GameObject circle;
    public PickerSensor m_picker;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (m_picker.IsCarryingPickable())
        {
            arrow.SetActive(true);
            circle.SetActive(true);
        }
        else if (!m_picker.IsCarryingPickable())
        {
            arrow.SetActive(false);
            circle.SetActive(false);
        }
    }

}
