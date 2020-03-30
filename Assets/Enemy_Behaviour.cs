using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Behaviour : NetworkBehaviour
{
    private Collider player;


    [SerializeField]
    private GameObject enemy;

    public GameObject MagicSpawnPoint;
    public GameObject MagicBall;

    //private IEnumerator coroutine;


    // Start is called before the first frame update
    void Start()
    {
        player = null;
        InvokeRepeating("InstantiateMagicBall", 0f, 0.7f);
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            //Transform lookAt = player.transform;
            Vector3 lookat = player.transform.position;
            Vector3.ProjectOnPlane(lookat, Vector3.forward);
            lookat = lookat + new Vector3(0, 1f, 0);
            enemy.transform.LookAt(lookat);
        }
    }

    [Server]
    public void InstantiateMagicBall()
    {
        if (player != null)
        {
            GameObject magicBallInstance;
            magicBallInstance = Instantiate(MagicBall, MagicSpawnPoint.transform.position, MagicSpawnPoint.transform.rotation) as GameObject;
            NetworkServer.Spawn(magicBallInstance);
            IEnumerator coroutine = WaitAndDestroyMagicBall(2.0f, magicBallInstance);
            StartCoroutine(coroutine);
        }
    }

    [Server]
    private IEnumerator WaitAndDestroyMagicBall(float waitTime, GameObject magicBall)
    {
        yield return new WaitForSeconds(waitTime);
        NetworkServer.Destroy(magicBall);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && player == null)
        {
            player = other;
        }
    }

    [Server]
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player" && (player == other || player == null))
        {
            player = other;
        }
    }

    [Server]
    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player" && player == other)
        {
            player = null;
        }
    }
}
