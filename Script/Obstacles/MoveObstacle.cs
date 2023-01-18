using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObstacle : MonoBehaviour
{
    [SerializeField]private float maxWidth;
    [SerializeField]private float maxHeight;
    [SerializeField]private float speedX = 0.1f;
    [SerializeField]private float speedY = 0.1f;

    // Update is called once per frame
    void Update()
    {
        this.transform.position =  new Vector2(Mathf.PingPong(this.transform.position.x + Time.time * speedX, maxWidth), Mathf.PingPong(this.transform.position.y + Time.time * speedY, maxHeight));
    }
}
