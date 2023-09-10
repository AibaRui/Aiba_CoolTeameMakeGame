using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudioManager : MonoBehaviour
{
    [Header("地上にいるときの音源")]
    [SerializeField] private GroundAudio _groundAudio;

    [Header("空中にいるときの音源")]
    [SerializeField] private AirAudio _airAudio;

    [Header("Swing時のの音源")]
    [SerializeField] private SwingAudio _swingAudio;

    [Header("Zipの音")]
    [SerializeField] private ZipAudio _zipAudio;

    [Header("ループする音")]
    [SerializeField] private LoopAudio _loopAudio;

    [Header("マントの音")]
    [SerializeField] private MantAudio _mantAudio;

    [Header("重複しない音")]
    [SerializeField] private AudioSource _audioSourceOnly;

    [Header("重複する音")]
    [SerializeField] private List<AudioSource> _deplicateAudioSorce = new List<AudioSource>();

    [SerializeField] private PlayerControl _playerControl;

    public LoopAudio LoopAudio => _loopAudio;
    public PlayerControl PlayerControl => _playerControl;
    public AudioSource AudioSourceOnly => _audioSourceOnly;
    public List<AudioSource> DeplicateAudioSorce => _deplicateAudioSorce;
    public ZipAudio ZipAudio => _zipAudio;
    public GroundAudio GroundAudio => _groundAudio;
    public SwingAudio SwingAudio => _swingAudio;
    public AirAudio AirAudio => _airAudio;
    public MantAudio MantAudio => _mantAudio;

    private void Start()
    {
        _groundAudio.Init(this);
        _airAudio.Init(this);
        _swingAudio.Init(this);
        _zipAudio.Init(this);
        _loopAudio.Init(this);
        _mantAudio.Init(this);
    }

    public void PlayDeplicateAudio(AudioClip audioClip)
    {
        foreach (var a in _deplicateAudioSorce)
        {
            if (!a.isPlaying)
            {
                a.PlayOneShot(audioClip);
                return;
            }
        }
    }


}
