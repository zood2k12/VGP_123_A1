using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupSpawn : MonoBehaviour
{
    //Script it`s a bit less generic and have to check for more random ones to apply for future projects
    //This script serves to spawn randomly for Lab 5, solution
     
    public GameObject spawnPrefab;


    // Start is called before the first frame update
    void Start()
    {
        float randXPos = Random.Range(-8.05f, 200.01f);
        float randYPos = Random.Range(-8.05f, 200.01f);

        transform.position = Vector2(randXPos, randYPos);

        int randNum = Random.Range(0, spawnPrefab.Length);

        if (spawnPrefab[randNum] == null) return;

        Instantiate(spawnPrefab[randNum], transform.position, transform.rotation
    }

 
}
