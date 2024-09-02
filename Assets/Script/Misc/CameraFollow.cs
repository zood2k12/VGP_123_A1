using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;

    public float minXClamp = -10.05f;
    public float maxXClamp = 224.01f;
    public float minYClamp = -1.43f;
    public float maxYClamp = 9.81f;

    

    //This function runs after fixed update - Unity specifies this is where camera movement should happens
    private void LateUpdate()
    {
        PlayerController pc = GameManager.Instance.PlayerInstance;
        Vector3 cameraPos = transform.position;

        cameraPos.x = Mathf.Clamp(pc.transform.position.x, minXClamp, maxXClamp);
        cameraPos.y = Mathf.Clamp(pc.transform.position.y, minYClamp, maxYClamp);

        transform.position = cameraPos;

    } 
}
