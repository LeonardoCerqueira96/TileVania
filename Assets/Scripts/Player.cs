using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Player : MonoBehaviour
{
    // Config
    [Tooltip("In tiles/s")] [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float jumpSpeed = 5f;
    [SerializeField] private float climbSpeed = 5f;

    // State
    private bool isAlive = true;


    // Cached component references
    private Rigidbody2D myRigidBody;
    private Animator myAnimator;
    private Transform childBody;
    private Transform childFeet;
    private float defaultGravity;

    // Use this for initialization
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();

        childBody = transform.Find("Body");
        childFeet = transform.Find("Feet");

        defaultGravity = myRigidBody.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        Run();
        Jump();
        ClimbLadders();
        FlipSprite();
    }

    private void Run()
    {
        float controlThrow = CrossPlatformInputManager.GetAxis("Horizontal");
        Vector2 playerVelocity = new Vector2(controlThrow * moveSpeed, myRigidBody.velocity.y);

        myRigidBody.velocity = playerVelocity;

        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;
        myAnimator.SetBool("Running", playerHasHorizontalSpeed);
    }

    private void Jump()
    {
        if (!childFeet.GetComponent<Collider2D>().IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            return;
        }

        if (CrossPlatformInputManager.GetButtonDown("Jump"))
        {
            Vector2 jumpVelocityToAdd = new Vector2(0f, jumpSpeed);
            myRigidBody.velocity += jumpVelocityToAdd;
        }
    }

    private void ClimbLadders()
    {
     
        if (!myRigidBody.IsTouchingLayers(LayerMask.GetMask("Ladders")))
        {
            myAnimator.SetBool("Climbing", false);
            myRigidBody.gravityScale = defaultGravity;
            return;
        }

        myRigidBody.gravityScale = 0f;

        float controlThrow = CrossPlatformInputManager.GetAxis("Vertical");
        float climbSpeedToSet = controlThrow * climbSpeed;
        myRigidBody.velocity = new Vector2(myRigidBody.velocity.x, climbSpeedToSet);

        bool playerHasVerticalSpeed = Mathf.Abs(myRigidBody.velocity.y) > Mathf.Epsilon;
        myAnimator.SetBool("Climbing", playerHasVerticalSpeed);
    }
    
    private void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;
        if (playerHasHorizontalSpeed)
        {  
            childBody.localScale = new Vector3(Mathf.Sign(myRigidBody.velocity.x), 
                                    childBody.localScale.y, childBody.localScale.z);
        }
    }
}
