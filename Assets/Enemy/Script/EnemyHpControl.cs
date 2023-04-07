using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHpControl : MonoBehaviour
{
    [Header("弱点をセットする")]
    [SerializeField] private List<EnemyWeakPointHp> _weakPonts = new List<EnemyWeakPointHp>();


    [SerializeField] private EnemyControl _enemyControl;


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

    public void DeadWealkPoint()
    {

    }

}
