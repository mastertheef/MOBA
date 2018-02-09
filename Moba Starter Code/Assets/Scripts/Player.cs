using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour {

    private Transform targetEnemy;
    private bool enemyClicked = false;
    private bool isWalking;

    private NavMeshAgent nav;
    private Animator anim;
    private float nextFire;

    [SerializeField] float ShootDistance = 10;
    [SerializeField] float TimeBetweenShots = 10;

    // Use this for initialization
    void Start () {
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Input.GetButtonDown("Fire2"))
        {
            if (Physics.Raycast(ray, out hit, 100))
            {
                if (hit.collider.CompareTag("Enemy"))
                {
                    targetEnemy = hit.transform;
                    enemyClicked = true;
                }
                else
                {
                    isWalking = true;
                    enemyClicked = false;

                    nav.destination = hit.point;
                    nav.isStopped = false;

                }
            }
        }

        if (enemyClicked)
        {
            MoveAndShoot();
        }

        // isWalking = nav.remainingDistance > nav.stoppingDistance;

        else if (nav.remainingDistance <= nav.stoppingDistance)
        {
            isWalking = false;
        }
        else
        {
            isWalking = true;
        }

        anim.SetBool("IsWalking", isWalking);
        
	}


    void MoveAndShoot()
    {
        if (targetEnemy == null)
        {
            return;
        }

        nav.destination = targetEnemy.position;
        if (nav.remainingDistance >= ShootDistance)
        {
            nav.isStopped = false;
            isWalking = true;
        }

        if (nav.remainingDistance <= ShootDistance)
        {
            transform.LookAt(targetEnemy);
            if (Time.time > nextFire)
            {
                nextFire = Time.time + TimeBetweenShots;
                Fire();
            }
            nav.isStopped = true;
            isWalking = false;
            
        }
    }

    private void Fire()
    {
        anim.SetBool("IsWalking", false);
        anim.SetTrigger("Attack");
        //Debug.Log("FIRE");
    }
}
