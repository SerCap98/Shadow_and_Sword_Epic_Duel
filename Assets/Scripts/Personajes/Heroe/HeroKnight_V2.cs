using UnityEngine;
using System.Collections;
using Cainos.PixelArtPlatformer_VillageProps; 
using System.Collections.Generic;
using System;

public class HeroKnight : MonoBehaviour,IDamageable
{

    [Header("Variables")]
    [SerializeField] float m_speed = 4.0f;
    [SerializeField] float m_jumpForce = 7.5f;
    [SerializeField] float m_rollForce = 6.0f;
    [SerializeField] bool m_noBlood = false;
    [SerializeField] GameObject m_slideDust;
    [SerializeField] string platformTagToDescend = "Descendable";
    [SerializeField] string platformTagToClimb = "Scalable";
    [SerializeField] private HealthBarHUDTester healthBarHUDTester;
    public PolygonCollider2D swordCollider;

    [Header("Controles")]
    [SerializeField] KeyCode moveLeftKey = KeyCode.A;
    [SerializeField] KeyCode moveRightKey = KeyCode.D;
    [SerializeField] KeyCode descendKey = KeyCode.S;
    [SerializeField] KeyCode jumpKey = KeyCode.Space;
    [SerializeField] KeyCode attackKey = KeyCode.Mouse0;
    [SerializeField] KeyCode blockKey = KeyCode.Mouse1;
    [SerializeField] KeyCode rollKey = KeyCode.LeftControl;

    [Header("Invulnerabilidad")]
    [SerializeField] private float invulnerabilityDuration = 1.0f;
    private bool isInvulnerable = false;

    // Variables para gestionar las colisiones
    private bool isDescending = false;
    private int originalLayer;
    private int descendibleLayer;

    public List<Chest> cofres;
    private Animator m_animator;
    private Rigidbody2D m_body2d;
    private Sensor_HeroKnight m_groundSensor;
    private Sensor_HeroKnight m_wallSensorR1;
    private Sensor_HeroKnight m_wallSensorL1;

    private bool m_isWallSliding = false;
    private bool m_isAttacked = false;
    private bool m_grounded = false;
    private bool m_rolling = false;
    private bool m_blocking= false;

    private bool m_isMoveLocked = false;
    private bool m_isActionLocked = false;
    private bool isAttacking = false;

    public int m_facingDirection = 1;
    private int m_currentAttack = 0;
    private float m_timeSinceAttack = 0.0f;
    private float m_delayToIdle = 0.0f;
    private float m_rollDuration = 1.0f;
    private float m_rollCurrentTime;
    private Collider2D groundCollider;
    string currentAnimation;

    [SerializeField] private GameObject ganadorB;

    // Use this for initialization
    void Start()
    {
        swordCollider = transform.Find("SwordCollider").GetComponent<PolygonCollider2D>();
        swordCollider.isTrigger = true;
        m_animator = GetComponent<Animator>();
        m_body2d = GetComponent<Rigidbody2D>();
        m_groundSensor = transform.Find("GroundSensor").GetComponent<Sensor_HeroKnight>();
        m_wallSensorR1 = transform.Find("WallSensor_R1").GetComponent<Sensor_HeroKnight>();
        m_wallSensorL1 = transform.Find("WallSensor_L1").GetComponent<Sensor_HeroKnight>();
        originalLayer = LayerMask.NameToLayer("Hero");
        descendibleLayer = LayerMask.NameToLayer("Descendible");
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("m_isAttacked: " + m_isAttacked);
        Debug.Log("m_isMoveLocked: " + m_isMoveLocked);


        groundCollider = m_groundSensor.GetCollider();

        // Increase timer that controls attack combo
        m_timeSinceAttack += Time.deltaTime;

        // Increase timer that checks roll duration
        if (m_rolling)
            m_rollCurrentTime += Time.deltaTime;

        // Disable rolling if timer extends duration
        //if (m_rollCurrentTime > m_rollDuration)
           // m_rolling = false;

        //Check if character just landed on the ground
        if (!m_grounded && m_groundSensor.State())
        {
            m_grounded = true;
            m_animator.SetBool("Grounded", m_grounded);
        }

        //Check if character just started falling
        if (m_grounded && !m_groundSensor.State())
        {
            m_grounded = false;
            m_animator.SetBool("Grounded", m_grounded);
        }
        if (!m_grounded && !m_groundSensor.State())
        {
            m_grounded = false;
            m_animator.SetBool("Grounded", m_grounded);
        }

        // -- Handle input and movement --
        float inputX = 0.0f;

        currentAnimation = m_animator.GetCurrentAnimatorClipInfo(0)[0].clip.name;
        bool isSlidingAnimation = currentAnimation.Contains(platformTagToClimb);

        if (!m_isAttacked)
        {
            if (!m_isMoveLocked && !isSlidingAnimation)
            {


                if (Input.GetKey(moveLeftKey))
                {
                    // Voltear el sprite hacia la izquierda
                    transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
                    inputX = -1.0f;
                    m_facingDirection = -1;

                    // Mover el collider hacia la izquierda multiplicando su posición actual por la escala X
                    Vector2 colliderPosition = swordCollider.transform.localPosition;
                    colliderPosition.x = -Mathf.Abs(colliderPosition.x);
                    swordCollider.transform.localPosition = colliderPosition;
                }
                else if (Input.GetKey(moveRightKey))
                {
                    // Voltear el sprite hacia la derecha
                    transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    inputX = 1.0f;
                    m_facingDirection = 1;

                    // Mover el collider hacia la derecha multiplicando su posición actual por la escala X
                    //Vector2 colliderPosition = swordCollider.transform.localPosition;
                    //colliderPosition.x = Mathf.Abs(colliderPosition.x);
                    //swordCollider.transform.localPosition = colliderPosition;
                }

                // Mueve solo si no estás rodando
                if (!m_rolling) m_body2d.velocity = new Vector2(inputX * m_speed, m_body2d.velocity.y);
            }
            else
            {
                // Detener el movimiento si está bloqueado
                m_body2d.velocity = new Vector2(0.0f, m_body2d.velocity.y);
            }
        }

        //Set AirSpeed in animator
        m_animator.SetFloat("AirSpeedY", m_body2d.velocity.y);

        // -- Handle Animations --

        // Roll
        if (Input.GetKeyDown(rollKey))
        {
            if (!m_rolling && !isSlidingAnimation && !m_isActionLocked && !m_isMoveLocked && m_grounded)
            {

                
                m_isActionLocked = true;
                m_animator.SetTrigger("Roll");
                m_rolling = true;
                m_body2d.velocity = new Vector2(m_facingDirection * m_rollForce, m_body2d.velocity.y);

            }
        }


        //Wall Slide
        m_isWallSliding = CheckWallSlideWithTag(platformTagToClimb);

        if (!m_isActionLocked && !m_isAttacked && !m_rolling)
        {

            m_animator.SetBool("WallSlide", m_isWallSliding);
        }

        if (Input.GetKeyDown(jumpKey) && m_isWallSliding)
        {


            // Realizar el salto
            m_animator.SetTrigger("Jump");
            m_body2d.velocity = new Vector2(m_body2d.velocity.x, m_jumpForce);

        }

        //Death
        if (PlayerStats.Instance.Health <= 0)
        {
            TriggerDeathAnimation();
            ganadorB.SetActive(true);
        }



        //Attack
        if (Input.GetKeyDown(attackKey))
        {
            if (m_timeSinceAttack > 0.25f && !m_rolling && !isSlidingAnimation && !m_isActionLocked)
            {
                isAttacking=true;
                // Marcar que está realizando una acción que bloquea el movimiento (ataque)
                m_isMoveLocked = true;

                m_currentAttack++;

                // Loop back to one after the third attack
                if (m_currentAttack > 3)
                    m_currentAttack = 1;

                // Reset Attack combo if time since last attack is too large
                if (m_timeSinceAttack > 1.0f)
                    m_currentAttack = 1;

                // Llamada a una de las tres animaciones de ataque "Attack1", "Attack2", "Attack3"
                m_animator.SetTrigger("Attack" + m_currentAttack);

                // Reset timer
                m_timeSinceAttack = 0.0f;

                foreach (var cofre in cofres)
                {
                    // Abre el cofre si está cerca y aún no está abierto
                    if (cofre != null && cofre.IsOpened == false && IsNearChest(cofre))
                    {
                        cofre.Open();
                    }
                }
            }
        }

        // Block
        if (Input.GetKeyDown(blockKey))
        {
            if (!m_rolling && !isSlidingAnimation && !m_isActionLocked)
            {

                // Marcar que está bloqueando
                m_isMoveLocked = true;
                m_blocking = true;
                m_animator.SetTrigger("Block");
                m_animator.SetBool("IdleBlock", true);
            }
        }
        else if (Input.GetMouseButtonUp(1))
        {
            // Desmarcar que está bloqueando al soltar el botón
            m_isMoveLocked = false;
            m_blocking = false;
            m_animator.SetBool("IdleBlock", false);
        }


        //Jump
        if (Input.GetKeyDown(jumpKey))
        {
            if (m_grounded && !m_rolling && !m_isActionLocked && !m_isMoveLocked)
            {
                m_animator.SetTrigger("Jump");
                m_grounded = false;
                m_animator.SetBool("Grounded", m_grounded);
                m_body2d.velocity = new Vector2(m_body2d.velocity.x, m_jumpForce);
                m_groundSensor.Disable(0.2f);
            }
        }

        //Run
        if (Mathf.Abs(inputX) > Mathf.Epsilon)
        {
            // Reset timer
            m_delayToIdle = 0.05f;
            m_animator.SetInteger("AnimState", 1);
        }

        //Idle
        else
        {
            // Prevents flickering transitions to idle
            m_delayToIdle -= Time.deltaTime;
            if (m_delayToIdle < 0)
                m_animator.SetInteger("AnimState", 0);
        }

        //Descender
        if (Input.GetKeyDown(descendKey) && !m_isAttacked && m_grounded && m_groundSensor.State())
        {
           if (groundCollider!=null) { 
            if (groundCollider.CompareTag(platformTagToDescend))
            {
                isDescending = true;
                gameObject.layer = descendibleLayer; // Cambia temporalmente la capa del personaje
                StartCoroutine(EnableColliderAfterDelay(0.5f));
            }
           }
        }



    }

    // Animation Events
    // Called in slide animation.
    void AE_SlideDust()
    {
        Vector3 spawnPosition;

        if (m_facingDirection == 1)
            spawnPosition = m_wallSensorR1.transform.position;
        else
            spawnPosition = m_wallSensorL1.transform.position;

        if (m_slideDust != null)
        {
            // Set correct arrow spawn position
            GameObject dust = Instantiate(m_slideDust, spawnPosition, gameObject.transform.localRotation) as GameObject;
            // Turn arrow in correct direction
            dust.transform.localScale = new Vector3(m_facingDirection, 1, 1);
        }
    }


    void OnAttackAnimationEnd()
    {
        // Desmarcar que está realizando una acción que bloquea el movimiento (ataque)
        m_isMoveLocked = false;
    }
    void OnRollingAnimationEnd()
    {
        // Desmarcar que está realizando una acción que bloquea el movimiento (ataque)
        m_isActionLocked = false;
        m_rolling = false;

    }
    void OnHurtAnimationEnd()
    {
        // Desmarcar que está realizando una acción que bloquea el movimiento (ataque)
        m_isAttacked = false;
    }

    public void TakeDamageAndKnockback(Vector3 attackDirection, GameObject attacker)
    {
        if (!isInvulnerable)
        {
            if (!m_rolling && !m_isAttacked && (!m_blocking || Mathf.Sign(attackDirection.x) != Mathf.Sign(m_facingDirection)))
            {

                // Comprobar si el atacante es un bandido
                if (attacker != null && attacker.CompareTag("Bandido"))
                {
                    m_isAttacked = true;
                    // Aplicar fuerza de empuje si el atacante es un bandido
                    Vector2 knockbackForce = new Vector2(-attackDirection.x * 5.0f, 3.0f);
                    m_body2d.velocity = knockbackForce;
                    healthBarHUDTester.Hurt(1);
                }
                else
                {

                    healthBarHUDTester.Hurt(0.5f);
                }


                m_animator.SetTrigger("Hurt");
                StartCoroutine(BecomeInvulnerable());
                m_isAttacked = false;
                m_isActionLocked = false;
                m_isMoveLocked = false;
            }
        }
    }
    bool CheckWallSlideWithTag(string tag)
    {
        bool rightWallDetected = m_wallSensorR1.State();
        bool leftWallDetected = m_wallSensorL1.State();

        if (rightWallDetected)
        {
            Collider2D rightCollider = Physics2D.OverlapArea(m_wallSensorR1.transform.position, m_wallSensorR1.transform.position);

            if (rightCollider != null && rightCollider.CompareTag(tag))
            {
                // Objeto tageado detectado en el lado derecho

                return true;
            }
        }

        if (leftWallDetected)
        {
            Collider2D leftCollider = Physics2D.OverlapArea(m_wallSensorL1.transform.position, m_wallSensorL1.transform.position);

            if (leftCollider != null && leftCollider.CompareTag(tag))
            {
                // Objeto tageado detectado en el lado izquierdo

                return true;
            }
        }

        return false;


    }

    private bool IsNearChest(Chest cofre)
    {

        float distanciaMinima = 2.0f;

        if (cofre != null)
        {
            float distancia = Vector2.Distance(transform.position, cofre.transform.position);
            return distancia <= distanciaMinima;
        }

        return false;
    }

    IEnumerator EnableColliderAfterDelay(float delay)
    {
 
        yield return new WaitForSeconds(delay);
        gameObject.layer = originalLayer;
        isDescending = false;
    }

    void OnTriggerEnter2D(Collider2D col)
    {

        if (isAttacking) { 
              
            IDamageable damageable = col.GetComponent<IDamageable>();
            if (damageable != null)
            {

                Vector3 attackDirection = transform.position - col.transform.position;
                damageable.TakeDamageAndKnockback(attackDirection.normalized, gameObject);

            } 
        }
        

    }
    private IEnumerator BecomeInvulnerable()
    {
        isInvulnerable = true;
        // Opcional: Cambia el color o la apariencia del personaje para indicar invulnerabilidad


        yield return new WaitForSeconds(invulnerabilityDuration);

        isInvulnerable = false;

    }
    void TriggerDeathAnimation()
    {
        // Asegúrate de que la animación de muerte solo se dispare una vez
        if (!m_isAttacked)
        {
            m_isActionLocked = true;
            m_isMoveLocked = true;
            
            m_isAttacked = true; // O cualquier otra variable que indique que el héroe está muerto
            m_animator.SetBool("noBlood", m_noBlood);
            m_animator.SetTrigger("Death");
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
}