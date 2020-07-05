﻿using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Following_Enemy_Behaviour : NetworkBehaviour
{
    private Collider player;

    [SerializeField]
    private NetworkAnimator networkAnimator;

    [SerializeField]
    private GameObject enemy;

    public GameObject MagicSpawnPoint;
    public GameObject MagicBall;

    [SerializeField]
    private NavMeshAgent agent;

    [SerializeField]
    private Animator anim;
    [SerializeField]
    private float AttackAnimationLength = 2f;

    
    [SerializeField]
    private Renderer graphicRenderer;
    private float MAX_DISTANCE = 30.7f;
    [SerializeField]
    Color near;
    [SerializeField]
    Color far;

    [SerializeField]
    public bool canAttack = true;


    // Start is called before the first frame update
    void Start()
    {
        player = null;
        if (isLocalPlayer) return;
        InvokeRepeating("InstantiateMagicBall", 0f, 1f);
    }

    
    // Update is called once per frame
    void Update()
    {
        if (isLocalPlayer) return;
        if (player != null && canAttack)
        {
            Vector3 lookat = player.transform.position;
            Vector3.ProjectOnPlane(lookat, Vector3.forward);
            lookat = lookat + new Vector3(0, 1.4f, 0);
            enemy.transform.LookAt(lookat);
            agent.destination = player.transform.position;

            //Get distance between those two Objects
            var distanceApart = Vector3.Distance(this.transform.position, player.transform.position);

            //Convert 15 and 30.7 distance range to 0f and 1f range
            var lerp = MapValue(distanceApart, 15f, MAX_DISTANCE, 0f, 1f);

            //Lerp Color between near and far color
            Color lerpColor = Color.Lerp(near, far, lerp);
            ChangeColor(lerpColor);
        }
        if(!canAttack)
        {
            agent.destination = enemy.transform.position;
        }
    }

    public void InstantiateMagicBall()
    {
        if (!isServer) return;
        var distance = player != null ? Vector3.Distance(player.transform.position, this.transform.position) : -1;
        if (player != null && distance < 15 && distance > 0 && canAttack)
        {
            StartCoroutine(ShootingAnimationAndSpawnMagic());
        }
    }

    public void ChangeColor(Color color)
    {
        if (!isServer) return;
        RpcChangeColor(color);
    }

    [ClientRpc]
    public void RpcChangeColor(Color color)
    {
        this.graphicRenderer.material.color = color;
    }

    [Server]
    public IEnumerator ShootingAnimationAndSpawnMagic()
    {
        networkAnimator.SetTrigger("AttackTrigger");
        yield return new WaitForSecondsRealtime(AttackAnimationLength);
        GameObject magicBallInstance;
        magicBallInstance = Instantiate(MagicBall, MagicSpawnPoint.transform.position, MagicSpawnPoint.transform.rotation) as GameObject;
        NetworkServer.Spawn(magicBallInstance);
        IEnumerator coroutine = WaitAndDestroyMagicBall(2.0f, magicBallInstance);
        StartCoroutine(coroutine);
    }

    [Server]
    private IEnumerator WaitAndDestroyMagicBall(float waitTime, GameObject magicBall)
    {
        yield return new WaitForSeconds(waitTime);
        NetworkServer.Destroy(magicBall);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (isLocalPlayer) return;

        if (other.gameObject.tag == "Player" && player == null)
        {
            player = other;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (isLocalPlayer) return;

        if (other.gameObject.tag == "Player" && (player == other || player == null))
        {
            player = other;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (isLocalPlayer) return;

        if (other.gameObject.tag == "Player" && player == other)
        {
            player = null;
        }
    }

    public float MapValue(float mainValue, float inValueMin, float inValueMax, float outValueMin, float outValueMax)
    {
        return (mainValue - inValueMin) * (outValueMax - outValueMin) / (inValueMax - inValueMin) + outValueMin;
    }
}
