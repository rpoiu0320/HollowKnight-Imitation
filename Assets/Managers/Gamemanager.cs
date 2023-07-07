using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    private static PoolManager poolManager;
    private static ResourceManager resourceManager;
    private static UIManager uiManager;
    private static DataManager dataManager;
    private static ParticleManager particleManager;

    public static GameManager Instance { get { return instance; } }
    public static PoolManager Pool {  get { return poolManager; } }
    public static ResourceManager Resource { get { return resourceManager; } }
    public static UIManager UI { get { return uiManager; } }
    public static DataManager Data {  get { return dataManager; } }
    public static ParticleManager Particle { get { return particleManager; } }

    private GameManager() { }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this);
        InitManagers();
    }

    private void OnDestroy()
    {
        if (instance == this)
            instance = null;
    }

    private void InitManagers()
    {
        GameObject poolObj = new GameObject();
        poolObj.name = "PoolManager";
        poolObj.transform.parent = transform;
        poolManager = poolObj.AddComponent<PoolManager>();

        GameObject resouceObj = new GameObject();
        resouceObj.name = "ResourceManager";
        resouceObj.transform.parent = transform;
        resourceManager = resouceObj.AddComponent<ResourceManager>();

        GameObject uiObj = new GameObject();
        uiObj.name = "UIManager";
        uiObj.transform.parent = transform;
        uiManager = uiObj.AddComponent<UIManager>();

        GameObject dataObj = new GameObject();
        dataObj.name = "DataManager";
        dataObj.transform.parent = transform;
        dataManager = dataObj.AddComponent<DataManager>();

        GameObject particleObj = new GameObject();
        particleObj.name = "ParticleManager";
        particleObj.transform.parent = transform;
        particleManager = particleObj.AddComponent<ParticleManager>();
    }
}
