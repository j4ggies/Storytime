using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour {

    //Intergers 
    public int curHealth;
    public int maxHealth;

    //floats
    public float distance;
    public float wakeRange;
    public float shootInterval;
    public float bulletSpeed = 100;
    public float bulletTimer;

    public bool awake = false;
    public bool lookingRight = true;

    public GameObject bullet;
    public Transform target;
    public Animator anim;
    public Transform shootPointLeft;
    public Transform shootPointRight;

    void Awake()
    {

        anim = gameObject.GetComponent<Animator>();

    }

    void Start()
    {

        curHealth = maxHealth;

    }

    void Update()
    {

        anim.SetBool("Awake", awake);
        anim.SetBool("LookingRight", lookingRight);

        RangeCheck();

        if (target.transform.position.x > target.transform.position.x)
        {

            lookingRight = true;

        }

    }

    void RangeCheck()
    {
        distance = Vector3.Distance(transform.position, target.transform.position);

        if(distance < wakeRange)
        {
            awake = true;

        }

        if (distance > wakeRange)
        {
            awake = false;

        }

    }

    public void Attack(bool attackingRight)
    {
        bulletTimer += Time.deltaTime;

        if(bulletTimer >= shootInterval)
        {
            Vector2 direction = target.transform.position - transform.position;
            direction.Normalize();

            if (!attackingRight)
            {
                GameObject bulletClone;
                bulletClone = Instantiate(bullet, shootPointLeft.transform.position, shootPointLeft.transform.rotation) as GameObject;
                bulletClone.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed;

                bulletTimer = 0;

            }


            if (attackingRight)
            {

                GameObject bulletClone;
                bulletClone = Instantiate(bullet, shootPointLeft.transform.position, shootPointLeft.transform.rotation) as GameObject;
                bulletClone.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed;
                bulletClone.transform.rotation = Quaternion.FromToRotation(Vector3.right, direction);
                bulletTimer = 0;

            }


        }

    }






}
