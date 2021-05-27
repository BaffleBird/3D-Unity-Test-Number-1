using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class TextUpdate : MonoBehaviour
{
	private static TextUpdate _instance;
	public static TextUpdate Instance { get { return _instance; } }

	private Dictionary<string, string> textDictionary = new Dictionary<string, string>();

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

	public void SetText(string textID, string newText)
	{
		if (!textDictionary.ContainsKey(textID))
			textDictionary.Add(textID, newText);
		else
			textDictionary[textID] = newText;
		UpdateTextDisplay();
	}

	private void UpdateTextDisplay()
	{
		textMesh.text = "";
		foreach (string key in textDictionary.Keys.ToList())
		{
			textMesh.text += key + ": " + textDictionary[key] + "\n";
		}
	}
}
