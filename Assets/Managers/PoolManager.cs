using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PoolManager : MonoBehaviour
{
    Dictionary<string, ObjectPool<GameObject>> poolDic;
    Dictionary<string, Transform> poolContainer;
    Transform poolRoot;
    Canvas canvasRoot;

    //private void Awake()
    //{

    //    poolDic = new Dictionary<string, ObjectPool<GameObject>>();
    //    poolContainer = new Dictionary<string, Transform>();
    //    poolRoot = new GameObject("PoolRoot").transform;
    //    canvasRoot = Gamemanager.Resources.Instantiate <Canvas>("UI/Canvas");

    //    public T Get<T>(T original, Vector2 position, Quaternion rotation, Transform parent) where T : Object
    //    {
    //        if(original is GameObject)
    //        {
    //            GameObject prefab = original as GameObject;
    //            string key = prefab.name;

    //            if(!poolDic.ContainsKey(key))
    //            {

    //            }
    //        }
    //    }
    //}
}
