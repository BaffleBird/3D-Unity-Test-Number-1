using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextUpdate : MonoBehaviour
{
	private static TextUpdate _instance;
	public static TextUpdate Instance { get { return _instance; } }

	private void Awake()
	{
		if (_instance != null && _instance != this)
		{
			Destroy(this.gameObject);
		}
		else
		{
			_instance = this;
		}
	}

	TextMeshProUGUI textMesh;
    void Start()
    {
		textMesh = GetComponent<TextMeshProUGUI>();
    }

	public void setText(string newText)
	{
		textMesh.text = newText;
	}
}
