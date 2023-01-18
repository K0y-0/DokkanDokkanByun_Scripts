using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartProcess : _SelectBase
{
    [SerializeField]GameObject titleObject;
    
    public void InitStartProcess(bool active)
    {
        Init();
        titleObject.SetActive(active);
    }

    public bool StartSelect()
    {
        Select();

        // ゲーム終了
        if(GetIndex == 1 && Decide())
        {
            Application.Quit();
        }

        // スタートフラグを送信
        if(GetIndex == 0 && Decide())
        {
            titleObject.SetActive(false);
            return true;
        }
        else
        {
            return false;
        }
    }

    
}
