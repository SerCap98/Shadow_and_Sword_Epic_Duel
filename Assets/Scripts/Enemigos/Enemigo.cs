using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemigo : MonoBehaviour
{
    public int rutina;
    public float cronometro;
    public Animator ani;
    public int direccion;
    public float speed_walk;
    public float speed_run;
    public GameObject target;
    public bool atacando;

    public float rango_vision;
    public float rango_ataque;
 
    public GameObject Hit;

    public float tiempoEsperaAtaque = 1f; // Tiempo de espera entre ataques

    void Start()
    {
        ani = GetComponent<Animator>();
        target = GameObject.Find("HeroKnight");
    
    }

    void ComportamientoMovimiento()
    {
        if (Mathf.Abs(transform.position.x - target.transform.position.x) > rango_vision && !atacando)
        {
            MovimientoAleatorio();
        }
        else
        {
            PerseguirObjetivo();
        }
    }

    void MovimientoAleatorio()
    {
        cronometro += 1 * Time.deltaTime;
        if (cronometro >= 4)
        {
            rutina = Random.Range(0, 2);
            cronometro = 0;
        }

        if (rutina == 0)
        {
            ani.SetBool("run", false);
        }
        else if (rutina == 1)
        {
            direccion = Random.Range(0, 2);
            rutina++;
        }
        else if (rutina == 2)
        {
            if (direccion == 0)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }

            transform.Translate(Vector3.right * speed_walk * Time.deltaTime);
            ani.SetBool("run", true);
        }
    }

    void PerseguirObjetivo()
    {
        if (Mathf.Abs(transform.position.x - target.transform.position.x) > rango_ataque)
        {
            // Perseguir objetivo
            ani.SetBool("run", true);
            transform.Translate(Vector3.right * speed_run * Time.deltaTime);
            transform.rotation = transform.position.x < target.transform.position.x ? Quaternion.Euler(0, 0, 0) : Quaternion.Euler(0, 180, 0);
            ani.SetBool("attack", false);
        }
        else
        {
            ani.SetBool("run", false);
            // Preparar ataque
            if (!atacando)
            {
                transform.rotation = transform.position.x < target.transform.position.x ? Quaternion.Euler(0, 0, 0) : Quaternion.Euler(0, 180, 0);
                ani.SetBool("run", false);
                Atacar();
            }
        }
    }

    public void Atacar()
    {
        if (!atacando)
        {
            ani.SetBool("attack", true);
            atacando = true;
            StartCoroutine(EsperarAntesDeAtacar());
        }
    }

    IEnumerator EsperarAntesDeAtacar()
    {
        yield return new WaitForSeconds(tiempoEsperaAtaque);
        atacando = false;
    }

    public void Final_Ani()
    {
        ani.SetBool("attack", false);
     
    }

    public void ColliderWeaponTrue()
    {
        BoxCollider2D hitCollider = Hit.GetComponent<BoxCollider2D>();
        if (hitCollider != null)
        {
            hitCollider.enabled = true;
        }
    }

    public void ColliderWeaponFalse()
    {
        BoxCollider2D hitCollider = Hit.GetComponent<BoxCollider2D>();
        if (hitCollider != null)
        {
            hitCollider.enabled = false;
        }
    }

    void Update()
    {
        ComportamientoMovimiento();
    }
}
