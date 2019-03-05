using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
namespace Assets.Editor
{
	public class DialogNode : NodeTool
	{
		public string SceneNumber = "";
		public string Dialog = "";

		public DialogNode()
		{
			windowTitle = "Node Window";				// 창 이름
			thisid = NodeID.A;							// 현재 아이디
			WindowSize = new Rect(0, 0, 100, 40);		// 창 크기
		}

		public override void DrawWindow()
		{
			base.DrawWindow();                          // 기본적으로 생성되어야 할 GUI 생성
			GUILayout.Label("#" + SceneNumber);			// Label GUI 생성
		}

		public override void Drawline()
		{
			if(ConnectNode && hasLine)
			{
				// 연결된 노드가 있으며, 앞으로 연결 하는 것이 true 일 경우
				Rect rect = WindowSize;
				rect.x += WindowSize.width;
				rect.width = 1;
				rect.height = 1;
				// 연결될 노드의 크기와 좌표들을 받아온다.
				Rect connect = ConnectNode.WindowSize;
				connect.width = 1;
				connect.height = 1;
				Cs_CreateWindow.DrawCurve(rect, connect, lineColor, 50.0f);
				
			}
		}

		public override void SetDrawData(NodeTool setData)
		{
			hasLine = true;
			ConnectNode = setData;
		}

	}
}
