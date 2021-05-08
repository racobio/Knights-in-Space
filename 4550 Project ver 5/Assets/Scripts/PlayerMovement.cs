using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	public CharacterController2D controller;
	public Animator animator;
	public float runSpeed = 40f;

	float horizontalMove = 0f;
	bool jump = false;
    bool dash = false;
    

    // Update is called once per frame
    void Update () {

		horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

		animator.SetFloat("Speed", Mathf.Abs(horizontalMove));
      

		if (Input.GetButtonDown("Jump"))
		{
			jump = true;
			animator.SetBool("IsJumping", true);
		}

        if (Input.GetButtonDown("Dash"))
        {
            dash = true;
            animator.SetTrigger("Dash");
        }
		

	}

	public void OnLanding ()
	{
		animator.SetBool("IsJumping", false);
	}



	void FixedUpdate ()
	{
		// Move our character
		StartCoroutine(controller.Move(horizontalMove * Time.fixedDeltaTime, jump, dash));
		jump = false;
        dash = false;
	}
}
