using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GroundAudio
{
    [Header("�����Ă��鎞�̑���")]
    [SerializeField] private List<AudioClip> _walkStepSounds = new List<AudioClip>();

    [Header("�����Ă��鎞�̑���")]
    [SerializeField] private List<AudioClip> _runStepSounds = new List<AudioClip>();

    [Header("�W�����v�������̉�")]
    [SerializeField] private AudioClip _jumpAudio;

    [Header("���n�̉�")]
    [SerializeField] private AudioClip _landAudio;

    [Header("�������璅�n�������̉�")]
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

    /// <summary>���n�̉�</summary>
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

    /// <summary>�W�����v�̉�</summary>
    public void JumpAudioPlay()
    {
        if (_jumpAudio == null) return;
        _playerAudioManager.PlayDeplicateAudio(_jumpAudio);
    }




}