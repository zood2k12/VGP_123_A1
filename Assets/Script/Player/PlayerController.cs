using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer), typeof(Animation))]
public class NewBehaviourScript : MonoBehaviour
{
    //Movement Variables
    [SerializeField, Range(1, 20)] private float speed = 5;
    [SerializeField, Range(1, 20)] private float jumpForce = 10;
    [SerializeField, Range(0.01f, 1)] private float groundCheckRadius = 0.02f;
    [SerializeField] private LayerMask isGroundLayer;

    //GroundCheck stuff
    private Transform groundcheck;
    private bool isGround = false;

    //Component References
    Rigidbody2D rb;
    SpriteRenderer sr;
    Animator anim;



    // Start is called before the first frame update
    void Start()
    {
        //Components References Filled *Need to add these components everytime you add comp referecens*
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        Debug.Log(rb.name);

        if (speed <= 0)
        {
            speed = 5;
            Debug.Log("Speed was set incorrectly");
        }

        if (jumpForce <= 0)
        {
            jumpForce = 10;
            Debug.Log("JumpForce was set incorrectly");
        }

        if (!groundcheck)
        {
            GameObject obj = new GameObject();
            obj.transform.SetParent(gameObject.transform);
            obj.transform.localPosition = Vector3.zero;
            obj.name = "GroundCheck";
            groundcheck = obj.transform;
        }

    }

    // Update is called once per frame
    void Update()
    {
       if (!isGround)

        {
            if (rb.velocity.y <= 0)
            {
                isGround = IsGround();
            }
        }

        isGround = Physics2D.OverlapCircle(groundcheck.position, groundCheckRadius, isGroundLayer);

       //grab horizontal axis - Check Project Settings > Input Manager to see the inputs defined
        float hInput = Input.GetAxis("Horizontal");



        rb.velocity = new Vector2(hInput * speed, rb.velocity.y);

        if (Input.GetButtonDown("Jump") && isGround)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

        }

        //Sprite Flipling
        if (hInput != 0) sr.flipX = (hInput < 0);
        //if (hInput > 0 && sr.flipX || hInput < 0 && !sr.flipX) sr.flipX = !sr.flipX;

        anim.SetFloat("hInput", Mathf.Abs(hInput));
        anim.SetBool("isGrounded", isGround);

    }
    bool Isground()
    {
        return Physics2D.OverlapCircle(groundcheck.position, groundCheckRadius, isGroundLayer);
    }
}