using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private int minCubesOnScene = 5;
    private int maxCubesOnScene = 10;

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
        _marginSafeBuffer = _borderWidthPx + _borderMarginPx;

        // Wzór na przekątną sześcianu, (max szerokośc np. gdy Cube jest obrócony x = 45, y = 45);
        _maxCubeSize = 1 * Mathf.Sqrt(3);

        SetCubesPositionRange();

        for (int i = 0; i < minCubesOnScene; i++)
        {
            AddNewCubeToScene();
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // jeśli użytkownik kliknął myszką
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // stwórz promień z pozycji myszki

            if (Physics.Raycast(ray, out RaycastHit hit)) // wykonaj raycast
            {
                // jeśli raycast trafił w obiekt, to wywołaj funkcję Clicked na tym obiekcie
                hit.collider.gameObject.SendMessage("CubeClicked", SendMessageOptions.DontRequireReceiver);
            }
        }

        if (_currentScreenSize.x != Screen.width || _currentScreenSize.y != Screen.height)
        {
            SetCubesPositionRange();
        }
    }

    private void AddNewCubeToScene()
    {
        GameObject newCube = _poolingSystemManager.GetSingleItem();

        float randomX = Random.Range(xMin, xMax);
        float randomY = Random.Range(yMin, yMax);

        newCube.transform.position = new Vector3(randomX, randomY, 0);
        newCube.transform.rotation = Quaternion.Euler(new Vector3(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360)));

        newCube.SetActive(true);
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
