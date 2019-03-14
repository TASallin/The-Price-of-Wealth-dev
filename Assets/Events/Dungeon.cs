using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class Dungeon : MonoBehaviour {

	public Event current;
	public Event consequence;
	public GameObject eventSpace;
	public EventSpace e;
	public GameObject nextMenu;
	public GameObject partyMenu;
	public GameObject itemMenu;
	public GameObject bagMenu;
	public GameObject useItemMenu;
	public GameObject nameMenu;
	public GameObject dungeonMaps;
	public GameObject replaceMember;
	public bool isEvent;
	public bool usingSpecial;
    public int y;
	public int partyIndex;
	public static bool fled;
	public static Character[] leftEnemies;
	public static Event investigated;
	public static bool toOverworld;
	Type t;
	MethodInfo method;
	public Queue<TimedMethod> effects1;
	public Queue<TimedMethod> effects2;
	public Queue<TimedMethod> effects3;
	public Queue<TimedMethod> effects4;
	
	// Use this for initialization
	void Start () {
		if (fled && Party.area != "overworld") {
			fled = false;
			EscapeEnemies();
		}
		effects1 = new Queue<TimedMethod>();
		effects2 = new Queue<TimedMethod>();
		effects3 = new Queue<TimedMethod>();
		effects4 = new Queue<TimedMethod>();
		t = this.GetType();
		if (Areas.followUp != null) {
			consequence = Areas.followUp;
			Areas.followUp = null;
			Next();
		}
		string loc = Areas.location;
		foreach (Transform current in dungeonMaps.gameObject.transform) {
			if (current.gameObject.name == loc || current.gameObject.name == "EdgeSpace") {
				current.gameObject.SetActive(true);
			} else {
				current.gameObject.SetActive(false);
			}
		}
		if (toOverworld) {
			toOverworld = false;
			SceneManager.LoadScene("Overworld");
		}
		if (investigated != null) {
			RunEvent(investigated);
			investigated = null;
			toOverworld = true;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public void RunEvent(Event current) {
		consequence = current;
		Next();
	}
	
	public void Next () {
		y = 0;
		//if (Areas.tower.Count == 0) {
			//SceneManager.LoadScene("Overworld");
			//return;
		//}
		if (consequence == null) {
		    return;
		} else {
			current = consequence;
			consequence = null;
		}
		current.Enact();
		e.text.text = current.text;
		effects1 = new Queue<TimedMethod>();
		effects2 = new Queue<TimedMethod>();
		effects3 = new Queue<TimedMethod>();
		effects4 = new Queue<TimedMethod>();
		if (current.options1 != null) {
			foreach (TimedMethod t in current.options1) {
			    effects1.Enqueue(t);
			}
			e.option1.GetComponentInChildren<Text>().text = current.optionText1;
			e.option1.gameObject.SetActive(true);
		} else {
			e.option1.gameObject.SetActive(false);
		}
		
		if (current.options2 != null) {
			foreach (TimedMethod t in current.options2) {
			    effects2.Enqueue(t);
			}
			e.option2.GetComponentInChildren<Text>().text = current.optionText2;
			e.option2.gameObject.SetActive(true);
		} else {
			e.option2.gameObject.SetActive(false);
		}
		
		if (current.options3 != null) {
			foreach (TimedMethod t in current.options3) {
			    effects3.Enqueue(t);
			}
			e.option3.GetComponentInChildren<Text>().text = current.optionText3;
			e.option3.gameObject.SetActive(true);
		} else {
			e.option3.gameObject.SetActive(false);
		}
		
		if (current.options4 != null) {
			foreach (TimedMethod t in current.options4) {
			    effects4.Enqueue(t);
			}
			e.option4.GetComponentInChildren<Text>().text = current.optionText4;
			e.option4.gameObject.SetActive(true);
		} else {
			e.option4.gameObject.SetActive(false);
		}
		
		//nextMenu.SetActive(false);
		try {
		nextMenu.transform.Find("Next").gameObject.GetComponent<Button>().interactable = false;
		nextMenu.transform.Find("Exit").gameObject.GetComponent<Button>().interactable = false;
		} catch {};
		dungeonMaps.SetActive(false);
		isEvent = true;
		eventSpace.SetActive(true);
	}
	
	public void Resolve () {
		//nextMenu.SetActive(true);
		nextMenu.transform.Find("Next").gameObject.GetComponent<Button>().interactable = true;
		nextMenu.transform.Find("Exit").gameObject.GetComponent<Button>().interactable = true;
		dungeonMaps.SetActive(true);
		isEvent = false;
		eventSpace.SetActive(false);
		if (toOverworld) {
			toOverworld = false;
			SceneManager.LoadScene("Overworld");
		}
	}
	
	public void Escape () {
		nextMenu.transform.Find("Next").gameObject.GetComponent<Button>().interactable = true;
		nextMenu.transform.Find("Exit").gameObject.GetComponent<Button>().interactable = true;
		dungeonMaps.SetActive(true);
		isEvent = false;
		eventSpace.SetActive(false);
		if (toOverworld) {
			toOverworld = false;
			SceneManager.LoadScene("Overworld");
			return;
		}
		dungeonMaps.transform.Find(Areas.location).GetComponent<MapViewer>().Escape();
	}
	
	public void EscapeEnemies () {
		nextMenu.transform.Find("Next").gameObject.GetComponent<Button>().interactable = true;
		nextMenu.transform.Find("Exit").gameObject.GetComponent<Button>().interactable = true;
		dungeonMaps.SetActive(true);
		isEvent = false;
		eventSpace.SetActive(false);
		if (toOverworld) {
			toOverworld = false;
			SceneManager.LoadScene("Overworld");
			return;
		}
		dungeonMaps.transform.Find(Areas.location).GetComponent<MapViewer>().EscapeEnemies(leftEnemies);
		//leftEnemies = null;
	}
	
	public void OpenParty() {
	    nextMenu.SetActive(false);
		eventSpace.SetActive(false);
        partyMenu.SetActive(true);		
		dungeonMaps.SetActive(false);
	}
	
	public void CloseParty() {
		partyMenu.SetActive(false);
		nextMenu.SetActive(true);
		if (isEvent) {
			eventSpace.SetActive(true);
		} else {
			dungeonMaps.SetActive(true);
		}
	}
	
	public void click1() {
	    foreach (TimedMethod current in effects1) {
			method = t.GetMethod(current.method);
			if (current.args == null) {
				method.Invoke(this, null);
			} else {
				method.Invoke(this, current.args);
			}	
		}
	}
	
	public void click2() {
	    foreach (TimedMethod current in effects2) {
			method = t.GetMethod(current.method);
			if (current.args == null) {
				method.Invoke(this, null);
			} else {
				method.Invoke(this, current.args);
			}	
		}
	}
	
	public void click3() {
	    foreach (TimedMethod current in effects3) {
			method = t.GetMethod(current.method);
			if (current.args == null) {
				method.Invoke(this, null);
			} else {
				method.Invoke(this, current.args);
			}	
		}
	}
	
	public void click4() {
	    foreach (TimedMethod current in effects4) {
			method = t.GetMethod(current.method);
			if (current.args == null) {
				method.Invoke(this, null);
			} else {
				method.Invoke(this, current.args);
			}	
		}
	}
	
	public void CauseEvent (Event e) {
		consequence = e;
		Next();
	}
	
	public void NextEvent (Event e) {
		Areas.followUp = e;
		//Resolve();
	}
	
	
	public void Battle (Character [] enemies) {
		foreach (Character c in enemies) {
			if (c != null) {
			    Party.AddEnemy(c);
			}
	    }
		Party.area = "Dungeon";
		isEvent = false;
		SceneManager.LoadScene("Battle");
	}
	
	public void Item (Item[] loot) {
		foreach (Item current in loot) {
			Party.AddLoot(current);
		}
		itemMenu.SetActive(true);
		eventSpace.SetActive(false);
		nextMenu.SetActive(false);
	}
	
	public void LoseItems() {
		Party.SetItems(new Item[10]);
	}
	
	public void LoseItem(int index) {
		Party.GetItems()[index] = null;
	}
	
	public void CloseItem () {
		itemMenu.SetActive(false);
		nextMenu.SetActive(true);
		Resolve();
	}
	
	public void OpenBag () {
		bagMenu.SetActive(true);
		nextMenu.SetActive(false);
		eventSpace.SetActive(false);
		dungeonMaps.SetActive(false);
	}
	
	public void CloseBag () {
		bagMenu.SetActive(false);
		nextMenu.SetActive(true);
		if (isEvent) {
			eventSpace.SetActive(true);
		} else {
			dungeonMaps.SetActive(true);
		}
	}
	
	public void UseItem (Item item) {
		bagMenu.SetActive(false);
		useItemMenu.SetActive(true);
		useItemMenu.GetComponent<PartyMenu>().item = item;
	}
	
	public void CancelItemUse () {
		Party.AddItem(useItemMenu.GetComponent<PartyMenu>().item);
		if (!usingSpecial) {
		    bagMenu.SetActive(true);
		} else {
			usingSpecial = false;
			nextMenu.SetActive(true);
			if (isEvent) {
			eventSpace.SetActive(true);
		    } else {
			dungeonMaps.SetActive(true);
		    }
		}
		useItemMenu.SetActive(false);
		useItemMenu.GetComponent<PartyMenu>().item = null;
	}
	
	/*
	public void ConfirmItemUse () {
		useItemMenu.SetActive(false);
		nextMenu.SetActive(true);
		useItemMenu.GetComponent<PartyMenu>().item = null;
		if (isEvent) {
			eventSpace.SetActive(true);
		} else {
			dungeonMaps.SetActive(true);
		}
	}
	*/
	
	public void ConfirmItemUse () {
		if (usingSpecial) {
			usingSpecial = false;
			SpecialSelects();
			return;
		}
		Item item = useItemMenu.GetComponent<PartyMenu>().item;
		item.UseOutOfCombat(useItemMenu.GetComponent<PartyMenu>().ConfirmCharacter());
		useItemMenu.SetActive(false);
		nextMenu.SetActive(true);
		useItemMenu.GetComponent<PartyMenu>().item = null;
		if (isEvent) {
			eventSpace.SetActive(true);
		} else {
			dungeonMaps.SetActive(true);
		}
	}
	
	public void OpenName () {
		Party.latestRecruit = partyMenu.GetComponent<PartyMenu>().GetActive();
		nameMenu.transform.Find("Name").gameObject.GetComponent<InputField>().text = Party.latestRecruit.GetName();
		nameMenu.SetActive(true);
		partyMenu.SetActive(false);
	}
	
	public void Name () {
		Party.latestRecruit.SetName(nameMenu.transform.Find("Name").gameObject.GetComponent<InputField>().text);
		Party.latestRecruit = null;
		nextMenu.SetActive(true);
		nameMenu.SetActive(false);
		if (isEvent) {
			eventSpace.SetActive(true);
		} else {
			dungeonMaps.SetActive(true);
		}
	}
	
	public void Poison (int amount) {
		for (int i = 0; i < 4; i++) {
			Character current = Party.GetCharacter(i);
			if (current != null) {
				current.status.Poison(amount);
			}
		}
	}
	
	public void Apathize (int amount) {
		for (int i = 0; i < 4; i++) {
			Character current = Party.GetCharacter(i);
			if (current != null) {
				current.status.CauseApathy(amount);
			}
		}
	}
	
	public void DamageAll (int amount) {
		for (int i = 0; i < 4; i++) {
			Character current = Party.GetCharacter(i);
			if (current != null) {
				current.Damage(amount);
				if (current.GetHealth() <= 0) {
					if (current.GetChampion()) {
						SceneManager.LoadScene("GameOver");
						return;
					}
					Party.playerCount--;
					Party.members[i] = null;
				}
			}
		}
	}
	
	public void Damage (int amount) {
		Party.members[partyIndex].Damage(amount);
		if (Party.members[partyIndex].GetHealth() <= 0) {
			if (Party.members[partyIndex].GetChampion()) {
				SceneManager.LoadScene("GameOver");
				return;
			}
			Party.playerCount--;
			Party.members[partyIndex] = null;
		}
	}
	
	public void Heal (int amount) {
		for (int i = 0; i < 4; i++) {
			Character current = Party.GetCharacter(i);
			if (current != null) {
				current.SetHealth(System.Math.Min(current.GetHealth() + amount, current.GetMaxHP()));
			}
		}
	}
	
	public void GainSP (int amount) {
		Party.UseSP(amount * -1);
	}
	
	public void ChooseMember (int index) {
		partyIndex = index;
	}
	
	public void StatChange (string stat, int amount) {
		MethodInfo m = Party.members[partyIndex].GetType().GetMethod(stat);
		m.Invoke(Party.members[partyIndex], new object[] {amount});
	}
	
	public void AllStatChange(string stat, int amount) {
		foreach (Character c in Party.members) {
			if (c != null && c.GetAlive()) {
        		MethodInfo m = c.GetType().GetMethod(stat);
		        m.Invoke(c, new object[] {amount});
			}
		}
	}
	
	public void StatusChange (string method, int? degree) {
		MethodInfo m = Party.members[partyIndex].status.GetType().GetMethod(method);
		if (degree == null) {
			m.Invoke(Party.members[partyIndex].status, new object[] {null});
		} else {
		    m.Invoke(Party.members[partyIndex].status, new object[] {degree});
		}
	}
	
	public void AllStatusChange (string method, int degree) {
		foreach (Character c in Party.members) {
			if (c != null && c.GetAlive()) {
        		MethodInfo m = c.status.GetType().GetMethod(method);
		        if (degree == null) {
		        	m.Invoke(c.status, new object[] {null});
        		} else {
		            m.Invoke(c.status, new object[] {degree});
        		}
			}
		}
	}
	
	public void ChangeQuirk (Passive quirk) {
		Party.members[partyIndex].SetQuirk(quirk);
		quirk.SetSelf(Party.members[partyIndex]);
	}
	
	public void Ally (Character[] recruits) {
		foreach (Character c in recruits) {
			if (Party.playerCount < 4) {
	    		Party.AddPlayer(c);
			} else {
				Party.AddPlayer(c);
			    replaceMember.SetActive(true);
				eventSpace.SetActive(false);
		        //menu.SetActive(false);
	    		//largeMenuHides.SetActive(false);
			    replaceMember.transform.Find("Member 1").gameObject.GetComponent<Button>().interactable = false;
				return;
			}
		}
		Resolve();
	}
	
	public void OpenSpecial (Special special) {
		useItemMenu.SetActive(true);
		partyMenu.SetActive(false);
		//largeMenuHides.SetActive(false);
	    useItemMenu.GetComponent<PartyMenu>().currentSpecial = special;
		usingSpecial = true;
	}
	
	public void SpecialSelects () {
		Special special = useItemMenu.GetComponent<PartyMenu>().currentSpecial;
		Party.UseSP(special.GetCost());
		TimedMethod[] moves = special.UseSelects(useItemMenu.GetComponent<PartyMenu>().ConfirmCharacter());
		//foreach (TimedMethod m in moves) {
	        //methodQueue.Enqueue(m);
		//}
		//methodQueue.Enqueue(new TimedMethod(2, "EndTurn"));
		useItemMenu.GetComponent<PartyMenu>().currentSpecial = null;
		useItemMenu.SetActive(false);
		nextMenu.SetActive(true);
		if (isEvent) {
			eventSpace.SetActive(true);
		} else {
			dungeonMaps.SetActive(true);
		}
		//largeMenuHides.SetActive(true);
	}
	
	public void CloseRecruit () {
		replaceMember.SetActive(false);
	}
	
	public void SpendTime (int amount) {
		Time.Increment(amount);
	}
	
	public void Win () {
	    SceneManager.LoadScene("Win");	
	}
	
	public void Exit () {
		SceneManager.LoadScene("Overworld");
	}
}