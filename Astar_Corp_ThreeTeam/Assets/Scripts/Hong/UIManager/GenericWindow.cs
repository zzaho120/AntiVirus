using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

// OnNextWindow로 이동할 때는 현재 창 닫고 다음 윈도우로 이동
// 다음 오브젝트.Open 하면 현재 창 열어둔 채로 다음 윈도우 Open

public class GenericWindow : MonoBehaviour
{
	public static WindowManager manager;
	public Windows nextWindow;
	public Windows previousWindow;

	public GameObject firstSelected;

	protected EventSystem eventSystem
	{
		get { return GameObject.Find("EventSystem").GetComponent<EventSystem>(); }
	}

	public virtual void OnFocus()
	{
		eventSystem.SetSelectedGameObject(firstSelected);
	}

	protected virtual void Display(bool value)
	{
		//Debug.Log("Display");
		gameObject.SetActive(value);
	}

	public virtual void Open()
	{
		Display(true);
		OnFocus();
	}

	public virtual void Close()
	{
		Display(false);
	}

	protected virtual void Awake()
	{
		Close();
	}

	public void OnNextWindow()
	{
		//Debug.Log("NextWindow Clicked");
		manager.Open((int)nextWindow - 1);
	}

	public void OnPreviousWindow()
	{
		manager.Open((int)previousWindow - 1);
	}
}
