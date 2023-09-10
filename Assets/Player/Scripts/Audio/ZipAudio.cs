using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ZipAudio 
{

    [Header("Zip‚µ‚½Žž‚Ì‰¹")]
    [SerializeField] private AudioClip _zipAudio;

    private PlayerAudioManager _playerAudioManager;

    public void Init(PlayerAudioManager playerAudioManager)
    {
        _playerAudioManager = playerAudioManager;
    }

    public void ZipAudioPlay()
    {
        if (_zipAudio == null) return;
        _playerAudioManager.PlayDeplicateAudio(_zipAudio);
    }

}
