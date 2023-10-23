using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ZipAudio
{

    [Header("Zip‚ÌƒRƒCƒ“‚ðŒ‚‚Á‚½Žž‚Ì‰¹")]
    [SerializeField] private List<AudioClip> _zipAudioFire = new List<AudioClip>();

    [Header("•—‚Ì‰¹")]
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
