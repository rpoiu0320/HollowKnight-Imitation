using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    [SerializeField] Animator hitAnimator;
    // TODO : Data 연동 
    public PlayerData data;
    public DataManager dataManager;
    public UnityEvent OnCameraNoise;

    private void Awake()
    {
        // TODO : Data 연동 
        data = new PlayerData();
        dataManager = new DataManager();
    }
}
