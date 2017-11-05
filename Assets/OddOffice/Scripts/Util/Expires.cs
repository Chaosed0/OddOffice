using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Expires : MonoBehaviour
{
    public float expiryTime;
    public StackPool pool;

    void Start()
    {
        StartCoroutine(ExpireCoroutine());
    }

    IEnumerator ExpireCoroutine()
    {
        yield return new WaitForSeconds(expiryTime);
        if (pool)
        {
            pool.Push(gameObject);
            if (gameObject.GetComponent<CoffeeProjectile>())
            {
                gameObject.GetComponent<BoxCollider>().enabled = false;
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
