using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CanvasFollowCamera : MonoBehaviour
{
    public Transform heroCameraTransform; // Asigna aquí la transformada de la cámara del héroe
    public Vector3 offset; // Ajusta este valor para cambiar la posición relativa del canvas respecto a la cámara
    public float fixedZPosition = 1f; // Ajusta este valor para establecer una posición fija en el eje Z

    void Update()
    {
        if (heroCameraTransform != null)
        {
            // Ajusta la posición del canvas para que coincida con la de la cámara, aplicando el desplazamiento solo en X e Y
            transform.position = new Vector3(heroCameraTransform.position.x + offset.x,
                                             heroCameraTransform.position.y + offset.y,
                                             fixedZPosition);
        }
    }
}
