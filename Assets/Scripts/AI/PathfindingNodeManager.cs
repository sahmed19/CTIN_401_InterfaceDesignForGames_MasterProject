using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node {
    //[SerializeField]
    Vector3 position;
    List<Node> neighbors;

    public Node(Vector3 p) {
        position = p;
        neighbors = new List<Node>();
    }

    public Node() {
        neighbors = new List<Node>();
    }

    public Node[] GetNeighbors() {
        return neighbors.ToArray();
    }

    public bool AddNeighbor(Node neighbor) {
        if(IndexOfNeighbor(neighbor) == -1) {
            neighbors.Add(neighbor);
            return true;
        }
        return false;
    }

    public int IndexOfNeighbor(Node neighbor) {
        return neighbors.IndexOf(neighbor);
    }

    public override string ToString() {return position.ToString();}

    public static Node operator +(Node node, Vector3 v3) { node.position += v3; return node; }
    public static explicit operator Vector3(Node node) {return node.position;}

    public static float Distance(Node node1, Vector3 v3) { return Vector3.Distance((Vector3) node1, v3); }
    public static float Distance(Node node1, Node node2) { return Vector3.Distance((Vector3) node1, (Vector3) node2); }
    public static float SqrDistance(Node node1, Vector3 v3) { return ((Vector3) node1 - v3).sqrMagnitude; }
    public static float SqrDistance(Node node1, Node node2) { return ((Vector3) node1 - (Vector3) node2).sqrMagnitude; }
}

[ExecuteInEditMode]
public class PathfindingNodeManager : MonoBehaviour
{
    public List<Node> nodeList;
    public LayerMask obstacleMask;
    public Vector3 offset = Vector3.up;

    public List<Vector3> nodePositions;

    public static PathfindingNodeManager INSTANCE;

    public float range = 15.0f;

    public float sqrRange {
        get {
            return range * range;
        }
    }

    private void Awake() {
        INSTANCE = this;
        GenerateNodesAndNodeConnections();
    }

    public Node ClosestNodeToPoint(Vector3 position) {

        float minDistance = float.MaxValue;

        Node closestNode = null;
        foreach(Node node in nodeList) {
            float dist = Node.SqrDistance(node, position);
            if(dist < minDistance) {
                minDistance = dist;
                closestNode = node;
            }
        }

        return closestNode;
    }

    public List<Node> GetListOfNodesAroundPoint(Vector3 position, float range) {

        List<Node> ret = new List<Node>();

        float sqrRange = range * range;

        foreach(Node n in nodeList) {
            if(Node.SqrDistance(n, position) < sqrRange) {
                ret.Add(n);
            }
        }

        return ret;

    }

    public void GenerateNodesAndNodeConnections() {
        GenerateNodes();
        MakeConnections();
    }

    public void DeleteNodesAndConnections() {
        nodeList = new List<Node>();
        nodePositions = new List<Vector3>();
    }

    private void GenerateNodes() {
        nodeList = new List<Node>();
        foreach(Vector3 pos in nodePositions) {
            nodeList.Add(new Node(pos));
        }
    }

    private void MakeConnections() {

        for(int n = 0; n < nodeList.Count; n++) {

            Node node = nodeList[n];
            Vector3 nodePosition = (Vector3) node + offset;

            for(int tn = nodeList.Count-1; tn > 0; tn--) {

                if(n == tn) { continue; }

                Node targetNode = nodeList[tn];
                Vector3 targetNodePosition = (Vector3) targetNode + offset;

                float sqrDistance = Node.SqrDistance(node, targetNode);
                
                if(sqrDistance > sqrRange) {
                    continue;
                }

                RaycastHit hit;
                if(!Physics.Raycast(nodePosition, targetNodePosition - nodePosition, out hit, range, obstacleMask.value)) {
                    
                    bool addingFirst = node.AddNeighbor(targetNode);
                    bool addingSecond = targetNode.AddNeighbor(node);
                }
            }
        }
    }

    public void PopulateGrid(int x, int y, float spacing) {

        float backwash = spacing * (.5f * (x-1));

        nodePositions = new List<Vector3>();

        for(int i = 0; i < x; i++) {
            for(int j = 0; j < y; j++) {
                nodePositions.Add(new Vector3(-backwash + i * spacing, 0f, -backwash + j * spacing) + 
                new Vector3(Random.Range(-.1f, .1f), 0f, Random.Range(-.1f, .1f)) * spacing);
            }
        }

    }

}
