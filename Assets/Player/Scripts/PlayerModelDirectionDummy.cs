using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModelDirectionDummy : MonoBehaviour
{
    [Header("プレイヤーのマテリアル")]
    [Header("Arnmfoot")]
    [SerializeField] private SkinnedMeshRenderer _arnmfoot;
    [Header("Coatunder")]
    [SerializeField] private SkinnedMeshRenderer _coatunder;
    [Header("Face")]
    [SerializeField] private SkinnedMeshRenderer _face;
    [Header("Inner")]
    [SerializeField] private SkinnedMeshRenderer _inner;
    [Header("Tops")]
    [SerializeField] private SkinnedMeshRenderer _tops;

    [Header("====透明のマテリアル====")]
    [SerializeField] private Material _clearMaterial;

    [Header("====変更するマテリアル====")]
    [Header("Arnmfoot")]
    [SerializeField] private Material _grayArnmfoot;
    [Header("Coatunder")]
    [SerializeField] private Material _grayCoatunder;
    [Header("Face")]
    [SerializeField] private Material _grayFace;
    [Header("Inner")]
    [SerializeField] private Material _grayInner;
    [Header("Tops")]
    [SerializeField] private Material _grayTops;

    [Header("何秒後にマテリアル変更をするか")]
    [SerializeField] private float _setTime = 1;

    private float _time = 0;

    private bool _isBool = false;

    private void OnEnable()
    {
        _arnmfoot.material = _clearMaterial;
        _coatunder.material = _clearMaterial;
        _face.material = _clearMaterial;
        _inner.material = _clearMaterial;
        _tops.material = _clearMaterial;
        _time = 0;
        _isBool= false;
    }

    private void Update()
    {
        _time+= Time.deltaTime;

        if(_time>_setTime)
        {
            ChangePlayerMaterial();
            _isBool = true;
        }
    }

    public void ChangePlayerMaterial()
    {
        _arnmfoot.material = _grayArnmfoot;
        _coatunder.material = _grayCoatunder;
        _face.material = _grayFace;
        _inner.material = _grayInner;
        _tops.material = _grayTops;
    }

}
