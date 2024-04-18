using Hikanyan_Assets.ShaderGraph;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPostEffectSetting : MonoBehaviour
{
    [SerializeField] private PostEffectAddFeature _postEffectFeature; // インスペクターから割り当て
    [SerializeField] private Material _material; // インスペクターから割り当て
    [SerializeField] private bool _isEnabled = true; // インスペクターから割り当て
    [SerializeField] private float _duration = 1.0f; // 変化にかける時間（秒）

    public bool IsEnable => _isEnabled;

    private readonly int _impactRange = Shader.PropertyToID("_ImpactRange");
    private Coroutine _currentEffectRoutine;

    private void Start()
    {
        if (_postEffectFeature == null)
        {
            Debug.LogWarning("PostEffectAddFeature is not assigned.");
        }
        //画面効果PostEffect_Off
        _postEffectFeature.SetEffectEnabled(false);
        _material.SetFloat(0,0);
    }

    public void OnPostEffect()
    {
        _isEnabled = true;

        // エフェクトをtrueに切り替える場合、ここで有効にする
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

        // エフェクトをtrueに切り替える場合、ここで有効にする
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

        _material.SetFloat(_impactRange, targetValue); // 最終的な値を確実に設定

        // エフェクトをfalseに切り替える場合、イージングが終了してから無効にする
        if (!_isEnabled)
        {
            _postEffectFeature.SetEffectEnabled(false);
        }
    }
}
