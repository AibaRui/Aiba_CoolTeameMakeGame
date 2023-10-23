using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GroundAudio
{
    [Header("歩いている時の足音")]
    [SerializeField] private List<AudioClip> _walkStepSounds = new List<AudioClip>();

    [Header("走っている時の足音")]
    [SerializeField] private List<AudioClip> _runStepSounds = new List<AudioClip>();

    [Header("ジャンプした時の音")]
    [SerializeField] private AudioClip _jumpAudio;

    [Header("着地の音")]
    [SerializeField] private AudioClip _landAudio;

    [Header("高所から着地した時の音")]
    [SerializeField] private AudioClip _landHighFallAudio;


    private PlayerAudioManager _playerAudioManager;

    public List<AudioClip> WalkStepSounds => _walkStepSounds;
    public List<AudioClip> RunStepSounds => _runStepSounds;
    public AudioClip JumpAudio => _jumpAudio;
    public AudioClip LandAudio => _landAudio;


    public void Init(PlayerAudioManager playerAudioManager)
    {
        _playerAudioManager = playerAudioManager;
    }

    /// <summary>着地の音</summary>
    public void LandAudioPlay()
    {
        if (_playerAudioManager.PlayerControl.Rb.velocity.y < -19)
        {
            if (_landHighFallAudio == null) return;

            _playerAudioManager.PlayDeplicateAudio(_landHighFallAudio);
        }
        else
        {
            if (_landAudio == null) return;
            _playerAudioManager.PlayDeplicateAudio(_landAudio);
        }

    }

    /// <summary>ジャンプの音</summary>
    public void JumpAudioPlay()
    {
        if (_jumpAudio == null) return;
        _playerAudioManager.PlayDeplicateAudio(_jumpAudio);
    }




}
