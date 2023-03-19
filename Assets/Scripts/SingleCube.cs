using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleCube : MonoBehaviour
{
    private CubesPoolingManager _poolingSystemManager;

    float minTimeToDeactivate = 1.0f;
    float maxTimeToDeactivate = 3.0f;

    private Coroutine _deactivateCoroutine;

    private void OnEnable()
    {
        _deactivateCoroutine = StartCoroutine(DeactivateCube());
    }

    private IEnumerator DeactivateCube()
    {
        yield return new WaitForSeconds(Random.Range(minTimeToDeactivate, maxTimeToDeactivate));

        _poolingSystemManager.ReturnSingleItem(this.gameObject);
    }

    public void AddPoolManager(CubesPoolingManager poolingSystemManager)
    {
        _poolingSystemManager = poolingSystemManager;
    }

    private void CubeClicked()
    {
        if (_deactivateCoroutine != null)
        {
            StopCoroutine(_deactivateCoroutine);

            _poolingSystemManager.ReturnSingleItem(this.gameObject);
        }
    }
}
