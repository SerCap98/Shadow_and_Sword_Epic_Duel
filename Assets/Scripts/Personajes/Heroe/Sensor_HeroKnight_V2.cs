using UnityEngine;
using System.Collections;


public class Sensor_HeroKnight : MonoBehaviour
{

    private int m_ColCount = 0;
    private float m_DisableTimer;

    private Collider2D m_CurrentCollider;





    private void OnEnable()
    {
        m_ColCount = 0;
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
        m_CurrentCollider = other; // Actualiza el collider actual al entrar en contacto con uno nuevo
    }

    void OnTriggerExit2D(Collider2D other)
    {
        m_ColCount--;
        if (m_CurrentCollider == other)
        {
            m_CurrentCollider = null; // Restablece el collider actual si sale el que se estaba registrando
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

    public Collider2D GetCollider()
    {
        return m_CurrentCollider;
    }
}
