using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class btnScript : MonoBehaviour {

    private Button button;
	// Use this for initialization
	void Start () {
        button = gameObject.GetComponent<Button>();
        button.onClick.AddListener(TaskOnClick);
        CursorLockMode locked = CursorLockMode.None;
        Cursor.visible = true;

	}

    public void Awake()
    {
        CursorLockMode locked = CursorLockMode.None;
        Cursor.visible = true;
    }

    void TaskOnClick()
    {
        System.Console.WriteLine("IT IS CLICKED");
    }
	
	
}
