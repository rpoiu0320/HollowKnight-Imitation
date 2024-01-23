using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    public void LoadGameScene()
    {
        GameManager.Scene.ChangeScene("", "GameScene1");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
