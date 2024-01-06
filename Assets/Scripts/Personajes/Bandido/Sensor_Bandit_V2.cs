using UnityEngine;
using System.Collections;

public class Sensor_Bandit : MonoBehaviour
{
    private int m_ColCount = 0;
    private Collider2D m_CurrentCollider; // Nuevo campo para almacenar el collider detectado

    private float m_DisableTimer;

    private void OnEnable()
    {
        m_ColCount = 0;
        m_CurrentCollider = null; // Inicializa el collider detectado como nulo al habilitar el sensor
    }

    public bool State()
    {
        if (m_DisableTimer > 0)
            return false;
        return m_ColCount > 0;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        m_ColCount++;
        m_CurrentCollider = other; // Almacena el collider que entra en el sensor
    }

    void OnTriggerExit2D(Collider2D other)
    {
        m_ColCount--;
        if (other == m_CurrentCollider) // Si el collider que sale es el almacenado, lo vuelve nulo
        {
            m_CurrentCollider = null;
        }
    }

    void Update()
    {
        m_DisableTimer -= Time.deltaTime;
    }

    public void Disable(float duration)
    {
        m_DisableTimer = duration;
    }

    // Nuevo método para obtener el collider detectado por el sensor
    public Collider2D GetCollider()
    {
        return m_CurrentCollider;
    }
}

