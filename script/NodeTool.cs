using UnityEngine;
using UnityEditor;

// 추상 클래스
public abstract class NodeTool : ScriptableObject
{
	public Rect WindowSize;
	public NodeID thisid;
	// 이 노드는 연결된 녀석이 있는지 확인
	public bool hasLine = false;
	public NodeTool ConnectNode = null;		// 이걸로 Null인지 확인 가능
	public enum NodeID
	{
		A,
		B,
		C,
		DrawCurve
	};

	// 타이틀
	public string windowTitle;
	
	// 선 색상
	public Color lineColor = Color.black;

	// 가상 매서드
	public virtual void DrawWindow()
	{
		// 생성될 모든 노드들의 공통적으로 만들어지는 GUI
	}

	// 추상 메서드
	public abstract void Drawline();		// 상속받을때 무조건 만들어줘야 하는 함수

	public virtual void SetDrawData(NodeTool setData)
	{
		// 곡선이 그려져야 한다고 판단했을 때 각각 데이터 입력
	}
}
