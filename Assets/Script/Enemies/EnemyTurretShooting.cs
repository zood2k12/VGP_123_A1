using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurretShooting : MonoBehaviour
{
    public GameObject bullet;
    public Transform bulletPos;

    public float timer;
    
    // Start is called before the first frame update
    void start()
    {

    }

    // Update is called once per frame
    private void Update()
    {
        timer += Time.deltaTime;

        if (timer > 2)
        {
            timer = 0;
            shoot();


        }
        void shoot()
        {
            Instantiate(bullet, bulletPos.position, Quaternion.identity);

        }
    }