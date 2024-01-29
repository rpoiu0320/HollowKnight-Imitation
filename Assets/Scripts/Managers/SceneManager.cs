using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;

public class SceneManager : MonoBehaviour
{
    private string beforeSceneName;
    private bool lastPlayerFlipX;

    private void Awake()
    {
        UnitySceneManager.activeSceneChanged += OnSceneChanged;
        UnitySceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void ChangeScene(string curSceneName, string nextSceneName, bool playerFlipX = false)
    {
        beforeSceneName = curSceneName;
        lastPlayerFlipX = playerFlipX;
        changeSceneRoutine = StartCoroutine(ChangeSceneRoutine(nextSceneName));
    }

    private void OnSceneChanged(Scene currentScene, Scene nextScene)
    {
        LoadUI();
        GameManager.Pool.Init();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        string doorName = beforeSceneName + "Door";

        foreach (GameObject sceneDoor in GameObject.FindGameObjectsWithTag("Door"))
        {
            if (sceneDoor.name == doorName)
            {
                SpriteRenderer playerRen = GameObject.FindWithTag("Player").GetComponent<SpriteRenderer>();
                playerRen.transform.position = sceneDoor.transform.position;
                playerRen.flipX = lastPlayerFlipX;
            }
        }
    }

    private void LoadUI()
    {
        Scene scene = UnitySceneManager.GetActiveScene();

        if (scene.name != "TitleScene")
            GameManager.UI.LoadHpNSoul();
    }

    Coroutine changeSceneRoutine;
    IEnumerator ChangeSceneRoutine(string sceneName)
    {
        GameManager.UI.fadeInOut.FadeOut();

        yield return new WaitForSeconds(2f);

        AsyncOperation asyncOper = UnitySceneManager.LoadSceneAsync(sceneName);
        asyncOper.allowSceneActivation = true;
    }
}
