using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveDestroy : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject, 4.1f);
    }
}
