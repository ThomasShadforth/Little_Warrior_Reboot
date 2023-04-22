using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillTreeUI : MonoBehaviour
{
    [SerializeField] Material _lockedSkillMaterial;
    [SerializeField] Material _unlockedSkillMaterial;
    [SerializeField] SkillUnlockPath[] _unlockPathArray;
    [SerializeField] Sprite _pathLineSprite;
    [SerializeField] Sprite _pathLineGlowSprite;

    PlayerSkillManager _playerSkills;
    List<AbilityButton> _abilityButtonList;

    //Hard code skills
    public void SetPlayerSkills(PlayerSkillManager playerSkills)
    {
        _playerSkills = playerSkills;

        _abilityButtonList = new List<AbilityButton>();
        _abilityButtonList.Add(new AbilityButton(transform.Find("healthMax1Btn"), playerSkills, PlayerSkillManager.AbilityType.HealthMax_1, _lockedSkillMaterial, _unlockedSkillMaterial));
        _abilityButtonList.Add(new AbilityButton(transform.Find("healthMax2Btn"), playerSkills, PlayerSkillManager.AbilityType.HealthMax_2, _lockedSkillMaterial, _unlockedSkillMaterial));
        

        _playerSkills.OnSkillUnlocked += PlayerSkillManager_OnSkillUnlocked;

        UpdateUI();
    }

    void PlayerSkillManager_OnSkillUnlocked(object sender, PlayerSkillManager.OnSkillUnlockedEventArgs e)
    {
        //Update the UI
        UpdateUI();
    }

    void UpdateUI()
    {
        foreach(AbilityButton button in _abilityButtonList)
        {
            button.UpdateUI();
        }

        foreach(SkillUnlockPath unlockPath in _unlockPathArray)
        {
            foreach(Image linkImage in unlockPath.linkImageArray)
            {
                linkImage.color = new Color(.5f, .5f, .5f);
                linkImage.sprite = _pathLineSprite;
            }
        }

        foreach(SkillUnlockPath unlockPath in _unlockPathArray)
        {
            if (_playerSkills.IsAbilityUnlocked(unlockPath.abilityType) || _playerSkills.CanUnlock(unlockPath.abilityType))
            {
                foreach(Image linkImage in unlockPath.linkImageArray)
                {
                    linkImage.color = Color.white;
                    linkImage.sprite = _pathLineGlowSprite;
                }
            }
        }

        //set the image colour for the links to be dark at the start
    }

    private class AbilityButton
    {
        Transform _transform;
        Image _image;
        Image _backgroundImage;
        PlayerSkillManager _playerSkills;
        PlayerSkillManager.AbilityType _abilityType;
        Material _lockedSkillMaterial;
        Material _unlockedSkillMaterial;

        public AbilityButton(Transform transform, PlayerSkillManager playerSkills, PlayerSkillManager.AbilityType abilityType, Material lockedSkillMat, Material unlockedSkillMat)
        {
            this._transform = transform;
            this._playerSkills = playerSkills;
            this._abilityType = abilityType;
            this._lockedSkillMaterial = lockedSkillMat;
            this._unlockedSkillMaterial = unlockedSkillMat;

            this._image = transform.Find("image").GetComponent<Image>();
            this._backgroundImage = transform.Find("background").GetComponent<Image>();

            transform.GetComponent<Button>().onClick.AddListener(() =>
            {
                if (!playerSkills.IsAbilityUnlocked(abilityType))
                {
                    if (!playerSkills.TryUnlockAbility(abilityType))
                    {
                        //Output UI warning that skill cannot be unlocked
                    }
                }
            });
            
        }

        public void UpdateUI()
        {
            if (_playerSkills.IsAbilityUnlocked(_abilityType))
            {
                _image.material = null;
                _backgroundImage.material = null;
            }
            else
            {
                if (_playerSkills.CanUnlock(_abilityType))
                {
                    _image.material = _unlockedSkillMaterial;
                    _backgroundImage.color = new Color(75, 103, 125);
                    _transform.GetComponent<Button>().enabled = true;
                }
                else
                {
                    _image.material = _lockedSkillMaterial;
                    _backgroundImage.color = new Color(.3f, .3f, .3f);
                    _transform.GetComponent<Button>().enabled = false;
                }
            }
        }
    }

    [System.Serializable]
    public class SkillUnlockPath
    {
        public PlayerSkillManager.AbilityType abilityType;
        public Image[] linkImageArray;
    }

}
