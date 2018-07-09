using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Player : MonoBehaviour
{
    [Tooltip("In tiles/s")] [SerializeField] private float moveSpeed = 2f;

    private Rigidbody2D rigidbody2d;
    private Transform childBody;

    // Use this for initialization
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        childBody = transform.Find("Body");
    }

    // Update is called once per frame
    void Update()
    {
        Run();
        FlipSprite();
    }

    private void Run()
    {
        float controlThrow = CrossPlatformInputManager.GetAxis("Horizontal");
        Vector2 playerVelocity = new Vector2(controlThrow * moveSpeed, rigidbody2d.velocity.y);

        rigidbody2d.velocity = playerVelocity;
    }
    
    private void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(rigidbody2d.velocity.x) > Mathf.Epsilon;
        if (playerHasHorizontalSpeed)
        {
            childBody.localScale = new Vector3(Mathf.Sign(rigidbody2d.velocity.x), 
                                    childBody.localScale.y, childBody.localScale.z);
        }
    }
}
