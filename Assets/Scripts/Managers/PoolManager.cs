using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UIElements;

public class PoolManager : MonoBehaviour
{
    Dictionary<string, ObjectPool<GameObject>> poolDic;
    Dictionary<string, Transform> poolContainer;
    Transform poolRoot;
    Canvas canvasRoot;

    private void Awake()
    {
        poolDic = new Dictionary<string, ObjectPool<GameObject>>();
        poolContainer = new Dictionary<string, Transform>();
        poolRoot = new GameObject("PoolRoot").transform;
        //canvasRoot = GameManager.Resource.Instantiate<Canvas>("Prefab/UI/Canvas");
    }

    public T Get<T>(T original, Vector3 position, Transform parent) where T : Object    // T is Object
    {
        if(original is GameObject)                          // 형변환(Casting) 이 가능하면 true
        {
            GameObject prefab = original as GameObject;     // original 을 GameObject로 Casting
            string key = prefab.name;

            if (!poolDic.ContainsKey(key))                  // poolDic에 없으면 새로 생성
                CreatePool(key, prefab);

            GameObject gameObject = poolDic[key].Get();     // use ObjectPool
            gameObject.transform.parent = parent;
            gameObject.transform.position = position;
            return gameObject as T;                         // T로 Casting 후 return
        }
        else if (original is Component)
        {
            Component component = original as Component;
            string key = component.gameObject.name;

            if (!poolDic.ContainsKey(key))
                CreatePool(key, component.gameObject);

            GameObject obj = poolDic[key].Get();
            obj.transform.parent = parent;
            obj.transform.position = position;
            return obj.GetComponent<T>();
        }
        else
        {
            Debug.Log("else 일 때, Get보류");
            return null;
        }
    }

    public T Get<T>(T original, Transform parent) where T : Object
    {
        return Get<T>(original, Vector3.zero, parent);
    }

    public T Get<T>(T original) where T : Object
    {
        return Get(original, Vector3.zero, null);
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
            Component component = instance as Component;
            string key = component.gameObject.name;

            if (!poolDic.ContainsKey(key))
                return false;

            poolDic[key].Release(component.gameObject);
            return true;
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
            Component component = original as Component;
            string key = component.gameObject.name;

            if (poolDic.ContainsKey(key))
                return true;
            else
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
                GameObject gameObject = Instantiate(prefab);
                gameObject.name = key;
                return gameObject;
            },
            actionOnGet: (GameObject gameObject) =>
            {
                gameObject.gameObject.SetActive(true);
                gameObject.transform.parent = null;
            },
            actionOnRelease: (GameObject gameObject) =>
            {
                gameObject.gameObject.SetActive(false);
                gameObject.transform.parent = poolContainer[key];
            },
            actionOnDestroy: (GameObject gameObject) =>
            {
                Destroy(gameObject);
            }
            );
        poolDic.Add(key, pool);
    }

    public T GetUI<T>(T original, Vector3 position) where T : Object
    {
        if (original is GameObject)
        {
            GameObject prefab = original as GameObject;
            string key = prefab.name;

            if (!poolDic.ContainsKey(key))
                CreateUIPool(key, prefab);

            GameObject gameObject = poolDic[key].Get();
            gameObject.transform.position = position;
            return gameObject as T;
        }
        else if (original is Component)
        {
            Component component = original as Component;
            string key = component.gameObject.name;

            if (!poolDic.ContainsKey(key))
                CreateUIPool(key, component.gameObject);

            GameObject gameObject = poolDic[key].Get();
            gameObject.transform.position = position;
            return gameObject.GetComponent<T>();
        }
        else
            return null;
    }

    public T GetUI<T>(T original) where T : Object
    {
        if (original is GameObject)
        {
            GameObject prefab = original as GameObject;
            string key = prefab.name;

            if (!poolDic.ContainsKey(key))
                CreateUIPool(key, prefab);

            GameObject gameObject = poolDic[key].Get();
            return gameObject as T;
        }
        else if (original is Component)
        {
            Component component = original as Component;
            string key = component.gameObject.name;

            if (!poolDic.ContainsKey(key))
                CreateUIPool(key, component.gameObject);

            GameObject gameObject = poolDic[key].Get();
            return gameObject.GetComponent<T>();
        }
        else
            return null;
    }

    private void CreateUIPool(string key, GameObject prefab)
    {
        ObjectPool<GameObject> pool = new ObjectPool<GameObject>(
            createFunc: () =>
            {
                GameObject gameObject = Instantiate(prefab);
                gameObject.name = key;
                return gameObject;
            },
            actionOnGet: (GameObject gameObject) =>
            {
                gameObject.gameObject.SetActive(true);
            },
            actionOnRelease: (GameObject gameObject) =>
            {
                gameObject.SetActive(false);
                gameObject.transform.SetParent(canvasRoot.transform, false);
            },
            actionOnDestroy: (GameObject gameObject) =>
            {
                Destroy(gameObject);
            }
            );
        poolDic.Add(key, pool);
    }
}
