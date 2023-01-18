using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boost : MonoBehaviour
{
    [SerializeField]private float power;
   void OnTriggerEnter2D(Collider2D other)
   {
        if(other.gameObject.CompareTag("Player"))
        {
            Vector3 world = transform.TransformDirection(Vector3.up);
            other.GetComponent<Rigidbody2D>().AddForce(world * power, ForceMode2D.Impulse);
        }
   }
}
