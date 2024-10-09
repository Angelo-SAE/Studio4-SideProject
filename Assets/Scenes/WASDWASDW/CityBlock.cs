using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityBlock : MonoBehaviour
{
    [Header("Scriptable sideObjects")]
    [SerializeField] private ObjectArrayObject sideObjects;
    [SerializeField] private ObjectArrayObject roadObjects;
    [SerializeField] private ObjectArrayObject startObjectes;
    [SerializeField] private ObjectArrayObject exitObjects;
    [SerializeField] private ObjectArrayObject safeZoneObjects;

    [Header("Generation Values")]
    [SerializeField] private int sideObjectsToGenerate;
    [SerializeField] private int roadObjectsToGenerate;

    private GameObject objHolder;
    private bool hasStart;
    private bool hasExit;
    private bool hasSafeZone;

    public bool HasStart {get{return hasStart;} set{hasStart = value;}}
    public bool HasExit {set{hasExit = value;}}
    public bool HasSafeZone {get{return hasSafeZone;} set{hasSafeZone = value;}}

    [Header("Platform Values")]
    [SerializeField] private Vector2Int platformSize;
    [SerializeField] private Vector2Int[] safeZonePoints;
    [SerializeField] private Vector4 roadSizeOne;
    [SerializeField] private Vector4 roadSizeTwo;

    private bool[,] sideGrid;
    private bool[,] roadGrid;
    private int platformRotation;

    public int PlatformRotation {set{platformRotation = value;}}

    /*
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.G))
        {
            SpawnObjects();
        }
    }
    */

    public void SpawnObjects()
    {
        Destroy(objHolder);
        objHolder = new GameObject();
        objHolder.transform.SetParent(transform);
        GenerateGrids();
        if(hasStart)
        {
            CheckForCollidingObjects(roadGrid, ChooseRandomObject(startObjectes), new Vector2Int(platformSize.x/2, platformSize.y/2));
        }
        if(hasExit)
        {
            CheckForCollidingObjects(roadGrid, ChooseRandomObject(exitObjects), new Vector2Int(platformSize.x/2, platformSize.y/2));
        }
        if(hasSafeZone)
        {
            for(int a = 0; a < 50; a++)
            {
                if(CheckForCollidingObjects(sideGrid, ChooseRandomObject(safeZoneObjects), ChooseRandomSafeZonePoint()))
                {
                    break;
                }
            }

        }
        for(int a = 0; a < sideObjectsToGenerate; a++)
        {
            GenerateObject(sideGrid, sideObjects);
        }

        for(int a = 0; a < roadObjectsToGenerate; a++)
        {
            GenerateObject(roadGrid, roadObjects);
        }

        transform.rotation = Quaternion.Euler(0f, platformRotation, 0f);
    }

    private void GenerateGrids()
    {
        sideGrid = new bool[platformSize.x, platformSize.y];
        for(int a = (int)roadSizeOne.y; a < (int)roadSizeOne.w; a++)
        {
            for(int b = (int)roadSizeOne.x; b < (int)roadSizeOne.z; b++)
            {
                sideGrid[b,a] = true;
            }
        }
        for(int a = (int)roadSizeTwo.y; a < (int)roadSizeTwo.w; a++)
        {
            for(int b = (int)roadSizeTwo.x; b < (int)roadSizeTwo.z; b++)
            {
                sideGrid[b,a] = true;
            }
        }
        roadGrid = new bool[platformSize.x, platformSize.y];
        for(int a = 0; a < platformSize.y; a++)
        {
            for(int b = 0; b < platformSize.x; b++)
            {
                roadGrid[b,a] = !sideGrid[b,a];
            }
        }
    }

    private void GenerateObject(bool[,] grid, ObjectArrayObject objects)
    {
        for(int a = 50; a > 0; a--)
        {
            if(CheckForCollidingObjects(grid, ChooseRandomObject(objects), ChooseRandomPoint(grid)))
            {
                return;
            }
        }
    }


    private bool CheckForCollidingObjects(bool[,] grid, ObjectToSpawn obj, Vector2Int position)
    {
        if(position.x < 0)
        {
            return false;
        }
        if((position.x + (obj.Size.x/2)) > platformSize.x || (position.y + (obj.Size.x/2)) > platformSize.y || (position.x - (obj.Size.x/2)) < 0 || (position.y - (obj.Size.x/2)) < 0)
        {
            return false;
        } else {
            for(int a = position.y - (obj.Size.y/2); a < (position.y + (obj.Size.y/2)); a++)
            {
                for(int b = position.x - (obj.Size.x/2); b < (position.x + (obj.Size.x/2)); b++)
                {
                    if(grid[b, a])
                    {
                        return false;
                    }
                }
            }
            for(int a = position.y - (obj.Size.y/2); a < (position.y + (obj.Size.y/2)); a++)
            {
                for(int b = position.x - (obj.Size.x/2); b < (position.x + (obj.Size.x/2)); b++)
                {
                    grid[b, a] = true;
                }
            }
        }
        Instantiate(obj, new Vector3(transform.position.x - (platformSize.x/2) + position.x, obj.transform.position.y, transform.position.z - (platformSize.y/2) + position.y), transform.rotation, objHolder.transform);
        return true;
    }

    private ObjectToSpawn ChooseRandomObject(ObjectArrayObject objects)
    {
        return objects.value[Random.Range(0, objects.value.Length)];
    }

    private Vector2Int ChooseRandomPoint(bool[,] grid)
    {
        for(int a = 100; a > 0; a--)
        {
            Vector2Int temp = new Vector2Int(Random.Range(0,platformSize.x), Random.Range(0,platformSize.y));
            if(!grid[temp.x, temp.y])
            {
                return temp;
            }
        }
        return new Vector2Int(-1, -1);
    }

    private Vector2Int ChooseRandomSafeZonePoint()
    {
        return safeZonePoints[Random.Range(0, safeZonePoints.Length)];
    }
}
