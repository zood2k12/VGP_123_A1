using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEditor;
using UnityEngine;


[RequireComponent(typeof(SpriteRenderer), typeof(Animator))]
public abstract class Enemy : MonoBehaviour
{
    //private - private to the class that has created it. It is only 
    //
    //protected - private but also accessable to child class
    protected SpriteRenderer sr;
    protected Animator an;

    protected int health;
    [SerializeField] protected int maxHealth;



    // Start is called before the first frame update

    // virtual = something to do with the child classes
    void public virtual Start()
    {
        sr = GetComponent<SpriteRenderer>();
        an = GetComponent<Animator>();

        if (maxHealth <= 0) maxHealth = 10;

        health = maxHealth;
        
    }

    public void virtual TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            anim.SetTrigger("Death");
        }
    }
    
}
