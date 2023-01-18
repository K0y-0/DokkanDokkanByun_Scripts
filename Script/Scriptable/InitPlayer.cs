using UnityEngine;

[CreateAssetMenu]
public class InitPlayer : ScriptableObject
{
    [Header("キャラクター")]
    public string charaName;
    public int HP;
    public float superArmourTime;
    
    [Header("コントロール関連")]
    public float swingSensitivity;
    public float rotateSensitivity;
    public float accPower;
    public float speedLimit;
    public bool isSpaceControl;

    [Header("コリジョン関連")]
    public float itemPower;

    [Header("スタートダッシュ設定")]
    public float gageSpeed;
    public float min;
    public float max;
    public float just;
    public float normal;
    public float accelSEPower;

    [Header("サウンド")]
    public AudioClip dash;
    public AudioClip death;
    public AudioClip outCamera;
    public AudioClip accelSE;
}
