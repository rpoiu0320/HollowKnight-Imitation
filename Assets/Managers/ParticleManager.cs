using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    public void PlayParticle<T>(T original, Vector3 position, Transform parent) where T : Object
    {
        GameManager.Pool.Get(original, position, parent);
    }
}
