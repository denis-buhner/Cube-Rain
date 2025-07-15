using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(ObjectPool))]
public class Spawner : MonoBehaviour
{
    [SerializeField] private float _spawnDelay = 1f;
    [SerializeField] private int _childCount = 100;
    [SerializeField] private Cube _prefab;
    [SerializeField] private float _spawnRadius;
    [SerializeField] private ObjectPool _pooler;

    private Coroutine _spawnCoroutine;
    private Action<Cube> _deleteCubeAction;
    private Vector3 _spawnCentre => transform.position;

    private void Awake()
    {
        _pooler = GetComponent<ObjectPool>();
    }

    private void OnEnable()
    {
        _spawnCoroutine = StartCoroutine(SpawnCube());
    }

    private void OnDisable()
    {
        _spawnCoroutine = null;
    }   

    private IEnumerator SpawnCube()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(_spawnDelay);

        for (int i = 0;  i < _childCount; i++)
        {
            Vector2 randomOffset = UnityEngine.Random.insideUnitCircle * _spawnRadius;
            Vector3 spawnPosition = new Vector3(_spawnCentre.x + randomOffset.x, _spawnCentre.y, _spawnCentre.z + randomOffset.y);
            Quaternion randomRotation = UnityEngine.Random.rotation;

            Cube cube = _pooler.GetCube();
            cube.transform.position = spawnPosition;
            cube.transform.rotation = randomRotation;
            cube.OnDie += DespawnCube;

            yield return waitForSeconds;
        }
    }

    private void DespawnCube(Cube cube)
    {
        cube.OnDie -= DespawnCube;

        _pooler.ReturnObjectToPool(cube);
    }
}
