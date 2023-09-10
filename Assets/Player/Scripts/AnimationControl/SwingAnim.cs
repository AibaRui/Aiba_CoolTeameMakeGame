using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class SwingAnim
{
    private PlayerAnimationControl _animationControl;


    public void Init(PlayerAnimationControl animationControl)
    {
        _animationControl = animationControl;
    }

    public void Swing(bool a)
    {
        _animationControl.PlayerControl.Anim.SetBool("IsSwing", a);
    }

    public void SetSwingEndType(int i)
    {
        _animationControl.PlayerControl.Anim.SetInteger("SwingEndType", i);
    }

    public void SetSwingHighEnd()
    {
        var r = Random.Range(0, 3);
        _animationControl.PlayerControl.Anim.SetInteger("SwingEndHighUpType", r);
    }

    public void SetHighFallType()
    {
        var r = Random.Range(0, 2);
        _animationControl.PlayerControl.Anim.SetInteger("HighFallType", r);
    }

    public void SetJumpEndType()
    {
        var r = Random.Range(1, 3);
        _animationControl.PlayerControl.Anim.SetInteger("SwingJumpEndType", r);
    }


}
