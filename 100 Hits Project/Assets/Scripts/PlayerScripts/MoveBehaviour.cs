using UnityEngine;
using System.Collections;

// MoveBehaviour inherits from GenericBehaviour. This class corresponds to basic walk and run behaviour, it is the default behaviour.
public class MoveBehaviour : GenericBehaviour
{
	public float walkSpeed = 0.15f;                 // Default walk speed.
	public float runSpeed = 1.0f;                   // Default run speed.
	public float sprintSpeed = 2.0f;                // Default sprint speed.
    public float dashSpeed = 20.0f;                 // Default dash speed.
    public float speedDampTime = 0.1f;              // Default damp time to change the animations based on current speed.
	public float jumpHeight = 1.0f;                 // Default jump height.

	private float speed;                            // Moving speed.
	private int jumpBool;                           // Animator variable related to jumping.
	private int groundedBool;                       // Animator variable related to whether or not the player is on ground.
    private int Dashbool;                           // Animator variable related to whether or not the player is dashing.
    private bool run;                               // Boolean to determine whether or not the player activated the run mode.
	private bool jump;                              // Boolean to determine whether or not the player started a jump.
    private bool dash;                              // Boolean to determine whether or not the player is dashing.
    private float DashStartTime;                //keeps track of the point in time the dash started
    private float DashDurration;            //how long the dash is in time

    // Start is always called after any Awake functions.
    void Start() 
	{
        dash = false;
        // Set up the references.
        jumpBool = Animator.StringToHash("Jump");
		groundedBool = Animator.StringToHash("Grounded");
		anim.SetBool (groundedBool, true);
        Dashbool = Animator.StringToHash("isDashing");
        DashStartTime = 0;
        DashDurration = 0.01f;

        // Subscribe and register this behaviour as the default behaviour.
        behaviourManager.SubscribeBehaviour (this);
		behaviourManager.RegisterDefaultBehaviour (this.behaviourCode);
	}

	// Update is used to set features regardless the active behaviour.
	void Update ()
	{
		// Activate run by input.
		run = Input.GetButton ("Run");
		if(Input.GetButtonDown ("Jump"))
			jump = true;
        if (Time.time > (DashStartTime + DashDurration))
        {
            dash = false;
            anim.SetBool(Dashbool, false);
            //print("dashend");
        }
        if (Input.GetButtonDown("Dash"))
        {
            DashStartTime = Time.time;
            //print("dash time!!~!~!!");
            dash = true;
            anim.SetBool(Dashbool, true);
        }
    }

	// LocalFixedUpdate overrides the virtual function of the base class.
	public override void LocalFixedUpdate()
	{
		// Call the basic movement manager.
		MovementManagement (behaviourManager.GetH, behaviourManager.GetV, run, dash);

		// Call the jump manager.
		JumpManagement();
	}

	// Execute the idle and walk/run jump movements.
	void JumpManagement()
	{
		// Already jumped, landing.
		if (anim.GetBool(jumpBool) && rbody.velocity.y < 0)
		{
			// Set jump boolean on the Animator controller.
			jump = false;
			anim.SetBool (jumpBool, false);
		}
		// Start jump.
		if (jump && !anim.GetBool(jumpBool) && IsGrounded())
		{
			// Set jump boolean on the Animator controller.
			anim.SetBool(jumpBool, true);
			if(speed > 0)
			{
				// Set jump vertical impulse when moving.
				rbody.AddForce (Vector3.up * jumpHeight * rbody.mass * 10, ForceMode.Impulse);
			}
		}
	}

	// Deal with the basic player movement
	void MovementManagement(float horizontal, float vertical, bool running, bool dashing)
	{
		// On ground, obey gravity.
		if (anim.GetBool(groundedBool))
			rbody.useGravity = true;

		// Call function that deals with player orientation.
		Rotating(horizontal, vertical);

		// Set proper speed.
		if(behaviourManager.IsMoving())
		{
			if(behaviourManager.isSprinting())
			{
				speed = sprintSpeed;
			}
			else if (running)
			{
				speed = runSpeed;
			}
            else if (dashing)
            {
                speed = dashSpeed;
            }
			else
			{
				speed = walkSpeed;
			}
		}
		else
		{
			speed = 0f;
		}
		anim.SetFloat(speedFloat, speed, speedDampTime, Time.deltaTime);
	}

	// Rotate the player to match correct orientation, according to camera and key pressed.
	Vector3 Rotating(float horizontal, float vertical)
	{
		// Get camera forward direction, without vertical component.
		Vector3 forward = behaviourManager.playerCamera.TransformDirection(Vector3.forward);

		// Player is moving on ground, Y component of camera facing is not relevant.
		forward.y = 0.0f;
		forward = forward.normalized;

		// Calculate target direction based on camera forward and direction key.
		Vector3 right = new Vector3(forward.z, 0, -forward.x);
		Vector3 targetDirection;
		float finalTurnSmoothing;
		targetDirection = forward * vertical + right * horizontal;
		finalTurnSmoothing = behaviourManager.turnSmoothing;

		// Lerp current direction to calculated target direction.
		if((behaviourManager.IsMoving() && targetDirection != Vector3.zero))
		{
			Quaternion targetRotation = Quaternion.LookRotation (targetDirection);

			Quaternion newRotation = Quaternion.Slerp(rbody.rotation, targetRotation, finalTurnSmoothing * Time.deltaTime);
			rbody.MoveRotation (newRotation);
			behaviourManager.SetLastDirection(targetDirection);
		}
		// If idle, Ignore current camera facing and consider last moving direction.
		if(!(Mathf.Abs(horizontal) > 0.9 || Mathf.Abs(vertical) > 0.9))
		{
			behaviourManager.Repositioning();
		}

		return targetDirection;
	}
}
