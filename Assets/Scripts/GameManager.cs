using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    private DesignManager _desingManager;

    private const string CUBE_TAG = "GameCube";

    // Play Again and Win System

    private int points = 0;
    private int maxPoints = 15;

    private bool _isGameRunning = false;
    public bool IsGameRunning => _isGameRunning;


    // Cubes number

    private int _minCubesOnScene = 5;
    private int _maxCubesOnScene = 10;
    private int _currentCubesOnScene = 0;
    public int CurrentCubesOnScene
    {
        get { return _currentCubesOnScene; }
        set { _currentCubesOnScene = value; }
    }


    // Positioning system based on screen size

    private Vector2 _currentScreenSize;

    private Vector3 _screenWorldScaleSize;
    private float _xMin, _xMax, _yMin, _yMax;

    private int _borderWidthPx = 5;
    private int _borderMarginPx = 15;
    private int _marginSafeBuffer;

    private float _maxCubeSize;


    // Pooling system

    private CubesPoolingManager _poolingSystemManager;
    public CubesPoolingManager PoolingSystemManager => _poolingSystemManager;

    private int poolStartSize = 10;


    // Double click system

    private float _lastClickTime = 0f;
    private float _maxTimeBetweenClicks = 0.2f;
    private GameObject _lastClickedGameObj;


    void Awake()
    {
        UIDocument uiDoc = GetComponent<UIDocument>();

        _desingManager = new DesignManager(uiDoc, this);

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

        _isGameRunning = true;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Tworzy promień z pozycji myszy
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // Sprawdza czy promień uderzył w jakiś obiekt w scenie
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                // Sprawdza, czy użytkownik kliknął obiekt tylko raz w ciągu określonego czasu
                if ((Time.time - _lastClickTime) > _maxTimeBetweenClicks)
                {
                    _lastClickedGameObj = hit.collider.gameObject;
                }
                // Sprawdza, czy użytkownik kliknął ten sam obiekt dwa razy w ciągu określonego czasu
                else
                {
                    // Jeśli kliknięto ten sam obiekt dwa razy, wywołuje funkcję CubeClicked na obiekcie
                    if (_lastClickedGameObj == hit.collider.gameObject)
                    {
                        hit.collider.gameObject.SendMessage("CubeClicked", SendMessageOptions.DontRequireReceiver);
                        _lastClickedGameObj = null; // resetuje zapisany obiekt
                    }
                    else
                    {
                        _lastClickedGameObj = hit.collider.gameObject;
                    }
                }
            }

            _lastClickTime = Time.time; // zapisuje czas ostatniego kliknięcia
        }

        // Sprawdza czy rozmiar ekranu się zmienił i ustawia zakres pozycji klocków na nowo
        if (_currentScreenSize.x != Screen.width || _currentScreenSize.y != Screen.height)
        {
            SetCubesPositionRange();
        }
    }

    // Dodaje nowy sześcian do sceny
    public void AddNewCubeToScene()
    {
        // Pobiera nowy obiekt z puli
        GameObject newCube = _poolingSystemManager.GetSingleItem();

        // Inicjalizuje zmienne do losowania pozycji i rotacji sześcianu
        float randomX, randomY;
        Quaternion randomRot;

        // Zmienna pomocnicza określająca czy wystąpiła kolizja z innym obiektem
        bool isCollision;

        // Losuje pozycję i rotację sześcianu dopóki nie zostanie wylosowana pozycja bez kolizji z innymi obiektami
        do
        {
            isCollision = false;

            randomX = Random.Range(_xMin, _xMax);
            randomY = Random.Range(_yMin, _yMax);
            randomRot = Quaternion.Euler(new Vector3(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360)));

            // Sprawdza czy nowy sześcian nie koliduje z innymi obiektami
            Collider[] hitColliders = Physics.OverlapBox(new Vector3(randomX, randomY, 0), new Vector3(0.5f, 0.5f, 0.5f), randomRot, LayerMask.GetMask("Default"));

            foreach (Collider hitCollider in hitColliders)
            {
                if (hitCollider.gameObject.activeSelf && hitCollider.gameObject.CompareTag(CUBE_TAG))
                {
                    // Znaleziono kolizję z aktywnym sześcianem
                    isCollision = true;
                }
            }

        } while (isCollision);

        // Ustawia pozycję i rotację nowego sześcianu oraz aktywuje go
        newCube.transform.position = new Vector3(randomX, randomY, 0);
        newCube.transform.rotation = randomRot;

        newCube.SetActive(true);

        _currentCubesOnScene++;
    }

    public void TryAddNewCubesToScene()
    {
        // Sprawdzanie czy jest wystarczająca liczba sześcianów na scenie
        if (_currentCubesOnScene < _minCubesOnScene)
        {
            // Dodawanie nowych sześcianów, aż osiągną wymaganą liczbę
            for (int i = 0; i < (_minCubesOnScene - _currentCubesOnScene); i++)
            {
                AddNewCubeToScene();
            }
        }

        // Sprawdzanie, czy liczba sześcianów na scenie jest mniejsza niż maksymalna liczba sześcianów
        if (_currentCubesOnScene < _maxCubesOnScene - 1)
        {
            // Losowe dodawanie 0, 1 lub 2 nowych sześcianów
            int newCubesToSpawn = Random.Range(0, 3);
            for (int i = 0; i < newCubesToSpawn; i++)
            {
                AddNewCubeToScene();
            }
        }
        else if (_currentCubesOnScene < _maxCubesOnScene)
        {
            // Losowe dodawanie 0 lub 1 nowego sześcianu
            int newCubesToSpawn = Random.Range(0, 2);
            for (int i = 0; i < newCubesToSpawn; i++)
            {
                AddNewCubeToScene();
            }
        }
    }

    // Ustawianie zakresu pozycji sześcianów na scenie w zależności od rozmiaru ekranu
    private void SetCubesPositionRange()
    {
        _currentScreenSize = new Vector2(Screen.width, Screen.height);

        // Konwersja rozmiaru ekranu na skalę światową
        _screenWorldScaleSize = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width - _marginSafeBuffer, Screen.height - _marginSafeBuffer, 0));

        // Ustawianie pozycji minimalnej i maksymalnej w osi X na scenie
        _xMax = _screenWorldScaleSize.x - (_maxCubeSize / 2);
        _xMin = -_screenWorldScaleSize.x + (_maxCubeSize / 2);

        // Ustawianie pozycji minimalnej i maksymalnej w osi Y na scenie
        _yMax = _screenWorldScaleSize.y - (_maxCubeSize / 2);
        _yMin = -_screenWorldScaleSize.y + (_maxCubeSize / 2);

    }

    // Zwracanie tagu dla sześcianów
    public string GetCubeTag()
    {
        return CUBE_TAG;
    }

    // Funkcja dodająca punkt i próbująca dodać nowe obiekty typu cube na scenę
    public void AddPointAndTryAddNewCube()
    {
        // Zwiększ punktację o 1.
        points++;

        if (points < maxPoints)
        {
            // Aktualizuj etykietę wyniku na ekranie i spróbuj dodać nowe obiekty typu sześcian.
            _desingManager.SetScoreLabel(points);
            TryAddNewCubesToScene();
        }
        else if (points == maxPoints)
        {
            // Aktualizuj etykietę wyniku, zatrzymaj grę i dezaktywuj wszystkie aktywne obiekty typu sześcian.
            _desingManager.SetScoreLabel(points);

            _isGameRunning = false;
            DeactivateActiveCubes();
            _desingManager.OnWin();
        }
    }


    // Funkcja deaktywująca aktywne obiekty typu cube
    private void DeactivateActiveCubes()
    {
        // Przejrzyj wszystkie aktywne obiekty typu sześcian i dezaktywuj je natychmiastowo.
        foreach (Transform child in this.transform)
        {
            if (child.gameObject.activeSelf)
                child.GetComponent<SingleCube>().DeactivateCubeInstantly();
        }
    }

    // Funkcja uruchamiana po kliknięciu przycisku "Play again"
    public void OnPlayAgain()
    {
        // Zresetuj punktację i ustaw zakres pozycji dla obiektów typu sześcian.
        points = 0;
        SetCubesPositionRange();

        // Dodaj minimalną liczbę obiektów typu sześcian na scenę.
        for (int i = 0; i < _minCubesOnScene; i++)
        {
            AddNewCubeToScene();
        }

        _isGameRunning = true; // Rozpoczęcie gry
    }
}
