using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillCooltimeUI : MonoBehaviour
{
    [SerializeField] 
    private Image CooldownFillImage;
    [SerializeField] 
    private Text CooldownText;
    SkillCooldownManager CooldownManager;
    [SerializeField]
    CharacterState SkillType;

    public void Initizlize(SkillCooldownManager NewCooldownManager)
    {
        CooldownManager = NewCooldownManager;
        CooldownManager.OnCooldownUpdateEvent += (float remainingCooldown) => UpdateUI((float)remainingCooldown);

        ResetCooldownUI();
    }

    private void UpdateUI(float remainingCooldown)
    {
        if (CooldownManager == null) 
        { 
            return; 
        }

        float cooldownPercentage = remainingCooldown / CooldownManager.CooldownDuration;
        SetCooldownUI(cooldownPercentage, remainingCooldown);
    }

    public void SetCooldownUI(float cooldownPercentage, float remainingTime)
    {
        if (CooldownFillImage != null)
        {
            CooldownFillImage.fillAmount = 1.0f - cooldownPercentage;
        }

        if (CooldownText != null)
        {
            CooldownText.text = Mathf.Ceil(remainingTime).ToString();
        }
    }

    public void ResetCooldownUI()
    {
        if (CooldownFillImage != null)
        {
            CooldownFillImage.fillAmount = 1.0f;
        }

        if (CooldownText != null)
        {
            CooldownText.text = "0";
        }
    }
}
