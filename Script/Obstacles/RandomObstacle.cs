using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomObstacle : MonoBehaviour
{
    [SerializeField]private List<GameObject> obstacles;
    [SerializeField]private float spawnNum;
    [SerializeField]private float zPos;
    [SerializeField]private float minX;
    [SerializeField]private float maxX;
    [SerializeField]private float minY;
    [SerializeField]private float maxY;
    // Start is called before the first frame update
    void Start()
    {
        var parentObj = GameObject.Find("Obstacles");
        for(int i=0; i<spawnNum; i++)
        {
            int randomCharaIndex = Random.Range(0, obstacles.Count);
            var tmp = obstacles[randomCharaIndex];
            Vector3 spawnPosition = new Vector3(Random.Range(minX, maxX), Random.Range(minY, maxY), zPos);
            tmp = Instantiate(tmp, spawnPosition, Quaternion.identity);
            tmp.transform.parent = parentObj.transform;
            tmp.name = obstacles[randomCharaIndex].name;
        }
    }
}
