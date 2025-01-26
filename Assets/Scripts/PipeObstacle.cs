using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeObstacle : Obstacle
{
    [SerializeField]
    private float minYMod = -1;

    [SerializeField]
    private float maxYMod = 1;

    [SerializeField]
    private Transform model;

    private void Awake()
    {
        float mod = Random.Range(minYMod, maxYMod);
        model.transform.position += new Vector3(0, mod);
    }

    private void OnDrawGizmos()
    {
        Vector3 pos = transform.position;

        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(pos + new Vector3(0, minYMod), 0.25f);

        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(pos + new Vector3(0, maxYMod), 0.25f);
    }
}
