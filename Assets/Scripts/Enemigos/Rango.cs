using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rango : MonoBehaviour
{
    public Animator ani;
    public Enemigo enemigo;

    void OnTriggerEnter2D(Collider2D coll)
    {
        IDamageable damageable = coll.GetComponent<IDamageable>();
        if (coll.CompareTag("Heroe") && damageable != null && enemigo.atacando != true)
        {

     



        }

    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
