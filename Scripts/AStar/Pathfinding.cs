using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System;

public class Pathfinding : MonoBehaviour
{
    PathRequestManager requestManager;
    Grid grid;

    private void Awake()
    {
        requestManager = GetComponent<PathRequestManager>();
        grid = GetComponent<Grid>();
    }
    //���Ž��
    public void StartFindPath(Vector3 startPos, Vector3 endPos)
    {
        StartCoroutine(FindPath(startPos, endPos));
    }

    //��� ã��
    IEnumerator  FindPath(Vector3 startPos, Vector3 endPos)
    {
        //heap ���� �ð�
        Stopwatch sw = new Stopwatch();
        sw.Start();


        Vector3[] waypoints = new Vector3[0];
        bool pathSuccess = false;
        
        //���� ���� ���� ��� �� �׸��� ��ǥ��
        Node startNode = grid.NodeFromWorldPoint(startPos);
        Node endNode = grid.NodeFromWorldPoint(endPos);
        if(startNode.walkable && endNode.walkable)
        {
            Heap<Node> openSet = new Heap<Node>(grid.MaxSize);
            //�ߺ����� ���� ������ ��� �Ҷ� ��� ���� ����
            HashSet<Node> closeSet = new HashSet<Node>();

            //���� ��� ���� ����
            openSet.Add(startNode);
        
        
            while(openSet.Count > 0)
            {
                //ù��° ���
                Node currentNode = openSet.RemoveFirst();

            
                //Ŭ�����Ʈ�� ��带 �����ش�
                closeSet.Add(currentNode);

                //�ֱ� ���� ������尡 ���ٸ� ������(while�� Ż��)
                if(currentNode == endNode)
                {
                    sw.Stop();
                    //print("Path found : " + sw.ElapsedMilliseconds + "ms");
                    pathSuccess = true;
             
                    break;
                }
                //�ݺ� ������ �ִ� ��� 
                foreach(Node neighbour in grid.GetNeighbours(currentNode))
                {
                    //������ ���� ��ġ�ų� Ŭ�����尡 �߰��Ǿ��մٸ� �ǳʶٱ�
                    if(!neighbour.walkable || closeSet.Contains(neighbour))
                    {
                        continue;
                    }

                    //������ �̿��Ǿ��ִ� ���鿡 ��� ���� ���ϰ� �ּڰ� ��θ� �߰����ش�
                    int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
                    if(newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                    {
                        neighbour.gCost = newMovementCostToNeighbour;
                        neighbour.hCost = GetDistance(neighbour, endNode);
                        neighbour.parent = currentNode;

                        if(!openSet.Contains(neighbour))
                        {
                            openSet.Add(neighbour);
                        }
                    }
                    //�̺κ��� ���� ������忡 ���� ���� �����̹Ƿ� �ٽ� while �� ����
                }
            }

        }
        yield return null;
        if(pathSuccess)
        {
            waypoints = RetracePath(startNode, endNode);
        }
        requestManager.FinishedProcessingPath(waypoints, pathSuccess);
    }

    //ã����θ� �������Ѽ� ù��° ��尡 ����ǰ��Ѵ�
    Vector3[] RetracePath(Node starNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while(currentNode != starNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        Vector3[] waypoints = SimplifyPath(path);
        Array.Reverse(waypoints);

        return waypoints;
    }

    Vector3[] SimplifyPath(List<Node> path)
    {
        List<Vector3> waypoints = new List<Vector3>();
        Vector2 directionOld = Vector2.zero;

        for(int i = 1; i < path.Count; i++)
        {
            Vector2 directionNew = new Vector2(path[i - 1].gridX - path[i].gridX, path[i - 1].gridY - path[i].gridY);
            if(directionNew != directionOld)
            {
                waypoints.Add(path[i].worldPosition);
            }
            directionOld = directionNew;
        }
        return waypoints.ToArray();
    }

    //�Ÿ� ���ϱ�
    int GetDistance(Node nodeA, Node nodeB)
    {
        int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if(dstX > dstY)
        {
            return 14 * dstY + 10 * (dstX - dstY);
        }
        return 14 * dstX + 10 * (dstY - dstX);

    }
}
