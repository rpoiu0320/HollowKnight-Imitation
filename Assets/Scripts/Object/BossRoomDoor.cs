using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoomDoor : MonoBehaviour
{
    [SerializeField] Transform entranceDoor;
    [SerializeField] Transform exitDoor;
    [SerializeField] LayerMask groundLayer;
    private bool entranceDoorIsGround = false;
    private bool exitDoorIsGround = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        doorRockRoutine = StartCoroutine(DoorRockRoutine());
    }

    private void GroundCheck()
    {
        RaycastHit2D hitEnter = Physics2D.Raycast(entranceDoor.position, Vector2.down, 4f, groundLayer);
        RaycastHit2D hit = Physics2D.Raycast(entranceDoor.position, Vector2.down, 4f, groundLayer);
        Debug.DrawRay(transform.position, Vector2.down * 4f, Color.red);

        if (hit.collider != null)
        {
            entranceDoorIsGround = true;
            exitDoorIsGround = true;
        }
        else
        {
            entranceDoorIsGround = true;
            exitDoorIsGround = true;
        }
    }

    Coroutine doorRockRoutine;
    IEnumerator DoorRockRoutine()
    {
        while(!entranceDoor && !exitDoorIsGround)
        {


            yield return null;
        }

        yield break;
    }
}
