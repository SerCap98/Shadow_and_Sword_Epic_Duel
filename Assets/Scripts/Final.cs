using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Final : MonoBehaviour
{

    [SerializeField] private GameObject ganadorH;

    private void OnTriggerEnter2D(Collider2D collision)
    { 

        if (collision.CompareTag("Player"))
        {
            ganadorH.SetActive(true);
        }
    }


}