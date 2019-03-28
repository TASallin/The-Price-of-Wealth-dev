using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Reflection;
using UnityEngine.SceneManagement;

public class Battle : MonoBehaviour {
	
	Character player;
	Character enemy;
	public GameObject menu;
	public GameObject itemSpace;
	public GameObject specialMenu;
	public GameObject defenseMenu;
	public GameObject messageLog;
	public GameObject partyMenu;
	public GameObject winMenu;
	public GameObject recruitMember;
	public GameObject previousMessages;
	public GameObject useItemMenu;
	public GameObject audio;
	public GameObject sprites;
	public GameObject nameMenu;
	public GameObject statusBars;
	public GameObject largeMenuHides;
	public int delay;
	public Queue<TimedMethod> methodQueue;
	Type t;
	MethodInfo method;
	bool running;
	int formerHP;
	bool usingSpecial;
	int lastGuard;
	int guardStrength;
	// Use this for initialization
	void Start () {
		//Debug.Log(Party.playerCount.ToString());
		//menu = gameObject.transform.Find("MenuSpace").gameObject;
		//itemSpace = gameObject.transform.Find("ItemSpace").gameObject;
		//specialMenu = gameObject.transform.Find("SpecialMenu").gameObject;
		//defenseMenu = gameObject.transform.Find("Defense Menu").gameObject;
		//messageLog = gameObject.transform.Find("Message Log").gameObject;
		//partyMenu = gameObject.transform.Find("Party Menu").gameObject;
		//winMenu = gameObject.transform.Find("Win Menu").gameObject;
	    //recruitMember = gameObject.transform.Find("Replace Member").gameObject;
		//previousMessages = gameObject.transform.Find("Previous Messages").gameObject;
		//useItemMenu = gameObject.transform.Find("Use Item Menu").gameObject;
		//audio = gameObject.transform.Find("Audio").gameObject;
		//sprites = gameObject.transform.Find("Character Sprites").gameObject;
		//nameMenu = gameObject.transform.Find("Name Menu").gameObject;
		//statusBars = gameObject.transform.Find("Status Bars").gameObject;
		Party.latestRecruit = null;
		delay = 0;
		methodQueue = new Queue<TimedMethod>();
		t = this.GetType();
		GetPlayer(); GetEnemy();
		running = false;
		lastGuard = -1;
		guardStrength = 5;
		audio.GetComponent<GameAudio>().InitiateMusic();
		ItemButton.inBattle = true;
		//itemSpace.GetComponent<ItemSpace>().Check();
		//itemSpace.SetActive(false);
		//if (Party.area == "Overworld") {
		//    defenseMenu.gameObject.transform.Find("Run Button").gameObject.GetComponent<Button>().interactable = true;	
		//} else {
		//	defenseMenu.gameObject.transform.Find("Run Button").gameObject.GetComponent<Button>().interactable = false;	
		//}
		Queue<TimedMethod> moves = Party.InitializePassives();
		foreach (TimedMethod t in moves) {
			methodQueue.Enqueue(t);
		}
		methodQueue.Enqueue(new TimedMethod(0, "NextTurn", new object[] {false}));
	}
	
	// Update is called once per frame
	void Update () {
        Debug.Log(Party.GetPlayer().partyIndex.ToString());
		if (delay > 0) {
			delay--;
		} else {
		    if (methodQueue.Count > 0) {
				delay = methodQueue.Peek().delay;
				TimedMethod current = methodQueue.Dequeue();
				method = t.GetMethod(current.method);
				if (current.args == null) {
					method.Invoke(this, null);
				} else {
					method.Invoke(this, current.args);
				}
			}
		}
	}
	
	public void Attack (bool playerTurn) {
		if  (playerTurn) {
			TimedMethod[] moves = player.BasicAttack();
			foreach (TimedMethod m in moves) {
		        methodQueue.Enqueue(m);
		    }
			moves = Party.CheckDeath();
			foreach (TimedMethod m in moves) {
		        methodQueue.Enqueue(m);
		    }
			GetEnemy();
			//delay += 60;
			menu.SetActive(false);
			methodQueue.Enqueue(new TimedMethod(0, "EndTurn"));
		} else {
			TimedMethod[] moves = Attacks.Attack(enemy, player);
			foreach (TimedMethod m in moves) {
		        methodQueue.Enqueue(m);
		    }
			moves = Party.CheckDeath();
			foreach (TimedMethod m in moves) {
		        methodQueue.Enqueue(m);
		    }		
			GetPlayer();
		}
	}
	
	public void StagnantAttack (bool playerTurn, int lower, int upper, int accuracy, bool usesPower, bool consumeCharge, bool piercing) {
		if  (playerTurn) {
			TimedMethod[] moves = Attacks.Attack(player, enemy, lower, upper, accuracy, usesPower, consumeCharge, piercing);
			foreach (TimedMethod m in moves) {
		        methodQueue.Enqueue(m);
		    }
			if (consumeCharge) {
		    	moves = Party.CheckDeath();
		    	foreach (TimedMethod m in moves) {
	    	        methodQueue.Enqueue(m); 
    		    }
			}			
			GetEnemy();
		} else {
			TimedMethod[] moves = Attacks.Attack(enemy, player, lower, upper, accuracy, usesPower, consumeCharge, piercing);
			foreach (TimedMethod m in moves) {
		        methodQueue.Enqueue(m);
		    }
		  	if (consumeCharge) {
	    		moves = Party.CheckDeath();
    			foreach (TimedMethod m in moves) {
		            methodQueue.Enqueue(m);
		        }
			}			
			GetPlayer();
		}
	}
	
	public void AttackAny (Character atkr, Character targ, int lower, int upper, int accuracy, bool usesPower, bool consumeCharge, bool piercing) {
		TimedMethod[] moves = Attacks.Attack(atkr, targ, lower, upper, accuracy, usesPower, consumeCharge, piercing);
		foreach (TimedMethod m in moves) {
		    methodQueue.Enqueue(m);
	    }
		if (consumeCharge) {
		    moves = Party.CheckDeath();
			foreach (TimedMethod m in moves) {
  	        methodQueue.Enqueue(m); 
    	    }
		}			
		GetEnemy(); GetPlayer();
	}
	
	public void AttackAll(bool playerTurn, int lower, int upper, int accuracy, bool usesPower) {
		TimedMethod[] moves;
		if  (playerTurn) {
			if (!Attacks.EvasionCheck(Party.GetEnemy(), accuracy)) {
				StagnantAttack(playerTurn, lower, upper, accuracy, usesPower, true, false);
			}
			bool[] slots = new bool[] {Party.GetEnemy(0) != null && Party.GetEnemy(0).GetAlive(),
    			Party.GetEnemy(1) != null && Party.GetEnemy(1).GetAlive(), Party.GetEnemy(2) != null && Party.GetEnemy(2).GetAlive(),
				Party.GetEnemy(3) != null && Party.GetEnemy(3).GetAlive()};
			for (int i = 0; i < 4; i++) {
				if (slots[i]) {
					moves = Attacks.Attack(player, Party.GetEnemy(i), lower, upper, accuracy, usesPower, false, false);
					foreach (TimedMethod m in moves) {
		                methodQueue.Enqueue(m);
		            }
				}
			}
			player.SetCharge(0);
			moves = Party.CheckDeath();
			foreach (TimedMethod m in moves) {
		        methodQueue.Enqueue(m);
		    }			
			GetEnemy();
		} else {
			if (!Attacks.EvasionCheck(Party.GetPlayer(), accuracy)) {
				StagnantAttack(playerTurn, lower, upper, accuracy, usesPower, true, false);
			}
			bool[] slots = new bool[] {Party.GetCharacter(0) != null && Party.GetCharacter(0).GetAlive(),
    			Party.GetCharacter(1) != null && Party.GetCharacter(1).GetAlive(), Party.GetCharacter(2) != null &&
				Party.GetCharacter(2).GetAlive(), Party.GetCharacter(3) != null && Party.GetCharacter(0).GetAlive()};
			for (int i = 0; i < 4; i++) {
				if (slots[i]) {
					moves = Attacks.Attack(enemy, Party.GetCharacter(i), lower, upper, accuracy, usesPower, false, false);
					foreach (TimedMethod m in moves) {
		                methodQueue.Enqueue(m);
		            }
				}
			}
			enemy.SetCharge(0);
			moves = Party.CheckDeath();
			foreach (TimedMethod m in moves) {
		        methodQueue.Enqueue(m);
		    }					
			GetPlayer();
		}
	}
	
	public void Special () {
		specialMenu.SetActive(true);
		menu.SetActive(false);
		//string message = player.GetSpecial().ToString();
		//messageLog.SendMessage("SetMessage", message);
	}
	
	public void UseSpecial (Special special) {
		if (special.selects) {
		    useItemMenu.SetActive(true);
			specialMenu.SetActive(false);
			largeMenuHides.SetActive(false);
		    useItemMenu.GetComponent<PartyMenu>().currentSpecial = special;
			usingSpecial = true;
		} else {
	    	TimedMethod[] specialMethods = special.Use();
    		foreach (TimedMethod m in specialMethods) {
		        methodQueue.Enqueue(m);
		    }
		    menu.SetActive(false);
		    methodQueue.Enqueue(new TimedMethod(2, "EndTurn"));
		    Party.UseSP(special.GetCost());
		    specialMenu.SetActive(false);
		}
	}
	
	public void SupportSpecial (Special special, int index) {
	    if (special.selects) {
		    useItemMenu.SetActive(true);
			specialMenu.SetActive(false);
			largeMenuHides.SetActive(false);
		    useItemMenu.GetComponent<PartyMenu>().currentSpecial = special;
			usingSpecial = true;
		} else {
	    	TimedMethod[] specialMethods = special.UseSupport(index);
    		foreach (TimedMethod m in specialMethods) {
		        methodQueue.Enqueue(m);
		    }
	    	menu.SetActive(false);
    		methodQueue.Enqueue(new TimedMethod(2, "EndTurn"));
		    Party.UseSP(special.GetCost());
		    specialMenu.SetActive(false);
		}
	}
	
	public void SpecialSelects () {
		Special special = useItemMenu.GetComponent<PartyMenu>().currentSpecial;
		Party.UseSP(special.GetCost());
		TimedMethod[] moves = special.UseSelects(useItemMenu.GetComponent<PartyMenu>().ConfirmCharacter());
		foreach (TimedMethod m in moves) {
	        methodQueue.Enqueue(m);
		}
		methodQueue.Enqueue(new TimedMethod(2, "EndTurn"));
		useItemMenu.GetComponent<PartyMenu>().currentSpecial = null;
		useItemMenu.SetActive(false);
		largeMenuHides.SetActive(true);
	}
	
	public void Defense () {
		defenseMenu.SetActive(true);
		menu.SetActive(false);
	}
	
	public void Block () {
		if (lastGuard >= Party.turn - 1) {
		    guardStrength--;
		} else {
			guardStrength = 5;
		}
		lastGuard = Party.turn;
		Party.GetPlayer().GainGuard(guardStrength);
		CharLogSprite(guardStrength.ToString(), Party.playerSlot - 1, "guard", true);
	    string message = player.ToString() + " is guarding";
		messageLog.SendMessage("SetMessage", message);
		menu.SetActive(false);
	    methodQueue.Enqueue(new TimedMethod(30, "EndTurn"));
		defenseMenu.SetActive(false);
		Audio("Metal Hit");
	}
	
	public void Dodge () {
		Party.GetPlayer().GainEvasion(5);
		string message = player.ToString() + " is dodging";
		messageLog.SendMessage("SetMessage", message);
		menu.SetActive(false);
		CharLogSprite("5", Party.playerSlot - 1, "evasion", true);
	    methodQueue.Enqueue(new TimedMethod(30, "EndTurn"));
		defenseMenu.SetActive(false);
	}
	
	public void Run () {
		string message = player.GetName() + " is running";
		messageLog.SendMessage("SetMessage", message);
		//Set flag to run away next turn. Enemy gets one more turn and then
		//you automatically run at the start of the next turn
		running = true;
		if (player.GetQuirk().GetType().Equals(typeof(Paranoid))) {
			methodQueue.Enqueue(new TimedMethod(60, "Log", new object[] {"The paranoid escape plan was successful"})); 
		    methodQueue.Enqueue(new TimedMethod(2, "Flee"));
		} else {
		    methodQueue.Enqueue(new TimedMethod(60, "Log", new object[] {"The enemy team gets the last move"})); 
		    methodQueue.Enqueue(new TimedMethod(2, "EndTurn"));
		}
		menu.SetActive(false);
		defenseMenu.SetActive(false);
	}	
	
	public void Recruit () {
		string message;
		if (enemy.GetRecruitable() && Party.enemyCount == 1) {
		    System.Random rnd = new System.Random();
			if (rnd.Next(2) == 1 || Party.autoRecruit) {
				message = "You recruited " + enemy.type;
				messageLog.SendMessage("SetMessage", message);
				menu.SetActive(false);
				delay += 60;
				methodQueue.Enqueue(new TimedMethod("RecruitSuccess"));
				return;
			} else {
				message = enemy.ToString() + " Didn't feel like it";
				if (Party.BagContains(new VotedBadge())) {
					delay += 60;
					methodQueue.Enqueue(new TimedMethod("Win"));
				}
			}
		} else if (!enemy.GetRecruitable()) {
		    message = "Cannot recruit the rich";	
		} else {
			message = "They won't split from their group";
		}
		messageLog.SendMessage("SetMessage", message);
		menu.SetActive(false);
		methodQueue.Enqueue(new TimedMethod(60, "EndTurn"));
	}
	
	public void RecruitSuccess () {
		Audio("Recruit");
		if (Party.playerCount < 4) {
		    Party.AddPlayer(enemy);
		    methodQueue.Clear();
		    delay += 30;
		    methodQueue.Enqueue(new TimedMethod(0, "Win"));
		} else {
			methodQueue.Clear();
			Party.fullRecruit = enemy;
			recruitMember.SetActive(true);
			menu.SetActive(false);
			largeMenuHides.SetActive(false);
			methodQueue.Enqueue(new TimedMethod("DisableLeadReplace"));
		}
	}
	
	public void DisableLeadReplace () {
		recruitMember.transform.Find("Member 1").gameObject.GetComponent<Button>().interactable = false;
	}
	
	public void Items () {
		itemSpace.SetActive(true);
		menu.SetActive(false);
	}
	
	public void UseItem (Item item) {
		if (item.selects) {
		    useItemMenu.SetActive(true);
			largeMenuHides.SetActive(false);
		    useItemMenu.GetComponent<PartyMenu>().item = item;
		} else {
			string message = player.GetName() + " used " + item.GetName();
		    messageLog.SendMessage("SetMessage", message);
			TimedMethod[] itemMethods = item.Use();
		    foreach (TimedMethod m in itemMethods) {
		        methodQueue.Enqueue(m);
		    }
			Party.usedItems++;
		    methodQueue.Enqueue(new TimedMethod(2, "EndTurn"));
		}
		itemSpace.SetActive(false);
		delay += 60;
		menu.SetActive(false);
	}
	
	public void CancelItemUse () {
		Party.AddItem(useItemMenu.GetComponent<PartyMenu>().item);
		if (usingSpecial) {
			specialMenu.SetActive(true);
			usingSpecial = false;
		} else {
		    itemSpace.SetActive(true);
		}
		useItemMenu.SetActive(false);
		largeMenuHides.SetActive(true);
		useItemMenu.GetComponent<PartyMenu>().item = null;
		useItemMenu.GetComponent<PartyMenu>().currentSpecial = null;
	}
	
	public void ConfirmItemUse () {
		if (usingSpecial) {
			usingSpecial = false;
			SpecialSelects();
			return;
		}
		Item item = useItemMenu.GetComponent<PartyMenu>().item;
		string message = player.GetName() + " used " + item.GetName();
		messageLog.SendMessage("SetMessage", message);
		delay += 60;
		TimedMethod[] itemMethods = item.UseSelected(useItemMenu.GetComponent<PartyMenu>().ConfirmCharacter());
		foreach (TimedMethod m in itemMethods) {
	        methodQueue.Enqueue(m);
		}
		Party.usedItems++;
		methodQueue.Enqueue(new TimedMethod(2, "EndTurn"));
		useItemMenu.GetComponent<PartyMenu>().item = null;
		useItemMenu.SetActive(false);
		largeMenuHides.SetActive(true);
	}
	
	public void Cancel (string menu) {
	    this.menu.SetActive(true);
		gameObject.transform.Find(menu).gameObject.SetActive(false);
		largeMenuHides.SetActive(true);
		messageLog.SendMessage("SetMessage", "");
	}
	
	public void ToggleMenu(bool isActive) {
	    menu.SetActive(isActive);	
	}
	
	public void Switch () {
		int index = partyMenu.GetComponent<PartyMenu>().active;
		statusBars.SetActive(true);
		SwitchTo(index);
	}
	
	public void SwitchTo (int index) {
		//StorePlayer();
		//Party.playerSlot = index;
		if (index != Party.playerSlot) {
		    sprites.GetComponent<CharSprites>().Switch(Party.playerSlot - 1, index - 1);
		    Party.Switch(index, true);
		    GetPlayer();
		    methodQueue.Enqueue(new TimedMethod(60, "Log", new object[] {"Switched to " + player.ToString()}));
		//methodQueue.Enqueue(new TimedMethod(60,"EndTurn"));
		    menu.transform.Find("Attack Button").gameObject.GetComponent<Button>().interactable = true;
		//menu.transform.Find("Special Button").gameObject.GetComponent<Button>().interactable = true;
		    menu.transform.Find("Item Button").gameObject.GetComponent<Button>().interactable = true;
		    menu.transform.Find("Defense Button").gameObject.GetComponent<Button>().interactable = true;
			Audio("Running");
		}
		partyMenu.SetActive(false);
	}
	
	public void EnemySwitch(int a, int b) {
		sprites.GetComponent<CharSprites>().Switch(false, a, b);
		GetEnemy();
		Audio("Running");
	}	
	
	
	public void Status () {
		//StorePlayer();
		partyMenu.SetActive(true);
		menu.SetActive(false);
		largeMenuHides.SetActive(false);
		messageLog.SendMessage("SetMessage", "");
	}
	
	public void NextTurn (bool isEnemy) {
		sprites.GetComponent<CharSprites>().UnfreezeAll();
		if (isEnemy) {
			statusBars.transform.Find("Player Status").GetComponent<StatusBarP>().Check();
		    statusBars.transform.Find("Enemy Status").GetComponent<StatusBarE>().Check();
		   	TimedMethod[] statuses;
			for (int i = 0; i < 4; i++) {
				if (i == Party.enemySlot - 1) {
					statuses = Party.enemies[i].status.CheckLead();
				} else if (Party.enemies[i] != null && Party.enemies[i].GetAlive()) {
					statuses = Party.enemies[i].status.Check();
				} else {
					statuses = new TimedMethod[0];
				}
				foreach (TimedMethod t in statuses) {
				    methodQueue.Enqueue(t);
			    }
			}
			GetEnemy();
			if (enemy.GetAlive()) {
			    TimedMethod[] moves = Party.GetEnemy().EnemyTurn();
			    foreach (TimedMethod move in moves) {
				    methodQueue.Enqueue(move);
			    }
			}
			methodQueue.Enqueue(new TimedMethod(0, "NextTurn", new object[] {false}));
		} else {
			if (running) {
				if (Party.GetPlayer().GetGooped() || Party.GetPlayer().GetAsleep() || Party.GetPlayer().GetStunned()) {
				    messageLog.SendMessage("SetMessage", "You can't escape in this condition!");
					delay += 60;
					running = false;
				} else {
				    Flee();
				    return;
				}
			}
			Queue<TimedMethod> moves = Party.CheckPassives();
			foreach (TimedMethod t in moves) {
				methodQueue.Enqueue(t);
			}
			methodQueue.Enqueue(new TimedMethod("StatusCheck"));
		}
	}
	
	public void StatusCheck () {
		TimedMethod[] statuses;
			for (int i = 0; i < 4; i++) {
				if (i == Party.playerSlot - 1) {
					statuses = Party.members[i].status.CheckLead();
				} else if (Party.members[i] != null && Party.members[i].GetAlive()) {
					statuses = Party.members[i].status.Check();
				} else {
					statuses = new TimedMethod[0];
				}
				foreach (TimedMethod t in statuses) {
				    methodQueue.Enqueue(t);
			    }
			}
			//TimedMethod[] statuses = Party.GetPlayer().status.Check();
			//foreach (TimedMethod t in statuses) {
				//methodQueue.Enqueue(t);
			//}
			GetPlayer();
			if (Party.GetPlayer().GetAsleep() || Party.GetPlayer().GetStunned()) {
				methodQueue.Enqueue(new TimedMethod(0, "Incapacitated"));
			} else {
				methodQueue.Enqueue(new TimedMethod(0, "Awaken"));
			}
			if (Party.GetPlayer().GetGooped()) {
				methodQueue.Enqueue(new TimedMethod(0, "Stuck"));
			} else {
				methodQueue.Enqueue(new TimedMethod(0, "Freed"));
			}
			if (Party.GetPlayer().GetPassing()) {
				Party.GetPlayer().status.passing = false;
				CharLogSprite("SKIP", Party.playerSlot - 1, "skip",  true);
				Audio("Skip Turn");
				methodQueue.Enqueue(new TimedMethod("EndTurn"));
			} else {
		        methodQueue.Enqueue(new TimedMethod(0, "ToggleMenu", new object[] {true}));
			    methodQueue.Enqueue(new TimedMethod(0, "Log", new object[] {""}));
			}
			statusBars.transform.Find("Player Status").GetComponent<StatusBarP>().Check();
		    statusBars.transform.Find("Enemy Status").GetComponent<StatusBarE>().Check();
	}
	
	public void EndTurn () {
		largeMenuHides.SetActive(true);
		object[] args = new object[1];
		args[0] = true;
		methodQueue.Enqueue(new TimedMethod(0, "NextTurn", args));
	}
	
	//Warning: Clears all methods from the queue
	public void ContinueTurn () {
		methodQueue.Clear();
		menu.SetActive(true);
	}
	
	public void Incapacitated () {
		menu.transform.Find("Attack Button").gameObject.GetComponent<Button>().interactable = false;
		//menu.transform.Find("Special Button").gameObject.GetComponent<Button>().interactable = false;
		menu.transform.Find("Item Button").gameObject.GetComponent<Button>().interactable = false;
		menu.transform.Find("Defense Button").gameObject.GetComponent<Button>().interactable = false;
		menu.transform.Find("Recruit Button").gameObject.GetComponent<Button>().interactable = false;
	}
	
	public void Awaken () {
		menu.transform.Find("Attack Button").gameObject.GetComponent<Button>().interactable = true;
		//menu.transform.Find("Special Button").gameObject.GetComponent<Button>().interactable = true;
		menu.transform.Find("Item Button").gameObject.GetComponent<Button>().interactable = true;
		menu.transform.Find("Defense Button").gameObject.GetComponent<Button>().interactable = true;
		menu.transform.Find("Recruit Button").gameObject.GetComponent<Button>().interactable = true;
	
	}
    
	public void Stuck () {
		partyMenu.transform.Find("Switch").gameObject.GetComponent<Button>().interactable = false;
		defenseMenu.transform.Find("Dodge Button").gameObject.GetComponent<Button>().interactable = false;
		defenseMenu.transform.Find("Run Button").gameObject.GetComponent<Button>().interactable = false;
	}
	
	public void Freed () {
		partyMenu.transform.Find("Switch").gameObject.GetComponent<Button>().interactable = true;
		defenseMenu.transform.Find("Dodge Button").gameObject.GetComponent<Button>().interactable = true;
		//if (Party.area == "Overworld") {
		defenseMenu.transform.Find("Run Button").gameObject.GetComponent<Button>().interactable = true;
		//}
	}
	
	public void Skip () {
		if (Party.GetPlayer().GetGooped() && !(Party.GetPlayer().GetAsleep() || Party.GetPlayer().GetStunned())) {
			Party.GetPlayer().status.gooped = false;
			methodQueue.Enqueue(new TimedMethod(0, "CharLogSprite", new object[] {"Cleaned", Party.playerSlot - 1, "goop", true}));
			//methodQueue.Enqueue(new TimedMethod(60, "Log", new object[] {"The goop was cleaned"}));
		} else {
			methodQueue.Enqueue(new TimedMethod(0, "CharLogSprite", new object[] {"SKIP", Party.playerSlot - 1, "skip", true}));
		}
		menu.SetActive(false);
		methodQueue.Enqueue(new TimedMethod(0, "EndTurn"));
		Audio("Skip Turn");
	}
	
    public void PreviousMessages() {
		previousMessages.SetActive(true);
		menu.SetActive(false);
		messageLog.SendMessage("SetMessage", "");
	}
	
	public void Log (string message) {
	    messageLog.SendMessage("SetMessage", message);	
	}
	
	public void CharLog (string message, int index, bool player) {
		if (player) {
	    	sprites.GetComponent<CharSprites>().Log(message, index);
		} else {
			sprites.GetComponent<CharSprites>().Log(message, index + 4);
		}
	}
	
	public void CharLogDelay(string message, int index, string sprite, bool player) {
		methodQueue.Enqueue(new TimedMethod(0, "CharLogSprite", new object[] {message, index, sprite, player}));
	}
	
	public void CharLogSprite(string message, int index, string sprite, bool player) {
		if (player) {
		    sprites.GetComponent<CharSprites>().LogSprite(message, index, sprite);
		} else {
			sprites.GetComponent<CharSprites>().LogSprite(message, index + 4, sprite);
		}
	}
	
	public void ChangeHP(int damage, int index, bool player) {
		if (player) {
			sprites.GetComponent<CharSprites>().ChangeHP(damage, index);
		} else {
			sprites.GetComponent<CharSprites>().ChangeHP(damage, index + 4);
		}
	}
	public void RefreshStatusP() {
	    statusBars.transform.Find("Player Status").GetComponent<StatusBarP>().Check();
	}
	
	public void PartyDeath(Character dead) {
	    string message = dead.ToString() + " was defeated. Your champion was sent out";
		messageLog.SendMessage("SetMessage", message);
		GetPlayer();
	}
	
	public void EnemyDeath(Character dead) {
		string message = "You defeated " + dead.ToString();
		messageLog.SendMessage("SetMessage", message);
		GetEnemy();
	}
	
	public void Win () {
		    methodQueue.Clear();
			Score.victories++;
			statusBars.transform.Find("Player Status").GetComponent<StatusBarP>().Check();
		    statusBars.transform.Find("Enemy Status").GetComponent<StatusBarE>().Check();
			if (Party.fullRecruit != null) {
				if (Party.playerCount == 4) {
				    recruitMember.SetActive(true);
			        menu.SetActive(false);
					largeMenuHides.SetActive(false);
			        methodQueue.Enqueue(new TimedMethod("DisableLeadReplace"));
				    return;
				} else {
					Party.AddPlayer(Party.fullRecruit);
					Party.fullRecruit = null;
				}
			}
			//Debug.Log("None");
		    messageLog.SendMessage("SetMessage", "");
		    menu.SetActive(false);
			largeMenuHides.SetActive(true);
		    winMenu.SetActive(true);
		//}
	}
	
	public void NameRecruit () {
		if (Party.latestRecruit != null) {
			nameMenu.transform.Find("Name").gameObject.GetComponent<InputField>().text = Party.latestRecruit.GetName();
			nameMenu.SetActive(true);
			winMenu.SetActive(false);
		} else {
			BattleEnd();
		}
	}
	
	public void ConfirmName () {
		Party.latestRecruit.SetName(nameMenu.transform.Find("Name").gameObject.GetComponent<InputField>().text);
	}
	
	public void BattleEnd () {
		ItemButton.inBattle = false;
		Dungeon.fled = false;
		Party.PostBattle();
		SceneManager.LoadScene(Party.area);
	}
	
	public void WinDelay () {
		methodQueue.Enqueue(new TimedMethod("Win"));
	}
	
	public void Flee () {
		Time.Increment(1);
		if (Party.area != "Overworld") {
		    Dungeon.fled = true;
			Dungeon.leftEnemies = new Character[4];
			for (int i = 0; i < 4; i++) {
				Dungeon.leftEnemies[i] = Party.enemies[i];
			}
		}
		ItemButton.inBattle = false;
		Party.PostBattle();
		SceneManager.LoadScene(Party.area);
	}
	
	public void Lose () {
		SceneManager.LoadScene("GameOver");
	}
	
	public void StorePlayer () {Party.SetPlayer(player);}
	public void StoreEnemy () {Party.SetEnemy(enemy);}
	public void GetPlayer () {player = Party.GetPlayer();}
	public void GetEnemy () {enemy = Party.GetEnemy();}
	
	public void Audio (string name) {
		audio.GetComponent<GameAudio>().Play(name);
	}
	
	public void AudioAfter (string name, int frames) {
		audio.GetComponent<GameAudio>().PlayAfter(name, frames);
	}
	
	public void AudioRandom (string[] names) {
		audio.GetComponent<GameAudio>().PlayRandom(names);
	}
	
	public void AudioNumbered (string name, int lower, int upper) {
		audio.GetComponent<GameAudio>().PlayNumbered(name, lower, upper);
	}
	
	public void AudioAmount (string name, int amount) {
		audio.GetComponent<GameAudio>().PlayAmount(name, amount);
	}
	
	public void Null () {}
	
	public void ShowMessages (LinkedList<TimedMessage> messages, string finishMethod, object[] finishArgument) {
		TimedMessage current = messages.First.Value;
		messages.RemoveFirst();
		messageLog.SendMessage("SetMessage", current.GetMessage());
		//delay = current.GetDuration();
		//if (messages.Count > 0) {
		  //  nextMethod = "ShowMessages";
		    //nextArgument = new object[1];
		    //nextArgument[0] = messages;
		//} else {
		  //  nextMethod = finishMethod;
			//nextArgument = finishArgument;
		//}
	}
}
