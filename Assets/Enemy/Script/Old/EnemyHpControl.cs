using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHpControl : MonoBehaviour
{
    [Header("��_���Z�b�g����")]
    [SerializeField] private List<EnemyWeakPointHp> _weakPonts = new List<EnemyWeakPointHp>();

    [SerializeField] private EnemyControl _enemyControl;

    /// <summary>�j�󂳂ꂽ�A��_�̐�</summary>
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

    /// <summary>��_��1�A�j�󂳂ꂽ���̏���</summary>
    public void DeadWealkPoint()
    {
        _weakPointDeadNum++;

        //��_�����ׂĔj�󂳂ꂽ��A�I���
        if (_weakPointDeadNum == _weakPonts.Count)
        {
            _enemyControl.Dead();
        }
    }

}