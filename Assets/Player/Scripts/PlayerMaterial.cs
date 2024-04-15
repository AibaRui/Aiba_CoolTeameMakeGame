using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerMaterial
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

    [Header("====普通のマテリアル====")]
    [Header("Arnmfoot")]
    [SerializeField] private Material _nArnmfoot;
    [Header("Coatunder")]
    [SerializeField] private Material _nCoatunder;
    [Header("Face")]
    [SerializeField] private Material _nFace;
    [Header("Inner")]
    [SerializeField] private Material _nInner;
    [Header("Tops")]
    [SerializeField] private Material _nTops;

    [Header("====白黒のマテリアル====")]
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


    public void ChangePlayerMaterial(PlayerMaterialType type)
    {
        if (type == PlayerMaterialType.Nomal)
        {
            _arnmfoot.material= _nArnmfoot;
            _coatunder.material= _nCoatunder;
            _face.material= _nFace;
            _inner.material= _nInner;
            _tops.material = _nTops;
        }
        else if (type == PlayerMaterialType.GrayScal)
        {
            _arnmfoot.material = _grayArnmfoot;
            _coatunder.material = _grayCoatunder;
            _face.material = _grayFace;
            _inner.material = _grayInner;
            _tops.material = _grayTops;
        }
    }

}


public enum PlayerMaterialType
{
    /// <summary>通常のマテリアル </summary>
    Nomal,
    /// <summary>白黒</summary>
    GrayScal,
}
