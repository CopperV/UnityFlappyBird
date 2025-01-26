using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> obstaclePrefabs;

    [SerializeField]
    private float maxDistance = 7f;
    [SerializeField]
    private float minDistance = 3f;

    [SerializeField]
    private int playerPointsTreshold = 100;

    [SerializeField]
    private float xPointSpawner;

    private float currentDistance;

    [SerializeField]
    private Transform scrollingTransform;

    private bool isPlaying;
    private Transform obstacleDistanceController;

    private void Start()
    {
        obstacleDistanceController = new GameObject("ObstacleDistanceController").transform;

        obstacleDistanceController.transform.position = new Vector3(xPointSpawner, 0, 0);
        obstacleDistanceController.transform.rotation = Quaternion.identity;

        obstacleDistanceController.parent = scrollingTransform;
    }

    private void OnEnable()
    {
        GameManager.StartGameEvent += OnGameStartListener;
        GameManager.StopGameEvent += OnGameStopListener;
        GameManager.PointsChangeEvent += OnPlayerPointsChangeListener;
    }
    private void OnDisable()
    {
        GameManager.StartGameEvent -= OnGameStartListener;
        GameManager.StopGameEvent -= OnGameStopListener;
        GameManager.PointsChangeEvent -= OnPlayerPointsChangeListener;
    }

    private void Update()
    {
        if (isPlaying && obstacleDistanceController.transform.position.x < xPointSpawner)
            SpawnObstacle();
    }

    private void SpawnObstacle()
    {
        GameObject randomObstacle = GetRandomObstacle();
        if (randomObstacle == null)
            return;

        GameObject spawnedObstacle = Instantiate(randomObstacle, obstacleDistanceController.transform.position, Quaternion.identity, scrollingTransform);
        Obstacle obstacle = spawnedObstacle.GetComponent<Obstacle>();

        obstacleDistanceController.position = new Vector3(obstacle.EndPoint.position.x + currentDistance, 0);
    }

    private GameObject GetRandomObstacle()
    {
        var validObstacles = obstaclePrefabs
            .Select(obj => obj.GetComponent<Obstacle>())
            .Where(obstacle => obstacle != null && obstacle.CanSpawn(GameManager.Instance.Points))
            .ToList();

        float totalWeight = 0;
        validObstacles.ForEach(obstacle => totalWeight += obstacle.Weight);

        float randomValue = Random.Range(0, totalWeight);
        float cumulativeWeight = 0f;
        foreach (var item in validObstacles)
        {
            cumulativeWeight += item.Weight;
            if (randomValue <= cumulativeWeight)
            {
                return item.gameObject;
            }
        }

        return null;
    }

    private void UpdateDistance()
    {
        currentDistance = Mathf.Lerp(maxDistance, minDistance, Mathf.Clamp01((float)GameManager.Instance.Points / playerPointsTreshold));
    }

    #region Listeners
    private void OnGameStartListener()
    {
        isPlaying = true;

        UpdateDistance();
    }

    private void OnGameStopListener()
    {
        isPlaying = false;
    }

    private void OnPlayerPointsChangeListener()
    {
        UpdateDistance();
    }
    #endregion

    private void OnDrawGizmos()
    {
        Vector3 min = new Vector3(xPointSpawner, -15);
        Vector3 max = new Vector3(xPointSpawner, 15);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(min, max);
    }

}
