using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputSystem : MonoBehaviour
{

    //staticのインスタンスの確保
    public static PlayerInputSystem Instance
    {
        get { return s_Instance; }
    }

    protected static PlayerInputSystem s_Instance;


    protected Vector2 m_Movement;
    protected Vector2 m_Camera;
    protected bool m_Jump;

    public Vector2 MoveInput => m_Movement;

    public bool JumpInput => m_Jump;

    void Awake()
    {
        if (s_Instance == null)
            s_Instance = this;
        else if (s_Instance != this)
            throw new UnityException("There cannot be more than one PlayerInput script.  The instances are " + s_Instance.name + " and " + name + ".");
    }


    void Update()
    {
        m_Movement.Set(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        m_Camera.Set(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        m_Jump = Input.GetButton("Jump");
    }

}
