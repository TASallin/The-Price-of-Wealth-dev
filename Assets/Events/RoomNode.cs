using UnityEngine;
using UnityEngine.UI;

public class RoomNode : MonoBehaviour {
	
	public bool cleared;
	public string ID;
	public string description;
	public Event contents;
	public Vector3 position;
    
	public void OnClick () {
		//Empty contents to the Dungeon class and run the event
		Clear();
		gameObject.transform.parent.gameObject.GetComponent<MapViewer>().graph.SetPosition(ID);
		gameObject.transform.parent.gameObject.GetComponent<MapViewer>().RunEvent(ID);
		//if (contents == null) {
		//	Clear();
		//	gameObject.transform.parent.gameObject.GetComponent<MapViewer>().UpdateMap();
		//} else {
		//	gameObject.transform.parent.gameObject.GetComponent<MapViewer>().eventCatcher.consequence = contents;
		//	gameObject.transform.parent.gameObject.GetComponent<MapViewer>().eventCatcher.Next();
		//}
	}
	
	public void Clear () {
		gameObject.transform.parent.gameObject.GetComponent<MapViewer>().graph.cleared[ID] = true;
	}
	
	public void FullClear () {
		cleared = true;
		contents = null;
	}
	
	public void Hover () {
		gameObject.transform.parent.gameObject.GetComponent<MapViewer>().mapText.text = description;
	}
	
	public void ExitHover () {
		gameObject.transform.parent.gameObject.GetComponent<MapViewer>().mapText.text = "";
	}
	
}