using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovieActiver : MonoBehaviour
{
    [Header("非/表示　したいオブジェクト")]
    [SerializeField] private GameObject _obj;

    [Header("非/表示　したいオブジェクト2")]
    [SerializeField] private GameObject _obj2;

    [SerializeField] private Animator _anim;

    [Header("ワイヤーを繋ぎたい位置")]
    [SerializeField] private Transform _wireHitPo;

    [Header("ワイヤーを繋ぐ腕の位置")]
    [SerializeField] private Transform _wireArmPos;

    [Header("Joint")]
    [SerializeField] private LineRenderer _lineRenderer;

    private bool _isJoint = false;
    public void OnActive()
    {
        _obj.SetActive(true);
    }

    public void OnOff()
    {
        _obj.SetActive(false);
    }

    public void OnActive2()
    {
        _obj2.SetActive(true);
    }

    public void OnOff2()
    {
        _obj2.SetActive(false);
    }


    public void NextAnimation()
    {
        _anim.SetTrigger("Next");
    }


    public void Onjoint()
    {
        Debug.Log("a");
        _isJoint = true;
        _lineRenderer.positionCount = 2;
        _lineRenderer.SetPosition(1, _wireArmPos.position);
        _lineRenderer.SetPosition(0, _wireHitPo.position);
    }

    public void OffJoint()
    {
        _isJoint = false;
        _lineRenderer.positionCount = 0;
    }

    private void LateUpdate()
    {
        if (_isJoint)
        {
            _lineRenderer.SetPosition(0, _wireHitPo.position);
            _lineRenderer.SetPosition(1, _wireArmPos.position);
            Debug.Log("joint");
        }
    }




}
