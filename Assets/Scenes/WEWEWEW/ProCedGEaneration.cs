using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProCedGEaneration : MonoBehaviour
{
    [Header("Variabels")]
    [SerializeField] private int spawnDistance;
    [SerializeField] private GameObject[] objectToSpawn;
    [SerializeField] private int generationSize;
    [SerializeField] private int generationAmount;

    private GameObject objectHolder;
    private GenerationNode[] nodes;

    private List<Vector2Int> directions = new List<Vector2Int>
    {
        new Vector2Int(0,1),
        new Vector2Int(1,0),
        new Vector2Int(0,-1),
        new Vector2Int(-1,0)
    };

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.G))
        {
            GenerateMap();
            GetTileTypes();
            SpawnPlanes();
        }
    }

    private void GenerateMap()
    {
        Destroy(objectHolder);
        objectHolder = new GameObject();
        nodes = new GenerationNode[generationSize + 1];
        nodes[generationSize] = new GenerationNode(new Vector2Int(0, 0), spawnDistance);
        for(int a = 0; a < generationAmount; a++)
        {
            CreateFirstGridNode((generationSize / generationAmount * a));
            for(int b = (generationSize / generationAmount * a) + 1; b < (generationSize / generationAmount) * (a + 1); b++)
            {
                GetNextGridPosition(b);
            }
        }

        //CheckNodes();
    }

    private void CheckNodes()
    {
        for(int a = 0; a < generationSize; a++)
        {
            Debug.Log(nodes[a].gridPosition);
        }
    }

    private void CreateFirstGridNode(int node)
    {
        Vector2Int tempGridPositon = new Vector2Int(0, 0) + directions[Random.Range(0, directions.Count)];
        for(int a = 0; a < 1000; a++)
        {
            if(CheckForExistingGridPosition(node, tempGridPositon))
            {
                tempGridPositon = tempGridPositon + directions[Random.Range(0, directions.Count)];
            } else {
                nodes[node] = new GenerationNode(tempGridPositon, spawnDistance);
                return;
            }
        }
    }

    private void GetNextGridPosition(int node)
    {
        Vector2Int tempGridPositon = nodes[node - 1].gridPosition + directions[Random.Range(0, directions.Count)];
        for(int a = 0; a < 1000; a++)
        {
            if(CheckForExistingGridPosition(node, tempGridPositon))
            {
                tempGridPositon = tempGridPositon + directions[Random.Range(0, directions.Count)];
            } else {
                nodes[node] = new GenerationNode(tempGridPositon, spawnDistance);
                return;
            }
        }
    }

    private bool CheckForExistingGridPosition(int currentNode, Vector2Int gridPosition)
    {
        for(int a = 0; a < currentNode; a++)
        {
            if(nodes[a].gridPosition == gridPosition)
            {
                return true;
            }
        }
        return false;
    }

    private bool CheckForExistingGridPosition(Vector2Int gridPosition)
    {
        for(int a = 0; a < generationSize + 1; a++)
        {
            if(nodes[a].gridPosition == gridPosition)
            {
                return true;
            }
        }
        return false;
    }

    private void GetTileTypes()
    {
        for(int a = 0; a < generationSize + 1; a++)
        {
            GetTileType(nodes[a]);
        }
    }

    private void GetTileType(GenerationNode node)
    {
        bool n = false, e = false, s = false, w = false;
        int tileCount = 0;
        if(CheckForExistingGridPosition(node.gridPosition + directions[0]))
        {
            n = true;
            tileCount++;
        }
        if(CheckForExistingGridPosition(node.gridPosition + directions[1]))
        {
            e = true;
            tileCount++;
        }
        if(CheckForExistingGridPosition(node.gridPosition + directions[2]))
        {
            s = true;
            tileCount++;
        }
        if(CheckForExistingGridPosition(node.gridPosition + directions[3]))
        {
            w = true;
            tileCount++;
        }

        if(tileCount < 2)
        {
            if(e)
            {
                node.rotation = 90;
            }
            if(s)
            {
                node.rotation = 180;
            }
            if(w)
            {
                node.rotation = 270;
            }
        }

        if(tileCount == 2)
        {
            node.tileType = 1;
            if(n && s)
            {
                return;
            } else if(e && w)
            {
                node.rotation = 90;
                return;
            }

            node.tileType = 2;
            if(n && e)
            {

            } else if(e && s)
            {
                node.rotation = 90;
            } else if(s && w)
            {
                node.rotation = 180;
            } else if(w && n)
            {
                node.rotation = 270;
            }
        }

        if(tileCount == 3)
        {
            node.tileType = 3;
            if(n && e && s)
            {

            } else if(e && s && w)
            {
                node.rotation = 90;
            } else if(s && w && n)
            {
                node.rotation = 180;
            } else if(w && n && e)
            {
                node.rotation = 270;
            }
        }

        if(tileCount == 4)
        {
            node.tileType = 4;
        }
    }

    private void SpawnPlanes()
    {
        for(int a = 0; a < generationSize + 1; a++)
        {
            Instantiate(objectToSpawn[nodes[a].tileType], new Vector3(nodes[a].worldPosition.x, 0f, nodes[a].worldPosition.y), Quaternion.Euler(0f, nodes[a].rotation, 0f), objectHolder.transform);
        }
    }
}
