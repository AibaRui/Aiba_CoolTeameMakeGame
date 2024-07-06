using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHpControl : MonoBehaviour
{
    [Header("弱点をセットする")]
    [SerializeField] private List<EnemyWeakPointHp> _weakPonts = new List<EnemyWeakPointHp>();

    [SerializeField] private EnemyControl _enemyControl;

    /// <summary>破壊された、弱点の数</summary>
    private int _weakPointDeadNum = 0;

    void Start()
    {
        foreach(var a in _weakPonts)
        {
            a.Init(_enemyControl, this);
        }
    }


    void Update()
    {

    }

    /// <summary>弱点が1つ、破壊された時の処理</summary>
    public void DeadWealkPoint()
    {
        _weakPointDeadNum++;

        //弱点がすべて破壊されたら、終わり
        if (_weakPointDeadNum == _weakPonts.Count)
        {
            _enemyControl.Dead();
        }
    }

}
