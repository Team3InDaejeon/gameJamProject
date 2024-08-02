using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    [Header("PlayerType")]
    public bool FireType = false;
    public bool IceType = false;
    [Range(0, 100)]public float FireHealth = 100.0f;
    [Range(0, 100)]public float IceHealth = 100.0f;

    [Header("SliderBar")]
    public Slider FireHPSlider;
    public Slider IceHPSlider;

    void Start() //Reset
    {
        FireHPSlider.value = -1.0f;
        IceHPSlider.value = -1.0f;
        FireHealth = 100.0f;
        IceHealth = 100.0f;
        FireType = true;
        IceType = true;
    }

    void Update()
    {
        if (FireHealth == 100.0f && IceHealth == 100.0f) //노말상태
        {
            FireType = true;
            IceType = true;
        }

        if (FireHealth < 100.0f && FireType == true) //Red상태
        {
            FireHPSlider.value = -1 * FireHealth/100;
            if (FireHealth > 0.0f)
            {
                FireType = true;
                IceType = false;
                FireHPSlider.value = -1 * FireHealth/100;
            }
        }
        if (IceHealth < 100.0f && IceType == true) //Blue상태
        {
            IceHPSlider.value = -1 * IceHealth/100;
            if (IceHealth > 0.0f)
            {
                IceType = true;
                FireType = false;
                IceHPSlider.value = -1 * IceHealth/100;
            }
        }
    }
}
