using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZipMedal : MonoBehaviour
{
    [Header("Hit‚·‚éƒŒƒCƒ„[")]
    [SerializeField] private LayerMask _layer;

    [SerializeField] private PlayerControl _playerControl;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != _layer) return;
        _playerControl.ZipLineRenderer.HitMedal(transform.position);
    }
}
