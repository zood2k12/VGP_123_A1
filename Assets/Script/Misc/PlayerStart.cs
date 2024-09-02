using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStart : MonoBehaviour
{
    public float startingLives = 5;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.SpawnPlayer(transform);
        GameManager.Instance.lives = 5;
    }
}
