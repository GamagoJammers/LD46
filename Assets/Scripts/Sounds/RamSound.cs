using System.Collections.Generic;
using UnityEngine;

public class RamSound : MonoBehaviour
{
    public Attacker m_attacker;
    public Damageable m_damageable;
    public RamNPC m_ramNPC;

    public AudioSourceControl m_audioSourceControl;
    public AudioSource m_chargeAudioSource;

    public List<AudioClip> m_attackSounds;
    public List<AudioClip> m_hitSounds;
    public List<AudioClip> m_chargeSounds;
    public AudioClip m_healSound;
    public List<AudioClip> m_deathSounds;

    public float m_attackVolume;
    public float m_hitVolume;
    public float m_healVolume;
    public float m_deathVolume;

    void Start()
    {
        m_attacker.m_attackEvent.AddListener(Attack);
        m_damageable.m_deathEvent.AddListener(Death);
        m_damageable.m_onDamageEvent.AddListener(Damage);
        m_damageable.m_onHealEvent.AddListener(Heal);
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
            m_damageable.m_deathEvent.RemoveListener(Death);
            m_damageable.m_onHealEvent.RemoveListener(Heal);
        }
    }

    private void Update()
    {
        if (transform.parent == null && !m_audioSourceControl.m_audioSource.isPlaying)
        {
            Destroy(gameObject);
            return;
        }

        bool isCharging = m_ramNPC.GetState() == RamNPC.RamNPCState.CHARGE;
        if ((GameManager.instance.isPaused || !isCharging) && m_chargeAudioSource.isPlaying)
        {
            m_chargeAudioSource.Stop();
        }
        else if (isCharging && !m_chargeAudioSource.isPlaying)
        {
            Charge();
        }
    }

    void Charge()
    {
        m_chargeAudioSource.Stop();
        m_chargeAudioSource.clip = m_attackSounds[Random.Range(0, m_chargeSounds.Count)];
        m_chargeAudioSource.Play();
    }

    void Attack(Damageable _damageable)
    {
        if (!m_audioSourceControl.m_audioSource.isPlaying)
        {
            m_audioSourceControl.m_audioSource.clip = m_attackSounds[Random.Range(0, m_attackSounds.Count)];
            m_audioSourceControl.SetLerpedVolume(m_attackVolume);
            m_audioSourceControl.m_audioSource.Play();
        }
    }

    void Death()
    {
        transform.parent = null;
        m_audioSourceControl.m_audioSource.Stop();
        m_audioSourceControl.m_audioSource.clip = m_deathSounds[Random.Range(0, m_deathSounds.Count)];
        m_audioSourceControl.SetLerpedVolume(m_deathVolume);
        m_audioSourceControl.m_audioSource.Play();
    }

    void Damage()
    {
        if (m_damageable.IsAlive())
        {
            m_audioSourceControl.m_audioSource.Stop();
            m_audioSourceControl.m_audioSource.clip = m_hitSounds[Random.Range(0, m_hitSounds.Count)];
            m_audioSourceControl.SetLerpedVolume(m_hitVolume);
            m_audioSourceControl.m_audioSource.Play();
        }
    }

    void Heal()
    {
        m_audioSourceControl.m_audioSource.Stop();
        m_audioSourceControl.m_audioSource.clip = m_healSound;
        m_audioSourceControl.SetLerpedVolume(m_healVolume);
        m_audioSourceControl.m_audioSource.Play();
    }
}