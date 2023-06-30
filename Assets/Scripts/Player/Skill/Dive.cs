using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dive : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    private PlayerMover playerMover;
    private ContactFilter2D contactFilter;
    private Animator diveAnimator;
    private bool isGround;

    private void OnEnable()
    {
        playerMover = GameObject.FindWithTag("Player").GetComponent<PlayerMover>();
        diveAnimator = GetComponent<Animator>();
        contactFilter.SetLayerMask(LayerMask.GetMask("Monster"));
        diveRoutine = StartCoroutine(DiveRoutine());
    }

    private void OnDisable()
    {
        playerMover.LimitMove(false);
    }

    private void Update()
    {

    }

    Coroutine diveRoutine;
    IEnumerator DiveRoutine()
    {
        playerMover.LimitMove(true);
        
        yield return new WaitForSeconds(0.5f);

        while (!playerMover.IsGround())
        {
            playerMover.transform.Translate(new Vector3(0, -moveSpeed * Time.deltaTime, 0));
            
            yield return null;
        }
        Debug.Log("GroundCheck");
        diveAnimator.SetTrigger("GroundCheck");
        yield return new WaitForSeconds(0.35f);
        GameManager.Resource.Destory(gameObject);
        StopCoroutine(diveRoutine);
    }

    private void OnTriggerEnter2D(Collider2D target)
    {
        if (target.gameObject.layer == LayerMask.NameToLayer("Monster"))
        {
            Debug.Log("Monster Enter");
        }
    }
}
