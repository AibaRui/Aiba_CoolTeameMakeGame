using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class PlayerEffectControl
{
    [Header("集中線")]
    [SerializeField] private Image _concentrationLineeffectImage;

    [Header("集中線の最大透明度")]
    [SerializeField] private float _concentrationLineeffectMaxColorAlpha;

    [Header("集中線を有効にする速度")]
    [SerializeField] private float _useConcentrationLineeffectVelocity = -20;

    [Header("Zip")]
    [SerializeField] private GameObject _zipImage;

    [SerializeField] private bool _isOnEffect;

    [SerializeField] private ExampleUsage _exampleUsage;

    private PlayerControl _playerControl;

    private Color _concentrationLineeffectColor;

    public void Init(PlayerControl playerControl)
    {
        _playerControl = playerControl;
    }


    /// <summary>集中線の管理</summary>
    public void ConcentrationLineEffect()
    {
        if (!_isOnEffect) return;

        Vector3 speed = _playerControl.Rb.velocity;
        speed.y = 0;

        if ((_playerControl.Rb.velocity.y <= _useConcentrationLineeffectVelocity) || (_playerControl.Swing.IsSwingNow && speed.magnitude >= 30))
        {
            if (_concentrationLineeffectImage.color.a >= _concentrationLineeffectMaxColorAlpha)
            {
                return;
            }

           // if (!_exampleUsage.isEnabled) _exampleUsage.isEnabled = true;

            var setColor = _concentrationLineeffectImage.color;
            setColor.a += Time.deltaTime;
            if (_concentrationLineeffectMaxColorAlpha - setColor.a < 0.1f)
            {
                setColor.a = _concentrationLineeffectMaxColorAlpha;
                _concentrationLineeffectImage.color = setColor;
            }
            else
            {
                _concentrationLineeffectImage.color = setColor;
            }
        }
        else
        {
            if (_concentrationLineeffectImage.color.a <= 0)
            {
                return;
            }

            //if (_exampleUsage.isEnabled) _exampleUsage.isEnabled = false;

            var setColor = _concentrationLineeffectImage.color;
            setColor.a -= Time.deltaTime;

            if (setColor.a < 0f)
            {
                setColor.a = 0;
                _concentrationLineeffectImage.color = setColor;
            }
            else
            {
                _concentrationLineeffectImage.color = setColor;
            }
        }
    }

    public void ZipSet(bool isOn)
    {
       //s _zipImage.SetActive(isOn);
    }
}
