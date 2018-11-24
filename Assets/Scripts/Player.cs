using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public float speed = 50f;
    public float jumpPower = 150f;
    public float maxSpeed = 3;

    public bool grounded;
    public bool canDoubleJump;
 
    private Rigidbody2D rb2d;

    public Vector3 respawnPoint;


    //private bool facingRight;

    //stats
    public int curHealth;
    public int maxHealth = 100;

    private Animator anim;
    private gameMaster gm;

	// Use this for initialization

    

    void Start () {

        rb2d = gameObject.GetComponent<Rigidbody2D>();
        anim = gameObject.GetComponent<Animator>();
        gm = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<gameMaster>();


    }

    void Update()
    {

        anim.SetBool("Grounded",grounded);
        anim.SetFloat("Speed", Mathf.Abs(rb2d.velocity.x));

        if (Input.GetAxis("Horizontal") <  0.1f)
        {
            transform.localScale = new Vector3(-1,1,1);

        }

        if (Input.GetAxis("Horizontal") > 0.1f)
        {
            transform.localScale = new Vector3(1, 1, 1);

        }

        if (Input.GetButtonDown("Jump"))
        {
            if (grounded)
            {
                rb2d.AddForce(Vector2.up * jumpPower);
                canDoubleJump = true;
            }
            else
            {

                if (canDoubleJump)
                {

                    canDoubleJump = false;
                    rb2d.velocity = new Vector2(rb2d.velocity.x, 0);
                    rb2d.AddForce(Vector2.up * jumpPower/ 1.75f);

                }

            }
        }

    }
	
    void FixedUpdate()
    {
        Vector3 easeVelocity = rb2d.velocity;
        easeVelocity.y = rb2d.velocity.y;
        easeVelocity.z = 0.0f;
        easeVelocity.x *= 0.75f;


        float h = Input.GetAxis("Horizontal");

        //make fiction
        if(grounded)
        {
            rb2d.velocity = easeVelocity;

        }

        //move player
        rb2d.AddForce((Vector2.right * speed) * h);


        //speed limit
        if (rb2d.velocity.x > maxSpeed)
        {
            rb2d.velocity = new Vector2(maxSpeed, rb2d.velocity.y);

        }

        if (rb2d.velocity.x < -maxSpeed)
        {

            rb2d.velocity = new Vector2(-maxSpeed, rb2d.velocity.y);
        }



    }


    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Coin")
        {
            //ScopeScript.scoreValue += 10;
            Destroy(col.gameObject);
            gm.point += 1;
        }

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "FallDetector")
        {

            transform.position = respawnPoint;
        }

        if (other.tag == "CheckPoint")
        {
            respawnPoint = other.transform.position;

        }

    }



}
