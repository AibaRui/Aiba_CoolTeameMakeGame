using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class BossHp
{
    [Header("左腕の弱点")]
    [SerializeField] private BossWeakPoint _leftWeakPoint;

    [Header("右腕の弱点")]
    [SerializeField] private BossWeakPoint _rightWeakPoint;

    [Header("中央の弱点")]
    [SerializeField] private BossWeakPoint _centerWeakPoint;

    [Header("腕の体力")]
    [SerializeField] private int _armHP = 3;

    [Header("胴体の体力")]
    [SerializeField] private int _bodyHP = 3;

    [Header("腕の弱点のオブジェクト")]
    [SerializeField] private GameObject _armWeakPoint;

    [Header("胸の弱点のオブジェクト")]
    [SerializeField] private GameObject _bodyWeakPoint;

    [Header("弱点のレイヤー")]
    [SerializeField] private LayerMask _layer;

    [Header("弱点のレイヤーでなくする")]
    [SerializeField] private LayerMask _outLayer;
    private bool _isDamage = false;

    private bool _isAllBrake = false;

    public bool IsDamage => _isDamage;

    private BossControl _bossControl;
    public void Init(BossControl bossControl)
    {
        _bossControl = bossControl;
        _leftWeakPoint.Init(this);
        _rightWeakPoint.Init(this);
        _centerWeakPoint.Init(this);
        _centerWeakPoint.gameObject.SetActive(false);
    }

    public void DamageEnd()
    {
        _isDamage = false;

        if(_isAllBrake)
        {
            BreakeFrontWeakPoint();
        }
    }

    public void BreakeArmWeakPoint()
    {
        _rightWeakPoint.gameObject.SetActive(false);
        _leftWeakPoint.gameObject.SetActive(false);
        _centerWeakPoint.gameObject.SetActive(true);
        _armWeakPoint.gameObject.SetActive(false);
    }

    public void BreakeFrontWeakPoint()
    {
        SceneManager.LoadScene("GameScene_TestView_EndMovie");
    }


    public void Damage(DamageType type, BossDamagePoint bossDamagePoint)
    {
        if (_isDamage) return;

        _isDamage = true;

        //アニメーション再生
        _bossControl.AnimControl.Damage(bossDamagePoint);


        if (bossDamagePoint == BossDamagePoint.RightArm)
        {
            _armHP--;

            if (_armHP <= 0)
            {
                _bossControl.HitDirection.BossDamageStartDirection(BossDirectionType.Breake, false, 0);
                BreakeArmWeakPoint();
            }
            else
            {
                _bossControl.HitDirection.BossDamageStartDirection(BossDirectionType.Hit, true, 0);
            }

        }
        else if (bossDamagePoint == BossDamagePoint.LeftArm)
        {
            _armHP--;

            if (_armHP <= 0)
            {
                _bossControl.HitDirection.BossDamageStartDirection(BossDirectionType.Breake, false, 0);
                BreakeArmWeakPoint();
            }
            else
            {
                _bossControl.HitDirection.BossDamageStartDirection(BossDirectionType.Hit, true, 0);
            }
        }
        else
        {
            _bodyHP--;

            if (_bodyHP <= 0)
            {
                _bossControl.HitDirection.BossDamageStartDirection(BossDirectionType.Breake, false, 1);
                _isAllBrake= true;
            }
            else
            {
                _bossControl.HitDirection.BossDamageStartDirection(BossDirectionType.Hit, false, 0);
            }
        }


    }

}

public enum BossDamagePoint
{
    RightArm,
    LeftArm,
    Center,
}