using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] Image _healthImage;
    [SerializeField] float _fillSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateHealthFillAmount(float fillPercent)
    {
        StartCoroutine(UpdateHealthFillCo(fillPercent));
    }

    IEnumerator UpdateHealthFillCo(float fillPercent)
    {
        while(_healthImage.fillAmount != fillPercent)
        {
            _healthImage.fillAmount = Mathf.MoveTowards(_healthImage.fillAmount, fillPercent, _fillSpeed * GamePause.deltaTime);
            yield return null;
        }

        
    }
}
