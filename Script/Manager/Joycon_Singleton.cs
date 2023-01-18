using System.Collections.Generic;
using System;
using UnityEngine;

public class Joycon_Singleton : MonoBehaviour
{
    public static Joycon_Singleton Instance;

    private static readonly Joycon.Button[] m_buttons =
        Enum.GetValues( typeof( Joycon.Button ) ) as Joycon.Button[];

    public List<Joycon>    m_joycons;
    public Joycon          m_joyconL;
    public Joycon          m_joyconR;
    public Joycon.Button?  m_pressedButtonL;
    public Joycon.Button?  m_pressedButtonR;

    //インスタンス取得用変数
    public Vector3 joyconGyro;
    // public List<Joycon> instance;
    // public Joycon joyconL;
    // public Joycon joyconR;

    void Awake()
    {
        // シングルトン
        if(Instance == null)
        {
            Instance = this;
            this.gameObject.name = "Joycon_Singleton";
            DontDestroyOnLoad(this.gameObject);
            Debug.Log(Instance);
            
            if(Instance == null)
            {
                Debug.Log("SIJoyconInoput Instance Error");
            }
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    void Start()
    {
        SetControllers();
    }

    void Update()
    {
        m_pressedButtonL = null;
        m_pressedButtonR = null;

        if ( m_joycons == null || m_joycons.Count <= 0 ) return;

        //SetControllers();

        //各種ボタン情報取り出し
        foreach ( var button in m_buttons )
        {
            if ( m_joyconL.GetButton( button ) )
            {
                m_pressedButtonL = button;
            }
            if ( m_joyconR.GetButton( button ) )
            {
                m_pressedButtonR = button;
            }
        }
    }

    void SetControllers()
    {
        m_joycons = JoyconManager.Instance.j;
        if (m_joycons == null || m_joycons.Count <= 0) return;
        m_joyconL = m_joycons.Find (c => c.isLeft);
        m_joyconR = m_joycons.Find (c => !c.isLeft);
    }

    public void Rumble(float low, float high, float amp, int time)
    {
        m_joyconL.SetRumble(low, high, amp, time);
        m_joyconR.SetRumble(low, high, amp, time);
    }
}

// 参考 https://tech.mof-mof.co.jp/blog/unity-joycon-introduce/
