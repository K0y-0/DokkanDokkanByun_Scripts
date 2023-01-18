using UnityEngine;
using UnityEngine.UI;

public class GageStart
{
    public float gageSpeed { get; set; }
    public bool isAccel { get; set; }

    float minJust;
    float maxJust;
    float justAccelPower;
    float normalAccelPower;

    /// <summary>
    //  スタートダッシュ時の各種パラメータの初期化
    /// </summary>
    /// <param name="min">タイミング下値</param>
    /// <param name="max">タイミング上値</param>
    /// <param name="just">タイミング内パワー</param>
    /// <param name="normal">タイミング外パワー</param>
    /// <param name="speed">タイミングゲージのスピード</param>
    public GageStart(float min, float max, float just, float normal, float speed)
    {
        gageSpeed = speed;
        minJust = min;
        maxJust = max;
        justAccelPower = just;
        normalAccelPower = normal;
    }

    /// <summary>
    /// スタートダッシュ
    /// </summary>
    /// <param name="rb">リジッドボディ</param>
    /// <param name="dir">キャラクターの方向</param>
    /// <param name="gageSlider">スライダーオブジェクト</param>
    public bool Accel(Rigidbody2D rb, Vector3 dir, Slider gageSlider)
    {
        gageSlider.value = Mathf.PingPong(Time.time * gageSpeed, gageSlider.maxValue);
        if(Joycon_Singleton.Instance.m_joyconR.GetButtonDown(Joycon.Button.DPAD_RIGHT))
        {
            //Vector3 dir = playerPos.TransformDirection(Vector3.up);

            if(gageSlider.value >= minJust && gageSlider.value <= maxJust)
            {
                rb.velocity = dir * justAccelPower;
            }
            else
            {
                rb.velocity = dir * normalAccelPower;
            }
            //gageSlider.gameObject.SetActive(false);
            return true;
        }
        return false;
    }

    public void Reset(Slider gageSlider)
    {
        isAccel = false;
        gageSlider.gameObject.SetActive(true);
    }
}