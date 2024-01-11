using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;

public class SceneManager : MonoBehaviour
{
    private void Awake()
    {
        UnitySceneManager.activeSceneChanged += OnSceneChanged;
    }

    public void ChangeScene(string sceneName)
    {
        changeSceneRoutine = StartCoroutine(ChangeSceneRoutine(sceneName));
    }

    private void OnSceneChanged(Scene currentScene, Scene nextScene)
    {
        LoadUI();
    }

    private void LoadUI()
    {
        Scene scene = UnitySceneManager.GetActiveScene();

        if (scene.name == "GameScene1" || scene.name == "GameScene2")
            GameManager.UI.LoadHpNSoul();
    }

    Coroutine changeSceneRoutine;
    IEnumerator ChangeSceneRoutine(string sceneName)
    {
        GameManager.UI.fadeInOut.FadeOut();

        yield return new WaitForSeconds(2f);

        UnitySceneManager.LoadScene("LoadingScene");

        yield return new WaitForSeconds(2f);

        AsyncOperation asyncOper =  UnitySceneManager.LoadSceneAsync(sceneName);
        asyncOper.allowSceneActivation = true;

        yield return null;
    }
}
