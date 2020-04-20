using System.Collections.Generic;
using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    public Animator m_playerAnimator;
    public AudioSourceControl m_audioSourceControl;

    public List<AudioClip> m_hitSounds;
    public AudioClip m_choppingSound;
    public List<AudioClip> m_throwSounds;

    public float m_hitVolume;
    public float m_choppingVolume;
    public float m_throwVolume;

    public enum State { NO_SOUND, HIT, CHOPPING, THROWING }
    State m_state;

    void Start()
    {
        m_state = State.NO_SOUND;
    }

    // Update is called once per frame
    void Update()
    {
        ProcesseAnimationToSound();
    }

    void ProcesseAnimationToSound()
    {
        AnimatorStateInfo info = m_playerAnimator.GetCurrentAnimatorStateInfo(0);
        if (info.IsName("Hit"))
        {
            SetState(State.HIT);
        } else if (info.IsName("Sawing"))
        {
            SetState(State.CHOPPING);
        } else if (info.IsName("Throwing"))
        {
            SetState(State.THROWING);
        } else
        {
            SetState(State.NO_SOUND);
        }
    }

    void SetState(State _state)
    {
        if (_state != m_state)
        {
            m_state = _state;
            switch (_state)
            {
                case State.NO_SOUND:
                    {
                        if (m_audioSourceControl.m_audioSource.isPlaying && m_audioSourceControl.m_audioSource.loop)
                        {
                            m_audioSourceControl.m_audioSource.Stop();
                        }
                        break;
                    }

                case State.HIT:
                    {

                        m_audioSourceControl.m_audioSource.Stop();
                        m_audioSourceControl.m_audioSource.clip = m_hitSounds[Random.Range(0, m_hitSounds.Count)];
                        m_audioSourceControl.m_audioSource.loop = false;
                        m_audioSourceControl.SetLerpedVolume(m_hitVolume);
                        m_audioSourceControl.m_audioSource.Play();

                        break;
                    }

                case State.CHOPPING:
                    {
                        m_audioSourceControl.m_audioSource.Stop();
                        m_audioSourceControl.m_audioSource.clip = m_choppingSound;
                        m_audioSourceControl.m_audioSource.loop = true;
                        m_audioSourceControl.SetLerpedVolume(m_choppingVolume);
                        m_audioSourceControl.m_audioSource.Play();
                        break;
                    }

                case State.THROWING:
                    {
                        m_audioSourceControl.m_audioSource.Stop();
                        m_audioSourceControl.m_audioSource.clip = m_throwSounds[Random.Range(0, m_hitSounds.Count)];
                        m_audioSourceControl.m_audioSource.loop = false;
                        m_audioSourceControl.SetLerpedVolume(m_throwVolume);
                        m_audioSourceControl.m_audioSource.Play();

                        break;
                    }
            }
        }
    }
}
