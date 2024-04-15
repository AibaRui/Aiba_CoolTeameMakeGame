using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossControl : MonoBehaviour
{
    [SerializeField]
    private BossMaterialChange _materialChange;

    public BossMaterialChange MaterialChange => _materialChange;
}
