using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Property")]
    public float Speed = 11; 
    private Rigidbody rig;
    private Animator anim;

    private Transform destination;
    private Vector3 direction;
    private float patrolRate = 3;
    private float currentPatrolTime;

    public bool isFixed = false;
    public bool findPlayer = false;
    // Start is called before the first frame update
    void Start()
    {
        rig = this.GetComponent<Rigidbody>();
        anim = this.GetComponent<Animator>();

        currentPatrolTime = patrolRate;
        direction = new Vector3(Random.Range(0, 1), 0, Random.Range(0, 1)).normalized;
    }

    // Update is called once per frame
    void Update()
    {
        Patrol();
    }

    private void Patrol()
    {
        if(findPlayer)
        {
            direction = (destination.position - transform.position).normalized;
            MoveMent(direction);
        }
        else
        {
            if(currentPatrolTime > 0)
            {
                currentPatrolTime -= Time.deltaTime;
                MoveMent(direction);
            }
            else
            {
                direction = new Vector3(Random.Range(0, 1), 0, Random.Range(0, 1)).normalized;
                currentPatrolTime = patrolRate;
            }
        }
    }
    private void MoveMent(Vector3 direction)
    {
        rig.velocity = direction * Speed;
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player")
        {
            destination = other.transform;
            findPlayer = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            findPlayer = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "CogBullet")
        {
            anim.SetBool("Fixed", true);
            Speed = 1.0f;
        }
        else if(collision.gameObject.tag != "Player")
        {
            patrolRate = 0;
        }
    }
}
