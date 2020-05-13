using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeath : NetworkBehaviour
{


    [SerializeField]
    private NetworkAnimator animator;

    [SerializeField]
    private ParticleSystem deathParticles;

    [SerializeField]
    private GameObject enemy;

    [SerializeField]
    private Following_Enemy_Behaviour attackingScript;

    private bool shouldDie = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (shouldDie)
        {
            if (!isServer) return;
            StartCoroutine(DeathAnimatonAndSpawnDeathParticles());
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (isLocalPlayer) return;

        if (other.gameObject.tag == "Spell")
        {
            this.shouldDie = true;
            attackingScript.canAttack = false;
        }
    }

    [Server]
    public IEnumerator DeathAnimatonAndSpawnDeathParticles()
    {
        if (!isLocalPlayer)
        {
            animator.SetTrigger("DeathTrigger");
            yield return new WaitForSecondsRealtime(1f);
            RpcPlayDeathParticles();
            yield return new WaitForSecondsRealtime(2f);
            NetworkServer.Destroy(this.enemy);
        }
    }

    [ClientRpc]
    public void RpcPlayDeathParticles()
    {
        deathParticles.Play();
    }
}
