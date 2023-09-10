using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ZipAnim
{
    private PlayerAnimationControl _animationControl;

    public void Init(PlayerAnimationControl animationControl)
    {
        _animationControl = animationControl;
    }

    public void FrontZip()
    {
        _animationControl.PlayerControl.Anim.SetTrigger("FrontZip");
    }

    public void SetZip(bool isZip)
    {
        _animationControl.PlayerControl.Anim.SetBool("IsZip", isZip);
    }

    public void SetDoZip(bool isZip)
    {
        _animationControl.PlayerControl.Anim.SetBool("IsDoZip", isZip);
    }
}
