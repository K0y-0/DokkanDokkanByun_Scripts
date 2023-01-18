using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharaSelecter : _SelectBase
{
    [SerializeField]GameObject charaSelectObj;
    [SerializeField]TextMeshProUGUI charaNameText;
    [SerializeField]TextMeshProUGUI charaTipText;
    [SerializeField]TextMeshProUGUI maxScoreText;
    [SerializeField]Image pickUpImage;
    //[SerializeField]GameObject difficultStars;
    [SerializeField]List<Sprite> pickUpSourceImage;
    [SerializeField]Sprite blank;
    [SerializeField]Sprite fill;
    [SerializeField]List<string> tipText;

    [SerializeField]Image[] stars;
    
    public void InitCharaSelecter(bool active)
    {
        charaSelectObj.SetActive(active);
        Init();
        //GetDifficultyImage();
    }

    public string CharaSelectUpdate()
    {
        Select();
        SelectingInfoUpdate();

        if(Decide())
        {

            return images[GetIndex].name;
        }
        else
        {
            return null;
        }
    }

    void SelectingInfoUpdate()
    {
        charaNameText.text = images[GetIndex].name;
        pickUpImage.sprite = pickUpSourceImage[GetIndex];
        charaTipText.text = tipText[GetIndex];
        maxScoreText.text = $"最速タイム : {PlayerPrefs.GetFloat(images[GetIndex].name).ToString("F3")} 秒";
        //difficultStars = (GameObject)Resources.Load($"stars{GetIndex}");
        ChangeDifficultyImage();
    }

    // void  GetDifficultyImage()
    // {
    //     // var children = difficultStars.GetComponentsInChildren<Image>();

    //     // for (int i = 0; i < children.Length - 1; i++)
    //     // {
    //     //     stars[i] = children[i];
    //     // }

    //     var children = new Transform[difficultStars.transform.childCount];
    //     var childIndex = 0;

    //     foreach(var child in children)
    //     {
    //         stars[childIndex++] = child.gameObject.GetComponent<Image>();
    //     }
    // }

    void ChangeDifficultyImage()
    {
        for(int i = 0; i < stars.Length; i++)
        {
            stars[i].sprite = blank;
        }

        for(int i = 0; i <= GetIndex; i++)
        {
            stars[i].sprite = fill;
        }
    }
}
