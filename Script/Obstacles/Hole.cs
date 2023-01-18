using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hole : MonoBehaviour
{
    [SerializeField]float power;

    /// <summary>
    /// 範囲内にプレイヤーが入っていたら
    /// </summary>
    /// <param name="other">プレイヤーオブジェクト</param>
    void OnTriggerStay2D(Collider2D other)
    {
        var rb = other.gameObject.GetComponent<Rigidbody2D>();

        //var dir = other.gameObject.transform.TransformDirection(Vector3.up);
        var dir = other.gameObject.transform.position - this.gameObject.transform.position;

        if(other.CompareTag("Player"))
        {
            rb.velocity *= 0.9f;
            rb.AddForce(dir * power, ForceMode2D.Force);
        }

        Debug.Log("stay");       
        
        // if(other.gameObject.CompareTag("Player"))
        // {
        //     rb.AddForce(dir * -power, ForceMode2D.Impulse);           
        // }
        // else
        // {
        //     rb.AddForce(dir * power, ForceMode2D.Force);
        // }
    }
}
