using System.Collections.Generic;
using UnityEngine;

public class TreeSound : MonoBehaviour
{
    public WoodenTree m_woodenTree;
    public AudioSourceControl m_audioSourceControl;

    public List<AudioClip> m_growSounds;
    public AudioClip m_dieSound;

    public float m_growVolume;
    public float m_dieVolume;

    void Awake()
    {
        m_woodenTree.m_growEvent.AddListener(PlayGrowSound);
        m_woodenTree.m_dieEvent.AddListener(PlayDieSound);
        m_audioSourceControl.m_audioSource.loop = false;
    }

    void OnDisable()
    {
        if (m_woodenTree != null)
        {
            m_woodenTree.m_growEvent.AddListener(PlayGrowSound);
            m_woodenTree.m_dieEvent.AddListener(PlayDieSound);
        }
    }

    void PlayGrowSound()
    {
        m_audioSourceControl.m_audioSource.Stop();
        m_audioSourceControl.m_audioSource.clip = m_growSounds[Random.Range(0, m_growSounds.Count)];
        m_audioSourceControl.SetLerpedVolume(m_growVolume);
        m_audioSourceControl.m_audioSource.Play();
    }

    void PlayDieSound()
    {
        transform.parent = null;
        m_audioSourceControl.m_audioSource.Stop();
        m_audioSourceControl.m_audioSource.clip = m_dieSound;
        m_audioSourceControl.SetLerpedVolume(m_dieVolume);
        m_audioSourceControl.m_audioSource.Play();
    }

    private void Update()
    {
        if (transform.parent == null && !m_audioSourceControl.m_audioSource.isPlaying)
        {
            Destroy(gameObject);
        }
    }
}
