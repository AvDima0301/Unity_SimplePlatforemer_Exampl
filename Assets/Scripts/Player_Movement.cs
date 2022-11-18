using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float jump;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private Vector3 offset;
    private GameObject mainCamera;
    private Rigidbody2D body;
    private Animator animator;
    private BoxCollider2D boxCollider;
    private bool grounded;
    private float wallJumpCooldown;
    private float horizontalInput;
    private Player_Attack player_Attack;
   

    private void Awake()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        player_Attack = GetComponent<Player_Attack>();
        speed = 2f;
        jump = 4f;
    }

    private void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        mainCamera.transform.position = new Vector3(body.position.x + offset.x, body.position.y + offset.y, offset.z);

        if (!player_Attack.isAttacking())
        {
            if (horizontalInput > 0.01f)
                transform.localScale = Vector3.one;
            if (horizontalInput < -0.01f)
                transform.localScale = new Vector3(-1, 1, 1);

            if (wallJumpCooldown > 0.2f)
            {
                if (onWall() && !onGround())
                {
                    body.gravityScale = 0;
                    body.velocity = Vector2.zero;
                }
                else
                    body.gravityScale = 1;

                body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);

                if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W))
                    Jump();
            }
            else
                wallJumpCooldown += Time.deltaTime;
        }


        animator.SetBool("run", horizontalInput != 0);
        animator.SetBool("grounded", onGround());
        animator.SetBool("on_wall", onWall());
        animator.SetBool("falling", isFalling());
    }

    private void Jump()
    {
        if (onGround())
        {
            body.velocity = new Vector2(body.velocity.x, jump);
            animator.SetTrigger("jump");
        }
        else if (onWall() && !onGround())
        {
            if (horizontalInput == 0)
            {
                body.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 2, 2);
                transform.localScale = new Vector3(-Mathf.Sign(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else
            {
                body.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 2, 2);
            }

            wallJumpCooldown = 0;
        }
    }

    private bool onGround()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.01f, groundLayer);
        return raycastHit.collider != null;
    }


    private bool onWall()
    {
        if (!onGround())
        {
            RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, new Vector2(transform.localScale.x, 0), 0.05f, wallLayer);
            return raycastHit.collider != null;
        }
        return false;
    }

    private bool isFalling()
    {
        if (body.velocity.y < 0 && !onGround())
        {
            return true;
        }
        return false;
    }

    public bool isMovingY()
    {
        if(body.velocity.y != 0)
        {
            return true;
        }
        return false;
    }

    public void PlayerStop()
    {
        body.velocity = Vector2.zero;
    }


    //private void OnCollisionExit2D(Collision2D collision)
    //{
    //    if (collision.gameObject.tag == "Ground" && !onGrounded() && !onWall())
    //    {
    //        animator.SetTrigger("fall");
    //    }
    //}

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if(collision.gameObject.tag == "Ground")
    //    {
    //        grounded = true;
    //    }
    //}
}

