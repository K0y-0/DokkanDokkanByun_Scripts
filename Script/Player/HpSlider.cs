using UnityEngine;
using UnityEngine.UI;

public class HpSlider
{
    Slider hpSlider;
    public HpSlider(Slider slider, int hp)
    {
        hpSlider = slider;
        hpSlider.maxValue = hp;
    }

    public void SetHp(int hp)
    {
        hpSlider.value = hp;
    }
}
