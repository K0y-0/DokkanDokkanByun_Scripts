using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHole : MonoBehaviour
{
    // [SerializeField] float moveSpeed = 1.0f;
    // [SerializeField] float rotateSpeed = 360.0f;//1秒で360°
    // [SerializeField] float circleRadius = 1.0f;

    [SerializeField]float inHolePower;
    
    Rigidbody2D r;
    //[SerializeField]GameObject target;

    // void OnTriggerEnter2D(Collider2D other)
    // {
    //     rb = other.gameObject.GetComponent<Rigidbody2D>();
    //     rb.velocity.Normalize();
    // }

    // private void OnTriggerStay2D(Collider2D other)
    // {
    //     rb = other.gameObject.GetComponent<Rigidbody2D>();

    //     float rad = rotateSpeed * Mathf.Deg2Rad * Time.time;//Sinの引数はラジアンなのでRad2DegではなくDeg2Radを使う
        
    //     other.transform.position = this.gameObject.transform.position + new Vector3(Mathf.Cos(rad) * circleRadius * 1.5f, Mathf.Sin(rad) * circleRadius * 0.6f);
    //     //rb.velocity = rb.velocity + new Vector2(Mathf.Cos(rad) * circleRadius * 1.5f, Mathf.Sin(rad) * circleRadius * 0.6f);

    //     if (circleRadius > 0f) circleRadius -= moveSpeed * Time.deltaTime;//半径を小さくしていく
    // }

    //private void OnTriggerStay2D(Collider2D other)
    //{
        //Rigidbody2D r = other.gameObject.GetComponent<Rigidbody2D>();

        // Vector3 vector3 = transform.position - other.gameObject.transform.position;
        // vector3.Normalize();

        // if (other.gameObject == other)
        // { 
        //     rigidbody.velocity *= 0.8f;
        //     rigidbody.AddForce(vector3 * rigidbody.mass * inHolePower.0f);
        // }
        // else
        // {
        //     rigidbody.AddForce(-vector3 * rigidbody.mass * 80.0f);
        // }

        //r.velocity = Vector2.Lerp(r.velocity, this.transform.position, 1);
    //}

    void OnTriggerEnter2D(Collider2D other)
    {
        r = other.gameObject.GetComponent<Rigidbody2D>();

        r.velocity = Vector3.zero;

    }

    void OnTriggerExit2D(Collider2D other)
    {
        //r.velocity = Vector3.zero;
        r.gravityScale = 1;
    }

    void OnTriggerStay2D(Collider2D other)
    {
        r.gravityScale = 0;

        var a = this.transform.TransformDirection(other.gameObject.transform.position);

        if(Mathf.Abs(a.x) > 0.01f)
        {
            r.velocity.Normalize();
        }

        if(Mathf.Abs(a.y) > 0.01f)
        {
            r.velocity.Normalize();
        }

        if(other.gameObject.transform.position.y > this.transform.position.y)
        {
            r.AddForce(Vector2.down * inHolePower, ForceMode2D.Force);
            // if(other.transform.TransformDirection(this.transform.position) > 0.5f)
            // {
                
            // }
        }
        else
        {
            r.AddForce(Vector2.up * inHolePower, ForceMode2D.Force);
        }
        
        if(other.gameObject.transform.position.x > this.transform.position.x)
        {
            r.AddForce(Vector2.left * inHolePower, ForceMode2D.Force);
        }
        else
        {
            r.AddForce(Vector2.right * inHolePower, ForceMode2D.Force);
        }
    }
}
