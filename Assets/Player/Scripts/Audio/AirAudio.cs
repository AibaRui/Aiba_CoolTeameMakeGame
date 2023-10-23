using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AirAudio 
{
    [Header("—Ž‰º‚Ì‰¹")]
    [SerializeField] private AudioClip _audioClip;

    private PlayerAudioManager _playerAudioManager;

    public void Init(PlayerAudioManager playerAudioManager)
    {
        _playerAudioManager = playerAudioManager;
    }


}
