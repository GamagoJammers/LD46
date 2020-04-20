using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainSound : MonoBehaviour
{
    public RainManager m_rainManager;
    public AudioSource m_rainAudioSource;
    public AudioSource m_thunderAudioSource;

    public List<AudioClip> m_thunderSounds;

    private void Start()
    {
        m_rainManager.m_thunderEvent.AddListener(PlayThunder);
    }

    private void OnDisable()
    {
        if(m_rainManager != null)
        {
            m_rainManager.m_thunderEvent.RemoveListener(PlayThunder);
        }
    }

    void Update()
    {
        if (m_rainAudioSource.isPlaying && !m_rainManager.IsRaining())
        {
            m_rainAudioSource.Stop();
        } else if (!m_rainAudioSource.isPlaying && m_rainManager.IsRaining())
        {
            m_rainAudioSource.Play();
        }
    }

    void PlayThunder(Vector3 _position)
    {
        m_thunderAudioSource.Stop();
        m_thunderAudioSource.transform.position = _position; // Assuming parent is on Vector.zero
        m_thunderAudioSource.clip = m_thunderSounds[Random.Range(0, m_thunderSounds.Count)];
        m_thunderAudioSource.Play();
    }
}
