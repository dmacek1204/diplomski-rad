using Mirror;
using System;
using System.Collections;
using UnityEngine;

public class PlayerAnimation : NetworkBehaviour

{
    [SyncVar]
    public bool Moving = false;
    [SerializeField]
    private Animator anim;
    private float AttackAnimationLength = 0.7f;

    void Update()
    {


        if (!isLocalPlayer)
        {
            return;
        }
        var horizontal = Input.GetAxisRaw("Horizontal");
        var vertical = Input.GetAxisRaw("Vertical");

        if (Input.GetButton("Fire2"))
        {
            
            PlayerAnimate(0, 0);

        }
        else
        {
            PlayerAnimate(horizontal, vertical);
        }

        if (Input.GetButtonDown("Fire1") && !Input.GetButton("Fire2"))
        {
            if (!isLocalPlayer)
            {
                return;
            }
            PlayerAttackAnimation();
        }
    }

    public void PlayerAnimate(float horizontal, float vertical)
    {

        if (vertical == 1 || vertical == -1 || horizontal == 1 || horizontal == -1)
        {
            if (!Moving)
            {
                Moving = true;
                Move(Moving);
                if (isServer)
                {
                    RpcMove(Moving);
                }
                else
                {
                    CmdMove(Moving);
                }
            }
        }
        else
        {
            if (Moving)
            {
                Moving = false;
                Move(Moving);
                if (isServer)
                {
                    RpcMove(Moving);
                }
                else
                {
                    CmdMove(Moving);
                }
            }
        }
    }

    public void PlayerAttackAnimation()
    {
        Attack(true);
        if (isServer)
        {
            RpcAttack(true);
        }
        else
        {
            CmdAttack(true);
        }
    }

    void Attack(bool moving)
    {
        anim.Play("Attack1Man");
        StartCoroutine(WaitForAttackAnimationToFinish());
    }

    [Command]
    void CmdAttack(bool moving)
    {
        RpcAttack(moving);
    }

    [ClientRpc]
    void RpcAttack(bool moving)
    {
        if (isLocalPlayer)
            return;

        Attack(moving);
    }

    void Move(bool moving)
    {
        anim.SetBool("Moving", moving);
    }

    [Command]
    void CmdMove(bool moving)
    {
        RpcMove(moving);
    }

    [ClientRpc]
    void RpcMove(bool moving)
    {
        if (isLocalPlayer)
            return;

        Move(moving);
    }


    private IEnumerator WaitForAttackAnimationToFinish()
    {
        anim.SetBool("Attack", true);
        yield return new WaitForSecondsRealtime(AttackAnimationLength);
        anim.SetBool("Attack", false);
    }

    public void Hit()
    {
    }

    public void FootR()
    {
    }

    public void FootL()
    {
    }
}