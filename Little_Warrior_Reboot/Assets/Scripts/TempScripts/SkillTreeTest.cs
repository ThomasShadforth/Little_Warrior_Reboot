using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTreeTest : MonoBehaviour
{
    // Start is called before the first frame update
    PlayerSkillManager _playerSkills;

    void Start()
    {
        _playerSkills = new PlayerSkillManager();
        FindObjectOfType<SkillTreeUI>().SetPlayerSkills(_playerSkills);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
