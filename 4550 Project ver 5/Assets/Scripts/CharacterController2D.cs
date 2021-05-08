using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class CharacterController2D : MonoBehaviour
{
    public Player player;
    

    [SerializeField] private float m_JumpForce = 600f;							// Amount of force added when the player jumps.
	[Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;	// How much to smooth out the movement
	[SerializeField] private bool m_AirControl = true;							// Whether or not a player can steer while jumping;
	[SerializeField] private LayerMask m_WhatIsGround;							// A mask determining what is ground to the character
	[SerializeField] private Transform m_GroundCheck;							// A position marking where to check if the player is grounded.
	[SerializeField] private Transform m_CeilingCheck;							// A position marking where to check for ceilings
    [SerializeField]
    private LayerMask dashLayerMask;


	const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
	private bool m_Grounded;            // Whether or not the player is grounded.
	const float k_CeilingRadius = .2f; // Radius of the overlap circle to determine if the player can stand up
	private Rigidbody2D m_Rigidbody2D;
	public bool m_FacingRight = true;  // For determining which way the player is currently facing.
	private Vector3 m_Velocity = Vector3.zero;
    public int jumpCount;
    public bool isOnGround;
    private Vector3 moveDir;
    //public GameObject DashEffect;
    public float timer=0;

    [Header("Events")]
	[Space]

	public UnityEvent OnLandEvent;

	[System.Serializable]
	public class BoolEvent : UnityEvent<bool> { }

	

	private void Awake()
	{
		m_Rigidbody2D = GetComponent<Rigidbody2D>();

		if (OnLandEvent == null)
			OnLandEvent = new UnityEvent();

	}

    private void Start()
    {
        moveDir = new Vector3(1, 0).normalized;

    }


    public void Update()
    {
        if (!m_Grounded)
        {
            timer += Time.deltaTime;
            FallingDetection(timer);
        }
        if (m_Grounded)
        {
            timer = 0;
        }
    }

    private void FixedUpdate()
	{
		bool wasGrounded = m_Grounded;
		m_Grounded = false;
        isOnGround = m_Grounded;
       
        // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
        // This can be done using layers instead but Sample Assets will not overwrite your project settings.
        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject)
			{
				m_Grounded = true;
                isOnGround = m_Grounded;
				if (!wasGrounded)
					OnLandEvent.Invoke();
			}
		}

	}


	public IEnumerator Move(float move,  bool jump, bool dash)
	{
		
		

		//only control the player if grounded or airControl is turned on
		if (m_Grounded || m_AirControl)
		{

            
			
			// Move the character by finding the target velocity
			Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
			// And then smoothing it out and applying it to the character
			m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

			// If the input is moving the player right and the player is facing left...
			if (move > 0 && !m_FacingRight)
			{
				// ... flip the player.
				Flip();
                moveDir = new Vector3(1,0).normalized;
			}
			// Otherwise if the input is moving the player left and the player is facing right...
			else if (move < 0 && m_FacingRight)
			{
				// ... flip the player.
				Flip();
                moveDir = new Vector3(-1, 0).normalized;
            }
		}

       


        if (m_Grounded && jump)
		{
			// Add a vertical force to the player.
			m_Grounded = false;
            isOnGround = false;
            m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
            jumpCount=1;
            jump = false;
        }

        if (jump && jumpCount <2 && !m_Grounded)
        {
            jumpCount ++;
            m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
            jump = false;
        }

        if (dash)
        {
            float dashAmount = 2f;
            Vector3 dashPosition = transform.position + moveDir * dashAmount;

            //If there is an untraversable object, then stop in front of it
            RaycastHit2D raycastHit2d = Physics2D.Raycast(transform.position, moveDir, dashAmount, dashLayerMask);
            if (raycastHit2d.collider != null)
            {
                dashPosition = raycastHit2d.point;
                dashPosition -= moveDir/2;
            }

            //Spawn visual effect

            //DashAudioStab.SetActive(true);
          
            yield return new WaitForSeconds(0.15f);
            m_Rigidbody2D.MovePosition(dashPosition);
            yield return new WaitForSeconds(0.15f);
            m_Rigidbody2D.MovePosition(dashPosition);
           
            //DashAudioStab.SetActive(false);


        }

    
	}





	private void Flip()
	{
		// Switch the way the player is labelled as facing.
		m_FacingRight = !m_FacingRight;

		transform.Rotate(0f, 180f, 0f);
	}

    public void FallingDetection(float timer)
    {

        if (timer > 3)
        {
            player.Falling();

        }
    }


}

