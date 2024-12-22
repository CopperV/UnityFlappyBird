using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class BackgroundScroller : MonoBehaviour
{
    [SerializeField, Min(0)]
    private float scrollSpeed = 10;

    private float backgroundLength;

    private SpriteRenderer backgroundRenderer;

    private bool isScrolling = false;

    private void Awake()
    {
    }

    private void OnEnable()
    {
        GameManager.StartGameEvent += OnGameStartListener;
        GameManager.StopGameEvent += OnGameStopListener;
    }
    private void OnDisable()
    {
        GameManager.StartGameEvent -= OnGameStartListener;
        GameManager.StopGameEvent -= OnGameStopListener;
    }

    private void Start()
    {
        backgroundRenderer = GetComponent<SpriteRenderer>();

        backgroundLength = backgroundRenderer.bounds.size.x;
    }

    private void Update()
    {
        if(isScrolling)
            UpdateBackground();
    }


    private void UpdateBackground()
    {
        Vector3 moveVector = Time.deltaTime * scrollSpeed * Vector3.left;
        transform.position += moveVector;

        if (backgroundLength <= -transform.position.x)
        {
            float xOffset = backgroundLength + transform.position.x;
            transform.position = new Vector3(xOffset, 0, 0);
        }
    }

    #region Listeners
    private void OnGameStartListener()
    {
        isScrolling = true;
    }

    private void OnGameStopListener()
    {
        isScrolling = false;
    }
    #endregion
}
