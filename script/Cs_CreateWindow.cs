using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEditor;

using Assets.Editor;	// DialogNode.cs
// 윈도우 창 생성
public class Cs_CreateWindow : EditorWindow {

	Vector2 mousePos;									// 마우스 위치
	List<NodeTool> Nodes = new List<NodeTool>();        // 노드 생성
	NodeTool SelectNode;                                // 선택한 노드

	private bool drawing = false;                       // 그려지고 있는 중
	private NodeTool Draw_SelectNode;

	public enum ContextID
	{
		A,B,C,

		DrawCurve
	};

	[MenuItem("Window/NodeTool")]
	static void Init()
	{
		Cs_CreateWindow window = EditorWindow.GetWindow<Cs_CreateWindow>();		// 창 생성
	}


	private void OnGUI()
	{
		Event e = Event.current;        // 현재 이벤트

		mousePos = e.mousePosition;     // 이벤트 위치 마우스 포인트

		if (e.button == 1 && e.type == EventType.MouseDown)
		{
			bool click = false;
			for (int i = 0; i < Nodes.Count; i++)
			{
				if (Nodes[i].WindowSize.Contains(mousePos))
				{
					// 현재 존재하는 노드들의 위에 마우스 포인터가 존재하는가?
					SelectNode = Nodes[i];
					click = true;
					break;
				}
			}

			if (!click)
			{
				// 노드가 클릭된 것이 아니라면
				GenericMenu menu = new GenericMenu();       // GenericMenu 생성

				menu.AddItem(new GUIContent("대사추가"), false, ContextCallback, ContextID.A);  // A를 Callback 함수에 넣어둠
				menu.AddItem(new GUIContent("분기점"), false, ContextCallback, ContextID.B);  // B를 Callback 함수에 넣어둠
				menu.AddItem(new GUIContent("디버그"), false, ContextCallback, ContextID.C);  // C를 Callback 함수에 넣어둠
				menu.AddItem(new GUIContent("선"), false, ContextCallback, ContextID.DrawCurve);  // C를 Callback 함수에 넣어둠
				menu.ShowAsContext();       // 메뉴를 보여줌
				e.Use();        // 이벤트 사용
			}
		} else if (e.button == 0 && e.type == EventType.MouseDown && !drawing)
		{

		} else if (e.button == 0 && e.type == EventType.MouseDown && drawing && Draw_SelectNode != null)
		{
			// 그려지고 있는 중에서 마우스 좌측 클릭시
			bool click = false;
			for (int i = 0; i < Nodes.Count; i++)
			{
				if (Nodes[i].WindowSize.Contains(mousePos))
				{
					// 현재 존재하는 노드들의 위에 마우스 포인터가 있고 우측을 눌렀나?
					SelectNode = Nodes[i];
					click = true;
					break;
				}
			}
			if (click)
			{
				// 클릭시
				Draw_SelectNode.SetDrawData(SelectNode);        // 연결할 노드 입력
				drawing = false;
			}
		}

		if (drawing && Draw_SelectNode != null)
		{
			Rect mouseRect = new Rect(e.mousePosition.x, e.mousePosition.y, 10, 10);
			DrawCurve(Draw_SelectNode.WindowSize, mouseRect, Color.black, 50.0f);
			Repaint();  // 계속해서 마우스가 움직일 때마다 커브를 그려야 하기 때문에 이걸 쓴다.
		}

		for (int i = 0; i < Nodes.Count; i++)
		{
			NodeTool drawCurve = Nodes[i];
			drawCurve.Drawline();		// 현재 연결되 있는 노드들의 전부를 그려주는 역활
		}

		// 그룹화
		BeginWindows();

		for(int i = 0; i < Nodes.Count; i++)
		{
			// 노드 리스트에 들어있는 창들 생성
			Nodes[i].WindowSize = GUI.Window(i, Nodes[i].WindowSize, DrawNodeWindow, Nodes[i].windowTitle);
		}

		EndWindows();
	}
	
	void DrawNodeWindow(int id)
	{
		Nodes[id].DrawWindow();

		GUI.DragWindow();
	}

	void ContextCallback(object obj)
	{
		ContextID id = (ContextID)obj;

		if(id.Equals(ContextID.A))
		{
			DialogNode dialogNode = new DialogNode();
			dialogNode.WindowSize = new Rect(mousePos.x, mousePos.y, dialogNode.WindowSize.width, dialogNode.WindowSize.height);
			Nodes.Add(dialogNode);		// 노드 리스트에 추가
			
			// 버튼 눌렀을 때
		}else if (id.Equals(ContextID.B))
		{

		}else if (id.Equals(ContextID.C))
		{

		}
		else if (id.Equals(ContextID.DrawCurve))
		{
			drawing = true;     // 그려지는중
			Draw_SelectNode = SelectNode;
		}
	}

	// 선 그리기
	public static void DrawCurve(Rect start, Rect end, Color color, float force)
	{
		Vector3 startPos = new Vector3(start.x + start.width, start.y, 0);				// 시작 위치
		Vector3 endPos = new Vector3(end.x + end.width / 2, end.y + end.height / 2, 0); // 마지막 위치
		Vector3 startTan = startPos + Vector3.right * force;
		Vector3 endTan = endPos + Vector3.left * force;

		// 3은 굵기
		Handles.DrawBezier(startPos, endPos, startTan, endTan, color, null, 3);
	}

	
}
