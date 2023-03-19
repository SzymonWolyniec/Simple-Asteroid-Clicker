using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private int maxCubesOnScene = 10;
    private int minCubesOnScene = 5;

    private int maxCubeLifetimeSec = 3;
    private int minCubeLifetimeSec = 1;

    private GameObject _testCube;
    private Vector3 _screenWorldScaleSize;
    private float xMin, xMax, yMin, yMax;

    private int _borderWidthPx = 5;
    private int _borderMarginPx = 15;
    private int _marginSafeBuffer;

    private float _maxCubeSize;

    private Vector2 _currentScreenSize;

    // Pooling system

    private CubesPoolingManager _poolingSystemManager;
    private int poolSize = 10;

    void Awake()
    {
        _poolingSystemManager = new CubesPoolingManager(poolSize);
        _poolingSystemManager.PreparePool();
    }

    void Start()
    {
        _testCube = _poolingSystemManager.GetSingleItem();

        _marginSafeBuffer = _borderWidthPx + _borderMarginPx;

        // Wzór na przekątną sześcianu, (max szerokośc np. gdy Cube jest obrócony x = 45, y = 45);
        _maxCubeSize = _testCube.transform.localScale.x * Mathf.Sqrt(3);

        SetCubesPositionRange();

        InvokeRepeating("ChangeCubePosAndRot", 0f, 1f);
    }

    private void ChangeCubePosAndRot()
    {
        float randomX = (xMax - xMin) * Random.value + xMin;
        float randomY = (yMax - yMin) * Random.value + yMin;

        _testCube.transform.position = new Vector3(randomX, randomY, 0);
        _testCube.transform.rotation = Quaternion.Euler(new Vector3(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360)));

    }

    void FixedUpdate()
    {
        if (_currentScreenSize.x != Screen.width || _currentScreenSize.y != Screen.height)
        {
            SetCubesPositionRange();
        }
    }

    private void SetCubesPositionRange()
    {
        _currentScreenSize = new Vector2(Screen.width, Screen.height);

        _screenWorldScaleSize = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width - _marginSafeBuffer, Screen.height - _marginSafeBuffer, 0));

        xMax = _screenWorldScaleSize.x - (_maxCubeSize / 2);
        xMin = -_screenWorldScaleSize.x + (_maxCubeSize / 2);

        yMax = _screenWorldScaleSize.y - (_maxCubeSize / 2);
        yMin = -_screenWorldScaleSize.y + (_maxCubeSize / 2);

    }
}
