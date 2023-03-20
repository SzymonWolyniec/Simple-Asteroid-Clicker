using UnityEngine;
using System.Collections.Generic;

public class CubesPoolingManager
{
    private GameManager _gameManager;

    // Rozmiary puli oraz kolejka przechowująca gotowe obiekty
    private int _poolStartSize;
    private Queue<GameObject> _singleItemsPool = new Queue<GameObject>();

    public CubesPoolingManager(int poolSize, GameManager gameManager)
    {
        _poolStartSize = poolSize;
        _gameManager = gameManager;
    }

    // Metoda przygotowująca pulę obiektów
    public void PreparePool()
    {
        // Tworzymy określoną ilość obiektów i dodajemy je do puli
        for (int i = 0; i < _poolStartSize; i++)
        {
            CreateNewSingleItem();
        }
    }

    // Metoda zwracająca pojedynczy obiekt z puli
    public GameObject GetSingleItem()
    {
        // Jeśli pula jest pusta, tworzymy nowy obiekt
        if (_singleItemsPool.Count == 0)
            CreateNewSingleItem();

        // Pobieramy obiekt z początku kolejki i zwracamy go
        GameObject singleItemFromPool = _singleItemsPool.Dequeue();
        return singleItemFromPool;
    }

    // Metoda umieszczająca obiekt z powrotem do puli
    public void ReturnSingleItem(GameObject singleItemToReturn)
    {
        // Wyłączamy obiekt i dodajemy go do końca kolejki
        singleItemToReturn.SetActive(false);
        _singleItemsPool.Enqueue(singleItemToReturn);
    }

    // Metoda tworząca nowy obiekt
    private void CreateNewSingleItem()
    {
        // Tworzymy nowy obiekt kostki
        GameObject newPoolItem = GameObject.CreatePrimitive(PrimitiveType.Cube);
        newPoolItem.SetActive(false);

        newPoolItem.tag = _gameManager.GetCubeTag();
        newPoolItem.transform.parent = _gameManager.transform;
        newPoolItem.AddComponent<SingleCube>().SetGameManager(_gameManager);

        // Dodajemy nowy obiekt do puli.
        _singleItemsPool.Enqueue(newPoolItem);
    }
}
