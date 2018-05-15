// Only available in Unity 5.6 or higher
#if !(UNITY_4_3 || UNITY_4_4 || UNITY_4_5 || UNITY_4_6 || UNITY_5_0 || UNITY_5_1 || UNITY_5_2 || UNITY_5_3 || UNITY_5_4 || UNITY_5_5)
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.AI;
using DunGen.Editor;

namespace DunGen.Adapters
{
	[CustomEditor(typeof(UnityNavMeshAdapter))]
	public class UnityNavMeshAdapterInspector : UnityEditor.Editor
	{
#region Constants

		private static readonly GUIContent bakeModeLabel = new GUIContent("Runtime Bake Mode", "Determine what to do as the runtime baking process");
		private static readonly GUIContent addNavMeshLinksBetweenRoomsLabel = new GUIContent("Link Rooms", "If checked, NavMeshLinks will be formed to connect rooms in the dungeon");
		private static readonly GUIContent navMeshAgentTypesLabel = new GUIContent("Agent Types Link Info", "Per-agent information about how to create NavMeshLinks between rooms");
		private static readonly GUIContent navMeshLinkDistanceFromDoorwayLabel = new GUIContent("Distance from Doorway", "The distance on either side of each doorway that the NavMeshLink positions will be placed");
		private static readonly GUIContent disableLinkWhenDoorIsClosedLabel = new GUIContent("Disable When Door is Closed", "If true, the link will only be active when the corresponding door is open");

#endregion

		private SerializedProperty priorityProp;
		private SerializedProperty bakeModeProp;
		private SerializedProperty addNavMeshLinksBetweenRoomsProp;
		private SerializedProperty navMeshAgentTypesProp;
		private SerializedProperty navMeshLinkDistanceFromDoorwayProp;


		private void OnEnable()
		{
			priorityProp = serializedObject.FindProperty("Priority");
			bakeModeProp = serializedObject.FindProperty("BakeMode");
			addNavMeshLinksBetweenRoomsProp = serializedObject.FindProperty("AddNavMeshLinksBetweenRooms");
			navMeshAgentTypesProp = serializedObject.FindProperty("NavMeshAgentTypes");
			navMeshLinkDistanceFromDoorwayProp = serializedObject.FindProperty("NavMeshLinkDistanceFromDoorway");
		}

		public override void OnInspectorGUI()
		{
			var data = target as UnityNavMeshAdapter;
			if (data == null)
				return;

			serializedObject.Update();


			EditorGUILayout.Space();
			EditorGUILayout.PropertyField(priorityProp, InspectorConstants.AdapterPriorityLabel);
			EditorGUILayout.PropertyField(bakeModeProp, bakeModeLabel);
			EditorGUILayout.Space();

			DrawLinksGUI();

			serializedObject.ApplyModifiedProperties();
		}

		private void DrawLinksGUI()
		{
			addNavMeshLinksBetweenRoomsProp.isExpanded = EditorGUILayout.Foldout(addNavMeshLinksBetweenRoomsProp.isExpanded, "Room Links");

			if (addNavMeshLinksBetweenRoomsProp.isExpanded)
			{
				EditorGUILayout.BeginVertical("box");
				EditorGUI.indentLevel++;

				EditorGUILayout.PropertyField(addNavMeshLinksBetweenRoomsProp, addNavMeshLinksBetweenRoomsLabel);

				using (new EditorGUI.DisabledScope(!addNavMeshLinksBetweenRoomsProp.boolValue))
				{
					EditorGUILayout.PropertyField(navMeshLinkDistanceFromDoorwayProp, navMeshLinkDistanceFromDoorwayLabel);

					EditorGUILayout.Space();
					EditorGUILayout.Space();

					EditorGUILayout.BeginVertical("box");
					EditorGUILayout.BeginHorizontal();

					EditorGUILayout.LabelField(navMeshAgentTypesLabel);

					if (GUILayout.Button("Add New"))
						navMeshAgentTypesProp.InsertArrayElementAtIndex(navMeshAgentTypesProp.arraySize);

					EditorGUILayout.EndHorizontal();

					int indexToRemove = -1;
					for (int i = 0; i < navMeshAgentTypesProp.arraySize; i++)
					{
						EditorGUILayout.BeginVertical("box");

						if (GUILayout.Button("x", EditorStyles.miniButton, GUILayout.Width(18)))
							indexToRemove = i;

						var elementProp = navMeshAgentTypesProp.GetArrayElementAtIndex(i);
						var agentTypeID = elementProp.FindPropertyRelative("AgentTypeID");
						var areaTypeID = elementProp.FindPropertyRelative("AreaTypeID");
						var disableWhenDoorIsClosed = elementProp.FindPropertyRelative("DisableLinkWhenDoorIsClosed");

						NavMeshComponentsGUIUtility.AgentTypePopup("Agent Type", agentTypeID);
						NavMeshComponentsGUIUtility.AreaPopup("Area", areaTypeID);
						EditorGUILayout.PropertyField(disableWhenDoorIsClosed, disableLinkWhenDoorIsClosedLabel);

						EditorGUILayout.EndVertical();
					}

					EditorGUILayout.EndVertical();

					if (indexToRemove >= 0 && indexToRemove < navMeshAgentTypesProp.arraySize)
						navMeshAgentTypesProp.DeleteArrayElementAtIndex(indexToRemove);
				}

				EditorGUI.indentLevel--;
				EditorGUILayout.EndVertical();
			}
		}
	}
}
#endif