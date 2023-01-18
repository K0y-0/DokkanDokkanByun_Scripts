using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]private GameObject[] spawnPoint;
    //GameObject spawnObj;
    GameObject currentPos;
    int spawnIndex = 0;

    void Start()
    {
        //spawnObj = (GameObject)Resources.Load("SpawnObj");
        foreach(var g in spawnPoint)
        {
            g.SetActive(false);
        }
        currentPos = spawnPoint[0];
        spawnIndex += 1;
    }

    /// <summary>
    /// リスポーンポイントにプレイヤーを移動
    /// </summary>
    /// <returns>リスポーンのポジション</returns>
    public Vector3 ReSpawn()
    {
        return currentPos.transform.position;
    }

    /// <summary>
    /// スポーン地点の更新
    /// </summary>
    /// <param name="playerPosY">プレイヤーのY座標</param>
    public void SpawnPointUpdate(float playerPosY)
    {
        if(spawnPoint[spawnIndex].gameObject.transform.position.y < playerPosY)
        {
            //Debug.Log("Update SpawnPoint");
            currentPos = spawnPoint[spawnIndex];
            if(spawnIndex != spawnPoint.Length-1) { spawnIndex += 1; }
        }
    }

    /// <summary>
    /// 現在のスポーンポイントをアクティブにする
    /// </summary>
    /// <param name="isActive">表示するか非表示にするか</param>
    public void ActivePoint(bool isActive)
    {
        currentPos.SetActive(isActive);
    }

    /// <summary>
    /// スポーン地点を非表示にし、現在のスポーン地点にスタート地点を指定
    /// </summary>
    public void ResetSpawnManager()
    {
        spawnIndex = 0;
        foreach(var g in spawnPoint)
        {
            g.SetActive(false);
        }
        currentPos = spawnPoint[0];
    }

    // public void ActivePoint(Vector3 spawnPos)
    // {
    //     var g = (GameObject)Instantiate(spawnObj, spawnPos, Quaternion.identity);
    // }
}