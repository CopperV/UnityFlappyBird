using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingController : MonoBehaviour
{
    [SerializeField]
    private float minSpeed = 1f;
    [SerializeField]
    private float maxSpeed = 3.5f;

    [SerializeField]
    private int playerPointsTreshold = 100;

    [SerializeField]
    private Transform scrollingTransform;

    private float currentSpeed;

    private bool isScrolling = false;

    public Transform ScrollingTransform => scrollingTransform;

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
        if (isScrolling)
            Scroll();
    }

    private void Scroll()
    {
        Vector3 moveVector = Time.deltaTime * currentSpeed * Vector3.left;
        scrollingTransform.position += moveVector;
    }

    private void UpdateSpeed()
    {
        currentSpeed = Mathf.Lerp(minSpeed, maxSpeed, Mathf.Clamp01((float)GameManager.Instance.Points / playerPointsTreshold));
    }

    #region Listeners
    private void OnGameStartListener()
    {
        isScrolling = true;

        UpdateSpeed();
    }

    private void OnGameStopListener()
    {
        isScrolling = false;
    }

    private void OnPlayerPointsChangeListener()
    {
        UpdateSpeed();
    }
    #endregion
}
