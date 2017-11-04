using UnityEngine;
using System.Collections.Generic;

public class StackPool : MonoBehaviour
{
    public GameObject pooledObject;
    public int preloadAmount = 20;
    public bool allowGrowth = true;
    public bool startActive = false;

    private Stack<GameObject> pooledObjects;

    void Awake ()
    {
        pooledObjects = new Stack<GameObject>();
        for (int i = 0; i < preloadAmount; i++)
        {
            GameObject obj = Instantiate(pooledObject) as GameObject;
            obj.transform.SetParent(transform);
            obj.SetActive(startActive);
            pooledObjects.Push(obj);
        }
    }

    public GameObject Pop ()
    {
        if (pooledObjects.Count > 0)
        {
            GameObject obj = pooledObjects.Pop();
            obj.SetActive(true);
            return obj;
        }

        if (allowGrowth)
        {
            GameObject obj = Instantiate(pooledObject) as GameObject;
            obj.transform.SetParent(transform);
            return obj;
        }

        return null;
    }

    public void Push (GameObject obj)
    {
        obj.SetActive(false);
        pooledObjects.Push(obj);
    }
}
