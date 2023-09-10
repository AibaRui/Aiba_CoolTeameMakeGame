using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelSetAudio : MonoBehaviour
{
    [SerializeField] private PlayerAudioManager _playerAudioManager;


    /// <summary>•à‚­‘«‰¹</summary>
    public void WalkStep()
    {
        if (_playerAudioManager.GroundAudio.WalkStepSounds.Count < 0)
        {
            return;
        }

        int r = Random.Range(0, _playerAudioManager.GroundAudio.WalkStepSounds.Count);

        _playerAudioManager.AudioSourceOnly.PlayOneShot(_playerAudioManager.GroundAudio.WalkStepSounds[r]);
    }

    /// <summary>‘–‚é‘«‰¹</summary>
    public void RunStep()
    {
        if (_playerAudioManager.GroundAudio.RunStepSounds.Count < 0)
        {
            return;
        }

        int r = Random.Range(0, _playerAudioManager.GroundAudio.RunStepSounds.Count);

        _playerAudioManager.AudioSourceOnly.PlayOneShot(_playerAudioManager.GroundAudio.RunStepSounds[r]);
    }


}
