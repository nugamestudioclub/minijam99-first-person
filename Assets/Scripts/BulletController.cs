using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField]
    private float lifetime = 5f;
    private float currentLifetime;
    [SerializeField]
    private float explosionTime = .25f;
    private float currentExplosionTime;

    private Rigidbody rigidbody;
    private SphereCollider collider;
    private float startingColliderRad;
    private Animator animator;

    [SerializeField]
    private float speed = 1f;
    [SerializeField]
    private int damage = 5;

    [SerializeField]
    private AudioSource audioSource;

    public Vector3 direction;

    private bool isExploding = false;

    void Awake()
    {
        currentLifetime = 0;
        currentExplosionTime = 0f;
        rigidbody = GetComponent<Rigidbody>();
        collider = GetComponent<SphereCollider>();
        startingColliderRad = collider.radius;
        animator = GetComponentInChildren<Animator>();
    }

 

    // Update is called once per frame
    void Update()
    {
        
        if (isExploding)
        {
            rigidbody.velocity = Vector3.zero;
            currentExplosionTime += Time.deltaTime;
            
        }else
        {
            //keep moving
            rigidbody.velocity = direction * speed;
        }
        currentLifetime += Time.deltaTime;
        if (currentLifetime < .1f)
        {
            collider.radius = startingColliderRad * 1.3f;
        } else
        {
            collider.radius = startingColliderRad;
        }
        if (currentLifetime > lifetime || currentExplosionTime > explosionTime)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (!collision.CompareTag("Sky"))
        {
            //if colliding with enemy
            if (collision.TryGetComponent(out EnemyAI enemy))
            {
                //deal dmg to enemy
                enemy.TakeDamage(damage);
                Explode();
            } else if (currentLifetime > .1f)
            {
                collider.radius = startingColliderRad * 1.3f;
                Explode();
            }

        }
       
    }

    private void Explode()
    {
        Debug.Log(gameObject.name + " is exploding");
        //play explosion animation
        animator.Play("explode");
        //play explosion sound
        audioSource.Play();
        //start exploding
        isExploding = true;


    }

}
