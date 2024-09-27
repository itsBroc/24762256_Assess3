using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;

public class Procedural_Generater : MonoBehaviour
{
    public GameObject outsideCornerPrefab;
    public GameObject outsideWallPrefab;
    public GameObject insideCornerPrefab;
    public GameObject insideWallPrefab;
    public GameObject pelletPrefab;
    public GameObject powerPelletPrefab;
    public GameObject tJunctionPrefab;

    public GameObject parentContainer;

    public int[,] levelMap =
    {
        {1,2,2,2,2,2,2,2,2,2,2,2,2,7},
        {2,5,5,5,5,5,5,5,5,5,5,5,5,4},
        {2,5,3,4,4,3,5,3,4,4,4,3,5,4},
        {2,6,4,0,0,4,5,4,0,0,0,4,5,4},
        {2,5,3,4,4,3,5,3,4,4,4,3,5,3},
        {2,5,5,5,5,5,5,5,5,5,5,5,5,5},
        {2,5,3,4,4,3,5,3,3,5,3,4,4,4},
        {2,5,3,4,4,3,5,4,4,5,3,4,4,3},
        {2,5,5,5,5,5,5,4,4,5,5,5,5,4},
        {1,2,2,2,2,1,5,4,3,4,4,3,0,4},
        {0,0,0,0,0,2,5,4,3,4,4,3,0,3},
        {0,0,0,0,0,2,5,4,4,0,0,0,0,0},
        {0,0,0,0,0,2,5,4,4,0,3,4,4,0},
        {2,2,2,2,2,1,5,3,3,0,4,0,0,0},
        {0,0,0,0,0,0,5,0,0,0,4,0,0,0},
    };


    // Start is called before the first frame update
    void Start()
    {
        ClearLevel();
        GenerateLevel();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void GenerateLevel()
    {
        int[,] totalLevelMap = GenerateFullGrid(levelMap);


        for (int row = 0; row < totalLevelMap.GetLength(0); row++)
        {
            for (int col = 0; col < totalLevelMap.GetLength(1); col++)
            {
                Vector3 position = new Vector3(col, -row, 0);
                InstantiateTile(totalLevelMap, row, col, position);

            }
        }
    }
    private int[,] GenerateFullGrid(int[,] levelMap)
    {
        int rows = levelMap.GetLength(0);
        int cols = levelMap.GetLength(1);
        int totalRows = rows * 2 - 1;
        int totalCols = cols * 2;

        int[,] totalLevelMap = new int[totalRows, totalCols];

        for (int x = 0; x < rows; x++)
        {
            for (int y = 0; y < cols; y++)
            {
                int num = levelMap[x, y];

                totalLevelMap[x, y] = num;
                totalLevelMap[x, totalCols - 1 - y] = num;

                if (x != totalCols - 1)
                {
                    totalLevelMap[totalRows - 1 - x, y] = num;
                    totalLevelMap[totalRows - 1 - x, totalCols - 1 - y] = num;
                }
            }
        }
        return totalLevelMap;
    }

    private void InstantiateTile(int[,] levelArray, int row, int col, Vector3 position)
    {
        int tileType = levelArray[row, col];
        GameObject tilePrefab = null;

        switch (tileType)
        {
            case 1:
                tilePrefab = outsideCornerPrefab;
                break;
            case 2:
                tilePrefab = outsideWallPrefab;
                break;
            case 3:
                tilePrefab = insideCornerPrefab;
                break;
            case 4:
                tilePrefab = insideWallPrefab;
                break;
            case 5:
                tilePrefab = pelletPrefab;
                break;
            case 6:
                tilePrefab = powerPelletPrefab;
                break;
            case 7:
                tilePrefab = tJunctionPrefab;
                break;
            default:
                return;

        }

        if (tilePrefab != null)
        {
            GameObject tileInstance = Instantiate(tilePrefab, position, Quaternion.identity);
            tileInstance.transform.parent = parentContainer.transform;

            int rotation = CalculateTileRotation(levelArray, row, col);
            tileInstance.transform.Rotate(0, 0, rotation);
        }
    }

    int CalculateTileRotation(int[,] levelArray, int row, int col)
    {
        int tileType = levelArray[row, col];

        int above = GetNumAt(levelArray, row - 1, col);
        int below = GetNumAt(levelArray, row + 1, col);
        int left = GetNumAt(levelArray, row, col - 1);
        int right = GetNumAt(levelArray, row, col + 1);

        switch (tileType)
        {
            case 1:
                return MatchCornerNeighbours(tileType, left, right, above, below);
            case 2:
                return MatchStraightNeighbours(tileType, left, right, above, below);
            case 3:
                return MatchCornerNeighbours(tileType, left, right, above, below);
            case 4:
                return MatchStraightNeighbours(tileType, left, right, above, below);
            case 7:
                return MatchTJunctionNeighbours(left, right, above, below);
            default:
                return 0;
        }
    }

    int GetNumAt(int[,] array, int row, int col)
    {
        if (row >= 0 && row < array.GetLength(0) && col >= 0 && col < array.GetLength(1))
        {
            return array[row, col];
        }
        return -1;
    }

    int MatchStraightNeighbours(int tileType, int left, int right, int above, int below)
    {
        if (IsWall(above) && IsWall(below))
        {
            return 0;
        }
        if (IsWall(left) && IsWall(right))
        {
            return 90;
        }
        return 0;
    }

    int MatchCornerNeighbours(int tileType, int left, int right, int above, int below)
    {
        if (IsWall(left) && IsWall(above))
        {
            return 0;
        }
        if (IsWall(right) && IsWall(above))
        {
            return -90;
        }
        if (IsWall(right) && IsWall(below))
        {
            return -180;
        }
        if (IsWall(left) && IsWall(below))
        {
            return -270;
        }
        return 0;
    }

    int MatchTJunctionNeighbours(int left, int right, int above, int below)
    {
        if (IsWall(left) && IsWall(right) && IsWall(above))
        {
            return -90;
        }
        if (IsWall(left) && IsWall(right) && IsWall(above))
        {
            return -90;
        }
        if (IsWall(above) && IsWall(below) && IsWall(left))
        {
            return 180;
        }
        if (IsWall(above) && IsWall(below) && IsWall(right))
        {
            return 0;
        }
        return 0;
    }
    bool IsWall(int tileNum)
    {
        return tileNum == 1 || tileNum == 2 || tileNum == 3 || tileNum == 4 || tileNum == 7;
    }



    private void ClearLevel()
    {
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Level"))
        {
            Destroy(obj);
        }
    }
}
