using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    public void LoadGameScene()
    {
        GameManager.Scene.ChangeScene("", "GameScene2");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
