using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class leftToRight : MonoBehaviour
{
    private Vector3 initPos;
    private SpriteRenderer sr;
    // Start is called before the first frame update
    void Start()
    {
        initPos = this.transform.position;
        sr = this.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        float sin = Mathf.Sin(Time.time) * 10;
        float cos = Mathf.Cos(Time.time) * 2;
        transform.position = new Vector3(initPos.x + sin, initPos.y + cos, initPos.z);

        if(sin < 0)
        {
            sr.flipX = true;
        }
        else
        {
            sr.flipX = false;
        }
    }
}
