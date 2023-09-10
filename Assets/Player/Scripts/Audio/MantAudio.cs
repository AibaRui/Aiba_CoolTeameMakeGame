using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MantAudio
{
    [Header("ƒ}ƒ“ƒg")]
    [SerializeField] private List<AudioClip> _mantAudio = new List<AudioClip>();

    private PlayerAudioManager _playerAudioManager;
    public void Init(PlayerAudioManager playerAudioManager)
    {
        _playerAudioManager = playerAudioManager;
    }

    public void PlayMant()
    {
        if (_mantAudio.Count == 0) return;

        var r = Random.Range(0, _mantAudio.Count);

        _playerAudioManager.PlayDeplicateAudio(_mantAudio[r]);

    }


}
