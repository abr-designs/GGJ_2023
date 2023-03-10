using System;
using GGJ.Audio;
using UnityEngine;

public class SFXTriggerTest : MonoBehaviour
{
    [SerializeField] private SFX _sfxType;
    //[SerializeField] private Transform _targetTransform;

    [ContextMenu("TriggerPlaySound")]
    void TriggerPlaySound()
    {
        PlaySound(_sfxType);
    }

    public void PlaySound(SFX sfx)
    {
        try
        {
            SFXController.PlaySound(sfx, 1f);
        }
        catch (NullReferenceException e)
        {
            Debug.Log("SFXManager not set\n" + e);
        }
    }
}
