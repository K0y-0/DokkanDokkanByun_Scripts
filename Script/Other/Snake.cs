using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField]float x;
    [SerializeField]float y;
    [SerializeField]float xSpeed;
    [SerializeField]float ySpeed;
    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 dir = transform.TransformDirection(Vector3.up);
        rb.velocity = new Vector2(Mathf.Sin(Time.time * x) * xSpeed, Mathf.Sin(y * Time.time) * ySpeed);
    }
}
