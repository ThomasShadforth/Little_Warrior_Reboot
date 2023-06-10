using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEnd : MonoBehaviour
{
    [SerializeField] string _nextLevel;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //For now, load levels via this script.
            //A dedicated script will be created at a later point in time
            StartCoroutine(_LoadNextLevelCo());
        }
    }

    IEnumerator _LoadNextLevelCo()
    {
        if(UIScreenFade.instance != null)
        {
            UIScreenFade.instance.FadeToBlack();
        }

        yield return new WaitForSeconds(1.5f);

        SceneManager.LoadSceneAsync(_nextLevel);
    }
}
