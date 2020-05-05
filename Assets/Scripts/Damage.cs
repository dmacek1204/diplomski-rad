using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    [SerializeField]
    private int damageRate = 10;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        DealDamage();
    }

    void OnParticleCollision(GameObject other)
    {
        DealDamage();
    }
    public void DealDamage()
    {
        GetComponent<Health>().health -= damageRate;
        if (GetComponent<Health>().health <= 0)
        {
            GetComponent<Respawn>().isDead = true;
        }
    }
}
