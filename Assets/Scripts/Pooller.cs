using UnityEngine;
using UnityEngine.Pool;
using System.Collections.Generic;

public class Pooler : MonoBehaviour
{
    private static Dictionary<GameObject, ObjectPool<GameObject>> s_objectPools = new Dictionary<GameObject, ObjectPool<GameObject>>();
    private static Dictionary<GameObject, GameObject> s_cloneToPrefabMap = new Dictionary<GameObject, GameObject>();

    public static GameObject SpawnObject(GameObject objectToSpawn, Vector3 spawnPosition, Quaternion spawnRotation)
    {
        if (s_objectPools == null || s_cloneToPrefabMap == null)
        {
            Debug.LogError("Словари пулов не инициализированы!");
            return null;
        }

        if (!s_objectPools.ContainsKey(objectToSpawn))
        {
            CreatePool(objectToSpawn, spawnPosition, spawnRotation);
        }

        GameObject gameObject = s_objectPools[objectToSpawn].Get();

        if (gameObject != null)
        {

            if (!s_cloneToPrefabMap.ContainsKey(gameObject))
            {
                s_cloneToPrefabMap.Add(gameObject, objectToSpawn);
            }

            gameObject.transform.position = spawnPosition;
            gameObject.transform.rotation = spawnRotation;

            return gameObject;
        }

        return null;
    }

    public static void ReturnObjectToPool(GameObject gameObject)
    {
        if (s_cloneToPrefabMap.TryGetValue(gameObject, out GameObject prefab))
        {
            if (s_objectPools.TryGetValue(prefab, out ObjectPool<GameObject> pool))
            {
                pool.Release(gameObject);
            }
        }
        else
        {
            Debug.Log("Пулл для указанного объекта не найден");
        }
    }

    private static void CreatePool(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        ObjectPool<GameObject> pool = new ObjectPool<GameObject>(
            createFunc: () => CreateObject(prefab, position, rotation),
            actionOnRelease: OnReleaseObject,
            actionOnGet: OnGetObject
            );

        s_objectPools.Add(prefab, pool);
    }

    private static GameObject CreateObject(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        GameObject newObject = Instantiate( prefab, position, rotation);

        return newObject;
    }

    private static void OnReleaseObject(GameObject gameObject)
    {
        gameObject.SetActive( false );
        gameObject.GetComponent<IResettable>().ResetState();
    }

    private static void OnGetObject(GameObject gameObject)
    {
        gameObject.SetActive( true );
    }
}
