using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timekeeper : MonoBehaviour {
    
	public GameObject sun;
	
	// Use this for initialization
	void Start () {
		sun.transform.localPosition = new Vector3(System.Math.Min(Time.timeUnit, 100) - 50,
    		sun.transform.localPosition.y, sun.transform.localPosition.z);
	}
	
	// Update is called once per frame
	void Update () {}
}