using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class SwingAudio
{
    [Header("���C���[���΂��Ƃ��̉�")]
    [SerializeField] private List<AudioClip> _wireFireSounds = new List<AudioClip>();

    [Header("Swing���̕��̉�")]
    [SerializeField] private AudioClip _windAudio;

    [Header("Swing��A�����W�����v�����Ƃ��̉�")]
    [SerializeField] private List<AudioClip> _swingUpEndSounds = new List<AudioClip>();

    [Header("Swing��A�O���ɃW�����v�����Ƃ��̉�")]
    [SerializeField] private List<AudioClip> _swingFrontEndSounds = new List<AudioClip>();

    [Header("Swing��A�{�^���𗣂����Ƃ��̉�")]
    [SerializeField] private List<AudioClip> _swingEndSounds = new List<AudioClip>();

    private PlayerAudioManager _playerAudioManager;

    public void Init(PlayerAudioManager playerAudioManager)
    {
        _playerAudioManager = playerAudioManager;
    }


    /// <summary>���C���[���΂���</summary>
    public void WireFireSounds()
    {
        if (_wireFireSounds.Count == 0)
        {
            return;
        }

        int r = Random.Range(0, _wireFireSounds.Count);

        _playerAudioManager.PlayDeplicateAudio(_wireFireSounds[r]);
    }


    /// <summary>������ɃW�����v</summary>
    public void UpJumpSounds()
    {
        if (_swingUpEndSounds.Count == 0)
        {
            return;
        }

        int r = Random.Range(0, _swingUpEndSounds.Count);

        _playerAudioManager.PlayDeplicateAudio(_swingUpEndSounds[r]);
    }

    /// <summary>�O�����ɃW�����v</summary>
    public void FrontJumpSounds()
    {
        if (_swingFrontEndSounds.Count == 0)
        {
            return;
        }
        int r = Random.Range(0, _swingFrontEndSounds.Count);

        _playerAudioManager.PlayDeplicateAudio(_swingFrontEndSounds[r]);
    }

    /// <summary>Swing�I���̃W�����v</summary>
    public void SwingEndSounds()
    {
        if (_swingEndSounds.Count == 0)
        {
            return;
        }

        int r = Random.Range(0, _swingEndSounds.Count);

        _playerAudioManager.PlayDeplicateAudio(_swingEndSounds[r]);
    }

}