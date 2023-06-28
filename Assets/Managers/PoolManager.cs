using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using static UnityEngine.UI.Image;

public class PoolManager : MonoBehaviour
{
    Dictionary<string, ObjectPool<GameObject>> poolDic;
    Dictionary<string, Transform> poolContainer;
    Transform poolRoot;

    private void Awake()
    {
        poolDic = new Dictionary<string, ObjectPool<GameObject>>();
        poolContainer = new Dictionary<string, Transform>();
        poolRoot = new GameObject("PoolRoot").transform;
    }

    public T Get<T>(T original, Vector3 position, Transform parent) where T : Object    // T is Object
    {
        if(original is GameObject)  // 형변환(Casting) 이 가능하면 true
        {
            GameObject prefab = original as GameObject;     // original 을 GameObject로 Casting
            string key = prefab.name;

            if (!poolDic.ContainsKey(key))      // poolDic에 없으면 새로 생성
                CreatePool(key, prefab);

            GameObject obj = poolDic[key].Get();    // use ObjectPool
            obj.transform.parent = parent;
            obj.transform.position = position;
            return obj as T;                    // T로 Casting 후 return
        }
        else if (original is Component)
        {
            Debug.Log("Component일 때, Get보류");
            return null;
        }
        else
        {
            Debug.Log("else 일 때, Get보류");
            return null;
        }

    }

    public bool Release<T>(T instance) where T : Object     // pool로 반환
    {
        if(instance is GameObject)
        {
            GameObject gameObject = instance as GameObject;
            string key = gameObject.name;

            if(!poolDic.ContainsKey(key))       // poolDic에 해당하는 Obj가 없으면 반환할게 없어야 하므로 false
                return false;

            poolDic[key].Release(gameObject);   // 있으면 ObjPool의 Release 실행 후 true
            return true;
        }
        else if (instance is Component)
        {
            Debug.Log("Component일 때, release보류");
            return false;
        }
        else
        {
            Debug.Log("else일 때, Release보류");
            return false;
        }
    }

    public bool IsContain<T>(T original) where T : Object       // ObjPool에 Key값이 있는지 확인
    {
        if (original is GameObject)
        {
            GameObject prefab = original as GameObject;
            string key = prefab.name;

            if (poolDic.ContainsKey(key))
                return true;
            else
                return false;
        }
        else if (original is Component)
        {
            Debug.Log("Component일 때, IsContain보류");
            return false;
        }
        else
        {
            Debug.Log("else일 때, IsContain보류");
            return false;
        }
    }
    
    private void CreatePool(string key, GameObject prefab)      // 풀에 생성
    {
        GameObject root = new GameObject(key);
        root.gameObject.name = $"{key}Container";
        root.transform.parent = poolRoot;
        poolContainer.Add(key, root.transform);

        ObjectPool<GameObject> pool = new ObjectPool<GameObject>(
            createFunc: () =>
            {
                GameObject obj = Instantiate(prefab);
                obj.gameObject.name = key;
                return obj;
            },
            actionOnGet: (GameObject obj) =>
            {
                obj.gameObject.SetActive(true);
                obj.transform.parent = null;
            },
            actionOnRelease: (GameObject obj) =>
            {
                obj.gameObject.SetActive(false);
                obj.transform.parent = poolContainer[key];
            },
            actionOnDestroy: (GameObject obj) =>
            {
                Destroy(obj);
            }
            );
        poolDic.Add(key, pool);
    }
}
