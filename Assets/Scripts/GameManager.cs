using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private GameObject _testCube;
    private Vector3 _screenWorldScaleSize;
    private float xMin, xMax, yMin, yMax;

    private int borderWidthPx = 5;
    private int borderMarginPx = 15;
    private int marginSafeBuffer;

    private float _maxCubeSize;

    private Vector2 _currentScreenSize;

    void Start()
    {
        _testCube = GameObject.CreatePrimitive(PrimitiveType.Cube);

        marginSafeBuffer = borderWidthPx + borderMarginPx;

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

        _screenWorldScaleSize = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width - marginSafeBuffer, Screen.height - marginSafeBuffer, 0));

        xMax = _screenWorldScaleSize.x - (_maxCubeSize / 2);
        xMin = -_screenWorldScaleSize.x + (_maxCubeSize / 2);

        yMax = _screenWorldScaleSize.y - (_maxCubeSize / 2);
        yMin = -_screenWorldScaleSize.y + (_maxCubeSize / 2);

    }
}
