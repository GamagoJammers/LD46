using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    public Vector3 m_offset;
    public Canvas m_canvas;
    public Image m_refill;

    private GameObject m_target;

    private void Start()
    {
        m_canvas.worldCamera = Camera.main;
        m_refill.fillAmount = 1;
        m_canvas.gameObject.SetActive(false);
    }

    void Update()
    {
        if (m_canvas.gameObject.activeSelf)
        {
            if (m_target == null)
            {
                Hide();
            }
            else
            {
                SetPositionAndAxis();
            }
        }
    }

    public void Show(GameObject _target)
    {
        m_target = _target;
        m_canvas.gameObject.SetActive(true);
        SetPositionAndAxis();
    }

    public void Hide()
    {
        m_canvas.gameObject.SetActive(false);
    }

    public void SetFillAmount(float _fillAmount)
    {
        m_refill.fillAmount = _fillAmount;
    }

    void SetPositionAndAxis()
    {
        transform.position = m_target.transform.position + m_offset;
        transform.LookAt(transform.position + Camera.main.transform.forward);
    }
}
