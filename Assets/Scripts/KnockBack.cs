using System.Collections;
using UnityEngine;

//[System.Serializable]
public class KnockBack : MonoBehaviour
{
    private float knockBackTime;

    // PlayerAttack�� OverlapBaxAll�� ����Ͽ� ���������� �ϱ⿡ OnCollisionEnter.Contact�� ����� �Ұ��ϴ�
    // ���� �� Player�� ��ġ Ȥ�� Skill�� ��ġ ���?
    // �з����� ������ �˱� ���ؼ��ε� monster�� �ٶ󺸴� ������ �������� �ϱ⺸�ٴ� ������ ��ü�� ��ġ�� ����ϴ°� ������
    // �׷� MonsterHit �� �� �о�;� �� �� ����
    // �׷��� PlayerAttack Ȥ�� Skill�� OnTriiger���� �˹��� ��Ű�°� ��������
    // ���� �����Ű�� PlayerHit�� �˹��� �״�� ���� Player�� �˹��Ű�� ��󿡰Ը� KnockBackRoutine�� �����ϵ��� �ϸ� �� �� ����
    // PlayerAttacker, Dive, Howling, ShotSoul������ ����ϴ°ɷ�
    // Skiil���� OntriggerEnter2d�� ������ �����ϰ� PlayerAttacker�� OverlapBoxAll�� ������ �����ϱ⿡ �ٸ� �۵������ �ʿ�
    // �ϴ��� SKill�� �������� �ۼ�

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
