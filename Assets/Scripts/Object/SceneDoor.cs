using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneDoor : MonoBehaviour
{
    [SerializeField] private string nextSceneName;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GameManager.Scene.ChangeScene(nextSceneName);
        }
    }
}
