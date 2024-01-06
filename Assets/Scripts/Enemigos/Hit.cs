using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hit : MonoBehaviour
{

 

    void OnTriggerEnter2D(Collider2D coll)
    {
        IDamageable damageable = coll.GetComponent<IDamageable>();
        if (coll.CompareTag("Heroe")&& damageable != null)
        {
         
                Vector3 attackDirection = transform.position - coll.transform.position;
                damageable.TakeDamageAndKnockback(attackDirection.normalized, gameObject);

        }
    }




    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
}