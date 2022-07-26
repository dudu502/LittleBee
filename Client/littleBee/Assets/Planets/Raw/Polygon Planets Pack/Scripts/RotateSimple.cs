using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateSimple : MonoBehaviour {

	public bool randomizeAxisOnStart = false;
	public Vector3 axis;
	public float speed = 100;

	// Use this for initialization
	void Start()
	{
		if (randomizeAxisOnStart)
			RandomizeAxis();
	}

	[ContextMenu("Randomize")]
	public void RandomizeAxis()
	{
		axis = new Vector3(Random.Range(-100, 100), Random.Range(-100, 100), Random.Range(-100, 100)).normalized;
	}
	// Update is called once per frame
	void Update () 
	{
		this.transform.Rotate (axis.normalized * Time.deltaTime * speed);	
	}
}
