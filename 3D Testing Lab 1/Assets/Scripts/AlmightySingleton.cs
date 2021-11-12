using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlmightySingleton : MonoBehaviour
{
    private static AlmightySingleton _instance;
    public static AlmightySingleton Instance { get { return _instance; } }

    // Start is called before the first frame update
    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }

	void Start()
	{
	}

	// Update is called once per frame
	void Update()
    {
    }

	private void FixedUpdate()
	{
    }
}
