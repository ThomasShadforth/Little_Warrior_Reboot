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

    [SerializeField] GameObject _infoPanel;
    [SerializeField] TextMeshProUGUI _nameText;
    [SerializeField] TextMeshProUGUI _descText;
    [SerializeField] TextMeshProUGUI _costText;
    [SerializeField] GameObject _buyButton;

    PlayerSkillManager _playerSkills;
    List<AbilityButton> _abilityButtonList;
    PlayerSkillManager.AbilityType _selectedAbilityType;
    int _selectedAbilityCost;

    //Hard code skills
    public void SetPlayerSkills(PlayerSkillManager playerSkills)
    {
        _playerSkills = playerSkills;

        _abilityButtonList = new List<AbilityButton>();
        _abilityButtonList.Add(new AbilityButton(transform.Find("SkillTreeElements").Find("healthMax1Btn"), playerSkills, PlayerSkillManager.AbilityType.HealthMax_1, _lockedSkillMaterial, _unlockedSkillMaterial, "Max Health Increase - 1", "Reinforces your metal plating, allowing for more survivability", 5, this));
        _abilityButtonList.Add(new AbilityButton(transform.Find("SkillTreeElements").Find("healthMax2Btn"), playerSkills, PlayerSkillManager.AbilityType.HealthMax_2, _lockedSkillMaterial, _unlockedSkillMaterial, "Max Health Increase - 2", "Reinforces your metal plating further, allowing for even more survivability", 10, this));
        _abilityButtonList.Add(new AbilityButton(transform.Find("SkillTreeElements").Find("risingPunch1Btn"), playerSkills, PlayerSkillManager.AbilityType.Rising_Punch_1, _lockedSkillMaterial, _unlockedSkillMaterial, "Rising Punch - 1", "A technique that sends your fist soaring into the air!", 10, this));
        _abilityButtonList.Add(new AbilityButton(transform.Find("SkillTreeElements").Find("risingPunch2Btn"), playerSkills, PlayerSkillManager.AbilityType.Rising_Punch_2, _lockedSkillMaterial, _unlockedSkillMaterial, "Rising Punch - 2", "Strengthens your rising punch, letting it deal more damage!", 10, this));

        _playerSkills.OnSkillUnlocked += PlayerSkillManager_OnSkillUnlocked;
        _playerSkills.OnSkillPointsChanged += _UpdateSkillPointsUI;

        _UpdateUI();
    }

    void PlayerSkillManager_OnSkillUnlocked(object sender, PlayerSkillManager.OnSkillUnlockedEventArgs e)
    {
        //Update the UI
        _UpdateUI();
    }

    void _UpdateSkillPointsUI(object sender, System.EventArgs e)
    {

    }

    void _UpdateUI()
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
    }

    public void BuyUpgrade()
    {
        if (_playerSkills.TryUnlockAbility(_selectedAbilityType, _selectedAbilityCost))
        {
            _buyButton.GetComponent<Button>().interactable = false;
        }
    }

    //Display the skill on the info panel
    public void DisplayInfoPanel(string abilityName, string abilityDescription, int abilityCost, PlayerSkillManager.AbilityType selectedAbilityType)
    {
        
        _infoPanel.SetActive(true);
        _nameText.text = abilityName;
        _descText.text = abilityDescription;
        _costText.text = $"Cost: {abilityCost}";
        _selectedAbilityCost = abilityCost;
        _selectedAbilityType = selectedAbilityType;
        _buyButton.GetComponent<Button>().interactable = (_playerSkills.CheckSkillPointRequirement(abilityCost) && !_playerSkills.IsAbilityUnlocked(selectedAbilityType));

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
        string _abilityName;
        string _abilityDescription;
        int _abilityCost;
        SkillTreeUI _ownerSkillTree;

        public AbilityButton(Transform transform, PlayerSkillManager playerSkills, PlayerSkillManager.AbilityType abilityType, Material lockedSkillMat, Material unlockedSkillMat, string abilityName, string abilityDesc, int abilityCost, SkillTreeUI ownerSkillTree)
        {
            if (transform == null) return;
           

            this._transform = transform;
            this._playerSkills = playerSkills;
            this._abilityType = abilityType;
            this._lockedSkillMaterial = lockedSkillMat;
            this._unlockedSkillMaterial = unlockedSkillMat;
            this._abilityName = abilityName;
            this._abilityDescription = abilityDesc;
            this._abilityCost = abilityCost;
            this._ownerSkillTree = ownerSkillTree;

            this._image = transform.Find("image").GetComponent<Image>();
            this._backgroundImage = transform.Find("background").GetComponent<Image>();

            /*
            transform.GetComponent<Button>().onClick.AddListener(() =>
            {
                if (!playerSkills.IsAbilityUnlocked(abilityType))
                {
                    if (!playerSkills.TryUnlockAbility(abilityType))
                    {
                        //Output UI warning that skill cannot be unlocked
                    }
                }
            });*/

            transform.GetComponent<Button>().onClick.AddListener(() => _ownerSkillTree.DisplayInfoPanel(_abilityName, _abilityDescription, _abilityCost, _abilityType));
            
        }

        public PlayerSkillManager.AbilityType GetAbilityType()
        {
            return _abilityType;
        }

        public void UpdateUI()
        {
            if (_transform == null) return;
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
                    //_transform.GetComponent<Button>().enabled = true;
                }
                else
                {
                    _image.material = _lockedSkillMaterial;
                    _backgroundImage.color = new Color(.3f, .3f, .3f);
                    //_transform.GetComponent<Button>().enabled = false;
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
