using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitScreen : MonoBehaviour
{
    public Camera Camera_Hero;
    public Camera Camera_Bandit;

    void Start()
    {
        if (Camera_Hero && Camera_Bandit)
        {
            // Configuraci�n de la c�mara del h�roe
            Camera_Bandit.rect = new Rect(0.0f, 0.0f, 0.5f, 1.0f); // La mitad izquierda de la pantalla

            // Configuraci�n de la c�mara del bandido
            Camera_Hero.rect = new Rect(0.5f, 0.0f, 0.5f, 1.0f); // La mitad derecha de la pantalla
        }
        else
        {
            Debug.LogError("Asigna las c�maras en el inspector.");
        }
    }
}
