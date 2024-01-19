using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BanditGroundMemoryTeleport : MonoBehaviour
{
    public Sensor_Bandit groundSensor; // Referencia al sensor de suelo del bandido
    public Rigidbody2D banditRigidbody; // Referencia al Rigidbody2D del bandido
    public float teleportTime = 10.0f; // Tiempo en segundos para teletransportar

    private Vector3 lastGroundedPosition;
    private float timeSinceGrounded;

    void Update()
    {
        // Comprueba si el bandido est? en el suelo
        if (groundSensor.State())
        {
            // Ajusta la posici?n guardada seg?n la direcci?n del movimiento
            float xAdjustment = banditRigidbody.velocity.x > 0 ? -1 : 1;
            lastGroundedPosition = transform.position + new Vector3(xAdjustment, 0, 0);
            timeSinceGrounded = 0;
        }
        else
        {
            // Incrementa el contador
            timeSinceGrounded += Time.deltaTime;

            // Si se excede el tiempo, teletransporta al bandido
            if (timeSinceGrounded > teleportTime)
            {
                transform.position = lastGroundedPosition;
                timeSinceGrounded = 0; // Resetear el contador
            }
        }
    }
}
