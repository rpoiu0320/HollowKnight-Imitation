using System.Collections;
using UnityEngine;

//[System.Serializable]
public class KnockBack : MonoBehaviour
{
    private float knockBackTime;

    // PlayerAttack은 OverlapBaxAll을 사용하여 공격판정을 하기에 OnCollisionEnter.Contact을 사용이 불가하다
    // 맞을 때 Player의 위치 혹은 Skill의 위치 계산?
    // 밀려나는 방향을 알기 위해서인데 monster가 바라보는 방향을 기준으로 하기보다는 때리는 주체의 위치로 계산하는게 좋을듯
    // 그럼 MonsterHit 할 때 읽어와야 할 거 같음
    // 그러면 PlayerAttack 혹은 Skill의 OnTriiger에서 넉백을 시키는게 맞을지도
    // 위를 적용시키면 PlayerHit의 넉백은 그대로 쓰고 Player가 넉백시키는 대상에게만 KnockBackRoutine을 적용하도록 하면 될 것 같음
    // PlayerAttacker, Dive, Howling, ShotSoul에서만 사용하는걸로
    // Skiil들은 OntriggerEnter2d로 공격을 진행하고 PlayerAttacker는 OverlapBoxAll로 공격을 진행하기에 다른 작동방식이 필요
    // 일단은 SKill을 기준으로 작성

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

            if (knockBackDir.y > 0.3)
                target.transform.Translate(new Vector3(0, -30 * Time.deltaTime));
            else if (knockBackDir.y < -0.3)
                target.transform.Translate(new Vector3(0, 30 * Time.deltaTime));

            knockBackTime += Time.deltaTime;

            yield return null;
        }

        yield break;
    }
}
