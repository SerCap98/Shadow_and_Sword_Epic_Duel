using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class Ganador : MonoBehaviour
{
    public void Menu(string name)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(name);
    }

    public void Salir()
    {
        Debug.Log("Saliendo...");
        Application.Quit();
    }

}