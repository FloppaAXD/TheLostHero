using UnityEngine;
using System.Collections.Generic;

public class TrigerMoveArena : MonoBehaviour
{
    [SerializeField] private List<GameObject> Keys;
    [SerializeField] private GameObject floor, floorSpike;
    [SerializeField] private GameObject ceiling, ceilingSpike;
    [SerializeField] private GameObject floorCollider;
    [SerializeField] private float moveSpeed = 5f;

    private int keyCounter, stage;
    private GameObject player;
    private Collider2D playerCol;
    private Vector3 floorStartPos, floorSpikeStartPos;
    private Vector3 ceilingStartPos, ceilingSpikeStartPos, colliderStartPos;
    private Vector3 floorTarget, floorSpikeTarget;
    private Vector3 ceilingTarget, ceilingSpikeTarget, colliderTarget;
    private bool moving;
    private bool firstStepDone;

    private void Start()
    {
        floorStartPos = floor.transform.position;
        floorSpikeStartPos = floorSpike.transform.position;
        ceilingStartPos = ceiling.transform.position;
        ceilingSpikeStartPos = ceilingSpike.transform.position;
        colliderStartPos = floorCollider.transform.position;
    }

    private void Update()
    {

        if (player == null || playerCol == null)
        {
            player = GameObject.Find("Player(Clone)");
            playerCol = player.GetComponent<Collider2D>();
            return;
        }

        for (int i = Keys.Count - 1; i >= 0; i--)
            if (playerCol.IsTouching(Keys[i].GetComponent<Collider2D>()))
            {
                keyCounter++;
                Destroy(Keys[i]);
                Keys.RemoveAt(i);
            }

        if (!moving && keyCounter >= 2 && stage < 1)
        {
            firstStepDone = false;
            SetTargets(2f, 0f, 0f);
            stage = 1;
            moving = true;
        }
        if (!moving && keyCounter >= 4 && stage < 2)
        {
            SetTargets(34f, 32f, 32f);
            stage = 2;
            moving = true;
        }
        if (!moving && keyCounter >= 6 && stage < 3)
        {
            SetTargets(83f, 66f, 66f);
            stage = 3;
            moving = true;
        }

        if (moving) Move();
    }

    private void Move()
    {
        moving = false;
        if (MoveTowards(floor, floorTarget)) moving = true;
        if (MoveTowards(floorSpike, floorSpikeTarget)) moving = true;
        if (MoveTowards(ceiling, ceilingTarget)) moving = true;
        if (MoveTowards(ceilingSpike, ceilingSpikeTarget)) moving = true;
        if (MoveTowards(floorCollider, colliderTarget)) moving = true;

        if (!moving && stage == 1 && !firstStepDone)
        {
            firstStepDone = true;
            SetTargets(18f, 16f, 16f);
            moving = true;
        }
    }

    private void SetTargets(float floorY, float ceilingY, float colliderY)
    {
        floorTarget = floorStartPos + Vector3.up * floorY;
        floorSpikeTarget = floorSpikeStartPos + Vector3.up * floorY;
        ceilingTarget = ceilingStartPos + Vector3.up * ceilingY;
        ceilingSpikeTarget = ceilingSpikeStartPos + Vector3.up * ceilingY;
        colliderTarget = colliderStartPos + Vector3.up * colliderY;
    }

    private bool MoveTowards(GameObject obj, Vector3 target)
    {
        if (obj == null) return false;
        obj.transform.position = Vector3.MoveTowards(obj.transform.position, target, moveSpeed * Time.deltaTime);
        return obj.transform.position != target;
    }

    public int GetKeyCount() => keyCounter;
    public bool AreAllKeysCollected() => Keys.Count == 0;
}