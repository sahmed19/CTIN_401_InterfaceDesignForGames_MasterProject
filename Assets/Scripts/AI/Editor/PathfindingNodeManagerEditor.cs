using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor(typeof(PathfindingNodeManager))]
public class PathfindingNodeManagerEditor : Editor
{
    
    private ReorderableList list;

	public static int selectedNodeIndex = -1;

	int selectedIndexLastFrame = -1;

	int x = 9, y = 9;
	float spacing = 20;

	bool displayAllNeighbors = false;

	bool listFolded = true;

    private void OnEnable() {
		list = new ReorderableList (serializedObject,
			serializedObject.FindProperty ("nodePositions"),
			true, true, true, true);
	
		list.drawElementCallback = 
			(Rect rect, int index, bool isActive, bool isFocused) => {
			var element = list.serializedProperty.GetArrayElementAtIndex (index);
			rect.y += 2;
			EditorGUI.PropertyField (
				new Rect (rect.x, rect.y, rect.width - 30, EditorGUIUtility.singleLineHeight),
				element, new GUIContent("Node " + index));
		};

		list.onAddCallback = (ReorderableList l) => {
			GUI.changed = true;
			ReorderableList.defaultBehaviours.DoAddButton(l);
		};
		list.onRemoveCallback = (ReorderableList l) => {
			GUI.changed = true;
			ReorderableList.defaultBehaviours.DoRemoveButton(l);
		};

	}

	void DrawLineWithHeader(Vector3 position1, Vector3 position2, float distance) {
			Vector3 centerPosition = (position1 + position2) / 2f;
			float betweenDistance = Mathf.Round(distance*10f)/10f;
			Handles.DrawLine(position1, position2);

			GUIStyle style = new GUIStyle();
			style.normal.textColor = Color.white;
			Handles.Label(centerPosition + Vector3.up, betweenDistance + " m", style);
	}

    protected virtual void OnSceneGUI() {
		PathfindingNodeManager nodeManager = (PathfindingNodeManager)target;

		//Draw positions
		if(nodeManager.nodePositions != null) {
			for(int i = 0; i < nodeManager.nodePositions.Count; i++) {

				Vector3 nodePosition = nodeManager.nodePositions[i];
				
				if(i == selectedNodeIndex) {

					Handles.color = Color.cyan;
					Handles.DrawWireCube(nodePosition + nodeManager.offset, Vector3.one * 2f);
					Handles.DrawWireCube(nodePosition + nodeManager.offset, Vector3.one * nodeManager.range * 2f);
					EditorGUI.BeginChangeCheck ();
					
					Vector3 newtargetPos = Handles.PositionHandle (nodePosition + nodeManager.offset, Quaternion.identity);
					
					if (EditorGUI.EndChangeCheck ()) {
						Undo.RecordObject (nodeManager, "changed node position");
						nodeManager.nodePositions [i] = newtargetPos - nodeManager.offset;
					}

				} else {
					Handles.color = Color.blue;
					if(Handles.Button(nodePosition + nodeManager.offset, Quaternion.LookRotation(Vector3.up), 1f, 2f, Handles.CircleHandleCap)) {
						selectedNodeIndex = i;
						Repaint();
					}
				}
			}
		}

		//Draw Connections
		if(nodeManager.nodeList != null) {
			
			//Draw connections
			if(displayAllNeighbors) {
				for(int i = 0; i < nodeManager.nodeList.Count; i++) {
					
					if(i == selectedNodeIndex) {continue;}

					Node node = nodeManager.nodeList[i];

					//Draw connections
					foreach(Node neighbor in node.GetNeighbors()) {
						Handles.color = Color.grey;
						Handles.DrawDottedLine((Vector3) node + nodeManager.offset, (Vector3) neighbor + nodeManager.offset, 10.0f);
					}

				}
			}

			//Draw selected node connections
			if(selectedNodeIndex > -1 && selectedNodeIndex < nodeManager.nodeList.Count) {
				Node selectedNode = nodeManager.nodeList[selectedNodeIndex];
				//Draw connections
				foreach(Node neighbor in selectedNode.GetNeighbors()) {
					Handles.color = Color.cyan;
					DrawLineWithHeader((Vector3) selectedNode + nodeManager.offset, (Vector3) neighbor + nodeManager.offset, Node.SqrDistance(selectedNode, neighbor));
				}
			}
		}
		
	}

	public override void OnInspectorGUI() {

		EditorGUILayout.LabelField (
			"PATHFINDING NODES - " +
			"Use this for moving platforms and such.");

		serializedObject.Update ();

		PathfindingNodeManager nodeManager = (PathfindingNodeManager)target;

		EditorGUILayout.LabelField(
			"NODES - "+
			"Vector3's for positions.");


		listFolded = EditorGUILayout.Foldout(listFolded, "List of Nodes");


		EditorGUI.BeginChangeCheck();
		if(listFolded) {
			list.DoLayoutList ();
		}
		if(EditorGUI.EndChangeCheck()) {
			Debug.Log("Changed!");
		}
		
		if(list.index != selectedIndexLastFrame) {
			selectedIndexLastFrame = list.index;
			selectedNodeIndex = list.index;
		} else if(selectedNodeIndex != list.index) {
			list.index = selectedNodeIndex;
			selectedIndexLastFrame = list.index;
		}

		x = EditorGUILayout.IntField("Grid Dimension X: ", x);
		y = EditorGUILayout.IntField("Grid Dimension Y: ", y);
		spacing = EditorGUILayout.FloatField("Spacing: ", spacing);

		if(GUILayout.Button("Grid")) {
			nodeManager.PopulateGrid(x, y, spacing);
		}

		nodeManager.range = EditorGUILayout.FloatField("Neighbor Detection Range: ", nodeManager.range);
		
		if(GUILayout.Button("Clear Nodes")) {
            nodeManager.DeleteNodesAndConnections();
        }

		if(GUILayout.Button("Generate Nodes and Make Connections")) {
			Debug.Log("Making Connections!");
            nodeManager.GenerateNodesAndNodeConnections();
        }
		
		serializedObject.ApplyModifiedProperties ();
	}

}
