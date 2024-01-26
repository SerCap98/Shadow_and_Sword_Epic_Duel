using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour
{
    void Start()
    {
        Invoke("WaitToEnd",27);
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Debug.Log("Saliendo...");
            Application.Quit();
        }
    }

    public void WaitToEnd()
    {
        Debug.Log("Saliendo...");
        Application.Quit();
    }
}
