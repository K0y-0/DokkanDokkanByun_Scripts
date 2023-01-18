using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class eightMove : MonoBehaviour
{
    private float angle;
    private Vector2 initPos;
    private SpriteRenderer sr;

    void Start()
    {
        initPos = this.transform.position;
        sr = this.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        angle += Time.deltaTime * 2;

        transform.position = new Vector3(initPos.x + Mathf.Sin(angle) * 12, initPos.y + Mathf.Sin(angle * 2) * 2, transform.position.z);

        if(angle < 0)
        {
            sr.flipX = true;
        }
        else
        {
            sr.flipX = false;
        }
    }
}
