using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameMaster : MonoBehaviour
{
    //private GUIStyle style;
    enum GAME_STATE
    {
        START, CHARA_SELECT, CHARGE,INGAME, GAME_OVER, GAME_CLEAR
    }

    enum STAGE_AREA
    {
        CITY, SKY, SPACE
    }

    GAME_STATE state;
    STAGE_AREA area;
    Player player;

    // クラス参照
    [SerializeField]StartProcess startProcess;
    [SerializeField]ItemController itemController;
    [SerializeField]CharaSelecter charaSelecter;
    [SerializeField]GameObject spawnPoint;

    // UI関連
    [SerializeField]GameObject resultObj;
    [SerializeField]GameObject achiveObj;
    [SerializeField]GameObject chargeObj;
    [SerializeField]TextMeshProUGUI clearText;
    [SerializeField]TextMeshProUGUI gameOverText;
    [SerializeField]TextMeshProUGUI newScoreText;
    [SerializeField]TextMeshProUGUI achiveText;

    [SerializeField]TextMeshProUGUI distanceText;
    [SerializeField]TextMeshProUGUI timeText;
    [SerializeField]TextMeshProUGUI countText;
    [SerializeField]string[] achiveLiteral;

    // スクロール設定
    // [SerializeField]GameObject[] city;
    // [SerializeField]GameObject[] sky;
    // [SerializeField]GameObject[] space;
    // List<GameObject> generatedStage;
    // Transform firstPlayerPos;
    // int startStageIndex;
    // int currentStageIndex;
    // int preorderStageIndex;
    // const int stageLength = 245;
    
    // カウントダウンスタート変数
    float countDown = 6;
    bool isStart;

    // タイム処理変数
    float seconds;
    float minute;
    float olsSeconds;
    string activeScene;

    // オーディオ関連
    AudioSource audioSource;
    [SerializeField]AudioClip titleBGM;
    [SerializeField]AudioClip inGameBGM;

    void Awake()
    {
        activeScene = SceneManager.GetActiveScene().name;
        Application.targetFrameRate = 60;
    }

    // Start is called before the first frame update
    void Start()
    {
        audioSource = this.GetComponent<AudioSource>();
        audioSource.Play();
        //currentStageIndex = startStageIndex -1;
        //area = STAGE_AREA.CITY;
        startProcess.InitStartProcess(true);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Delete)) PlayerPrefs.DeleteAll(); // 保存さているスコアの初期化
        if(Input.GetKeyDown(KeyCode.P)) SceneManager.LoadScene(activeScene); // 強制リスタート

        //Debug.Log(state);

        switch(state)
        {
            // タイトル画面
            case GAME_STATE.START:
                if(startProcess.StartSelect())
                {
                    charaSelecter.InitCharaSelecter(true);
                    state = GAME_STATE.CHARA_SELECT;
                }
                break;

            // キャラ選択画面
            case GAME_STATE.CHARA_SELECT:
                string charaName = charaSelecter.CharaSelectUpdate();
                if(charaName != null)
                {
                    GameObject obj = (GameObject)Resources.Load(charaName);
                    GameObject playerObject = Instantiate(obj, spawnPoint.transform.position, Quaternion.identity);
                    playerObject.name = charaName;
                    player = playerObject.GetComponent<Player>();
                    charaSelecter.InitCharaSelecter(false);
                    // itemController.SpawnArea("city");
                    // itemController.SpawnArea("sky");
                    // itemController.SpawnArea("space");
                    audioSource.Stop();
                    audioSource.PlayOneShot(inGameBGM);
                    countText.gameObject.SetActive(true);
                    state = GAME_STATE.CHARGE;
                }
                break;

            case GAME_STATE.CHARGE:
                    chargeObj.SetActive(true);
                    if(player.ChargeStart(CountDown()))
                    {
                        countText.gameObject.SetActive(false);
                        chargeObj.SetActive(false);
                        state = GAME_STATE.INGAME;
                    }
                    break;

            // ゲームプレイ画面
            case GAME_STATE.INGAME:
                if(player.GetHP <= 0) state = GAME_STATE.GAME_OVER;
                if(player.IsGoal) state = GAME_STATE.GAME_CLEAR;
                //player.PlayerUpdate();
                CountTime();
                CountDistance();
                break;

            case GAME_STATE.GAME_CLEAR:
                FastestTimeUpdate();
                resultObj.SetActive(true);
                clearText.gameObject.SetActive(true);
                var accelSum = player.GetAccelSum;
                
                AchiveJudge(player.GetAccelSum);
                achiveObj.SetActive(true);

                BackTitleMenu();
                break;

            // 終了画面
            case GAME_STATE.GAME_OVER:
                resultObj.SetActive(true);
                distanceText.gameObject.SetActive(true);
                gameOverText.gameObject.SetActive(true);
                BackTitleMenu();
                break;
        }
    }

    /// <summary>
    /// カウントダウン処理
    /// </summary>
    /// <returns></returns>
    int CountDown()
    {
        countDown -= Time.deltaTime;
        int count = (int)countDown;
        countText.text = count.ToString();
        //Debug.Log(countDown);
        return count;
    }

    /// <summary>
    /// リザルトの称号
    /// </summary>
    /// <param name="sum"踏み込によるベロシティの合計値</param>
    void AchiveJudge(float sum)
    {
        if(sum > 15000)
        {
            achiveText.text = achiveLiteral[0];
        }
        else if(sum > 12000)
        {
            achiveText.text = achiveLiteral[1];
        }
        else if(sum > 8000)
        {
            achiveText.text = achiveLiteral[2];
        }
        else
        {
            achiveText.text = achiveLiteral[3];
        }
    }
    
    void FixedUpdate()
    {
        if(state == GAME_STATE.INGAME)
        {
            //player.ChargeStart();
            player.PlayerUpdate();
            //Debug.Log(player.gameObject.name);
        }    
    }
    void CountTime()
    {
        // 時間計測
        seconds += Time.deltaTime;
        if(minute == 0)
        {
            // timeText.text = "タイム : " + (((int)Mathf.Floor(seconds))).ToString("00") + " 秒";
            timeText.text = "タイム : " + seconds.ToString("F3") + " 秒";
        }
        else
        {
            timeText.text = "タイム : " + minute.ToString("00") + " 分 " + seconds.ToString ("F3") + " 秒";
        }


        if(seconds >= 60f)
        {
            minute++;
            seconds = seconds - 60;
        }
        
        // if((int)seconds != (int)olsSeconds)
        // {
        //     timeText.text = "タイム :" + minute.ToString("00") + "  分" + ((int) seconds).ToString ("00") + "  秒";
        // }

        olsSeconds = seconds;
    }

    /// <summary>
    /// 距離を計算
    /// </summary>
    void CountDistance()
    {
        var currentPos = player.transform.position.y;
        distanceText.text = "キョリ : " + (currentPos).ToString("00") + " / 1500km" ;
    }

    /// <summary>
    /// タイトルメニューに戻る
    /// </summary>
    void BackTitleMenu()
    {
        if(Joycon_Singleton.Instance.m_joyconR.GetButtonDown(Joycon.Button.DPAD_RIGHT))
        {
            Reset();
        }
    }

    void Reset()
    {
        // 生成されたプレイヤーを消す
        //Debug.Log(player);
        player.ResetPlayer();
        Destroy(player.gameObject);
        countDown = 5;
        //Debug.Log(player);

        // UI初期化
        gameOverText.gameObject.SetActive(false);
        clearText.gameObject.SetActive(false);
        distanceText.gameObject.SetActive(false);
        newScoreText.gameObject.SetActive(false);
        resultObj.SetActive(false);
        achiveObj.SetActive(false);
        //achiveText.gameObject.SetActive(false);
        timeText.text = "タイム : ";
        
        // タイム初期化
        seconds = 0;
        olsSeconds = 0;
        minute = 0;

        //itemController.ResetItemController();

        // スタートUIを表示
        startProcess.InitStartProcess(true);
        audioSource.Stop();
        audioSource.PlayOneShot(titleBGM);

        // STARTステートに移行
        state = GAME_STATE.START;
    }

    // 現在のインデックスを調べる
    // void CalcStageIndex()
    // {
    //     int playerPosIndex = (int)(player.transform.position.z / startStageIndex);

    //     if(playerPosIndex + preorderStageIndex > currentStageIndex)
    //     {
    //         UpdateStage(playerPosIndex + preorderStageIndex);
    //     }
    // }

    // 生成ステージの指定と更新
    // void UpdateStage(int toStageIndex)
    // {
    //     if(toStageIndex <= currentStageIndex) return;

    //     for(int i = currentStageIndex + 1; i <= toStageIndex; i++)
    //     {
    //         switch(area)
    //         {
    //             case STAGE_AREA.CITY:
    //                 GameObject stageObject = GenerateStage(i, city);
    //                 generatedStage.Add(stageObject);
    //                 break;
    //             case STAGE_AREA.SKY:
    //                 stageObject = GenerateStage(i, sky);
    //                 generatedStage.Add(stageObject);
    //                 break;
    //             case STAGE_AREA.SPACE:
    //                 stageObject = GenerateStage(i, space);
    //                 generatedStage.Add(stageObject);
    //                 break;
    //         }
    //     }

    //     while(generatedStage.Count > preorderStageIndex + 2) DestroyOldestStage();
    // }

    /// <summary>
    /// 記録の更新
    /// </summary>
    void FastestTimeUpdate()
    {
        if(minute > 0) return;

        if(PlayerPrefs.GetFloat(player.gameObject.name) >= seconds  || PlayerPrefs.GetFloat(player.gameObject.name) == 0)
        {
            PlayerPrefs.SetFloat(player.gameObject.name, seconds);
            PlayerPrefs.Save();
        }

        Debug.Log(PlayerPrefs.GetFloat(player.name));
    }
    
    // 指定したエリアのステージを生成
    // GameObject GenerateStage(int stageIndex, GameObject[] generateArea)
    // {
    //     int nextStageIndex = Random.Range(0, generateArea.Length);

    //     GameObject stasgeObjects = (GameObject)Instantiate(generateArea[nextStageIndex], new Vector3(0, stageIndex * stageLength, -5), Quaternion.identity);

    //     return stasgeObjects;
    // }

    // 古い生成ステージを削除
    // void DestroyOldestStage()
    // {
    //     GameObject oldStage = generatedStage[0];
    //     generatedStage.RemoveAt(0);
    //     Destroy(oldStage);
    // }

    // void OnGUI()
    // {
    //     var accR = Joycon_Singleton.Instance.m_joyconR.GetAccel().z;
    //     var accL = Joycon_Singleton.Instance.m_joyconL.GetAccel().z;
    //     GUI.Label(new Rect(10,10,100,100), accR.ToString(), style);
    // }
}
