using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LoopAudio
{
    [Header("風の音を鳴らすAudioSorce")]
    [SerializeField] private AudioSource _audioSourceWind;

    [Header("マントの音を鳴らすAudioSorce")]
    [SerializeField] private AudioSource _audioSourceMant;

    private PlayerAudioManager _playerAudioManager;

    [Header("風の音の最大Volume")]
    [SerializeField] private float _maxWindSoundVolume;
    [Header("風の音の変更速度")]
    [SerializeField] private float _windSoundChangeSpeed = 1;

    [Header("マントの音の最大Volume")]
    [SerializeField] private float _maxMantSoundVolume;
    [Header("マントの音の変更速度")]
    [SerializeField] private float _mantSoundChangeSpeed = 1;

    private bool _isWind = false;

    private bool _isMant = false;



    public void Init(PlayerAudioManager playerAudioManager)
    {
        _playerAudioManager = playerAudioManager;
    }

    public void WindSetting()
    {
        if (_isWind)
        {
            if (_audioSourceWind.volume < _maxWindSoundVolume)
            {
                _audioSourceWind.volume += Time.deltaTime * _windSoundChangeSpeed;
            }

            if (_audioSourceWind.volume > _maxWindSoundVolume)
            {
                _audioSourceWind.volume = _maxWindSoundVolume;
            }
        }
        else
        {
            if (_audioSourceWind.volume > 0)
            {
                _audioSourceWind.volume -= Time.deltaTime * _windSoundChangeSpeed;

                if (_audioSourceWind.volume <= 0)
                {
                    _audioSourceWind.volume = 0;
                    _audioSourceWind.Stop();
                }
            }
        }
    }

    public void MantSetting()
    {
        if (_isMant)
        {
            if (_audioSourceMant.volume < _maxMantSoundVolume)
            {
                _audioSourceMant.volume += Time.deltaTime * _maxMantSoundVolume;
            }

            if (_audioSourceMant.volume > _maxWindSoundVolume)
            {
                _audioSourceMant.volume = _maxMantSoundVolume;
            }
        }
        else
        {
            if (_audioSourceMant.volume > 0)
            {
                _audioSourceMant.volume -= Time.deltaTime * _maxMantSoundVolume;

                if (_audioSourceMant.volume <= 0)
                {
                    _audioSourceMant.volume = 0;
                    _audioSourceMant.Stop();
                }
            }
        }
    }

    public void SettingLoopAudio()
    {
        WindSetting();
        MantSetting();
    }

    public void PlayWindAudio(bool isPlay)
    {
        if (isPlay)
        {
            if (!_audioSourceWind.isPlaying)
            {
                _audioSourceWind.volume = 0;
                _audioSourceWind.Play();
                _isWind = true;
            }
        }
        else
        {
            _isWind = false;
        }
    }

    public void PlayMantAudio(bool isPlay)
    {
        if (isPlay)
        {
            if (_audioSourceMant.isPlaying) return;

            _audioSourceMant.volume = 0;
            _audioSourceMant.Play();
            _isMant = true;
        }
        else
        {
            _isMant = false;
        }
    }



}
