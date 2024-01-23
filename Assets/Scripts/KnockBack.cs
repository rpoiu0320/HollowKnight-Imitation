using System.Collections;
using UnityEngine;

//[System.Serializable]
public class KnockBack : MonoBehaviour
{
    private float knockBackTime;

    public void OnKnockBackRoutine(Collider2D target)
    {
        if (target.tag == "Monster")
            knockBackRoutine = StartCoroutine(KnockBackRoutine(target));
    }

    Coroutine knockBackRoutine;
    IEnumerator KnockBackRoutine(Collider2D target)
    {
        knockBackTime = 0;
        Vector2 knockBackDir = (transform.position - target.transform.position).normalized;

        while (knockBackTime < 0.1f)
        {
            if (knockBackDir.x < 0)
                target.transform.Translate(new Vector3(30 * Time.deltaTime, 0));
            else if (knockBackDir.x > 0)
                target.transform.Translate(new Vector3(-30 * Time.deltaTime, 0));

            //if (knockBackDir.y > 0.3)
            //    target.transform.Translate(new Vector3(0, -30 * Time.deltaTime));
            //else if (knockBackDir.y < -0.3)
            //    target.transform.Translate(new Vector3(0, 30 * Time.deltaTime));

            knockBackTime += Time.deltaTime;

            yield return null;
        }

        yield break;
    }
}
