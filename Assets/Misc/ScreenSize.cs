using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class ScreenSize : MonoBehaviour {
    
	public Toggle s;
	
    // Start is called before the first frame update
    void Start() {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	public void Set() {
	    Screen.fullScreen = s.isOn;
	}
}
