using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    [Header("PlayerType")]
    public bool FireType = false;
    public bool IceType = false;
    [Range(-100, 100)]public float PlayerHealth = 0.0f;

    [Header("SliderBar")]
    public Slider FireHPSlider;
    public Slider IceHPSlider;

    void Start() //Reset
    {
        FireHPSlider.value = 0.0f;
        IceHPSlider.value = -1.0f;
        PlayerHealth = 0.0f;
        PlayerHealth = 0.0f;
        FireType = true;
        IceType = true;
    }

    void Update()
    {
        if (PlayerHealth == 0.0f) //Normal
        {
            FireType = true;
            IceType = true;
        }

        if (PlayerHealth > 0.0f) //Red
        {   
            FireType = true;
            IceType = false;
            FireHPSlider.value = PlayerHealth/100;
            if (PlayerHealth > 0.0f)
            {
                FireHPSlider.value = PlayerHealth/100;
            }
        }
        if (PlayerHealth < 0.0f) //Blue
        {
            IceType = true;
            FireType = false;
            IceHPSlider.value = PlayerHealth/100;
            if (PlayerHealth < 0.0f)
            {
                IceHPSlider.value = -1 * (PlayerHealth/100);
            }
        }
    }
}
