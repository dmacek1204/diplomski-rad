using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;  

public class SpawnMagic : NetworkBehaviour
{
    [SerializeField]
    private LayerMask mask;
    //Where do we cast spell from
    public GameObject MagicSpawnPoint;
    // Spell (magicBall) that we are instantiating
    public GameObject MagicBall;

    public float shootingTimeDisable;
    public float spawnDelay;
    private bool isShooting = false;
    private float MagicBall_Forward_Force = 280.0f;

    private GameObject magicBallInstance;
    private Rigidbody MagicBallCtrl;
    
    void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        if (Input.GetButtonDown("Fire1") && !isShooting && !Input.GetButton("Fire2"))
        {
            StartCoroutine(DisableShootingAfterSpawningMagic());
            CmdSpawnWithDelay();
        }
    }
    
    [Command]
    private void CmdSpawnWithDelay()
    {
        StartCoroutine(DelaySpawningUntilAnimationEnds(spawnDelay));
    }

    private IEnumerator DelaySpawningUntilAnimationEnds(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        Spawn();
    }

    private void Spawn()
    {
        GameObject magicBallInstance;
        magicBallInstance = Instantiate(MagicBall, MagicSpawnPoint.transform.position, MagicSpawnPoint.transform.rotation) as GameObject;

        NetworkServer.Spawn(magicBallInstance);
    }

    private IEnumerator DisableShootingAfterSpawningMagic()
    {
        isShooting = true;
        yield return new WaitForSecondsRealtime(shootingTimeDisable);
        isShooting = false;
    }

    //[Command] public void CmdInstantiateMagicBall()
    //{
    //    StartCoroutine(DelaySpawningUntilAnimationEnds());
    //    GameObject magicBallInstance;
    //    magicBallInstance = Instantiate(MagicBall, MagicSpawnPoint.transform.position, MagicSpawnPoint.transform.rotation) as GameObject;

    //    Rigidbody MagicBallCtrl;
    //    MagicBallCtrl = magicBallInstance.GetComponent<Rigidbody>();

    //    MagicBallCtrl.AddForce(transform.forward * MagicBall_Forward_Force);

    //    //Destroy(MagicBallCtrl, 3.0f);
    //    NetworkServer.Spawn(magicBallInstance);

    //}

}
