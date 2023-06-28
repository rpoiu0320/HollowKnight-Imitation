using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    Dictionary<string, Object> resources = new Dictionary<string, Object>();

    public T Load<T>(string path) where T : Object
    {
        string key = $"{typeof(T)}.{path}";

        if(resources.ContainsKey(key))
            return resources[key] as T;

        T resource = Resources.Load<T>(path);
        resources.Add(key, resource);
        return resource;
    }

    public T Instantiate<T>(T original, Vector3 position, Transform parent, bool pooling = false) where T : Object
    {
        if (pooling)
            return GameManager.Pool.Get(original, position, parent);
        else
            return Object.Instantiate(original, position, Quaternion.identity, parent);
    }

    public T Instantiate<T>(string path, Vector3 position, Transform parent, bool pooling = false) where T : Object
    {
        T original = Load<T>(path);
        return Instantiate<T>(original, position, parent, pooling);
    }

    public void Destory(GameObject gameObject)
    {
        if(GameManager.Pool.IsContain(gameObject))
            GameManager.Pool.Release(gameObject);
        else
            GameManager.Destroy(gameObject);
    }

    public void Destory(GameObject gameObject, float delay)
    {
        if (GameManager.Pool.IsContain(gameObject))
            StartCoroutine(DelayReleaseRoutine(gameObject, delay));
        else
            GameObject.Destroy(gameObject, delay);
    }

    IEnumerator DelayReleaseRoutine(GameObject gameObject, float delay)
    {
        yield return new WaitForSeconds(delay);
        GameManager.Pool.Release(gameObject);
    }
}