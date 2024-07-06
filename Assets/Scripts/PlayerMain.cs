using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class PlayerMain : MonoBehaviour
{
    Rigidbody2D rb;
    Animator animator;
    [SerializeField] Collider2D standingCollider;
    [SerializeField] Transform groundCheckCollider;
    [SerializeField] Transform overheadCheckCollider;
    [SerializeField] LayerMask groundLayer;

    [SerializeField] float speed = 1f;
    [SerializeField] float jumpPower = 500f;
    float horizontalValue;
    float runSpeedModifier = 2f;
    float crouchSpeedModifier = 0.5f;
    const float groundCheckRadius = 0.2f;
    const float overheadCheckRadius = 0.2f;

    bool jump = false;
    bool isGrounded = false;
    bool isRunning = false;
    public bool facingRight = true;
    bool crouchPressed;
    private CameraFollowObject _cameraFollowObject;
    private float fallSpeedYDampingChangeThreshold;

    [SerializeField] private GameObject cameraFollowGO;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        _cameraFollowObject = cameraFollowGO.GetComponent<CameraFollowObject>();
        fallSpeedYDampingChangeThreshold = CameraManager.instance.fallSpeedYDampingChangeThreshold;
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("yVelocity", rb.velocity.y);
        horizontalValue = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.LeftShift)) 
        {
            isRunning = true;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            isRunning = false;
        }
        if (Input.GetButtonDown("Jump"))
        {            
            jump = true;
            animator.SetBool("Jump", true);
        }
        else if (Input.GetButtonUp("Jump"))
        {
            jump = false;
        }
        if (Input.GetButtonDown("Crouch"))
        {
            crouchPressed = true;            
        }
        else if (Input.GetButtonUp("Crouch"))
        {
            crouchPressed = false;
        }

        if (rb.velocity.y < fallSpeedYDampingChangeThreshold && !CameraManager.instance.IsLerpimgYDamping && !CameraManager.instance.LerperdFromPlayerFalling) 
        {
            CameraManager.instance.LerpYDamping(true);
        }

        if (rb.velocity.y >= 0f && !CameraManager.instance.IsLerpimgYDamping && CameraManager.instance.LerperdFromPlayerFalling) 
        {
            CameraManager.instance.LerperdFromPlayerFalling = false;
            CameraManager.instance.LerpYDamping(false);
        }
    }

    void FixedUpdate()
    {
        GroundCheck();
        Move(horizontalValue, jump, crouchPressed);
    }

    void GroundCheck() 
    {
        isGrounded = false;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheckCollider.position, groundCheckRadius, groundLayer);
        if (colliders.Length > 0) 
        {
            isGrounded = true;
        }
        
        animator.SetBool("Jump", !isGrounded);
    }

    void Move(float dir, bool jumpFlag, bool crouchFlag)
    {
        #region Movement
        float xVal = dir * speed * 100 * Time.fixedDeltaTime;
        if (isRunning)
        {
            xVal *= runSpeedModifier;
        }
        if (crouchFlag) 
        {
            xVal *= crouchSpeedModifier;
        }
        Vector2 targetVelocity = new Vector2(xVal,rb.velocity.y);
        rb.velocity = targetVelocity;        

        if (facingRight && dir < 0)
        {
            Vector3 rotator = new Vector3(transform.rotation.x, 180f, transform.rotation.z);
            transform.rotation = Quaternion.Euler(rotator);
            facingRight = !facingRight;

            _cameraFollowObject.CallTurn();
        }

        else if (!facingRight && dir > 0) 
        {
            Vector3 rotator = new Vector3(transform.rotation.x, 0f, transform.rotation.z);
            transform.rotation = Quaternion.Euler(rotator);
            facingRight = !facingRight;

            _cameraFollowObject.CallTurn();
        }

        animator.SetFloat("xVelocity", Mathf.Abs(rb.velocity.x));
        #endregion

        #region Jump & Crouch
        if (!crouchFlag) 
        {
            if (Physics2D.OverlapCircle(overheadCheckCollider.position,overheadCheckRadius, groundLayer)) 
            {
                crouchFlag = true;
            }
        }

        if (isGrounded)
        {
            standingCollider.enabled = !crouchFlag;

            if (jumpFlag)
            {
                isGrounded = false;
                //jumpFlag = false;
                rb.AddForce(new Vector2(0f, jumpPower));
            }
        }

        animator.SetBool("Crouch", crouchFlag);
        #endregion
    }

}
