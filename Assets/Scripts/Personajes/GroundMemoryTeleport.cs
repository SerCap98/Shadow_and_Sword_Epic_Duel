using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GroundMemoryTeleport : MonoBehaviour
{
    public Sensor_HeroKnight groundSensor; // Referencia al sensor de suelo
    public HeroKnight heroKnight; // Referencia al script del personaje
    public float teleportTime = 10.0f; // Tiempo en segundos para teletransportar

    private Vector3 lastGroundedPosition;
    private float timeSinceGrounded;

    void Update()
    {
        // Comprueba si el personaje est? en el suelo
        if (groundSensor.State())
        {
            // Ajusta la posici?n guardada seg?n la direcci?n del movimiento
            float xAdjustment = heroKnight.m_facingDirection == 1 ? -1 : 1;
            lastGroundedPosition = heroKnight.transform.position + new Vector3(xAdjustment, 0, 0);
            timeSinceGrounded = 0;
        }
        else
        {
            // Incrementa el contador
            timeSinceGrounded += Time.deltaTime;

            // Si se excede el tiempo, teletransporta al personaje
            if (timeSinceGrounded > teleportTime)
            {
                heroKnight.transform.position = lastGroundedPosition;
                timeSinceGrounded = 0; // Resetear el contador
                Vector3 attackDirection = Vector3.zero; // No hay una direcci?n real de ataque en este caso
                GameObject attacker = null; // No hay un atacante real
                heroKnight.TakeDamageAndKnockback(attackDirection, attacker);
            }
        }
    }
}

