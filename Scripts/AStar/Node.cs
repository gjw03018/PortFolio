using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Node : IHeapItem<Node>
{
    public bool walkable; //갈수있는지 검사
    public Vector2 worldPosition;
    
    public int gridX;
    public int gridY;

    //지금 현재 경로 값
    public int gCost;
    //예상 경로 값
    public int hCost;

    //부모노드
    public Node parent;

    int heapIndex;

    public Node(bool _walkable, Vector2 _worldPos, int _gridX, int _gridY)
    {
        walkable = _walkable;
        worldPosition = _worldPos;
        gridX = _gridX;
        gridY = _gridY;
    }

    //현재 와 미래 에 값 
    public int fCost
    {
        get
        {
            return gCost + hCost;
        }
    }



    //인터페이스 구현
    public int HeapIndex
    {
        get
        {
            return heapIndex;
        }
        set
        {
            heapIndex = value;
        }
    }

    public int CompareTo(Node nodeToCompare)
    {
        int compare = fCost.CompareTo(nodeToCompare.fCost);
        if(compare == 0)
        {
            compare = hCost.CompareTo(nodeToCompare.hCost);
        }

        return -compare;
    }
}
