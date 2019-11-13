using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingRaiseEventOnClosestNodeChanged : MonoBehaviour
{
    public GameEvent eventToRaise;
    public float tickRate = .3f;

    private float timer;

    Node closestNode;

    PathfindingNodeManager manager;

    private void Start() {
        manager = PathfindingNodeManager.INSTANCE;
        closestNode = manager.ClosestNodeToPoint(transform.position);
    }

    public void Update() {
        if(timer < 0) {
            Node newClosest = manager.ClosestNodeToPoint(transform.position);
            if(newClosest != closestNode) {
                Debug.Log("Update!");
                eventToRaise.Raise();
                closestNode = newClosest;
            }
            timer = tickRate;
        } else {
            timer -= Time.deltaTime;
        }
    }

}
