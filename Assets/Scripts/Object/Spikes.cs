using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Spikes : MonoBehaviour, IHittable
{
    [SerializeField] private int spikesDamage;

    private LayerMask playerLayer;

    public UnityEvent OnSlashHitSound;

    private void Awake()
    {
        playerLayer = LayerMask.NameToLayer("Player");
    }

    public void TakeHit(int damage)
    {
        OnSlashHitSound?.Invoke();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == playerLayer)
        {
            PlayerHitter hittable = collision.gameObject.GetComponent<PlayerHitter>();
            hittable?.HitBySpikes(spikesDamage);
        }
    }
}
