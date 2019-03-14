using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class MapViewer : MonoBehaviour {
	
	public DungeonMap graph;
	public Dungeon eventCatcher;
	//public Dictionary<string, Event> eventMap;
	public string location;
	public Event current;
	public GameObject edgeLine;
	public Transform edgeSpace;
	public Text mapText;
	
	void Start () {
		UpdateMap();
	}
	
	void OnEnable() {
		if (graph != null) {
			//Debug.Log("here");
		    UpdateMap();
		} else {
			CallGraph();
		}
	}
	
	void CallGraph() {
		graph = DungeonMapData.data[location];
		//eventMap = DungeonMapData.lectureEvents;
		//eventCatcher = gameObject.transform.parent.parent.GetComponent<Dungeon>();
		graph.unityView = this;
		
		foreach (Transform child in gameObject.transform) {
			AdjacencyList edges = graph.edges[child.gameObject.GetComponent<RoomNode>().ID];
			string fromKey = edges.self;
			Vector3 fromPos = gameObject.transform.Find(fromKey).position;
			foreach (string toKey in edges.destinations) {
				Vector3 toPos = gameObject.transform.Find(toKey).position;
				GameObject line = Instantiate(edgeLine, edgeSpace);
				line.transform.position = new Vector3((fromPos.x + toPos.x) / 2, (fromPos.y + toPos.y) / 2, 0);
				line.transform.localScale = new Vector3(1, (float) System.Math.Sqrt(System.Math.Pow(fromPos.x - toPos.x, 2) 
				+ System.Math.Pow(fromPos.y - toPos.y, 2) + System.Math.Pow(fromPos.z - toPos.z, 2)) / 150, 1);
			    //Debug.Log(line.transform.localScale.ToString());
				if (toPos.x - fromPos.x != 0 && toPos.y - fromPos.y != 0) {
                    line.transform.Rotate(new Vector3(0, 0, (float) (-1 * (90 - System.Math.Atan((toPos.y - fromPos.y) / (toPos.x - fromPos.x)) * 180 / 3.14))));
					//Debug.Log(System.Math.Acos((toPos.x - fromPos.x) / (toPos.y - fromPos.y)).ToString());
				} else if (toPos.y - fromPos.y == 0) {
					line.transform.Rotate(new Vector3(0, 0, 90));
				}
				//line.transform.SetAsFirstSibling();
			}
		}
	}
	
	public bool IsCleared(string node) {
	    //return false;
		return gameObject.transform.Find(node).gameObject.GetComponent<RoomNode>().cleared;
	}
	
	public void UpdateMap () {
		foreach (Transform c in gameObject.transform) {
			c.Find("Button").gameObject.GetComponent<Button>().interactable = false;
		}
		List<string> available = graph.GetReachable();
		foreach (string s in available) {
			if (graph.cleared[s]) {
                gameObject.transform.Find(s).Find("Image").gameObject.GetComponent<Image>().color = Color.green;
			} else {
				gameObject.transform.Find(s).Find("Image").gameObject.GetComponent<Image>().color = Color.red;
			}
			gameObject.transform.Find(graph.position).Find("Image").gameObject.GetComponent<Image>().color = Color.yellow;
			gameObject.transform.Find(s).Find("Button").gameObject.GetComponent<Button>().interactable = true;
		}
	}
	
	public void RunEvent(string id) {
		//Event e = eventMap[id];
		current = graph.eventMap[id];
		if (current != null) {
			if (id.Contains("Boss")) {
				Areas.cleared[location] = true;
				Time.Increment(5);
			}
		    eventCatcher.RunEvent(current);
		    //Temporary solution
		    graph.eventMap[id] = null;
		} else {
			UpdateMap();
		}
	}
	
	public void Escape() {
		graph.eventMap[graph.position] = current;
		graph.cleared[graph.position] = false;
		if (graph.position.Contains("Boss")) {
			Areas.cleared[location] = false;
			Time.Increment(-5);
		}
		graph.position = graph.previousPosition;
		UpdateMap();
	}
	
	public void EscapeEnemies(Character[] enemies) {
		foreach (Character c in enemies) {
			if (c != null) {
				if (c.GetAlive()) {
					c.PostBattle();
					c.Heal(100);
					c.drops = new Item[0];
				}
			}
		}
		if (graph == null) {
			CallGraph();
		}
		//Debug.Log(graph.position);
		graph.eventMap[graph.position] = new BattleEvent(enemies, "Another Try");
		graph.cleared[graph.position] = false;
		if (graph.position.Contains("Boss")) {
			Areas.cleared[location] = false;
			graph.eventMap[graph.position] = current;
		}
		graph.position = graph.previousPosition;
		UpdateMap();
	}
	
}