using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampfireSound : MonoBehaviour
{

    public float m_volumeStartToDecreaseAt;
    public AudioSourceControl m_campfireSound;
    public Campfire m_campfire;

    void Update()
    {
        float volume = m_campfire.GetVivacityPercentage();
        if (volume > m_volumeStartToDecreaseAt)
        {
            volume = 1;
        }
        else
        {
            volume = Mathf.Lerp(0, m_volumeStartToDecreaseAt, volume);
        }
        m_campfireSound.SetLerpedVolume(volume);
    }
}
