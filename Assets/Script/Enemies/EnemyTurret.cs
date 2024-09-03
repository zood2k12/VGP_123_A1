using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurret : Enemy
{
    //Hisham's method
    //public Transform playerTransform;
    [SerializeField] private float distThreshold;
    [SerializeField] private float projectileFireRate;

    private float timeSinceLastFire = 0;


    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        if (projectileFireRate <= 0)
            projectileFireRate = 2;

        //Hisham's method
        if (projectileFireRate <= 0)
            projectileFireRate = 2;

        if (distThreshold <= 0)
            distThreshold = 2;
    }

    // Update is called once per frame
    void Update()
    {
        PlayerController pc = GameManager.Instance.PlayerInstance;
        AnimatorClipInfo[] curPlayingClips = anim.GetCurrentAnimatorClipInfo(0);

        //Hisham's method
        sr.flipX = (pc.transform.position.x < transform.position.x) ? true : false;

        //float distance = Vector2.Distance([playerTransform.position, transform.position.x);

        //if (playerTransform.position.x <  transform.position.x)
        //    sr.flipX = true;
        //else 
        //    sr.flipX = false;


        float distance = Vector2.Distance(pc.transform.position, transform.position);

        if (distance < distThreshold)
        {
            sr.color = Color.red;
            if (curPlayingClips[0].clip.name.Contains("Idle"))
            {
                if (Time.time >= timeSinceLastFire + projectileFireRate)
                {
                    anim.SetTrigger("Fire");
                    timeSinceLastFire = Time.time;
                }
            }
        }
        else
        {
            sr.color = Color.white;
        }


        //MY OLD SCRIPT
        //float distance = Vector2.Distance(GameObject.Find("Player").transform.position, GameObject.Find("Turret").transform.position);


        //if (curPlayingClips[0].clip.name.Contains("Idle"))
        //{
        //    if (Time.time >= timeSinceLastFire + projectileFireRate && distance <= 6.0f)
        //    {
        //        anim.SetTrigger("Fire");
        //        timeSinceLastFire = Time.time;
        //    }
        //}

        //if (GameObject.Find("Player").transform.position.x < GameObject.Find("Turret").transform.position.x)
        //{
        //    sr.flipX = true;
        //}
        //else
        //{
        //    sr.flipX = false;
        //}
    }
}