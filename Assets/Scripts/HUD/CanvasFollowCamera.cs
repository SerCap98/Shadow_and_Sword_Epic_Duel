using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CanvasFollowCamera : MonoBehaviour
{
    public Transform heroCameraTransform; // Asigna aqu� la transformada de la c�mara del h�roe
    public Vector3 offset; // Ajusta este valor para cambiar la posici�n relativa del canvas respecto a la c�mara
    public float fixedZPosition = 1f; // Ajusta este valor para establecer una posici�n fija en el eje Z

    void Update()
    {
        if (heroCameraTransform != null)
        {
            // Ajusta la posici�n del canvas para que coincida con la de la c�mara, aplicando el desplazamiento solo en X e Y
            transform.position = new Vector3(heroCameraTransform.position.x + offset.x,
                                             heroCameraTransform.position.y + offset.y,
                                             fixedZPosition);
        }
    }
}
