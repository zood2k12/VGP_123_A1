using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurret : Enemy
{
    //public Transform playerTransform;
    [SerializeField] private float disThreshold;
    [SerializeField] private float projectileFireRate;

    private float timeSinceLastFire = 0;


    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        if (projectileFireRate <= 0)
            projectileFireRate = 2;
    }

    // Update is called once per frame
    void Update()
    {
        PlayerController pc = GameManager.Instance.PlayerInstance;
        AnimatorClipInfo[] curPlayingClips = anim.GetCurrentAnimatorClipInfo(0);

        sr.flipX = (pc.transform.position.x < transform.position.x) ? true : false;

        //Changed from game GameObject.Find("Player") to pc
        float distance = Vector3.Distance(pc.transform.position, GameObject.Find("Turret").transform.position);

        if (curPlayingClips[0].clip.name.Contains("Idle"))
        {
            if (Time.time >= timeSinceLastFire + projectileFireRate && distance <= 4.0f)
            {
                anim.SetTrigger("Fire");
                timeSinceLastFire = Time.time;
            }
        }

        //GameObject.Find("Player") to pc
        if (pc.transform.position.x < GameObject.Find("Turret").transform.position.x)
        {
            sr.flipX = true;
        }
        else
        {
            sr.flipX = false;
        }
    }
}