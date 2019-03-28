using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusBarE : MonoBehaviour {
	
	public Text message;
	Character enemy;
	string charName;
	string offense;
	string defense;
	string accEvade;
	string quirk;
	string statuses;
	string hp;
	string recruitableText;
	string bonusText;
    string defaultText;
	string specificText;
	public static bool informed;
	public bool frozen;
	// Use this for initialization
	void Start () {
		message.text = "";
		bonusText = "\n";
		specificText = "";
		informed = false;
		Check();
	}
	
	// Update is called once per frame
	void Update () {
		//sCheck();
	}
	
	public void Check () {
		try { 
		    enemy = Party.GetEnemy();
		    charName = enemy.type;
			offense = "Power: " + enemy.GetPower().ToString() + "    Charge: " + enemy.GetCharge().ToString();
			defense = "Defense: " + enemy.GetDefense().ToString() + "    Guard: " + enemy.GetGuard().ToString();
			accEvade = "Acc: " + enemy.GetAccuracy().ToString() + "    Evade: " + enemy.GetEvasion().ToString();
		    quirk = "Quirk: " + enemy.GetQuirk().GetName();
			statuses = enemy.status.BarText();
			if (enemy.GetRecruitable()) {
				recruitableText = "recruitable if alone";
			} else {
				recruitableText = "unrecruitable";
			}
			defaultText = offense + "\n" + defense + "\n" + accEvade + "\n" + quirk + "\n" + statuses + "\n" + recruitableText + "\n";
			specificText = enemy.SpecificBarText();
		    if (informed) {
			    hp = enemy.GetHealth().ToString();
			    bonusText = "\n" + hp + " HP\n";
		    } else {
				bonusText = "\n";
			}
		    message.text = charName + bonusText + defaultText + specificText;
		} catch (System.NullReferenceException e) {
			message.text = "";
		}
	}
}
