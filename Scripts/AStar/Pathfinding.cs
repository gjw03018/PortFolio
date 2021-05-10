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
    //경로탐색
    public void StartFindPath(Vector3 startPos, Vector3 endPos)
    {
        StartCoroutine(FindPath(startPos, endPos));
    }

    //경로 찾기
    IEnumerator  FindPath(Vector3 startPos, Vector3 endPos)
    {
        //heap 구조 시간
        Stopwatch sw = new Stopwatch();
        sw.Start();


        Vector3[] waypoints = new Vector3[0];
        bool pathSuccess = false;
        
        //시작 노드와 도착 노드 를 그리드 좌표로
        Node startNode = grid.NodeFromWorldPoint(startPos);
        Node endNode = grid.NodeFromWorldPoint(endPos);
        if(startNode.walkable && endNode.walkable)
        {
            Heap<Node> openSet = new Heap<Node>(grid.MaxSize);
            //중복되지 않은 데이터 사용 할때 사용 값을 저장
            HashSet<Node> closeSet = new HashSet<Node>();

            //시작 노드 부터 시작
            openSet.Add(startNode);
        
        
            while(openSet.Count > 0)
            {
                //첫번째 노드
                Node currentNode = openSet.RemoveFirst();

            
                //클로즈리스트에 노드를 더해준다
                closeSet.Add(currentNode);

                //최근 노드와 도착노드가 같다면 끝낸다(while문 탈출)
                if(currentNode == endNode)
                {
                    sw.Stop();
                    //print("Path found : " + sw.ElapsedMilliseconds + "ms");
                    pathSuccess = true;
             
                    break;
                }
                //반복 주위에 있는 노드 
                foreach(Node neighbour in grid.GetNeighbours(currentNode))
                {
                    //걸을수 없는 위치거나 클로즈노드가 추가되어잇다면 건너뛰기
                    if(!neighbour.walkable || closeSet.Contains(neighbour))
                    {
                        continue;
                    }

                    //주위에 이웃되어있는 노드들에 경로 값을 더하고 최솟값 경로를 추가해준다
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
                    //이부분은 아직 도착노드에 하지 않은 상태이므로 다시 while 문 실행
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

    //찾은경로를 반전시켜서 첫번째 노드가 실행되게한다
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

    //거리 구하기
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
