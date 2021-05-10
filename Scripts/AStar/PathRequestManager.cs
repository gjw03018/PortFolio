using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;



public class PathRequestManager : MonoBehaviour
{
    //Queue FIFO
    Queue<PathRequest> pathRequestQueue = new Queue<PathRequest>();
    PathRequest currentPathRequest;
    Pathfinding pathfinding;

    bool isProcessingPath;

    static PathRequestManager instance;
    private void Awake()
    {
        instance = this;
        pathfinding = GetComponent<Pathfinding>();
    }

    public static void RequestPath(Vector3 pathStart, Vector3 pathEnd, Action<Vector3[], bool> callback)
    {
        PathRequest newRequest = new PathRequest(pathStart, pathEnd, callback);
        instance.pathRequestQueue.Enqueue(newRequest);
        instance.TryProcessNext();
    }
    //다음경로를 가져온다
    void TryProcessNext()
    {
        //Queue 안에 경로가 있다면
        if(!isProcessingPath && pathRequestQueue.Count > 0)
        {
            //경로를 가져온다
            currentPathRequest = pathRequestQueue.Dequeue();
            isProcessingPath = true;
            //경로탐색
            pathfinding.StartFindPath(currentPathRequest.pathStart, currentPathRequest.pathEnd);
        }
    }

    //경로찾기가 끝났을경우
    public void FinishedProcessingPath(Vector3[] path, bool success)
    {
        currentPathRequest.callback(path, success);
        isProcessingPath = false;
        TryProcessNext();
    }


    //구조체 경로에 시작과 끝을 알고 있다
    struct PathRequest
    {
        public Vector3 pathStart;
        public Vector3 pathEnd;
        public Action<Vector3[], bool> callback;

        public PathRequest(Vector3 _start, Vector3 _end, Action<Vector3[], bool> _callback) {
            pathStart = _start;
            pathEnd = _end;
            callback = _callback;
        }
    }
}
