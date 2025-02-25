using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ZipAudio
{

    [Header("Zipのコインを撃った時の音")]
    [SerializeField] private List<AudioClip> _zipAudioFire = new List<AudioClip>();

    [Header("風の音")]
    [SerializeField] private List<AudioClip> _zipAudio = new List<AudioClip>();

    private PlayerAudioManager _playerAudioManager;

    public void Init(PlayerAudioManager playerAudioManager)
    {
        _playerAudioManager = playerAudioManager;
    }

    public void ZipCoinFire()
    {
        if (_zipAudioFire == null) return;

        foreach (var clip in _zipAudioFire)
        {
            _playerAudioManager.PlayDeplicateAudio(clip);
        }
    }

    public void ZipAudioPlay()
    {
        if (_zipAudio == null) return;

        foreach (var clip in _zipAudio)
        {
            _playerAudioManager.PlayDeplicateAudio(clip);
        }
    }

}
