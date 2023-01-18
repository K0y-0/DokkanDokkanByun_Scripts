using System;
using UnityEngine;
using System.IO.Ports;
using UniRx;

public class M5_Accel : MonoBehaviour
{
    SerialPort sp;
    public string portName;
    public int baurate;


    //Rigidbody2D rb;
    //public float accPower;
    float accW;
    const float maxInputLimit = 90;
    bool isLoop = true;

    const float minInputLimit = 10;

    void Start()
    {
        this.sp = new SerialPort(portName, baurate, Parity.None, 8, StopBits.One);
        //this.rb = this.GetComponent<Rigidbody2D>();

        try
        {
            this.sp.Open();
            Scheduler.ThreadPool.Schedule (() => AccW()).AddTo(this);
        } 
        catch(Exception e)
        {
            Debug.Log ("can not open serial port");
        }
    }

    /// <summary>
    /// m5stickからの値を受け渡す関数
    /// </summary>
    /// <returns>加速の値</returns>
    public float Accel()
    {
        //var dir = transform.TransformDirection(Vector3.up);
        if(accW < maxInputLimit && accW > minInputLimit)
        {
            //Debug.Log(accW);
            return accW;
        }
        else
        {
            return 1;
        }
        
        //this.rb.AddForce(accW * dir * accPower, ForceMode2D.Impulse);
    }

    /// <summary>
    /// m5stickからの生の値を得てfloat型に変更
    /// </summary>
    void AccW()
    {
        while(this.isLoop)
        {
            string serialAccW = this.sp.ReadLine();
            accW = float.Parse(serialAccW);
            //Debug.Log(accW);
        }
    }

    void OnDestroy()
    {
        this.isLoop = false;
        this.sp.Close ();
    }
}


