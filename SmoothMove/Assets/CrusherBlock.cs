using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrusherBlock : MonoBehaviour
{
    [SerializeField] private float magnitude = 0.01f;
    [SerializeField] private float speed = 1f;

    private float initalPos;

    private void Start()
    {
        initalPos = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x, initalPos + Mathf.Sin(Time.time * speed) * magnitude, transform.position.z);
    }
}
