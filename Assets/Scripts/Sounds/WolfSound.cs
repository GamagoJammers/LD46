using System.Collections.Generic;
using UnityEngine;

public class WolfSound : MonoBehaviour
{
    public Attacker m_attacker;
    public Damageable m_damageable;
    public PickerSensor m_picker;
    public AudioSourceControl m_audioSourceControl;

    public List<AudioClip> m_hitSounds;
    public AudioClip m_grabSound;
    public List<AudioClip> m_attackSounds;

    public float m_hitVolume;
    public float m_grabVolume;
    public float m_attackVolume;

    void Start()
    {
        m_audioSourceControl.m_audioSource.loop = false;

        m_attacker.m_attackEvent.AddListener(Attack);
        m_damageable.m_onDamageEvent.AddListener(Damage);
        if (m_picker != null)
        {
            m_picker.m_pickEvent.AddListener(Grab);
        }
    }

    void OnDisable()
    {
        if (m_attacker != null)
        {
            m_attacker.m_attackEvent.RemoveListener(Attack);
        }

        if (m_damageable != null)
        {
            m_damageable.m_onDamageEvent.RemoveListener(Damage);
        }

        if (m_picker != null)
        {
            m_picker.m_pickEvent.RemoveListener(Grab);
        }
    }

    void Attack(Damageable _damageable)
    {
        m_audioSourceControl.m_audioSource.Stop();
        m_audioSourceControl.m_audioSource.clip = m_attackSounds[Random.Range(0, m_attackSounds.Count)];
        m_audioSourceControl.SetLerpedVolume(m_attackVolume);
        m_audioSourceControl.m_audioSource.Play();
    }

    void Grab()
    {
        m_audioSourceControl.m_audioSource.Stop();
        m_audioSourceControl.m_audioSource.clip = m_grabSound;
        m_audioSourceControl.SetLerpedVolume(m_grabVolume);
        m_audioSourceControl.m_audioSource.Play();
    }

    void Damage()
    {
        m_audioSourceControl.m_audioSource.Stop();
        m_audioSourceControl.m_audioSource.clip = m_hitSounds[Random.Range(0, m_hitSounds.Count)];
        m_audioSourceControl.SetLerpedVolume(m_hitVolume);
        m_audioSourceControl.m_audioSource.Play();
    }
}
