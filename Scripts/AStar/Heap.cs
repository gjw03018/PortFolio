using UnityEngine;
using System.Collections;
using System;

public class Heap<T> where T : IHeapItem<T>
{
    //제네릭 변수 아이템
    T[] items;
    //힙에 들어가 있는 개수
    int currentItemCount;

    //생성자
    public Heap(int maxHeapSize)
    {
        items = new T[maxHeapSize];
    }

    //ADD
    public void Add(T item)
    {
        //추가 될 아이템에 인덱스 부여
        item.HeapIndex = currentItemCount;
        //부여된 인덱스 공간에 아이템 추가
        items[currentItemCount] = item;
        //정력
        SortUp(item);
        //개수 증가
        currentItemCount++;
    }

    //첫번째 지우기
    public T RemoveFirst()
    {
        //첫번째 아이템
        T firstItem = items[0];
        //전체 개수 감소
        currentItemCount--;
        //첫본째 공간에 마지막힙에 들어가 있는 아이템을 추가
        items[0] = items[currentItemCount];
        //첫번째 공간 인덱스를 0으로
        items[0].HeapIndex = 0;
        //정렬
        SortDown(items[0]);
        return firstItem;
    }

    public void UpdateItem(T item)
    {
        SortUp(item);
    }

    public int Count
    {
        get
        {
            return currentItemCount;
        }
    }

    public bool Contains(T item)
    {
        return Equals(items[item.HeapIndex], item);
    }

    //정렬
    void SortDown(T item)
    {
        //반복
        while (true)
        {
            //왼쪽에 있는 힙공간은 *2+1 , 오른쪽 *2 +2 인덱스를 가지고 있다
            int childIndexLeft = item.HeapIndex * 2 + 1;
            int childIndexRight = item.HeapIndex * 2 + 2;
            int swapIndex = 0;

            //왼쪽에 있는 변수가 더작다면 바꾸기
            if(childIndexLeft < currentItemCount)
            {
                swapIndex = childIndexLeft;
                //오른쪽에 있는 변수가 더작다면 바꾸기
                if(childIndexRight < currentItemCount)
                {
                    if(items[childIndexLeft].CompareTo(items[childIndexRight]) < 0)
                    {
                        swapIndex = childIndexRight;
                    }
                }
                //바꾸기
                if(item.CompareTo(items[swapIndex]) < 0)
                    Swap(item, items[swapIndex]);
                else
                    return;
            }
            else
                return;
        }
    }
    //정렬 
    void SortUp(T item)
    {
        //부모 위치에 있는 힙인덱스
        int parentIndex = (item.HeapIndex - 1) / 2;

        while (true)
        {
            T parentItem = items[parentIndex];
            if(item.CompareTo(parentItem) > 0)
                Swap(item, parentItem);
            else
                break;
            parentIndex = (item.HeapIndex - 1) / 2;
        }
    }

    void Swap(T itemA, T itemB)
    {
        items[itemA.HeapIndex] = itemB;
        items[itemB.HeapIndex] = itemA;
        int itemAIndex = itemA.HeapIndex;
        itemA.HeapIndex = itemB.HeapIndex;
        itemB.HeapIndex = itemAIndex;
    }
}



//비교 인터페이스
public interface IHeapItem<T> : IComparable<T>
{
    int HeapIndex
    {
        get;
        set;
    }
}
