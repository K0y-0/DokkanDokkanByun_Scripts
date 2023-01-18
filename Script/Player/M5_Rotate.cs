using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System;
using System.IO.Ports;
using UnityEngine;
using UniRx;

public class M5_Rotate : MonoBehaviour {

    public string portName;
    public int baurate;

    SerialPort serial;
    bool isLoop = true;

    string serial_AccW;
    float accW;
    public float accPower;

    Rigidbody2D rb;

    void Start () 
    {
        this.serial = new SerialPort (portName, baurate, Parity.None, 8, StopBits.One);
        rb = this.GetComponent<Rigidbody2D>();

        try
        {
            this.serial.Open();
            Scheduler.ThreadPool.Schedule (() => ReadData ()).AddTo(this);
        } 
        catch(Exception e)
        {
            Debug.Log ("can not open serial port");
        }
    }

    void FixedUpdate()
    {
        Debug.Log(accW);

        if(accW >= 10)
        {
            var dir = this.transform.TransformDirection(Vector3.up);
            rb.AddForce(accW * dir * accPower, ForceMode2D.Impulse);
        }
    }
	
    public void ReadData()
    {
        while (this.isLoop)
        {
            serial_AccW = this.serial.ReadLine();
            //Debug.Log( serial_AccW );
            if(serial_AccW == null) return;

            accW = float.Parse(serial_AccW);
        }
    }

    void OnDestroy()
    {
        this.isLoop = false;
        this.serial.Close ();
    }
}