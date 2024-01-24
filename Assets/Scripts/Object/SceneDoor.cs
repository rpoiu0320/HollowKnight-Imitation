using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;

public class SceneDoor : MonoBehaviour
{
    [SerializeField] private string nextSceneName;

    private string curSceneName;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Scene scene = UnitySceneManager.GetActiveScene();
        curSceneName = scene.name;

        if (collision.gameObject.tag == "Player")
        {
            bool playerFlipX = collision.GetComponent<SpriteRenderer>().flipX;
            GameManager.Scene.ChangeScene(curSceneName, nextSceneName, playerFlipX);
        }
    }
}
