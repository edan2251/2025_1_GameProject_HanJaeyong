using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int gridWidth = 7;                                       //가로칸수
    public int gridHeight = 7;                                      //세로칸수
    public float cellSize = 1.4f;                                   //각칸의크기
    public GameObject cellPrefabs;                                  //빈칸프리팹
    public Transform gridContainer;                                 //그리드를 담을 부모 오브젝트

    public GameObject rankPrefabs;                                  //계급장프리팹
    public Sprite[] rankSprites;                                    //각레벨별계급장이미지
    public int maxRankLevel = 7;                                    //최대계급장레벨

    public GridCell[,] grid;                                        //모든 칸을 저장하는 2차원 배열

    void InitializeGrid()
    {
        grid = new GridCell[gridWidth, gridHeight];

        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                Vector3 position = new Vector3
                (
                    x * cellSize - (gridWidth * cellSize / 2) + cellSize / 2,
                    y * cellSize - (gridHeight * cellSize / 2) + cellSize / 2,
                    1f
                );

                GameObject cellObj = Instantiate(cellPrefabs, position, Quaternion.identity, gridContainer);
                GridCell cell = cellObj.AddComponent<GridCell>();
                cell.Initialize(x, y);

                grid[x, y] = cell;                  //배열에 저장
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        InitializeGrid();

        for(int i = 0; i < 4; i++)
        {
            SpawnNewRank();
        }   
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.D))
        {
            SpawnNewRank();
        }
    }

    public DraggableRank CreateRankInCell(GridCell cell, int level)
    {
        if (cell == null || !cell.IsEmpty()) return null;

        level = Mathf.Clamp(level, 1, maxRankLevel);

        Vector3 rankPosition = new Vector3(cell.transform.position.x, cell.transform.position.y, 0f);

        GameObject rankObj = Instantiate(rankPrefabs, rankPosition, Quaternion.identity, gridContainer);
        rankObj.name = "Rank_Lvel" + level;

        DraggableRank rank = rankObj.AddComponent<DraggableRank>();
        rank.SetRankLevel(level);

        return rank;
    }

    private GridCell FindEmptyCell()
    {
        List<GridCell> emptyCells = new List<GridCell>();           //빈칸 저장할 리스트

        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                if (grid[x,y].IsEmpty())
                {
                    emptyCells.Add(grid[x, y]);
                }
            }
        }

        if(emptyCells.Count == 0)
        {
            return null;
        }

        return emptyCells[Random.Range(0, emptyCells.Count)];
    }

    public bool SpawnNewRank()
    {
        GridCell emptyCell = FindEmptyCell();
        if (emptyCell == null) return false;

        int rankLevel = Random.Range(0, 100) < 80 ? 1 : 2;

        CreateRankInCell(emptyCell, rankLevel);

        return true;
    }

    public GridCell FindClosestCell(Vector3 position)               //가장 가까운 칸 찾기
    {
        for(int x = 0; x < gridWidth; x++)                          //1. 먼저 위치가 포함된 칸 확인
        {
            for(int y = 0; y < gridHeight; y++)
            {
                if (grid[x, y].ContainsPosition(position))
                {
                    return grid[x, y];
                }
            }
        }

        GridCell closestCell = null;                                //2.없다면 가장 가까운 칸 찾기
        float closestDistance = float.MaxValue;

        for(int x = 0; x < gridWidth; x++)
        {
            for(int y = 0; y < gridHeight; y++)
            {
                float distance = Vector3.Distance(position, grid[x, y].transform.position);
                if ((distance < closestDistance))
                {
                    closestDistance = distance;
                    closestCell = grid[x, y];
                }
            }
        }
        if(closestDistance > cellSize * 2)
        {
            return null;
        }

        return closestCell;
    }

    public void MergeRanks(DraggableRank draggedRank , DraggableRank targetRank)
    {
        if(draggedRank == null || targetRank == null || draggedRank.rankLevel != targetRank.rankLevel)
        {
            if (draggedRank != null) draggedRank.ReturnToOriginalPosition();
            return;
        }
        int newLevel = targetRank.rankLevel + 1;
        if(newLevel > maxRankLevel)
        {
            RemoveRank(draggedRank);
            return;
        }

        targetRank.SetRankLevel(newLevel);
        RemoveRank(draggedRank);

        if(Random.Range(0,100) < 60)
        {
            SpawnNewRank();
        }
    }

    public void RemoveRank(DraggableRank rank)
    {
        if (rank == null) return;

        if(rank.currentCell !=null)
        {
            rank.currentCell.currentRank = null;
        }
        Destroy(rank.gameObject);
    }
}
