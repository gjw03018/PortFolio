using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
public class Grid : MonoBehaviour
{
    public bool displayGridGizmos;
    public Transform player;

    public LayerMask unwalkableMask;
    //맵전체 크기
    public Vector2 gridWorldSize;
    //노드 크기
    public float nodeRadius;
    Node[,] grid;
    //노드 지름
    float nodeDiameter;
    //그리드 사이즈
    int gridSizeX, gridSizeY;

    //찾은 경로
    public List<Node> path;
    private void Awake()
    {
        nodeDiameter = nodeRadius * 2;
        //그리드 사이즈는 정수로 전체 사이즈에서 노드에 지름만큼 나눈다
        gridSizeX = Mathf.RoundToInt (gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        CreateGrid();
    }

    public int MaxSize
    {
        get
        {
            return gridSizeX * gridSizeY;
        }
    }

    void CreateGrid()
    {
        //그리드 생성 
        grid = new Node[gridSizeX, gridSizeY];
        //그리드에 시작 지점은 왼쪽 맨하단
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x/2 - Vector3.up * gridWorldSize.y/2;

        for(int x = 0; x < gridSizeX; x++)
        {
            for(int y = 0; y < gridSizeY; y++)
            {
                //월드좌표
                Vector2 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.up * (y * nodeDiameter + nodeRadius);
                //걸을수 있는 공간인지 판별
                bool walkable = !(Physics2D.OverlapCircle(worldPoint, nodeRadius, unwalkableMask));
                //노드 생성
                grid[x, y] = new Node(walkable, worldPoint,x,y);
            }
        }
    }

    //주위에 잇는 노드
    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neigbours = new List<Node>();

        //-1 0 1
        for(int x = -1; x <= 1; x++)
        {
            for(int y = -1; y <=1; y++)
            {
                if (x == 0 && y == 0)
                    continue;

                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if(checkX >= 0 && checkY < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                {
                    neigbours.Add(grid[checkX, checkY]);
                }
            }
        }

        return neigbours;
    }



    //월드 좌표를 노드 좌표로 치환
    public Node NodeFromWorldPoint(Vector3 worldPosition)
    {
        float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentY = (worldPosition.y + gridWorldSize.y / 2) / gridWorldSize.y;

        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt( (gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);

        return grid[x, y];
    }

    
    //기즈모를 그려서 보이게한다
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x,gridWorldSize.y,1));
        if (grid != null && displayGridGizmos)
        {
            foreach (Node n in grid)
            {
                Gizmos.color = (n.walkable) ? Color.white : Color.red;
                Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter - .1f));
            }
        }
        
        
    }
}
