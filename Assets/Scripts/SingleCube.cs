using System.Collections;
using UnityEngine;

public class SingleCube : MonoBehaviour
{
    private GameManager _gameManager;

    // Minimalny i maksymalny czas do dezaktywacji sześcianu
    float minTimeToDeactivate = 1.0f;
    float maxTimeToDeactivate = 3.0f;

    // Punkt życia sześcianu
    private int cubeHealthPoint;

    private Renderer _cubeRenderer;
    private Coroutine _deactivateCoroutine;

    private void Awake()
    {
        _cubeRenderer = gameObject.GetComponent<Renderer>();
    }

    private void OnEnable()
    {
        // Inicjalizacja punktów życia sześcianu
        cubeHealthPoint = 4;
        _deactivateCoroutine = StartCoroutine(DeactivateCube());
    }

    // Coroutine, która odpowiada za dezaktywację sześcianu
    private IEnumerator DeactivateCube()
    {
        // Oczekiwanie losowej ilości czasu
        yield return new WaitForSeconds(Random.Range(minTimeToDeactivate, maxTimeToDeactivate));

        _cubeRenderer.material.color = Color.white;

        // Zwrot sześcianu do puli
        _gameManager.PoolingSystemManager.ReturnSingleItem(this.gameObject);

        _gameManager.CurrentCubesOnScene -= 1;

        // Sprawdzenie, czy gra jest w trakcie działania i dodanie nowego sześcianu do sceny
        if (_gameManager.IsGameRunning)
            _gameManager.AddNewCubeToScene();
    }

    // Metoda odpowiadająca za natychmiastową dezaktywację sześcianu
    public void DeactivateCubeInstantly()
    {
        if (_deactivateCoroutine != null)
        {
            StopCoroutine(_deactivateCoroutine);

            _cubeRenderer.material.color = Color.white;

            // Zwrot sześcianu do puli
            _gameManager.PoolingSystemManager.ReturnSingleItem(this.gameObject);
            _gameManager.CurrentCubesOnScene -= 1;
        }
    }

    public void SetGameManager(GameManager gameManager)
    {
        _gameManager = gameManager;
    }

    // Funkcja wywoływana po kliknięciu w kostkę
    private void CubeClicked()
    {
        // Zmniejszenie liczby punktów życia kostki
        cubeHealthPoint--;

        // Wybór koloru kostki na podstawie liczby punktów życia
        switch (cubeHealthPoint)
        {
            case 0:
                {
                    // Jeśli kostka miała już uruchomiony coroutine do dezaktywacji, to go zatrzymujemy
                    if (_deactivateCoroutine != null)
                    {
                        StopCoroutine(_deactivateCoroutine);

                        _cubeRenderer.material.color = Color.white;

                        // Zwrócenie kostki do puli obiektów
                        _gameManager.PoolingSystemManager.ReturnSingleItem(this.gameObject);

                        _gameManager.CurrentCubesOnScene -= 1;

                        // Dodanie punktu i ewentualna próba dodania nowej kostki na scenie
                        _gameManager.AddPointAndTryAddNewCube();
                    }
                }
                break;
            case 1:
                _cubeRenderer.material.color = Color.blue;
                break;
            case 2:
                _cubeRenderer.material.color = Color.green;
                break;
            case 3:
                _cubeRenderer.material.color = Color.red;
                break;
            default:
                Debug.LogError("Nieprawidłowa wartość punktów życia: " + cubeHealthPoint);
                break;
        }
    }
}
