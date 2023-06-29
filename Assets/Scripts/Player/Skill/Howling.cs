using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Howling : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    private PlayerMover playerMover;
    private ContactFilter2D contactFilter;
    private float attackTime = 0;
    private bool limiteMove;

    private void OnEnable()
    {
        playerMover = GameObject.FindWithTag("Player").GetComponent<PlayerMover>();
        contactFilter.SetLayerMask(LayerMask.GetMask("Monster"));
        playerMover.LimitMove(limiteMove = true);
        howlingRoutine = StartCoroutine(HowlingRoutine());
    }

    private void OnDisable()
    {
        playerMover.LimitMove(limiteMove = false);
    }

    private void Update()
    {

    }

    Coroutine howlingRoutine;

    IEnumerator HowlingRoutine()
    {
        while (attackTime < 1f)
        {
            attackTime += Time.deltaTime;

            yield return new WaitForSeconds(0.2f);
        }


        GameManager.Resource.Destory(gameObject);

        //Physics2D.OverlapBoxAll();
        yield return null;
    }

    private void OnTriggerEnter2D(Collider2D target)
    {
        if (target.gameObject.layer == LayerMask.NameToLayer("Monster"))
        {
            Debug.Log("Monster Enter");
        }
    }
}
