using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class SwingAudio
{
    [Header("ワイヤーを飛ばすときの音")]
    [SerializeField] private List<AudioClip> _wireFireSounds = new List<AudioClip>();

    [Header("Swing中の風の音")]
    [SerializeField] private AudioClip _windAudio;

    [Header("Swing後、高くジャンプしたときの音")]
    [SerializeField] private List<AudioClip> _swingUpEndSounds = new List<AudioClip>();

    [Header("Swing後、前方にジャンプしたときの音")]
    [SerializeField] private List<AudioClip> _swingFrontEndSounds = new List<AudioClip>();

    [Header("Swing後、ボタンを離したときの音")]
    [SerializeField] private List<AudioClip> _swingEndSounds = new List<AudioClip>();

    private PlayerAudioManager _playerAudioManager;

    public void Init(PlayerAudioManager playerAudioManager)
    {
        _playerAudioManager = playerAudioManager;
    }


    /// <summary>ワイヤーを飛ばす音</summary>
    public void WireFireSounds()
    {
        if (_wireFireSounds.Count == 0)
        {
            return;
        }

        int r = Random.Range(0, _wireFireSounds.Count);

        _playerAudioManager.PlayDeplicateAudio(_wireFireSounds[r]);
    }


    /// <summary>上方向にジャンプ</summary>
    public void UpJumpSounds()
    {
        if (_swingUpEndSounds.Count == 0)
        {
            return;
        }

        int r = Random.Range(0, _swingUpEndSounds.Count);

        _playerAudioManager.PlayDeplicateAudio(_swingUpEndSounds[r]);
    }

    /// <summary>前方向にジャンプ</summary>
    public void FrontJumpSounds()
    {
        if (_swingFrontEndSounds.Count == 0)
        {
            return;
        }
        int r = Random.Range(0, _swingFrontEndSounds.Count);

        _playerAudioManager.PlayDeplicateAudio(_swingFrontEndSounds[r]);
    }

    /// <summary>Swing終了のジャンプ</summary>
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
