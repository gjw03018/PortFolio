using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
public class Grid : MonoBehaviour
{
    public bool displayGridGizmos;
    public Transform player;

    public LayerMask unwalkableMask;
    //����ü ũ��
    public Vector2 gridWorldSize;
    //��� ũ��
    public float nodeRadius;
    Node[,] grid;
    //��� ����
    float nodeDiameter;
    //�׸��� ������
    int gridSizeX, gridSizeY;

    //ã�� ���
    public List<Node> path;
    private void Awake()
    {
        nodeDiameter = nodeRadius * 2;
        //�׸��� ������� ������ ��ü ������� ��忡 ������ŭ ������
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
        //�׸��� ���� 
        grid = new Node[gridSizeX, gridSizeY];
        //�׸��忡 ���� ������ ���� ���ϴ�
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x/2 - Vector3.up * gridWorldSize.y/2;

        for(int x = 0; x < gridSizeX; x++)
        {
            for(int y = 0; y < gridSizeY; y++)
            {
                //������ǥ
                Vector2 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.up * (y * nodeDiameter + nodeRadius);
                //������ �ִ� �������� �Ǻ�
                bool walkable = !(Physics2D.OverlapCircle(worldPoint, nodeRadius, unwalkableMask));
                //��� ����
                grid[x, y] = new Node(walkable, worldPoint,x,y);
            }
        }
    }

    //������ �մ� ���
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



    //���� ��ǥ�� ��� ��ǥ�� ġȯ
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

    
    //����� �׷��� ���̰��Ѵ�
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
