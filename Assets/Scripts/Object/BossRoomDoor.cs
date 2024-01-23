using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoomDoor : MonoBehaviour
{
    [SerializeField] Monster boss;
    [SerializeField] Transform entranceDoor;
    [SerializeField] Transform exitDoor;
    [SerializeField] LayerMask groundLayer;
    private bool entranceDoorIsGround = false;

    private void Update()
    {
        if (!boss.alive)
        {
            // 열리는거
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            doorRockRoutine = StartCoroutine(DoorRockRoutine());
    }

    private void GroundCheck()
    {    // Enterance, Exit이 같은 y축을 가지고 있어 하나로만 판단
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(entranceDoor.position.x, entranceDoor.position.y - 8), Vector2.down, 0.5f, groundLayer);
        Debug.DrawRay(new Vector2(entranceDoor.position.x, entranceDoor.position.y - 8), Vector2.down * 0.5f, Color.red);

        if (hit.collider != null)
            entranceDoorIsGround = true;
        else
            entranceDoorIsGround = false;
    }

    Coroutine doorRockRoutine;
    IEnumerator DoorRockRoutine()
    {
        while(!entranceDoorIsGround)
        {
            GroundCheck();
            entranceDoor.Translate(new Vector2(0, -60 * Time.deltaTime));
            exitDoor.Translate(new Vector2(0, -60 * Time.deltaTime));

            yield return null;
        }

        yield break;
    }
}
