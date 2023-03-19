using UnityEngine;
using System.Collections.Generic;

public class CubesPoolingManager
{
    private int _poolStartSize;
    private Queue<GameObject> _singleItemsPool = new Queue<GameObject>();

    public CubesPoolingManager(int poolSize)
    {
        _poolStartSize = poolSize;
    }

    public void PreparePool()
    {
        for (int i = 0; i < _poolStartSize; i++)
        {
            CreateNewSingleItem();
        }
    }

    public GameObject GetSingleItem()
    {
        if (_singleItemsPool.Count == 0)
            CreateNewSingleItem();

        GameObject singleItemFromPool = _singleItemsPool.Dequeue();

        return singleItemFromPool;
    }

    public void ReturnSingleItem(GameObject singleItemToReturn)
    {
        singleItemToReturn.SetActive(false);
        _singleItemsPool.Enqueue(singleItemToReturn);
    }

    private void CreateNewSingleItem()
    {
        GameObject newPoolItem = GameObject.CreatePrimitive(PrimitiveType.Cube);
        newPoolItem.SetActive(false);

        newPoolItem.AddComponent<SingleCube>().AddPoolManager(this);

        _singleItemsPool.Enqueue(newPoolItem);
    }
}
