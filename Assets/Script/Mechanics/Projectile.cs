using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    [SerializeField, Range(1, 50)] private float lifetime;

    void Start()
    {
        if (lifetime <= 0) lifetime = 2.0f;
        Destroy(gameObject, lifetime);
    }

    public void SetVelocity(float xVel, float yVel)
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(xVel, yVel);
    }

}
