using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleCube : MonoBehaviour
{
    private GameManager _gameManager;

    float minTimeToDeactivate = 1.0f;
    float maxTimeToDeactivate = 3.0f;

    private Coroutine _deactivateCoroutine;

    private int cubeHealthPoint;

    private Renderer _cubeRenderer;

    private void Awake()
    {
        _cubeRenderer = gameObject.GetComponent<Renderer>();
    }

    private void OnEnable()
    {
        cubeHealthPoint = 4;
        _deactivateCoroutine = StartCoroutine(DeactivateCube());
    }

    private IEnumerator DeactivateCube()
    {
        yield return new WaitForSeconds(Random.Range(minTimeToDeactivate, maxTimeToDeactivate));

        _cubeRenderer.material.color = Color.white;
        _gameManager.PoolingSystemManager.ReturnSingleItem(this.gameObject);

        _gameManager.CurrentCubesOnScene -= 1;
        _gameManager.AddNewCubeToScene();
    }

    public void AddGameManager(GameManager gameManager)
    {
        _gameManager = gameManager;
    }

    private void CubeClicked()
    {
        cubeHealthPoint--;

        switch (cubeHealthPoint)
        {
            case 0:
                {
                    if (_deactivateCoroutine != null)
                    {
                        StopCoroutine(_deactivateCoroutine);

                        _cubeRenderer.material.color = Color.white;
                        _gameManager.PoolingSystemManager.ReturnSingleItem(this.gameObject);

                        _gameManager.CurrentCubesOnScene -= 1;
                        _gameManager.AddPoint();
                        _gameManager.TryAddNewCubesToScene();
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
