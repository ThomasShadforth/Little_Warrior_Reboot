using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ExpBar : MonoBehaviour
{
    [SerializeField] Image _expImage;
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] float _fillSpeed;

    bool _fillCoRunning;
    int _levelTextVal = 1;

    // Start is called before the first frame update
    void Start()
    {
        levelText.text = $"Level: {_levelTextVal}";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateEXPFillAmount(float fillPercent, int currentExp, int currentLevelCap, int nextLevelCap)
    {
        StartCoroutine(_UpdateEXPFillCo(fillPercent, currentExp, currentLevelCap, nextLevelCap));
    }

    void _UpdateLevelText()
    {
        levelText.text = $"Level: {_levelTextVal}";
    }
    

    IEnumerator _UpdateEXPFillCo(float fillPercent, int currentExp, int currentLevelCap, int nextLevelCap)
    {
        if(fillPercent > 1)
        {
            fillPercent = 1;
        }

        while(_expImage.fillAmount != fillPercent)
        {
            _expImage.fillAmount = Mathf.MoveTowards(_expImage.fillAmount, fillPercent, _fillSpeed * GamePause.deltaTime);
            yield return null;
        }

        
        if(_expImage.fillAmount >= 1)
        {
            _levelTextVal++;
            _UpdateLevelText();

            _expImage.fillAmount = 0;

            currentExp = currentExp -= currentLevelCap;
            float fillTarget = (float)currentExp / nextLevelCap;

            while(_expImage.fillAmount != fillTarget)
            {
                _expImage.fillAmount = Mathf.MoveTowards(_expImage.fillAmount, fillTarget, _fillSpeed * GamePause.deltaTime);
                yield return null;
            }

            
        }

        

    }
}
