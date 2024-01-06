﻿using UnityEngine;
using System.Collections;

public class Bandit : MonoBehaviour,IDamageable
{
    [Header("Variables")]
    [SerializeField] float m_speed = 4.0f;
    [SerializeField] float delay = 5.0f;
    [SerializeField] float m_jumpForce = 7.5f;
    [SerializeField] string platformTagToDescend = "Descendable";
    public EdgeCollider2D swordCollider;

    [Header("Controles")]
    [SerializeField] KeyCode attackKey = KeyCode.Mouse0;
    [SerializeField] KeyCode jumpKey = KeyCode.Space;
    [SerializeField] KeyCode moveLeftKey = KeyCode.LeftArrow;
    [SerializeField] KeyCode moveRightKey = KeyCode.RightArrow;
    [SerializeField] KeyCode descendKey = KeyCode.RightArrow;
    [SerializeField] bool m_combatIdle = false;
    // Variables para gestionar las colisiones
    private bool isDescending = false;
    private int originalLayer;
    private int descendibleLayer;
    private Collider2D groundCollider;
    private bool isAttacking = false;
    private bool m_isAttacked = false;
    private Animator m_animator;
    private Rigidbody2D m_body2d;
    private Sensor_Bandit m_groundSensor;
    private bool m_grounded = false;
    private bool m_isDead = false;

    // Use this for initialization
    void Start()
    {

        swordCollider = transform.Find("SwordCollider").GetComponent<EdgeCollider2D>();
        swordCollider.isTrigger = true; // Configura el collider como trigger al inicio
        m_animator = GetComponent<Animator>();
        m_body2d = GetComponent<Rigidbody2D>();
        m_groundSensor = transform.Find("GroundSensor").GetComponent<Sensor_Bandit>();
        originalLayer = LayerMask.NameToLayer("Bandit");
        descendibleLayer = LayerMask.NameToLayer("Descendible");

    }

    // Update is called once per frame
    void Update()
    {
        groundCollider = m_groundSensor.GetCollider();
        // Check if character just landed on the ground
        if (!m_grounded && m_groundSensor.State())
        {
            m_grounded = true;
            m_animator.SetBool("Grounded", m_grounded);
        }

        // Check if character just started falling
        if (m_grounded && !m_groundSensor.State())
        {
            m_grounded = false;
            m_animator.SetBool("Grounded", m_grounded);
        }

        // -- Handle input and movement --
        float inputX = 0.0f;

        // Verifica si no está ejecutando la animación de ataque antes de permitir el movimiento
    
        if (!m_isAttacked) { 
                // Mueve hacia la izquierda si se presiona la flecha izquierda
                if (Input.GetKey(moveLeftKey))
                    inputX = -1.0f;

                // Mueve hacia la derecha si se presiona la flecha derecha
                if (Input.GetKey(moveRightKey))
                    inputX = 1.0f;

                // Swap direction of sprite depending on walk direction
                if (inputX > 0)
                    transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
                else if (inputX < 0)
                    transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

                // Move
                m_body2d.velocity = new Vector2(inputX * m_speed, m_body2d.velocity.y);

                // Set AirSpeed in animator
                m_animator.SetFloat("AirSpeed", m_body2d.velocity.y);

        } 
        // -- Handle Animations --

        // Attack
        if (Input.GetKeyDown(attackKey) && !isAttacking && !m_isAttacked)
        {
            isAttacking = true;
            m_animator.SetTrigger("Attack");
        }


        // Jump
        else if (Input.GetKeyDown(jumpKey) && m_grounded && !m_isAttacked && !m_animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            m_animator.SetTrigger("Jump");
            m_grounded = false;
            m_animator.SetBool("Grounded", m_grounded);
            m_body2d.velocity = new Vector2(m_body2d.velocity.x, m_jumpForce);
            m_groundSensor.Disable(0.2f);
        }

        // Run
        else if (Mathf.Abs(inputX) > Mathf.Epsilon)
            m_animator.SetInteger("AnimState", 2);

        // Combat Idle
        else if (m_combatIdle)
            m_animator.SetInteger("AnimState", 1);

        // Idle
        else
            m_animator.SetInteger("AnimState", 0);


        if (Input.GetKeyDown(descendKey) && !m_isAttacked && m_grounded && m_groundSensor.State())
        {
            if (groundCollider != null)
            {
                if (groundCollider.CompareTag(platformTagToDescend))
                {
                    isDescending = true;
                    gameObject.layer = descendibleLayer; // Cambia temporalmente la capa del personaje
                    StartCoroutine(EnableColliderAfterDelay(0.5f));
                }
            }
        }

    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (isAttacking)
        {
            IDamageable damageable = col.GetComponent<IDamageable>();
            if (damageable != null)
            {

                Vector3 attackDirection = transform.position - col.transform.position;
                damageable.TakeDamageAndKnockback(attackDirection.normalized, gameObject);

            }
        }

    }
    public void TakeDamageAndKnockback(Vector3 attackDirection, GameObject attacker)
    {

                       
        if (attacker.CompareTag("Heroe"))
        {
            isAttacking = false;
            m_isAttacked = true;
            Vector2 knockbackForce = new Vector2(-attackDirection.x * 5.0f, 3.0f);
            m_body2d.velocity = knockbackForce;
            m_animator.SetTrigger("Hurt");
        }


    }

    public void EnterAttackCollision()
    {

        swordCollider.enabled = true; // Desactiva el trigger al golpear al enemigo
                                      // Realiza otras acciones relacionadas con la colisión aquí si es necesario
    }
    public void ExitAttackCollision()
    {

        swordCollider.enabled = false; // Desactiva el trigger al golpear al enemigo
        isAttacking = false;

   
    }   
    private IEnumerator EnableColliderAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        gameObject.layer = originalLayer;
        isDescending = false;
    }
    void OnHurtAnimationEnd()
    {
        // Desmarcar que está realizando una acción que bloquea el movimiento (ataque)
       
        m_animator.SetTrigger("Death");
    }
    void OnDeathAnimationEnd()
    {
        // Desmarcar que está realizando una acción que bloquea el movimiento (ataque)
        
        m_animator.SetTrigger("Recover");
        

    }
    IEnumerator OnRecoverAnimationEnd()
    {
        // Desmarcar que está realizando una acción que bloquea el movimiento (ataque)

        m_animator.speed = 0f;
        yield return new WaitForSeconds(delay);
        m_animator.speed = 1f;
        m_isAttacked = false;

    }

}
