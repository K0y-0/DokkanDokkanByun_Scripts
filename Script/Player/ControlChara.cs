using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlChara
{
    float gyroSensitivity;
    float magnitudeLimit;
    
    // コンストラクタ
    public ControlChara(float gyro, float limit)
    {
        gyroSensitivity = gyro;
        magnitudeLimit = limit;
    }

    /// <summary>
    /// 空中での姿勢制御
    /// </summary>
    /// <param name="dir">プレイヤーの方向</param>
    /// <param name="rb">プレイヤーのリジッドボディ</param>
    public void AirControll(Vector3 dir, Rigidbody2D rb)
    {
        //if(rb.velocity.sqrMagnitude > magnitudeLimit * magnitudeLimit)
        //{
            var power = rb.velocity.magnitude;
            rb.velocity = power * dir; // 移動velocityに入れる
        //}
    }

    // void SwingRotateRight(float accR)
    // {
    //     // 右回転 加速度適正ver
    //     if(accR > 0)
    //     {
    //         countR += Time.deltaTime;
    //         if(countR > 0.5f)
    //         {
    //             accR = 0;
    //         }
    //         controlChara.AirControll(playerDir, rb);
    //         rb.AddForce(playerDir * accR * swingAccel, ForceMode2D.Impulse);
    //         this.transform.Rotate(this.transform.rotation.x, 
    //                                 this.transform.rotation.y, 
    //                                 -accR * rotateSensi);
    //     }
    //     else
    //     {
    //         countR = 0;
    //     }
    // }

    void SwingRotateLeft(float accL)
    {
        
    }

    /// <summary>
    /// プレイヤー回転
    /// </summary>
    /// <param name="rot">プレイヤーの回転</param>
    /// <returns>回転を反映</returns>
    public Quaternion GyroRotate(Quaternion playerRot)
    {
        Joycon_Singleton.Instance.joyconGyro = Joycon_Singleton.Instance.m_joycons[0].GetGyro();
        //Quaternion playerRot = this.transform.rotation;
        playerRot.z += -Joycon_Singleton.Instance.joyconGyro[2] * gyroSensitivity;
        return playerRot;
    }

    /// <summary>
    /// ジョイコンの向きとキャラクターの向きを同期
    /// </summary>
    /// <param name="playerRot">プレイヤーの回転</param>
    /// <returns>補正後のプレイヤーの回転</returns>
    public Quaternion InitRotation(Quaternion playerRot)
    {
        Quaternion rot = playerRot;
        rot.z = -Joycon_Singleton.Instance.m_joyconL.GetVector().z;
        return rot;
    }
}
