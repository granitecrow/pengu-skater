using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : MonoBehaviour {
    
    private const float LANE_DISTANCE = 2.5f;
    private const float TURN_SPEED = 0.05f;

    //
    private bool isRunning = false;


    // Animation
    private Animator anim;


    //Movement vars
    private CharacterController controller;
    private float jumpForce = 4.0f;
    private float gravity = 12.0f;
    private float verticalVelocity;

    private int desiredLane = 1; // 0 = left; 1 = middle; 2 = right

    // speed modifier
    private float originalSpeed = 8.0f;
    private float speed = 8.0f;
    private float speedIncreaseLastTick;
    private float speedIncreaseTime = 2.5f;
    private float speedIncreaseAmount = 0.1f;

    private void Start()
    {
        speed = originalSpeed;
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!isRunning)
            return;

        if (Time.time - speedIncreaseLastTick > speedIncreaseTime)
        {
            speedIncreaseLastTick = Time.time;
            speed += speedIncreaseAmount;

            //change modifier text
            GameManager.Instance.UpdateModifier(speed - originalSpeed);
        }

        // gather the inputs on direction change
        if (MobileInput.Instance.SwipeLeft)
        {
            // move left
            MoveLane(-1);
        }
        if (MobileInput.Instance.SwipeRight)
        {
            // move right
            MoveLane(1);
        }

        //calculate where we want to go
        Vector3 targetPosition = transform.position.z * Vector3.forward;
        if (desiredLane == 0)
        {
            targetPosition += Vector3.left * LANE_DISTANCE;
        } else if (desiredLane == 2)
        {
            targetPosition += Vector3.right * LANE_DISTANCE;
        }

        //calculate move vector
        Vector3 moveVector = Vector3.zero;
        moveVector.x = (targetPosition - transform.position).normalized.x * speed;

        bool isGrounded = IsGrounded();
        anim.SetBool("Grounded", isGrounded);

        //calculate Y
        if (isGrounded) // if grounded
        {
            verticalVelocity = -0.1f; // snap to the floor
            if (MobileInput.Instance.SwipeUp)
            {
                //jump
                anim.SetTrigger("Jump");
                verticalVelocity = jumpForce;
            }
            else if (MobileInput.Instance.SwipeDown)
            {
                // slide
                // shrink character control, change center, slide animation
                StartSliding();
                Invoke("StopSliding", 1.0f);
            }
        }
        else
        {
            verticalVelocity -= (gravity * Time.deltaTime);

            // if you swipe down again, then fast fall
            if (MobileInput.Instance.SwipeDown)
            {
                verticalVelocity = -jumpForce;
            }
        }

        moveVector.y = verticalVelocity;
        moveVector.z = speed;

        //move the penguin
        controller.Move(moveVector * Time.deltaTime);

        //rotate penguin towards the direction it is moving
        Vector3 direction = controller.velocity;
        if (direction != Vector3.zero)
        {
            direction.y = 0; // could be jumping so we don't want this to be affected
            transform.forward = Vector3.Lerp(transform.forward, direction, TURN_SPEED);
        }

    }

    private void MoveLane(int laneChange)
    {
        desiredLane += laneChange;
        desiredLane = Mathf.Clamp(desiredLane, 0, 2);
    }

    private bool IsGrounded()
    {
        Ray groundRay = new Ray(
            new Vector3(
                controller.bounds.center.x, 
                (controller.bounds.center.y - controller.bounds.extents.y) + 0.2f, 
                controller.bounds.center.z), 
            Vector3.down);
        Debug.DrawRay(groundRay.origin, groundRay.direction, Color.cyan, 1.0f);

        return Physics.Raycast(groundRay, (0.2f + 0.1f));


    }

    public void StartRunning()
    {
        isRunning = true;
        anim.SetTrigger("StartRunning");
        //TODO: swap the camera from different view to the running view
    }

    private void StartSliding()
    {
        anim.SetBool("Sliding", true);
        controller.height /= 2;
        controller.center = new Vector3(controller.center.x, controller.center.y / 2, controller.center.z);
    }

    private void StopSliding()
    {
        anim.SetBool("Sliding", false);
        controller.height *= 2;
        controller.center = new Vector3(controller.center.x, controller.center.y * 2, controller.center.z);
    }

    private void Crash()
    {
        anim.SetTrigger("Death");
        isRunning = false;
        GameManager.Instance.OnDeath();
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        switch (hit.gameObject.tag)
        {
            case "Obstacle":
                Crash();
                break;
            

        }
    }

}
