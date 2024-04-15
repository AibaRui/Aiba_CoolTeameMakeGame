using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayerDamageAdder : MonoBehaviour
{
    [SerializeField] private PlayerControl _player;
    void Start()
    {
        _player.GetComponent<IDamageble>().Damage(DamageType.BossBigDamage);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
