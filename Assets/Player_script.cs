using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.InputSystem;
using UnityEngine;

public class Player_script : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float collisionOffset = 0.05f;
    public ContactFilter2D movementFilter;
    private Vector2 moveInput;
    private List<RaycastHit2D> castCollision = new List<RaycastHit2D>();
    private Rigidbody2D rb;
    Animator animator;

    // Start is called before the first frame update
     public void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    public void FixedUpdate()
    {
        // rb.MovePosition(rb.position+(moveInput * moveSpeed * Time.fixedDeltaTime));
        if(moveInput != Vector2.zero) { 
            bool success = MovePlayer(moveInput);

            if(!success)
            {
                success = MovePlayer(new Vector2(moveInput.x, 0));
                if (!success)
                {
                    success = MovePlayer(new Vector2(0, moveInput.y));
                }
            }
            animator.SetBool("isMoving",success);
        }else{
            animator.SetBool("isMoving", false);
        }
    }
    public bool MovePlayer(Vector2 direction)
    {
        int count = rb.Cast(
            direction,
            movementFilter,
            castCollision,
            moveSpeed * Time.fixedDeltaTime + collisionOffset);

        if (count == 0) { 
            Vector2 moveVector = direction * moveSpeed * Time.fixedDeltaTime;
            rb.MovePosition(rb.position + moveVector);
            return true;
        }
        else
        {
            foreach (RaycastHit2D hit in castCollision)
            {
                print(hit.ToString());
            }
            return false;
        }
    }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
