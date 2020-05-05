using Mirror;
using System.Collections.Generic;
using UnityEngine;

public class DestroyMagicOnCollision : NetworkBehaviour
{
    public ParticleSystem part;
    public List<ParticleCollisionEvent> collisionEvents;

    // Start is called before the first frame update
    void Start()
    {
        part = GetComponent<ParticleSystem>();
        collisionEvents = new List<ParticleCollisionEvent>();
    }

    void Update()
    {

    }

    // for collision with PS Magic Projectile
    void OnCollisionEnter(Collision collision)
    {
        Destroy(this.gameObject);
    }

    // used for collision with fireShot PS
    void OnParticleCollision(GameObject other)
    {
        Destroy(this.gameObject);
    }

    
}
