using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sensor_Wall : MonoBehaviour
{
    private int m_ColCount = 0;

    private void OnEnable()
    {
        m_ColCount = 0;
    }

    public bool State()
    {
        return m_ColCount > 0;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Aquí puedes agregar una comprobación para asegurarte de que el collider es una pared
        m_ColCount++;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        m_ColCount--;
    }
}
