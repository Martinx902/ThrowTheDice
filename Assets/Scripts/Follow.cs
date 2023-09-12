using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{

    [SerializeField]
    private Transform playerPos;

    [SerializeField, Range(0,1)]
    private float smoothTime;

    private Transform actualPos;

    private Vector3 vectorRef = Vector3.zero;

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.SmoothDamp(transform.position, GetPlayerPos().position , ref vectorRef, smoothTime);
    }

    Transform GetPlayerPos()
    {
        actualPos = playerPos;
        return actualPos;
    }
}
