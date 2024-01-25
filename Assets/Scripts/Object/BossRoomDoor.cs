using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BossRoomDoor : MonoBehaviour
{
    [SerializeField] GruzMother boss;
    [SerializeField] Transform entranceDoor;
    [SerializeField] Transform exitDoor;
    [SerializeField] AudioSource audioSource;
    [SerializeField] LayerMask groundLayer;

    private bool entranceDoorIsGround = false;
    private float moveTime;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (!boss.alive)
        {
            doorOpenRoutine = StartCoroutine(DoorOpenRoutine());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!entranceDoorIsGround && collision.tag == "Player" && boss.alive)
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
        audioSource.Play();

        while(!entranceDoorIsGround)
        {
            GroundCheck();
            entranceDoor.Translate(new Vector2(0, -60 * Time.deltaTime));
            exitDoor.Translate(new Vector2(0, -60 * Time.deltaTime));
            moveTime += Time.deltaTime;

            yield return null;
        }
    }

    Coroutine doorOpenRoutine;
    IEnumerator DoorOpenRoutine()
    {
        float openTime = 0;
        audioSource.Play();

        while (moveTime > openTime)
        {
            entranceDoor.Translate(new Vector2(0, +60 * Time.deltaTime));
            exitDoor.Translate(new Vector2(0, +60 * Time.deltaTime));
            openTime += Time.deltaTime;

            yield return null;
        }
    }
}
