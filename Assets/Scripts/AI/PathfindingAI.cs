using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public class PathfindingAI : MonoBehaviour
{
    
    public class AStarNode : IHeapItem<AStarNode> {
        
        public Node node;
        private AStarNode parentNode;
        private AStarNode targetNode;
        private int id;

        float g = 1000000f;
        float h = 1000000f;

        public AStarNode(Node n, int i) {
            node = n;
            id = i;
        }

        public void SetParent(AStarNode pn) {
            parentNode = pn;
            g = parentNode.g + Node.SqrDistance(node, parentNode.node);
        }

        public AStarNode GetParent() {
            return parentNode;
        }

        public void SetTarget(AStarNode tn) {
            targetNode = tn;
            h = Node.SqrDistance(node, targetNode.node);
        }

        public float G { get { return g; } }
        public float H { get { return h; } }
        public float F { get { return h + g; } }
        public int ID { get { return id; } }

        bool explored = false;

        public void CloseNode() { explored = true; }
        public bool IsExplored() { return explored; }

        //Operator stuff
        public override bool Equals(System.Object o) {
            if(o is AStarNode) 
            { 
                return Equals((AStarNode) o);
            } 
            return false;
        }

        public bool Equals(AStarNode other) {
            return id == (other).ID;
        }

        public static bool operator ==(AStarNode node1, AStarNode node2) { return node1.ID == node2.ID; }
        public static bool operator !=(AStarNode node1, AStarNode node2) { return node1.ID != node2.ID; }

        public override int GetHashCode() {
            return id;
        }

        //HEAP STUFF
        int heapIndex;

        public int HeapIndex {
            get {
                return heapIndex;
            }
            set {
                heapIndex = value;
            }
        }

        public int CompareTo(AStarNode other) {
            int compare = F.CompareTo(other.F);
            if(compare == 0) {
                compare = H.CompareTo(other.H);
            }
            return -compare;
        }

    }

    public Vector3Reference target;
    PathfindingNodeManager manager;

    private Dictionary<Node, AStarNode> nodeToAStarNode;
    private LineRenderer lineRenderer;



    void Start() {
        manager = PathfindingNodeManager.INSTANCE;
    }

    public void AStarAndDrawPath() {
        Stopwatch sw = new Stopwatch();
        sw.Start();
        
        List<Node> path = AStar();

        sw.Stop();
        print("Path found in " + sw.ElapsedMilliseconds + " ms.");


        lineRenderer = GetComponent<LineRenderer>();
        int index = 1;
        lineRenderer.SetPosition(0, transform.position + Vector3.up);

        if(path != null) {
            lineRenderer.positionCount = path.Count + 2;
            foreach(Node node in path) {
                lineRenderer.SetPosition(index, (Vector3) path[index-1] + Vector3.up);
                index++;
            }
        }
        
        lineRenderer.SetPosition(index, target + Vector3.up);
    }

    void InitializeAStarDictionary() {
        int id = 0;
        nodeToAStarNode = new Dictionary<Node, AStarNode>();
        foreach(Node n in PathfindingNodeManager.INSTANCE.nodeList) {
            nodeToAStarNode[n] = new AStarNode(n, id);
            id++;
        }

    }

    List<Node> AStar() {
        InitializeAStarDictionary();

        AStarNode startNode = nodeToAStarNode[manager.ClosestNodeToPoint(transform.position)];
        AStarNode endNode = nodeToAStarNode[manager.ClosestNodeToPoint(target)];

        //Set targets for everybody
        foreach(Node node in manager.nodeList) {
            nodeToAStarNode[node].SetTarget(endNode);
        }

        AStarNode currentNode = startNode;

        Heap<AStarNode> open = new Heap<AStarNode>(manager.nodeList.Count);

        open.Add(currentNode);

        bool pathSolved = false;

        int safeCounter = 0;

        while(open.Count > 0) {
            
            safeCounter++;
            if(safeCounter > 9999) {
                UnityEngine.Debug.LogWarning("Timed out!");
                break;
            }

            //Find the lowest f in open, then set it to current node
            currentNode = open.RemoveFirst();
            

            //If current node is end...
            if(currentNode == endNode) {
                pathSolved = true;
                break;
            }

            //Loop through neighbors
            foreach(Node nodeNeighbor in currentNode.node.GetNeighbors()) {

                AStarNode neighbor = nodeToAStarNode[nodeNeighbor];
                
                //If neighbor is already explored... continue
                if(neighbor.IsExplored()) { continue; }
                
                //Old G cost
                float oldG = neighbor.G;

                //If neighbor is already in open...
                if(open.Contains(neighbor)) {
                    //And new g cost is less than old g cost...
                    float newG = Node.Distance(neighbor.node, currentNode.node);
                    if(newG < oldG) {
                        //Make this the new parent
                        neighbor.SetParent(currentNode); //This sets parent and sets g value
                        open.UpdateItem(neighbor);
                    }    
                } else { //otherwise
                    //Set this to parent and add neighbor to open
                    neighbor.SetParent(currentNode);
                    open.Add(neighbor);
                }

            }

            //Explores current node
            currentNode.CloseNode();

        }

        if(pathSolved) {
            List<Node> path = new List<Node>();

            AStarNode n = currentNode;
            while(n != startNode) {
                path.Add(n.node);
                n = n.GetParent();
            }
            path.Reverse();
            return path;
        }
        return null;
    }

}
