using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
//TODO: 곡 저장하는 클래스
public class Sound
{
	//TODO: 변수 선언
	[Header("음악 닉네임")]
	[Tooltip("Mp3 파일 닉네임을 설정합니다. 나중에 음악 재생이 필요할 때 이 닉네임을 사용해서 작동시킨다.")]
	public string name;         //? Mp3 파일 닉네임 (이걸로 음악 실행)

	[Header("Mp3_File")]
	[Tooltip("Mp3 파일을 담는 곡입니다.")]
	public AudioClip clip;      //? Mp3 File 을 집어넣습니다.

}


public class SoundManager : MonoBehaviour {

	//TODO: 사운드 매니져 인스턴스
	static public SoundManager _instance;    //! 싱글턴화 시킨다. (공유자원)

	#region 싱글턴
	private void Awake()
	{
		if (_instance == null)
		{
			_instance = this;
			DontDestroyOnLoad(gameObject);      // 씬이 로드 될 때 자동으로 파괴되지 않는 오브젝트

		}
		else
			Destroy(this.gameObject);           // 연결되 있는 오브젝트를 삭제한다. ( SoundManager 2개가 될경우 발동)
	}
	#endregion

	//TODO: 전역변수 설정
	[SerializeField]
	[Header("배경음악_AudioSource Component")]
	[Tooltip("배경음악을 전용으로 하는 AudioSource Component를 넣어주는 변수")]
	private AudioSource audioSource_BGM;    //! 배경음악 전용 컴퍼넌트

	[SerializeField]
	[Header("효과음_AudioSource Component")]
	[Tooltip("효과음을 전용으로 하는 AudioSource Component를 넣어주는 변수")]
	private AudioSource[] audioSource_SE;       //! 효과음 전용 컴퍼넌트

	[SerializeField]
	[Header("재생중인 배경음")]
	[Tooltip("재생중인 배경음 (오디오 컴퍼넌트 만큼 배열을 맞게 설정해 줘야한다.")]
	public string play_bgmName;

	[SerializeField]
	[Header("재생중인 효과음")]
	[Tooltip("재생중인 효과음 (오디오 컴퍼넌트 만큼 배열을 맞게 설정해 줘야한다.)")]
	public string[] play_seName;

	[Header("배경음악 파일")]
	[Tooltip("배경음악에 필요한 MP3 파일을 가져온다.")]
	public Sound[] bgm_file;

	[Header("효과음 파일")]
	[Tooltip("효과음에 필요한 MP3 파일을 가져온다.")]
	public Sound[] se_file;


	[HideInInspector]
	public int toolbarTab;                  // 현재 탭
	public string currentTab;
	
	//TODO 사용방법
	//? 다른 C# 스크립트로 가서 string 변수로 음악 닉네임을 적은 다음
	//!? StartCorutin(SoundManager.instance.메서드이름(변수); 하면 된다.


	/// <summary>
	/// SoundManager.cs File 에 포함된 함수로써
	/// 효과음을 실행하기 위해 만들어진 메서드 이다.
	/// _name은 play_seName 변수와 비교하여 실행 하도록 한다.
	/// </summary>
	/// <param name="_name"></param>
	/// <returns></returns>
	public IEnumerator Play_SE(string _name)
	{
		//Todo: Mp3 파일 검색
		for(int i = 0; i < se_file.Length; i++)
		{ 
			//Todo: 닉네임이 등록된 MP3 파일의 닉네임과 동일 할 때
			if (_name.ToLower().Trim().Equals(se_file[i].name))
			{
				//!? Debug
				ConsoleProDebug.Watch("SE Equals", _name.ToLower().Trim().Equals(se_file[i].name) +"");

				//Todo: 재생중이지 않은 오디오 소스 찾기
				for (int j = 0; j < audioSource_SE.Length; j++)
				{
					//Todo: 사용중이지 않는 오디오 소스를 발견할 경우
					if(!audioSource_SE[j].isPlaying)
					{
						play_seName[j] = se_file[i].name;				//! 재생중인 효과음 이름을 등록
						audioSource_SE[j].clip = se_file[i].clip;       //! 클립 등록
						audioSource_SE[j].Play();                       //! 오디오 재생

						//!? Debug
						ConsoleProDebug.Watch("SE Play", se_file[i].name);

						yield break;	//! 함수 자체 종료
					}
				}

				//Todo: 만약 재생중이지 않은 오디오 소스가 없을 경우
				//!? Debug
				ConsoleProDebug.Watch("SE Play", se_file[i].name);
				yield break; //! 중지
			}
		} 
	}
	


}
