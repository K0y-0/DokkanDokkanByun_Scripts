using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    public GameObject parentObj;
    public List<Vector3> cityObjs;
    public List<Vector3> skyObjs;
    public List<Vector3> spaceObjs;
    [SerializeField]List<GameObject> spawnedItem;
    GameObject item;

    // Start is called before the first frame update
    void Start()
    {
        item = (GameObject)Resources.Load("Boost");
    }

    public void SpawnArea(string area)
    {
        if(area == "city") { Spawn(cityObjs); }
        if(area == "sky") { Spawn(skyObjs); }
        if(area == "space") { Spawn(spaceObjs); }
    }

    /// <summary>
    /// 指定したエリアのオブジェクトをスポーンさせる
    /// </summary>
    /// <param name="area">指定のエリア</param>
    void Spawn(List<Vector3> area)
    {
        foreach(var t in  area)
        {
            var ig = (GameObject)Instantiate(item, t, Quaternion.identity);
            ig.transform.parent = parentObj.transform;
            spawnedItem.Add(ig);
        }
    }

    /// <summary>
    /// スポーン済みのオブジェクトをアクティブにする
    /// </summary>
    void EnableObjs(bool isEnable)
    {
        foreach(var g in spawnedItem)
        {
            g.SetActive(true);
        }
    }

    // スポーン済みオブジェクトの削除
    void DeleteSpawnItems()
    {
        foreach(var g in spawnedItem)
        {
            Destroy(g.gameObject);
        }
    }

    public void ResetItemController()
    {
        //EnableObjs(true);
        DeleteSpawnItems();
    }
}
