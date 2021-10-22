using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Property")]
    public float speed = 10;
    public float jumpSpeed = 5;
    public Transform footPos;
    public LayerMask ground;
    private Rigidbody rig;
    private Animator anim;

    [Header("Item")]
    public GameObject launcher;
    public Transform launchPos;
    public float launcherSpeed = 20;

    [Header("Status")]
    public bool isJumping = false;

    private Vector3 lastDirection;
    private void Awake()
    {
        rig = this.GetComponent<Rigidbody>();
        anim = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        Attack();
        UpdateAnimation();
    }
    private void Attack()
    {
        if(Input.GetButtonDown("Fire1"))// Mouse0
        {
            GameObject la = Instantiate(launcher, launchPos.position+lastDirection.normalized,new Quaternion());
            la.GetComponent<Rigidbody>().velocity = launcherSpeed * new Vector3(lastDirection.x,0,lastDirection.z).normalized;
            Destroy(la, 2);
            anim.SetTrigger("Launch");
        }
    }
    private void Movement()
    {
        
        Ray ray = new Ray(footPos.position, new Vector3(0, -1, 0));
        if (Physics.Raycast(ray,0.2f,ground.value))
        {
            isJumping = false;
        }
        else
            isJumping = true;

        Vector3 moveDiraction = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
        
        
        moveDiraction *= speed;
        moveDiraction.y = rig.velocity.y;
        if (Input.GetButtonDown("Jump") && !isJumping)
        {
            moveDiraction.y += jumpSpeed;
        }
        moveDiraction = transform.TransformDirection(moveDiraction);
        rig.velocity = moveDiraction;

        // 控制动画状态，保持上一帧的动作
        if((Mathf.Abs(moveDiraction.x)>=0.001||Mathf.Abs(moveDiraction.z)>=0.001))
            lastDirection = moveDiraction.normalized;
    }

    private void UpdateAnimation()
    {
        
        anim.SetFloat("Look X", lastDirection.x);
        anim.SetFloat("Look Y", lastDirection.z);

        anim.SetFloat("Speed", rig.velocity.magnitude);
        
    }


}
