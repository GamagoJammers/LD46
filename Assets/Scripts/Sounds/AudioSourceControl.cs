using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourceControl : MonoBehaviour
{
    // Parameters
    public float m_defaultVolume;
    public Vector2 m_rangeVolume;

    public AudioSource m_audioSource;

    private void Awake()
    {
        m_audioSource = GetComponent<AudioSource>();
        m_rangeVolume.x = Mathf.Clamp(m_rangeVolume.x, 0, 1);
        m_rangeVolume.y = Mathf.Clamp(m_rangeVolume.y, m_rangeVolume.x, 1);
        m_defaultVolume = Mathf.Clamp(m_defaultVolume, m_rangeVolume.x, m_rangeVolume.y);
    }

    public void SetRawVolume(float _volume)
    {
        _volume = Mathf.Clamp(_volume, m_rangeVolume.x, m_rangeVolume.y);
        m_audioSource.volume = _volume;
    }

    public void SetLerpedVolume(float _volume)
    {
        _volume = Mathf.Clamp(_volume, 0.0f, 1.0f);
        _volume = Mathf.Lerp(m_rangeVolume.x, m_rangeVolume.y, _volume);
        m_audioSource.volume = _volume;
    }


    void Start()
    {
        SetRawVolume(m_defaultVolume);
    }
}
