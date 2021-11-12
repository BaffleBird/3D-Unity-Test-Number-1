using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceDissolveTarget : MonoBehaviour
{
    public Transform trackingObject;
	private Renderer _renderer;
	private Material _material;

	public Renderer Renderer
	{
		get
		{
			if (_renderer == null)
				_renderer = GetComponent<Renderer>();
			return _renderer;
		}
	}

	public Material MaterialRef
	{
		get
		{
			if (_material == null)
				_material = Renderer.material;
			return _material;
		}
	}

	private void Awake()
	{
		_renderer = GetComponent<Renderer>();
		_material = _renderer.material;
	}

	private void Update()
	{
		if (trackingObject)
		{
			MaterialRef.SetVector("Position", trackingObject.position);
		}
	}
}
