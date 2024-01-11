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
        if(original is GameObject)                          // ����ȯ(Casting) �� �����ϸ� true
        {
            GameObject prefab = original as GameObject;     // original �� GameObject�� Casting
            string key = prefab.name;

            if (!poolDic.ContainsKey(key))                  // poolDic�� ������ ���� ����
                CreatePool(key, prefab);

            GameObject gameObject = poolDic[key].Get();     // use ObjectPool
            gameObject.transform.parent = parent;
            gameObject.transform.position = position;
            return gameObject as T;                         // T�� Casting �� return
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
            Debug.Log("else �� ��, Get����");
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

    public bool Release<T>(T instance) where T : Object     // pool�� ��ȯ
    {
        if(instance is GameObject)
        {
            GameObject gameObject = instance as GameObject;
            string key = gameObject.name;

            if(!poolDic.ContainsKey(key))       // poolDic�� �ش��ϴ� Obj�� ������ ��ȯ�Ұ� ����� �ϹǷ� false
                return false;

            poolDic[key].Release(gameObject);   // ������ ObjPool�� Release ���� �� true
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
            Debug.Log("else�� ��, Release����");
            return false;
        }
    }

    public bool IsContain<T>(T original) where T : Object       // ObjPool�� Key���� �ִ��� Ȯ��
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
            Debug.Log("else�� ��, IsContain����");
            return false;
        }
    }
    
    private void CreatePool(string key, GameObject prefab)      // Ǯ�� ����
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
