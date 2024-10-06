using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PROgenSeecd : MonoBehaviour
{
    [Header("Scriptable Objects")]
    [SerializeField] private ObjectArrayObject objects;

    [Header("Generation Values")]
    [SerializeField] private int amountToGenerate;

    private GameObject objHolder;

    [Header("Platform Values")]
    [SerializeField] private Vector2Int platformSize;
    [SerializeField] private Vector2Int roadSize;

    private bool[,] grid;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.G))
        {
            SpawnObjects();
        }
    }

    private void SpawnObjects()
    {
        Destroy(objHolder);
        objHolder = new GameObject();
        GenerateGrid();
        //Instantiate(objects.value[Random.Range(0, 6)], transform);
        for(int a = 0; a < amountToGenerate; a++)
        {
            GenerateObject();
        }
    }

    private void GenerateGrid()
    {
        grid = new bool[50,50];
        for(int a = (platformSize.y/2) - (roadSize.y/2); a < (platformSize.y/2) + (roadSize.y/2); a++)
        {
            for(int b = (platformSize.x/2) - (roadSize.x/2); b < (platformSize.x/2) + (roadSize.x/2); b++)
            {
                grid[b,a] = true;
            }
        }
    }

    private void GenerateObject()
    {
        for(int a = 50; a > 0; a--)
        {
            if(CheckForCollidingObjects(ChooseRandomObject(), ChooseRandomPoint()))
            {
                return;
            }
        }
    }

    private bool CheckForCollidingObjects(ObjectToSpawn obj, Vector2Int position)
    {
        if(position.x < 0)
        {
            return false;
        }
        if((position.x + obj.Size.x) > platformSize.x || (position.y + obj.Size.y) > platformSize.y)
        {
            return false;
        } else {
            for(int a = position.y; a < (position.y + obj.Size.y); a++)
            {
                for(int b = position.x; b < (position.x + obj.Size.x); b++)
                {
                    if(grid[b, a])
                    {
                        return false;
                    }
                }
            }
            for(int a = position.y; a < (position.y + obj.Size.y); a++)
            {
                for(int b = position.x; b < (position.x + obj.Size.x); b++)
                {
                    grid[b, a] = true;
                }
            }
        }
        Instantiate(obj, new Vector3(transform.position.x - (platformSize.x/2) + position.x + (obj.Size.x/2), obj.transform.position.y, transform.position.z - (platformSize.y/2) + position.y + (obj.Size.y/2)), transform.rotation, objHolder.transform);
        return true;


    }

    private ObjectToSpawn ChooseRandomObject()
    {
        return objects.value[Random.Range(0, objects.value.Length)];
    }

    private Vector2Int ChooseRandomPoint()
    {
        for(int a = 100; a > 0; a--)
        {
            Vector2Int temp = new Vector2Int(Random.Range(0,50), Random.Range(0,50));
            if(!grid[temp.x, temp.y])
            {
                return temp;
            }
        }
        return new Vector2Int(-1, -1);
    }
}
