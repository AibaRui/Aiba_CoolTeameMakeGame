using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovieLineRenderer : MonoBehaviour
{
    [SerializeField] private List<LineRenderPosInfo> _info;

    [SerializeField] private LineRenderer _lineRenderer;

    private bool _isLine = false;

    private int _count = 0;

    public void SetLine()
    {
        _isLine = true;

        _lineRenderer.positionCount = 2;
        _lineRenderer.SetPosition(0, _info[_count].Pos1.position);
        _lineRenderer.SetPosition(1, _info[_count].Pos2.position);
    }

    /// <summary>LineRenderÇè¡Ç∑</summary>
    public void OffLine()
    {
        _isLine = false;

        _lineRenderer.positionCount = 0;
        _count++;

        if (_count == _info.Count)
        {
            this.gameObject.SetActive(false);
        }

    }

    private void LateUpdate()
    {
              if(_isLine)
        {
            _lineRenderer.SetPosition(0, _info[_count].Pos1.position);
            _lineRenderer.SetPosition(1, _info[_count].Pos2.position);
        }  
    }



}

[System.Serializable]
public class LineRenderPosInfo
{
    [Header("åqÇÆà íu_1")]
    [SerializeField] private Transform _pos1;

    [Header("åqÇÆà íu_2")]
    [SerializeField] private Transform _pos2;

    public Transform Pos1 => _pos1;
    public Transform Pos2 => _pos2;
}