using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]

public class PlayerController : MonoBehaviour
{
    
    public float walkSpeed = 5f;
    public float runSpeed = 8f;
    Vector2 moveInput;

    public float CurrentMoveSpeed
    {
        get  
        {
            if (IsMoving)
            {
                if (IsRunning)
                {
                    return runSpeed;
                }
                else
                {
                    return walkSpeed;
                }
            }
            else 
            {
                return 0;
            }
        }
        
        
    }
    

    [SerializeField]
    private bool isMoving = false;

    public bool IsMoving 
    { 
        get
        {
            return isMoving; 
        } 
        set 
        { 
            isMoving = value; 
            animator.SetBool(AnimationStrings.isMoving, value);
        }
    }

    [SerializeField] 
    private bool isRunning = false;

    public bool IsRunning 
    { 
        get
        {
            return isRunning; 
        } 
        set 
        { 
            isRunning = value; 
            animator.SetBool(AnimationStrings.isRunning, value);
        } 
    }

    public bool _isFacingRight = true;

    public bool IsFacingRight 
    { 
        get 
        {
            return _isFacingRight; 
        }
        private set 
        {
            if(_isFacingRight != value)
            {
                transform.localScale *= new Vector2(-1, 1);
            }
            IsFacingRight = value;
        }
        
    }

    Rigidbody2D rb;
    Animator animator;  

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(moveInput.x * CurrentMoveSpeed, rb.velocity.y);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();

        IsMoving = moveInput != Vector2.zero;

        SetFacingDirection(moveInput);
    }

    private void SetFacingDirection(Vector2 moveInput)
    {
        
        if(moveInput.x < 0)
        {
            transform.localScale = new Vector2(-1, 1);
        }
        else if(moveInput.x > 0)
        {
            transform.localScale = new Vector2(1, 1);
        }
        
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            IsRunning = true;
        }
        else if (context.canceled)
        {
            IsRunning = false;
        }
    }

}


