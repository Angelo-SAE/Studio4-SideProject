using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectToSpawn : MonoBehaviour
{
    [Header("Object Variables")]
    [SerializeField] private Vector2Int size;

    public Vector2Int Size => size;
}
