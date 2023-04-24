using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerAnimationControl
{


    private PlayerControl _playerControl;

    public void Init(PlayerControl playerControl)
    {
        _playerControl = playerControl;
    }

    public void AnimSet()
    {
        _playerControl.Anim.SetFloat("Speed", _playerControl.Rb.velocity.magnitude);
        _playerControl.Anim.SetFloat("SpeedY", _playerControl.Rb.velocity.y);
        _playerControl.Anim.SetBool("IsGround", _playerControl.GroundCheck.IsHit());
        _playerControl.Anim.SetFloat("PosY", _playerControl.PlayerT.position.y);
    }


    public void Avoid()
    {
        _playerControl.Anim.Play("AvoidGroundFront");
    }

    public void FrontZip()
    {

        _playerControl.Anim.SetTrigger("FrontZip");
    }

    public void Swing(bool a)
    {
        _playerControl.Anim.SetBool("IsSwing", a);
    }

    public void WallRunSet(bool isHit)
    {
        _playerControl.Anim.SetBool("IsWallHit", isHit);
    }

    public void WallRunTransition()
    {
        if (_playerControl.WallRunCheck.IsWallRightHit)
        {
            _playerControl.Anim.Play("WallHitRight");
        }
        else
        {
            _playerControl.Anim.Play("WallHitLeft");
        }

    }


    public void Jump()
    {
        _playerControl.Anim.Play("JumpStart");
    }
}
