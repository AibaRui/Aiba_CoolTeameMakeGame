using Hikanyan_Assets.ShaderGraph;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPostEffectSetting : MonoBehaviour
{
    [SerializeField] private PostEffectAddFeature _postEffectFeature; // �C���X�y�N�^�[���犄�蓖��
    [SerializeField] private Material _material; // �C���X�y�N�^�[���犄�蓖��
    [SerializeField] private bool _isEnabled = true; // �C���X�y�N�^�[���犄�蓖��
    [SerializeField] private float _duration = 1.0f; // �ω��ɂ����鎞�ԁi�b�j

    public bool IsEnable => _isEnabled;

    private readonly int _impactRange = Shader.PropertyToID("_ImpactRange");
    private Coroutine _currentEffectRoutine;

    private void Start()
    {
        if (_postEffectFeature == null)
        {
            Debug.LogWarning("PostEffectAddFeature is not assigned.");
        }
        //��ʌ���PostEffect_Off
        _postEffectFeature.SetEffectEnabled(false);
        _material.SetFloat(0,0);
    }

    public void OnPostEffect()
    {
        _isEnabled = true;

        // �G�t�F�N�g��true�ɐ؂�ւ���ꍇ�A�����ŗL���ɂ���
        if (_isEnabled)
        {
            _postEffectFeature.SetEffectEnabled(true);
        }

        if (_currentEffectRoutine != null)
        {
            StopCoroutine(_currentEffectRoutine);
        }
        _currentEffectRoutine = StartCoroutine(ChangeEffectOverTime(_isEnabled ? 1.0f : 0.0f));
    }

    public void OffPostEffect()
    {
        _isEnabled = false;

        // �G�t�F�N�g��true�ɐ؂�ւ���ꍇ�A�����ŗL���ɂ���
        if (_isEnabled)
        {
            _postEffectFeature.SetEffectEnabled(true);
        }

        if (_currentEffectRoutine != null)
        {
            StopCoroutine(_currentEffectRoutine);
        }
        _currentEffectRoutine = StartCoroutine(ChangeEffectOverTime(_isEnabled ? 1.0f : 0.0f));
    }


    private IEnumerator ChangeEffectOverTime(float targetValue)
    {
        float startValue = _material.GetFloat(_impactRange);
        float time = 0;

        while (time < _duration)
        {
            _material.SetFloat(_impactRange, Mathf.Lerp(startValue, targetValue, time / _duration));
            time += Time.deltaTime;
            yield return null;
        }

        _material.SetFloat(_impactRange, targetValue); // �ŏI�I�Ȓl���m���ɐݒ�

        // �G�t�F�N�g��false�ɐ؂�ւ���ꍇ�A�C�[�W���O���I�����Ă��疳���ɂ���
        if (!_isEnabled)
        {
            _postEffectFeature.SetEffectEnabled(false);
        }
    }
}