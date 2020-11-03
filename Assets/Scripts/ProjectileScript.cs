using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    public float speed;
    public float lifeTime;
    public GameObject projectileDestroyEffect;

    void Start()
    {
        Invoke("destroyProjectile", lifeTime);
    }

    void Update()
    {
        transform.Translate(Vector2.down * speed * Time.deltaTime);
    }

    void destroyProjectile()
    {
        Destroy(Instantiate(projectileDestroyEffect, transform.position, Quaternion.Euler(this.transform.rotation.x, this.transform.rotation.y, this.transform.rotation.z)), 0.5f);
        Destroy(this.gameObject);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        destroyProjectile();
    }
}
