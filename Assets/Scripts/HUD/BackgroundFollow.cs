using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundFollow : MonoBehaviour
{
    public Transform character; // Referencia al personaje

    private Vector3 offset; // Distancia inicial entre el fondo y el personaje

    void Start()
    {
        // Calcula y guarda la distancia inicial entre el fondo y el personaje
        offset = transform.position - character.position;
    }

    void Update()
    {
        // Actualiza solo la posición X del fondo para que siga al personaje manteniendo el offset
        transform.position = new Vector3(character.position.x + offset.x, transform.position.y, transform.position.z);
    }
}
