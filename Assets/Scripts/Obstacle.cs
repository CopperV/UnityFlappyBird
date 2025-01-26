using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField]
    private float weight = 1;

    [SerializeField, Min(0)]
    private int minPlayerPoints = 0;

    [SerializeField, Min(-1)]
    private int maxPlayerPoints = -1;

    [SerializeField]
    private Transform endPoint;

    [SerializeField]
    private float xLimitDestroy;

    public float Weight => weight;
    public int MinPlayerPoints => minPlayerPoints;
    public int MaxPlayerPoints => maxPlayerPoints;
    public Transform EndPoint => endPoint;

    private void Update()
    {
        if(transform.position.x < xLimitDestroy)
            Destroy(gameObject);
    }

    public bool CanSpawn(int playerPoints)
    {
        return playerPoints >= minPlayerPoints && (maxPlayerPoints < 0 || playerPoints <= maxPlayerPoints);
    }

    private void OnDrawGizmos()
    {
        Vector3 min = new Vector3(xLimitDestroy, -15);
        Vector3 max = new Vector3(xLimitDestroy, 15);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(min, max);
    }
}
