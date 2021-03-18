﻿using UnityEngine.UI;
using UnityEngine;
using System.Collections;

public class TankController : GenericSingletonClass<TankController>, IDamageable
{
    public float m_Speed = 12f;                 // How fast the tank moves forward and back.
    public float m_TurnSpeed = 180f;            // How fast the tank turns in degrees per second.
    public AudioSource m_MovementAudio;         // Reference to the audio source used to play engine sounds. NB: different to the shooting audio source.
    public AudioClip m_EngineIdling;            // Audio to play when the tank isn't moving.
    public AudioClip m_EngineDriving;           // Audio to play when the tank is moving.
    public float m_PitchRange = 0.2f;           // The amount by which the pitch of the engine noises can vary.
    private string m_MovementAxisName;          // The name of the input axis for moving forward and back.
    private string m_TurnAxisName;              // The name of the input axis for turning.
    private float m_MovementInputValue;         // The current value of the movement input.
    private float m_TurnInputValue;             // The current value of the turn input.
    private float m_OriginalPitch;              // The pitch of the audio source at the start of the scene.
    public static Rigidbody rb;                       // Reference used to move the tank.
    public GameObject shellInstance;
    public Transform fireTransform;
    public float fireForce = 2000;
    public Image m_FillImage;                           // The image component of the slider.
    public GameObject m_ExplosionPrefab;                // A prefab that will be instantiated in Awake, then used whenever the tank dies.
    public ParticleSystem m_ExplosionParticles;        // The particle system the will play when the tank is destroyed.
    private float currentHealth = 1f;                      // How much health the tank currently has.
    public bool isDead;                                // Has the tank been reduced beyond zero health yet?
    public float speed = 10.0f;
    public float rotationSpeed = 100.0f;
    public int shellCounter;
    private GameObject shellGo;


    IEnumerator DisableShell()
    {
        yield return new WaitForSeconds(2f);
        shellInstance.SetActive(false);
    }
    override public void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }


   

    public void TakeDamage(float amount)
    {
        // Reduce current health by the amount of damage done.
        currentHealth -= amount;

        // Change the UI elements appropriately.
        m_FillImage.fillAmount = currentHealth;

        if (currentHealth < 0.25f) { m_FillImage.color = Color.red; }

        // If the current health is at or below zero and it has not yet been registered, call OnDeath.
        if (currentHealth <= 0f && !isDead)
        {
            Die();
        }
    }

    public void Die()
    {
        // Set the flag so that this function is only called once.
        isDead = true;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Shells")
        {
            IDamageable takeDamage = GetComponent<IDamageable>();

            if (takeDamage != null)
                takeDamage.TakeDamage(0.1f);
            else
                Debug.Log(takeDamage);
            //TakeDamage(20f);
        }
    }

    private void OnEnable()
    {
        // When the tank is enabled, reset the tank's health and whether or not it's dead.
        m_FillImage.fillAmount = currentHealth = 1f;
        m_FillImage.color = Color.green;
        isDead = false;

        // Update the health slider's value and color.
        // Also reset the input values.
        m_MovementInputValue = 0f;
        m_TurnInputValue = 0f;

       
    }


    private void OnDisable()
    {
        // When the tank is turned off, set it to kinematic so it stops moving.
        rb.isKinematic = true;
    }


    private void Start()
    {
        // The axes names are based on player number.
        m_MovementAxisName = "Vertical";
        m_TurnAxisName = "Horizontal";

        // Store the original pitch of the audio source.
        m_OriginalPitch = m_MovementAudio.pitch;

        //TankService.Instance.GetTank();

        PoolManager.SetNetPoolSize(shellInstance, 10);
        PoolManager.SetPoolSize(shellInstance, 5);
    }


    private void Update()
    {
        // Store the value of both input axes.
        m_MovementInputValue = Input.GetAxis(m_MovementAxisName);
        m_TurnInputValue = Input.GetAxis(m_TurnAxisName);

        EngineAudio();

        if (MobileControls.instance.GetMobileButtonDown("FireButton") || Input.GetKeyDown(KeyCode.LeftControl))
        {
            //Mobile button has been pressed one time, equivalent to if(Input.GetKeyDown(KeyCode...))

            shellGo = PoolManager.Instantiate(shellInstance, fireTransform.position, fireTransform.rotation);
            Rigidbody shellRb = shellGo.GetComponent<Rigidbody>();
            shellRb.velocity = fireTransform.forward * fireForce;
            shellCounter++;
        }

        if (MobileControls.instance.GetMobileButton("FireButton"))
        {
            //Mobile button is being held pressed, equivalent to if(Input.GetKey(KeyCode...))
        }
    }


    private void EngineAudio()
    {
        // If there is no input (the tank is stationary)...
        if (Mathf.Abs(m_MovementInputValue) < 0.1f && Mathf.Abs(m_TurnInputValue) < 0.1f)
        {
            // ... and if the audio source is currently playing the driving clip...
            if (m_MovementAudio.clip == m_EngineDriving)
            {
                // ... change the clip to idling and play it.
                m_MovementAudio.clip = m_EngineIdling;
                m_MovementAudio.pitch = Random.Range(m_OriginalPitch - m_PitchRange, m_OriginalPitch + m_PitchRange);
                m_MovementAudio.Play();
            }
        }
        else
        {
            // Otherwise if the tank is moving and if the idling clip is currently playing...
            if (m_MovementAudio.clip == m_EngineIdling)
            {
                // ... change the clip to driving and play.
                m_MovementAudio.clip = m_EngineDriving;
                m_MovementAudio.pitch = Random.Range(m_OriginalPitch - m_PitchRange, m_OriginalPitch + m_PitchRange);
                m_MovementAudio.Play();
            }
        }
    }


    private void FixedUpdate()
    {
        // Adjust the rigidbodies position and orientation in FixedUpdate.
        Move();
        Turn();

        //Get normalized direction of a on-screen Joystick
        //Could be compared to: new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")) or new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"))
        Vector3 inputAxis = MobileControls.instance.GetJoystick("JoystickLeft");

        //Move Front/Back
        if (inputAxis.y != 0)
        {
            transform.Translate(transform.forward * Time.deltaTime * 5f * inputAxis.y, Space.World);
        }

        //Rotate Left/Right
        if (inputAxis.x != 0)
        {
            transform.Rotate(new Vector3(0, 14, 0) * Time.deltaTime * 4.5f * inputAxis.x, Space.Self);
        }

    }


    private void Move()
    {
        // Create a vector in the direction the tank is facing with a magnitude based on the input, speed and the time between frames.
        Vector3 movement = transform.forward * m_MovementInputValue * m_Speed * Time.deltaTime;

        // Apply this movement to the rigidbody's position.
        rb.MovePosition(rb.position + movement);
    }


    private void Turn()
    {
        // Determine the number of degrees to be turned based on the input, speed and time between frames.
        float turn = m_TurnInputValue * m_TurnSpeed * Time.deltaTime;

        // Make this into a rotation in the y axis.
        Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);

        // Apply this rotation to the rigidbody's rotation.
        rb.MoveRotation(rb.rotation * turnRotation);
    }
}