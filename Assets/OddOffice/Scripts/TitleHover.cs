using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleHover : MonoBehaviour
{

    public float movementAmount;
    public float secondsPerCycle;
    public float degPerSecond;
    public float modifier = .1f;

    private Vector3 initialPos;
    private float roundStart;

    void Awake ()
    {
        initialPos = transform.position;
        roundStart = Time.time + Random.Range(0, Mathf.PI * 2);
    }

    void Update ()
    {
        GetComponent<MeshRenderer>().material.SetFloat("_Modifier", modifier);
        transform.position = new Vector3(initialPos.x, initialPos.y +
            (movementAmount * Mathf.Sin(Mathf.PI * 2 * ((Time.time - roundStart) / secondsPerCycle))),  initialPos.z);
        float rotationAmount = degPerSecond * Time.deltaTime;
        transform.Rotate(0, rotationAmount, rotationAmount);
    }
}
