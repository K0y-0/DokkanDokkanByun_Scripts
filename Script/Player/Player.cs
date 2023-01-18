using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
#region 変数
    public InitPlayer Settings;

    string charName;
    int hp;

    float swingAccel;
    float armourTime;
    float maxSpeed;
    float rotateSensi;
    float itemAccel;
    float m5Accel;
    float sePower;
    bool isSpace;

    AudioClip dashSE;
    AudioClip deathSE;
    AudioClip outCameraSE;
    AudioClip accelSE;

    M5_Accel m5;
    HpSlider hpGage;
    GageStart gageStart;
    ControlChara controlChara;
    FollowPlayerCamera cam;
    SpawnManager spawnManager;

    Rigidbody2D rb;
    CapsuleCollider2D spCol;
    BoxCollider2D boxCol;
    Animator anim;
    GameObject chargeEffect;
    GameObject dashEffect;
    RawImage damageImg;
    Slider hpSlider;
    Slider gageSlider;
    SpriteRenderer sp;
    AudioSource aus;
    Vector3 playerDir;

    int hashFlying = Animator.StringToHash("flying");

    bool isHit;
    bool isVisible; // カメラに映っているか
    bool isStart; // スタートダッシュしているか
    bool isGoal; // ゴールしているか

    float accelSum; // 踏み込んだ力の総和
    //float accL;
    //float accR;
    float countL;
    float countR;
    float count = 3;

    public float acclimit;
#endregion

    /// <summary>
    /// 初期化関数
    /// </summary>
    void Init()
    {
        // プレイヤーの各種情報を取得
        charName = Settings.charaName;
        hp = Settings.HP;
        armourTime = Settings.superArmourTime;

        itemAccel = Settings.itemPower;
        swingAccel = Settings.swingSensitivity;
        m5Accel = Settings.accPower;
        maxSpeed = Settings.speedLimit;
        rotateSensi = Settings.rotateSensitivity;
        sePower = Settings.accelSEPower;
        
        isSpace = Settings.isSpaceControl;
        dashSE = Settings.dash;
        outCameraSE = Settings.outCamera;
        deathSE = Settings.death;
        accelSE = Settings.accelSE;

        // 各種コンポーネントの取得
        rb = this.GetComponent<Rigidbody2D>();
        sp = this.GetComponent<SpriteRenderer>();
        spCol = this.GetComponent<CapsuleCollider2D>();
        boxCol = this.GetComponent<BoxCollider2D>();
        aus = this.GetComponent<AudioSource>();
        anim = this.GetComponent<Animator>();
        m5 = this.GetComponent<M5_Accel>();

        // 各種フラグの初期化
        isHit = false;
        isVisible = true;
        isGoal = false;
        isStart = false;

        //SpawnManager Class
        spawnManager = GameObject.Find("SpawnManger").GetComponent<SpawnManager>();

        // GageStart Class
        gageStart = new GageStart(Settings.min, Settings.max, Settings.just, Settings.normal, Settings.gageSpeed);
        //gageSlider = GameObject.Find("GageSlider").GetComponent<Slider>();

        // ControlChara Class
        controlChara = new ControlChara(swingAccel, rotateSensi);

        // FollowPlayerCam Class
        cam = GameObject.Find("Main Camera").GetComponent<FollowPlayerCamera>();
        cam.SetFollowObj = this.gameObject;
        cam.SetUp();

        // hp slider
        hpSlider = GameObject.Find("HPSlider").GetComponent<Slider>();
        hpSlider.value = hp;

        // DamageImage
        damageImg = GameObject.Find("DamageImg").GetComponent<RawImage>();

        // HpSlider Class
        hpGage = new HpSlider(hpSlider, hp);
        hpGage.SetHp(hp);

        // パーティクルをロード
        dashEffect = (GameObject)Resources.Load("dashEffect");
        chargeEffect = (GameObject)Resources.Load("ChargeEffect");

        //chargeEffect = Instantiate(chargeEffect, this.transform.position, Quaternion.identity);
    }

    void Start()
    {
        // 初期化関数の実行
        Init();
    }

    /// <summary>
    /// スタートダッシュ関数
    /// </summary>
    /// <param name="dir">キャラクターの方向</param>
    void GageAccel(Vector3 dir)
    {
        // GageStart Class
        isStart = gageStart.Accel(rb, dir, gageSlider);

        // スタートしていないときにスライダー、スポーンポイントのアクティブ切り替え
        if(isStart)
        {
            anim.SetBool(hashFlying, true);
            gageSlider.gameObject.SetActive(false);
            //spawnManager.ActivePoint(false);
        }
        else
        {
            gageSlider.gameObject.SetActive(true);
            //spawnManager.ActivePoint(true);
        }
    }

    /// <summary>
    /// プレイヤーの各動作の呼び出し
    /// </summary>
    public void PlayerUpdate()
    {
        //Debug.Log(isStart);

        // HPが０ならすべての処理を停止し、物理の力を受けない
        if(hp <= 0)
        {
            this.rb.bodyType = RigidbodyType2D.Static;
            return;
        }

        //Debug.Log(rb.velocity.magnitude);
        // 最高速度に達していたら
        if(rb.velocity.sqrMagnitude > maxSpeed * maxSpeed)
        {
            //rb.velocity = new Vector2(maxSpeed, maxSpeed) / 1.5f;
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y).normalized * maxSpeed;

        }

        JudgeAccelDir(AccelR(), AccelL());
        //ApplySwingAccel(AccelR(), AccelL());

        // キャラがどの方向を向いているのか調べる
        playerDir = this.transform.TransformDirection(Vector3.up);

        // スタートしているか
        // if(!isStart)
        // { 
        //     GageAccel(playerDir);
        //     return;
        // }

        //SwingAccel(AccelR(), AccelL());
        anim.SetBool(hashFlying, true);

        // カメラの範囲外にいるか
        // if(sp.isVisible)
        // {
        //     isVisible = true;
        // }
        // else
        // {
        //     isVisible = false;
        //     //RespawnSetting();
        //     aus.PlayOneShot(outCameraSE);
        // }

        // スポーンポイントの更新
        //spawnManager.SpawnPointUpdate(this.transform.position.y);

        // ControlChara Class
        //this.transform.rotation = controlChara.GyroRotate(this.transform.rotation);

        // m5_Accel
        M5Accelerate();

        // if(rb.velocity.sqrMagnitude > rotateSensi * rotateSensi)
        // {
        //     controlChara.AirControll(playerDir, rb);
        // }

        if(isHit)
        {
            Debug.Log("isHit True");
            float alpha = Mathf.Abs(Mathf.Sin(Time.time * 10));
            sp.color = new Color(255f, 0f, 0f, alpha);

            //damageImg.color = new Color(100f, 0f,0f, alpha);
        }
    }

    // カメラ追従処理の呼び出し
    void LateUpdate()
    {
        if(rb.velocity.sqrMagnitude < 10.0f * 10.0f) return;

        // カメラを追従
        cam.FollowCam();
    }

    /// <summary>
    /// 右のジョイコンの加速度取得
    /// </summary>
    /// <returns>右の加速度</returns>
    Vector3 AccelR()
    {
        var accR = Joycon_Singleton.Instance.m_joyconR.GetAccel();
        return accR;
    }

    /// <summary>
    /// 左のジョイコンの加速度を取得
    /// </summary>
    /// <returns>左の加速度</returns>
    Vector3 AccelL()
    {
        var accL = Joycon_Singleton.Instance.m_joyconL.GetAccel();
        return accL;
    }

    void JudgeAccelDir(Vector3 accR, Vector3 accL)
    {
        if(accL.x > accL.z && accL.x > accL.y)
        {
            ApplySwingAccelL(accL.x);
        }
        else if(accL.y > accL.x && accL.y > accL.z)
        {
            ApplySwingAccelL(accL.y);
        }
        else if(accL.z > accL.x && accL.z > accL.y)
        {
            ApplySwingAccelL(accL.z);
        }

        if(accR.x > accR.z && accR.x > accR.y)
        {
            ApplySwingAccelR(accR.x);
        }
        else if(accR.y > accR.x && accR.y > accR.z)
        {
            ApplySwingAccelR(accR.y);
        }
        else if(accR.z > accR.x && accR.z > accR.y)
        {
            ApplySwingAccelR(accR.z);
        }

        //Debug.Log($"{accR.x} {accR.y} {accR.z}");
    }

    void ApplySwingAccel(float accR, float accL)
    {
        //右回転 加速度適正ver
        if(accR > 0)
        {
            countR += Time.deltaTime;
            if(countR > 0.5f)
            {
                accR = 0;
            }
            controlChara.AirControll(playerDir, rb);
            rb.AddForce(playerDir * accR * swingAccel, ForceMode2D.Impulse);
            this.transform.Rotate(this.transform.rotation.x, 
                                    this.transform.rotation.y, 
                                    -accR * rotateSensi);
        }
        else
        {
            countR = 0;
        }

        // 左回転 加速度適正ver
        //Debug.Log(countL);
        if(accL > 0)
        {
            countL += Time.deltaTime;
            if(countL > 0.5f)
            {
                accL = 0;
            }
            controlChara.AirControll(playerDir, rb);
            rb.AddForce(playerDir * accL * swingAccel, ForceMode2D.Impulse);
            this.transform.Rotate(this.transform.rotation.x, 
                                    this.transform.rotation.y, 
                                    accL * rotateSensi);
        }
        else
        {
            countL = 0;
        }

        //Debug.Log(Joycon_Singleton.Instance.m_joyconL.GetGyro());
    }

    /// <summary>
    /// 回転の実行
    /// </summary>
    /// <param name="accL">左の加速度</param>
    void ApplySwingAccelL(float accL)
    {
        // 左回転 加速度適正ver
        //Debug.Log(countL);
        if(accL > acclimit)
        {
            countL += Time.deltaTime;
            if(countL > 0.3f)
            {
                accL = 0;
            }
            controlChara.AirControll(playerDir, rb);
            rb.AddForce(playerDir * accL * swingAccel, ForceMode2D.Impulse);
            this.transform.Rotate(this.transform.rotation.x, 
                                    this.transform.rotation.y, 
                                    accL * rotateSensi);
        }
        else
        {
            countL = 0;
        }
    }

    void ApplySwingAccelR(float accR)
    {
        Debug.Log(countR);
        if(accR > acclimit)
        {
            countR += Time.deltaTime;
            if(countR > 0.3f)
            {
                accR = 0;
            }
            controlChara.AirControll(playerDir, rb);
            rb.AddForce(playerDir * accR * swingAccel, ForceMode2D.Impulse);
            this.transform.Rotate(this.transform.rotation.x, 
                                    this.transform.rotation.y, 
                                    -accR * rotateSensi);
        }
        else
        {
            countR = 0;
        }
    }

    /// <summary>
    /// カウントダウン中にチャージする
    /// </summary>
    /// <param name="count">カウントダウン時間</param>n
    public bool ChargeStart(int countDown)
    {
        count = countDown;

        //var allPower = AccelR() + AccelL() + m5Accel * 2;
        //var scale = Map(allPower, 0f, 10000f, 0f, 20f);
        //chargeEffect.transform.localScale = new Vector2(chargeEffect.transform.localScale.x, scale);

        if(count <= 0)
        {
            rb.AddForce(60f * Vector3.up, ForceMode2D.Impulse);
            return true;
        }
        else
        {
            return false;
        }
    }

    float Map(float value, float start1, float stop1, float start2, float stop2)
    {
        return start2 + (stop2 - start2) * ((value - start1) / (stop1 - start1));
    }

    /// <summary>
    /// M5Stickによる加速の実行
    /// </summary>
    void M5Accelerate()
    {
        //Vector2 accPow = m5.Accel() * playerDir * m5Accel;
        accelSum += m5.Accel();
        rb.AddForce(m5.Accel() * playerDir * m5Accel, ForceMode2D.Force);
    }

    /// <summary>
    /// リスポーンする際の各種設定
    /// </summary>
    // void RespawnSetting()
    // {
    //     controlChara.InitRotation(this.transform.rotation);
    //     anim.SetBool(hashFlying, false);
    //     var pos = spawnManager.ReSpawn();
    //     this.transform.position = new Vector2(pos.x, pos.y + 2);
    //     rb.velocity = Vector2.zero;
    //     isStart = false;
    //     cam.transform.position = new Vector3(transform.position.x, transform.position.y + 5, -10);
    // }

    void OnTriggerEnter2D(Collider2D other)
    {
        // アイテム加速処理
        if(other.gameObject.CompareTag("Boost"))
        {
            Joycon_Singleton.Instance.Rumble(160f,320f, 1f, 200);
            other.gameObject.SetActive(false);
            GenerateParticle();
            aus.PlayOneShot(dashSE);
            var dir = this.transform.TransformDirection(Vector3.up);
            //rb.velocity = new Vector2(itemAccel, itemAccel) * dir;
            rb.AddForce(playerDir * itemAccel, ForceMode2D.Impulse);
        }

        // 障害物に当たったら
        if(other.gameObject.CompareTag("Damage"))
        {
            Joycon_Singleton.Instance.Rumble(160f,320f, 1f, 200);
            hpGage.SetHp(hp -= 1);
            isHit = true;
            spCol.enabled = false;
            //boxCol.enabled = false;
            rb.velocity = Vector3.zero;
            rb.AddForce(Vector3.down * 10, ForceMode2D.Impulse);
            StartCoroutine("Hit");
            if(hp <= 0) rb.bodyType = RigidbodyType2D.Static;
            //RespawnSetting();
            aus.PlayOneShot(deathSE);
        }

        // ゴールフラグを設定
        if(other.gameObject.CompareTag("Goal"))
        {
            Debug.Log(accelSum);
            isGoal = true;
            rb.velocity = Vector2.zero;
            rb.isKinematic = true;
        }
    }

    // 障害物に当たったと際に一定時間当たり判定慣例の処理を止める
    IEnumerator Hit()
    {
        yield return  new WaitForSeconds(armourTime);

        isHit = false;
        spCol.enabled = true;
        //boxCol.enabled = true;
        sp.color = new Color(255f, 255f, 255f, 255);
        damageImg.color = new Color(0f,0f,0f,0f);
    }

    /// <summary>
    /// タイトル遷移時のリセット関数
    /// </summary>
    public void ResetPlayer()
    {
        count = 3;
        //gageSlider.gameObject.SetActive(true);
        spawnManager.ResetSpawnManager();
        cam.ResetCam();
        anim.SetBool(hashFlying, false);
    }

    // void ChargeAccel()
    // {
    //     float power = m5Accel +  + accR;
    // }

    /// <summary>
    /// パーティクル生成
    /// </summary>
    void GenerateParticle()
    {
        //エフェクトを生成する
        GameObject effect = Instantiate(dashEffect) as GameObject;
        //エフェクトが発生する場所を決定する(敵オブジェクトの場所)
        effect.transform.position = this.transform.position;
    }



    // HP取得プロパティ
    public int GetHP
    {
        get
        {
            return hp;
        }
    }

    public float GetAccelSum
    {
        get
        {
            return accelSum;
        }
    }

    // ゴールフラグ取得プロパティ
    public bool IsGoal
    {
        get
        {
            return isGoal;
        }
    }

    // ヒットフラグ取得プロパティ
    public bool IsHit
    {
        get
        {
            return isHit;
        }
    }
}