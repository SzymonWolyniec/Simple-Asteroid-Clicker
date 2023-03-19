using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private int _minCubesOnScene = 5;
    private int _maxCubesOnScene = 10;
    private int _currentCubesOnScene = 0;
    public int CurrentCubesOnScene
    {
        get { return _currentCubesOnScene; }
        set { _currentCubesOnScene = value; }
    }

    private Vector3 _screenWorldScaleSize;
    private float _xMin, _xMax, _yMin, _yMax;

    private int _borderWidthPx = 5;
    private int _borderMarginPx = 15;
    private int _marginSafeBuffer;

    private float _maxCubeSize;

    private Vector2 _currentScreenSize;

    // Pooling system

    private CubesPoolingManager _poolingSystemManager;
    public CubesPoolingManager PoolingSystemManager => _poolingSystemManager;

    private int poolStartSize = 10;

    void Awake()
    {
        _poolingSystemManager = new CubesPoolingManager(poolStartSize, this);
        _poolingSystemManager.PreparePool();
    }

    void Start()
    {
        _marginSafeBuffer = _borderWidthPx + _borderMarginPx;

        // Wzór na przekątną sześcianu, (max szerokośc np. gdy Cube jest obrócony x = 45, y = 45);
        _maxCubeSize = 1 * Mathf.Sqrt(3);

        SetCubesPositionRange();

        for (int i = 0; i < _minCubesOnScene; i++)
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

        float randomX = Random.Range(_xMin, _xMax);
        float randomY = Random.Range(_yMin, _yMax);

        newCube.transform.position = new Vector3(randomX, randomY, 0);
        newCube.transform.rotation = Quaternion.Euler(new Vector3(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360)));

        newCube.SetActive(true);

        _currentCubesOnScene++;
    }

    private void SetCubesPositionRange()
    {
        _currentScreenSize = new Vector2(Screen.width, Screen.height);

        _screenWorldScaleSize = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width - _marginSafeBuffer, Screen.height - _marginSafeBuffer, 0));

        _xMax = _screenWorldScaleSize.x - (_maxCubeSize / 2);
        _xMin = -_screenWorldScaleSize.x + (_maxCubeSize / 2);

        _yMax = _screenWorldScaleSize.y - (_maxCubeSize / 2);
        _yMin = -_screenWorldScaleSize.y + (_maxCubeSize / 2);

    }

    public void TryAddNewCubesToScene()
    {
        if (_currentCubesOnScene < _minCubesOnScene)
        {
            for (int i = 0; i < (_minCubesOnScene - _currentCubesOnScene); i++)
            {
                AddNewCubeToScene();
            }
        }

        if (_currentCubesOnScene < _maxCubesOnScene - 1)
        {
            // 0, 1 lub 2 nowe obiekty typu cube
            int newCubesToSpawn = Random.Range(0, 3);
            for (int i = 0; i < newCubesToSpawn; i++)
            {
                AddNewCubeToScene();
            }
        }
        else if (_currentCubesOnScene < _maxCubesOnScene)
        {
            // 0 lub 1 nowe obiekty typu cube
            int newCubesToSpawn = Random.Range(0, 2);
            for (int i = 0; i < newCubesToSpawn; i++)
            {
                AddNewCubeToScene();
            }
        }
    }
}
