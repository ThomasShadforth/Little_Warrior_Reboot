using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] Image _healthImage;
    [SerializeField] float _fillSpeed;
    [SerializeField] bool _isPlayerHealth;

    // Start is called before the first frame update
    void Start()
    {
        if (_isPlayerHealth)
        {
            FindObjectOfType<PlayerHealth>().SetHealthBar(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateHealthFillAmount(float fillPercent)
    {
        StartCoroutine(_UpdateHealthFillCo(fillPercent));
    }

    IEnumerator _UpdateHealthFillCo(float fillPercent)
    {
        while(_healthImage.fillAmount != fillPercent)
        {
            _healthImage.fillAmount = Mathf.MoveTowards(_healthImage.fillAmount, fillPercent, _fillSpeed * GamePause.deltaTime);
            yield return null;
        }

        
    }
}
