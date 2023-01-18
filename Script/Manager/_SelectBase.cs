using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class _SelectBase : MonoBehaviour
{
    public List<Image> images;
    public Color selectColor;
    public Color notSelectColor;
    public AudioClip selectSE;
    public AudioClip decideSE;
    
    AudioSource audioSource;
    int selectIndex;

    public void Init()
    {
        Debug.Log("Init_SelectBase");
        // すべて未選択状態にする
        foreach(var i in images)
        {
            i.color = notSelectColor;
        }

        // 最初のボタンだけ選択状態に
        selectIndex = 0;
        audioSource = this.GetComponent<AudioSource>();
        images[selectIndex].color = selectColor;
    }

    /// <summary>
    /// ボタン選択
    /// </summary>
    public void Select()
    {
        //Debug.Log("Select()_SelectBase");

        // 上へ
        //var rStick = Joycon_Singleton.Instance.m_joyconR.GetStick();
        //var lStick = Joycon_Singleton.Instance.m_joyconL.GetStick();

        if(Joycon_Singleton.Instance.m_joyconL.GetButtonDown(Joycon.Button.DPAD_UP) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            UpSelect();
        }

        // 下へ
        if(Joycon_Singleton.Instance.m_joyconL.GetButtonDown(Joycon.Button.DPAD_DOWN) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            DownSelect();
        }
    }

    /// <summary>
    /// 決定処理
    /// </summary>
    /// <returns>Aボタンが押されたか</returns>
    public bool Decide()
    {
        //Debug.Log("Dcide()_SelectBase");

        if(Joycon_Singleton.Instance.m_joyconR.GetButtonDown(Joycon.Button.DPAD_RIGHT) || Input.GetKeyDown(KeyCode.Return))
        {
            audioSource.PlayOneShot(decideSE);
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// 一つ下の項目へ
    /// </summary>
    void DownSelect()
    {
        //Debug.Log("ダウンキー");
        if(selectIndex == images.Count - 1) return;
        selectIndex += 1;
        images[selectIndex-1].color = notSelectColor;
        images[selectIndex].color = selectColor;
        audioSource.PlayOneShot(selectSE);
    }

    /// <summary>
    /// 一つ上の項目へ
    /// </summary>
    public void UpSelect()
    {
        //Debug.Log("アップキー");
        if(selectIndex == 0) return;
        selectIndex -= 1;
        images[selectIndex+1].color = notSelectColor;
        images[selectIndex].color = selectColor;
        audioSource.PlayOneShot(selectSE);
    }

    /// <summary>
    /// 現在選択されているIndexを取得
    /// </summary>
    /// <value>選択番号</value>
    public int GetIndex
    {
        get
        {
            return selectIndex;
        }
    }
}
