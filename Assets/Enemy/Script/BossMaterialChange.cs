using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BossMaterialChange
{
    [Header("�}�e���A���̃f�[�^")]
    [SerializeField] private List<ModelMaterialData> _data = new List<ModelMaterialData>();


    public void ChangeBossMaterial(ModelMaterialType type)
    {
        foreach (ModelMaterialData data in _data)
        {
            if (type == ModelMaterialType.Nomal)
            {
                data.Mesh.material = data.NomalM;
            }
            else if (type == ModelMaterialType.GrayScal)
            {
                data.Mesh.material= data.GrayM;
            }
        }
    }

}

[System.Serializable]
public class ModelMaterialData
{
    [SerializeField] private string _name;

    [Header("���b�V��")]
    [SerializeField] private MeshRenderer _mesh;
    [Header("�ʏ�̃}�e���A��")]
    [SerializeField] private Material _nomalM;
    [Header("�O���C�X�P�[���̃}�e���A��")]
    [SerializeField] private Material _grayM;

    public MeshRenderer Mesh => _mesh;
    public Material NomalM => _nomalM;
    public Material GrayM => _grayM;
}