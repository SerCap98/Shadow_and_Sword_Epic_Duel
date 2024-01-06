using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientalDamage : MonoBehaviour, IDamageable
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnCollisionEnter2D(Collision2D col)
    {
        
       
        IDamageable damageable = col.gameObject.GetComponent<IDamageable>();
        if (damageable != null)
            {

                Vector3 attackDirection = transform.position - col.transform.position;
                damageable.TakeDamageAndKnockback(attackDirection.normalized, gameObject);

            }
        

    }
    public void TakeDamageAndKnockback(Vector3 attackDirection, GameObject attacker)
    {



    }
}
