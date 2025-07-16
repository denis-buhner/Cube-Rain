using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] private Transform _container;
    [SerializeField] private Cube _prefab;

    private Queue<Cube> _pool = new Queue<Cube>();

    public Cube GetCube()
    {
        if(_pool.Count == 0)
        {
            Cube cube = Instantiate(_prefab);
            cube.transform.parent = _container;

            return cube;
        }

        Cube cubeToReturn = _pool.Dequeue();
        cubeToReturn.gameObject.SetActive(true);
        return cubeToReturn;
    }

    public void ReturnObjectToPool(Cube cube)
    {
        cube.gameObject.SetActive(false);
        _pool.Enqueue(cube);
    }
}
