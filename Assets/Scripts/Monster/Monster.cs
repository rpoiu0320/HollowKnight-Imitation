using System;
using UnityEngine;

public class Monster : MonoBehaviour, IHittable
{
    [SerializeField] public MonsterData data;
    [SerializeField] public MonsterData.MonsterName monsterName;

    [NonSerialized] public int curHp;
    [NonSerialized] public bool alive = true;

    public void Awake()
    {
        curHp = data.Monsters[(int)monsterName].maxHp;
    }

    public void TakeHit(int damage)
    {
        curHp -= damage;
        ParticleSystem hitEffect = GameManager.Resource.Instantiate<ParticleSystem>
            ("Prefab/Effect/MonsterHitEffect", transform.position, true);
        hitEffect.Play();
        GameManager.Resource.Destroy(hitEffect.gameObject, 2f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        BumpIntoPlayer(collision);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        BumpIntoPlayer(collision);
    }

    private void BumpIntoPlayer(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player") && alive)
        {
            IHittable hittable = collision.gameObject.GetComponent<IHittable>();
            hittable?.TakeHit(0);
        }
    }
}
