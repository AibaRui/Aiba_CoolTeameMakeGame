using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerAnimationControl
{
    [Header("Swingのアニメーション設定")]
    [SerializeField] private SwingAnim _swingAnim;

    [Header("Zipのアニメーション設定")]
    [SerializeField] private ZipAnim _zipAnim;

    private PlayerControl _playerControl;

    public ZipAnim ZipAnim => _zipAnim;
    public SwingAnim SwingAnim => _swingAnim;
    public PlayerControl PlayerControl => _playerControl;

    public void Init(PlayerControl playerControl)
    {
        _playerControl = playerControl;
        _swingAnim.Init(this);
        _zipAnim.Init(this);
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

    public void SetWallRunHitRight(bool isRight)
    {
        _playerControl.Anim.SetBool("IsWallRunRight", isRight);
    }

    public void WallRunStep(bool isStep)
    {
        _playerControl.Anim.SetBool("IsWallRunStep", isStep);
    }


    public void WallRunZipStart(bool isZipFront)
    {
        if (isZipFront)
        {
            _playerControl.Anim.Play("WallRun_UpZipToFrontUp");
        }
        else
        {
            _playerControl.Anim.Play("WallRun_UpZipStart");
        }

    }

    public void WallRunZipDo(bool isZipFront)
    {
        if (isZipFront)
        {
            _playerControl.Anim.Play("WallRun_UpZipToFrontEnd");
        }
        else
        {
            _playerControl.Anim.Play("WallRun_UpZip");
        }
    }

    public void WallRunUpSet(bool judge)
    {
        _playerControl.Anim.SetBool("IsWallRunUp", judge);
    }



    public void Jump()
    {
        _playerControl.Anim.Play("JumpStart");
    }


    public void IsPointZip()
    {
        _playerControl.Anim.SetTrigger("IsPointZip");
    }

    public void IsSetPointZipUp(bool isBool)
    {
        _playerControl.Anim.SetBool("IsPointZipStartUp", isBool);
    }


    public void IsPointZipMoveEndTrigger()
    {
        _playerControl.Anim.SetTrigger("IsPointMoveEnd");
    }

    public void IsPointZipJump()
    {
        _playerControl.Anim.SetTrigger("IsPointZipJump");
    }

    public void BigDamageAnim(string name)
    {
        _playerControl.Anim.Play(name);
    }

    public void RePlace1(bool isStart)
    {
        if (isStart)
        {
            _playerControl.Anim.Play("Player_UpRePlace_1_Start");
        }
        else
        {
            _playerControl.Anim.SetTrigger("NextUp");
        }
    }

    public void BossHit(PlayerBossHitType type, bool second)
    {
        if (type == PlayerBossHitType.Front)
        {
            _playerControl.Anim.Play("Player_BossHit_Front");
        }
        else if (type == PlayerBossHitType.Up)
        {
            _playerControl.Anim.Play("Player_BossHit_Up");
        }
        else if (type == PlayerBossHitType.Sliding)
        {
            if (second)
            {
                _playerControl.Anim.SetTrigger("NextJump");
            }
            else
            {
                _playerControl.Anim.Play("Player_BossHit_Slide");

            }
        }
    }

    public void BossMovieJump()
    {
        _playerControl.Anim.Play("Player_BossMovie_Jump");
    }


    public void SetUpSetBool(bool isSet)
    {
        _playerControl.Anim.SetBool("IsSetUp", isSet);
    }

    public void Attack()
    {
        _playerControl.Anim.SetTrigger("Attack");
    }

}
