using UnityEngine;

public class FollowPlayerCamera : MonoBehaviour
{
    [SerializeField]float camOffset;
    GameObject followObj;
    Vector3 initCamTransform;
    Vector3 offset;
    SpriteRenderer sr;
    bool isVisible;

    void Start()
    {
        initCamTransform = this.transform.position;
    }

    /// <summary>
    /// リスタート用関数
    /// </summary>
    public void ResetCam()
    {
        this.transform.position = initCamTransform;
        followObj = null;
    }

    /// <summary>
    /// どのくらいの距離で追従するかを計算
    /// </summary>
    public void SetUp()
    {
        offset = this.transform.position - new Vector3(
                followObj.transform.position.x, 
                followObj.transform.position.y - camOffset, 
                followObj.transform.position.z
            );
    }

    /// <summary>
    /// カメラを追従させる
    /// </summary>
    public void FollowCam()
    {
        this.transform.position = Vector3.Lerp(transform.position,
                            followObj.transform.position + offset,
                            2.0f * Time.deltaTime);

        this.transform.position = new Vector3(initCamTransform.x, this.transform.position.y, this.transform.position.z);
    }

    public GameObject SetFollowObj
    {
        set
        {
            followObj = value;
            sr = followObj.GetComponent<SpriteRenderer>();
        }
    }
}
