using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

//Can only add 3 components at a time so need to add another line
[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer), typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
public class PlayerController : MonoBehaviour
{
    //Player gameplay variables
    private Coroutine jumpForceChange;
    private Coroutine speedChange;


    AudioSource source;
    //Remove the comment below when ready; used to finish the Lab 4
    public void PowerupValueChange(Pickup.PickupType type)
    {
        if (type == Pickup.PickupType.PowerupSpeed)
            StartPowerupCoroutine(ref speedChange, ref speed, type);

        if (type == Pickup.PickupType.PowerupJump)
            StartPowerupCoroutine(ref jumpForceChange, ref jumpForce, type);
    }

    public void StartPowerupCoroutine(ref Coroutine InCoroutine, ref float inVar, Pickup.PickupType type)
    {
        if (InCoroutine != null)
        {
            StopCoroutine(InCoroutine);
            InCoroutine = null;
            inVar /= 2;
        }

        InCoroutine = StartCoroutine(PowerupChange(type));
    }

    IEnumerator PowerupChange(Pickup.PickupType type)
    {
        //this code runs before the wait
        if (type == Pickup.PickupType.PowerupSpeed)
            speed *= 2;

        if (type == Pickup.PickupType.PowerupJump)
            jumpForce *= 2;

        Debug.Log($"Jump force value is {jumpForce}, Speed value is {speed}");

        yield return new WaitForSeconds(5.0f);

        if (type == Pickup.PickupType.PowerupSpeed)
        {
            speed /= 2;
            speedChange = null;
        }
        if (type == Pickup.PickupType.PowerupJump)
        {
            jumpForce /= 2;
            jumpForceChange = null;
        }

        Debug.Log($"Jump force value is {jumpForce}, Speed value is {speed}");
    }

    //This section below from private _lives to SerializedField was cut and pasted on the GameManager.cs
    //Private Lives Variable
    //private int _lives = 10;


    //public variable for getting and setting lives
    //public int lives
    //{
    //    get
    //    {
    //        return _lives;
    //    }
    //    set
    //    {
    //        all lives lost(zero counts as a life due to the check)
    //        if (value < 0)
    //        {
    //            game over function called here
    //            return to prevent the rest of the function to be called
    //            if the game over has a scene and move over then theres nothing to return to, but this return below its more like a safe thing to have
    //            return;
    //        }

    //        lost a life
    //        if (value < _lives)
    //        {
    //            Respawn function called here
    //        }

    //        cannot roll over max lives
    //        if (value > maxLives)
    //        {
    //            value = maxLives;
    //        }

    //        _lives = value;

    //        Debug.Log($"Lives value on {gameObject.name} has changed to {lives}");
    //    }
    //}

    //max lives that are possible
    //[SerializeField] private int maxLives = 10;
    //Private Lives Variable
    //private int _lives = 10;


    //public variable for getting and setting lives


    //Movement Variables
    [SerializeField, Range(1, 20)] private float speed = 5;
    [SerializeField, Range(1, 20)] private float jumpForce = 10;
    [SerializeField, Range(0.01f, 1)] private float groundCheckRadius = 0.02f;
    [SerializeField] private LayerMask isGroundLayer;

    //GroundCheck Stuff
    private Transform groundCheck;
    private bool isGrounded = false;

    //Component References
    Rigidbody2D rb;
    SpriteRenderer sr;
    Animator anim;
    AudioSource audioSource;

    //Audio Clip References
    [SerializeField] private AudioClip jumpClip;
    [SerializeField] private AudioClip stompClip;
    [SerializeField] private AudioClip playerDeathClip;
    //AudioMixerCHannel reference
    public AudioMixerGroup SFXGroup;

    // Start is called before the first frame update
    void Start()
    {
        //Component References Filled
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        audioSource.outputAudioMixerGroup = GameManager.Instance.SFXGroup;

        //Checking values to ensure non garbage data
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

        //Creating groundcheck object
        if (!groundCheck)
        {
            GameObject obj = new GameObject();
            obj.transform.SetParent(transform);
            obj.transform.localPosition = Vector3.zero;
            obj.name = "GroundCheck";
            groundCheck = obj.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Added Early returns to help for not run the lines of code anymore
        if (Time.timeScale <= 0) return;

        AnimatorClipInfo[] curPlayingClips = anim.GetCurrentAnimatorClipInfo(0);
        //grab horizontal axis - Check Project Settings > Input Manager to see the inputs defined
        float hInput = Input.GetAxis("Horizontal");

        //Create a small overlap collider to check if we are touching the ground
        IsGrounded();


        //Animation check for our physics
        if (curPlayingClips.Length > 0)
        {
            if (curPlayingClips[0].clip.name == "Attack")
            {
                if (isGrounded)
                 rb.velocity = Vector2.zero;
                //new Vector2(0, rb.velocity.y);
            }
            else
                rb.velocity = new Vector2(hInput * speed, rb.velocity.y);
        }


        //Button Input Checks
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        if (Input.GetButtonDown("Fire1"))
        {
            if (!isGrounded && curPlayingClips[0].clip.name != "JumpAttack")
            {
                anim.SetTrigger("JumpAttack");
            }
            else if (!curPlayingClips[0].clip.name.Contains("Attack"))
            {
                anim.SetTrigger("Fire");
            }
        }

        //Sprite Flipping
        if (hInput != 0) sr.flipX = (hInput < 0);
        //if (hInput > 0 && sr.flipX || hInput < 0 && !sr.flipX) sr.flipX = !sr.flipX;

        anim.SetFloat("hInput", Mathf.Abs(hInput));
        anim.SetBool("isGrounded", isGrounded);

    }

    /// <summary>
    /// This function is used to check if we are grounded.  When we jump - we disable checking if we are grounded until our velocity reaches negative on the y-axis - this indicates that we are falling and we should start to check if we are grounded again. This is done to prevent us flipping to grounded when we jump through a platform.
    /// </summary>
    void IsGrounded()
    {
        if (!isGrounded)
        {
            if (rb.velocity.y <= 0)
            {
                isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, isGroundLayer);
            }
        }
        else
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, isGroundLayer);
    }

    void IncreaseGravity()
    {
        rb.gravityScale = 10;  
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            GameManager.Instance.lives--;
            audioSource.PlayOneShot(playerDeathClip);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Squish"))
        {
            collision.gameObject.GetComponentInParent<Enemy>().TakeDamage(9999);
            rb.velocity = Vector2.zero;
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            audioSource.PlayOneShot(stompClip);
        }
    }
    public void MarioDeathAudio()
    {
        audioSource.PlayOneShot(playerDeathClip);
    }
}