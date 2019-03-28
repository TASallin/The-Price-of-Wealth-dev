public class Status {
	
	Character self;
	public int asleep;
	public int poisoned;
	public int stunned;
	public bool gooped;
	public int blinded;
	public int regeneration;
	public bool passing;
	public int apathetic;
	public int caffeine;
	public int possessed;
	public bool sleepImmune;
	public bool poisonImmune;
	public bool stunImmune;
	public bool goopImmune;
	public bool blindImmune;
	public bool waking;
	public static int catalyst;
	public static bool firewall;
	public static System.Random rng = new System.Random();
	
	public Status (Character c) {
		self = c; asleep = 0; poisoned = 0; stunned = 0; gooped = false; blinded = 0; caffeine = 0; waking = false; regeneration = 0; passing = false;
		apathetic = 0; possessed = 0; sleepImmune = false; poisonImmune = false; stunImmune = false; goopImmune = false; blindImmune = false;
	}
	
	public TimedMethod[] CheckLead() {
		TimedMethod[] messages = new TimedMethod[0];
		TimedMethod[] temp;
		if (poisoned > 0) {
			self.SetHealth(System.Math.Max(self.GetHealth() - poisoned, 0));
			    temp = new TimedMethod[messages.Length + 2];
				messages.CopyTo(temp, 0);
				temp[messages.Length] = new TimedMethod(0, "Audio", new object[] {"Poison Damage"});
				temp[messages.Length + 1] = new TimedMethod(
				    0, "CharLogSprite", new object[] {poisoned.ToString() + " poison", self.partyIndex, "poison", self.GetPlayer()});
				messages = temp;
			if (self.GetHealth() == 0) {
			    TimedMethod[] dead = Party.CheckDeath();
				temp = new TimedMethod[messages.Length + dead.Length];
				messages.CopyTo(temp, 0);
			    dead.CopyTo(temp, messages.Length);
				messages = temp;
				return messages;
			}
		}
		if (apathetic > 0) {
			if (rng.Next(10) < 3) {
				passing = true;
				apathetic--;
				temp = new TimedMethod[messages.Length + 1];
				messages.CopyTo(temp, 0);
				temp[messages.Length] = new TimedMethod(60, "Log", new object[] {self.ToString() + " struggled to care"});
				messages = temp;
			}
		}
		if (possessed > 0) {
			temp = new TimedMethod[messages.Length + 1];
			messages.CopyTo(temp, 0);
			temp[messages.Length] = new TimedMethod(60, "SwitchTo", new object[] {1});
			messages = temp;
		}
		TimedMethod[] regular = Check();
		temp = new TimedMethod[messages.Length + regular.Length];
		messages.CopyTo(temp, 0);
		regular.CopyTo(temp, messages.Length);
		messages = temp;
		return messages;
	}
	
	public TimedMethod[] Check () {
		string message = "";
		TimedMethod[] messages = new TimedMethod[0];
		TimedMethod[] temp;
		if (caffeine > 0) {
			caffeine--;
			if (caffeine == 0) {
				//self.SetPower(self.GetPower() - 10); self.GainAccuracy(-10);
				caffeine = -5;
				temp = new TimedMethod[messages.Length + 1];
				messages.CopyTo(temp, 0);
				temp[messages.Length] = new TimedMethod(60, "Log", new object[] {self.ToString() + " crashed"});
				messages = temp;
			}
		} else if (caffeine < 0) {
			caffeine++;
			if (caffeine == 0) {
				//self.SetPower(self.GetPower() + 5); self.GainAccuracy(5);
				temp = new TimedMethod[messages.Length + 1];
				messages.CopyTo(temp, 0);
				temp[messages.Length] = new TimedMethod(60, "Log", new object[] {self.ToString() + " recovered"});
				messages = temp;
			}
		}
		if (regeneration > 0) {
			self.Heal(regeneration);
			temp = new TimedMethod[messages.Length + 1];
			messages.CopyTo(temp, 0);
			temp[messages.Length] = new TimedMethod(
			   0, "CharaLogSprite", new object[] {regeneration.ToString(), self.partyIndex, "regeneration", self.GetPlayer()});
			messages = temp;
		}
		if (blinded > 0) {
			blinded--;
		}
		if (asleep > 0) {
	        int threshold = rng.Next(6) + 1;
			if (waking || threshold <= asleep) {
				asleep = 0; waking = false;
				temp = new TimedMethod[messages.Length + 1];
				messages.CopyTo(temp, 0);
				temp[messages.Length] = new TimedMethod(60, "Log", new object[] {self.ToString() + " woke up"});
				messages = temp;
			} else {
				asleep++;
				temp = new TimedMethod[messages.Length + 1];
				messages.CopyTo(temp, 0);
				temp[messages.Length] = new TimedMethod(60, "Log", new object[] {self.ToString() + " is asleep"});
				messages = temp;
			}
		}
		if (possessed > 0) {
	        possessed--;
			if (possessed == 0) {
				temp = new TimedMethod[messages.Length + 1];
				messages.CopyTo(temp, 0);
				temp[messages.Length] = new TimedMethod(60, "Log", new object[] {self.ToString() + " got a hold of themself"});
				messages = temp;
			} else {
				temp = new TimedMethod[messages.Length + 2];
				messages.CopyTo(temp, 0);
				temp[messages.Length] = new TimedMethod(60, "Log", new object[] {self.ToString() + " attacked " + Party.GetPlayer().ToString()});
				temp[messages.Length + 1] = new TimedMethod(0, "AttackAny", new object[] {
					self, Party.GetPlayer(), self.GetStrength(), self.GetStrength(), self.GetAccuracy(), true, true, false});
				messages = temp;
			}
		}
		if (stunned > 0) {
			stunned--;
			if (stunned > 0) {
				temp = new TimedMethod[messages.Length + 1];
				messages.CopyTo(temp, 0);
				temp[messages.Length] = new TimedMethod(60, "Log", new object[] {self.ToString() + " is stunned"});
				messages = temp;
			}
		}
		if (passing == true) {
			//passing = false;
			temp = new TimedMethod[messages.Length + 1];
			messages.CopyTo(temp, 0);
			temp[messages.Length] = new TimedMethod(60, "Log", new object[] {self.ToString() + " skipped their turn"});
			messages = temp;
		}
		return messages;
	}
	
	public static void NullifyAttack (Character c) {
		c.SetCharge(0); c.SetPower(0);
		// return new TimedMethod(0, "Audio", new object[] {"Nullify"});
	}
	
	public static void NullifyDefense (Character c) {
		c.SetGuard(0); c.SetDefense(0);
		// return new TimedMethod(0, "Audio", new object[] {"Nullify"});
	}
	
	public TimedMethod[] Poison (int amount) {
		if (!poisonImmune) {
			if (poisoned >= amount + catalyst) {
				return new TimedMethod[] {new TimedMethod("Null"), new TimedMethod("Null")};
			} else {
			    poisoned = amount + catalyst;
			    return new TimedMethod[] {new TimedMethod(0, "Audio", new object[] {"Poison"}),
				    new TimedMethod(0, "CharLogDelay", new object[] {"Poison", self.partyIndex, "poison", self.GetPlayer()})};
			}
		}
		return new TimedMethod[] {new TimedMethod(0, "CharLogDelay", new object[] {"IMMUNE", self.partyIndex, "poison", self.GetPlayer()}),
    		new TimedMethod("Null")};
	}
	
	public TimedMethod[] Regenerate (int amount) {
		regeneration = System.Math.Max(amount, regeneration);
		return new TimedMethod[] {new TimedMethod(0, "CharLogDelay", new object[] {
			"Regeneration", self.partyIndex, "regeneration", self.GetPlayer()}), new TimedMethod("Null")};
	}
	
	public TimedMethod[] StackPoison (int amount) {
	    if (!poisonImmune) {
			poisoned = poisoned + amount + catalyst;
			return new TimedMethod[] {new TimedMethod(0, "CharLogDelay", new object[] {"Poison", self.partyIndex, "poison", self.GetPlayer()}),
			    new TimedMethod(0, "Audio", new object[] {"Poison"})};
		}
		return new TimedMethod[] {new TimedMethod(0, "CharLogDelay", new object[] {"IMMUNE", self.partyIndex, "poion", self.GetPlayer()}),
		    new TimedMethod("Null")};
	}
	
	public TimedMethod[] Sleep () {
		if (!sleepImmune && asleep == 0 && caffeine <= 0) {
			asleep = 1;
			return new TimedMethod[] {new TimedMethod(0, "Audio", new object[] {"Skip Turn"}),
			    new TimedMethod(0, "CharLogDelay", new object[] {"Asleep", self.partyIndex, "sleep", self.GetPlayer()})};
		}
		if (asleep == 0) {
		    return new TimedMethod[] {new TimedMethod(0, "CharLogDelay", new object[] {"IMMUNE", self.partyIndex, "sleep", self.GetPlayer()}),
    			new TimedMethod("Null")};
		}
		return new TimedMethod[] {new TimedMethod("Null"), new TimedMethod("Null")};
	}
	
	public void Awake() {
	    if (asleep > 0) {waking = true;}	
	}
	
	public TimedMethod[] Stun (int amount) {
		if (!stunImmune && stunned == 0 && rng.Next(2) == 0) {
			stunned = amount;
			return new TimedMethod[] {new TimedMethod(0, "Audio", new object[] {"S Explosion"}),
    			new TimedMethod(0, "CharLogDelay", new object[] {"Stun", self.partyIndex, "stun", self.GetPlayer()})};
		}
		if (stunned == 0) {
		    return new TimedMethod[] {new TimedMethod(0, "CharLogDelay", new object[] {"RESIST", self.partyIndex, "stun", self.GetPlayer()}),
    			new TimedMethod("Null")};
		}
		return new TimedMethod[] {new TimedMethod("Null"), new TimedMethod("Null")};
	}
	
	public TimedMethod[] Goop () {
		if (!goopImmune) {
			gooped = true;
			return new TimedMethod[] {new TimedMethod(0, "Audio", new object[] {"Goop"}),
			    new TimedMethod(0, "CharLogDelay", new object[] {"Goop", self.partyIndex, "goop", self.GetPlayer()})};
		}
		return new TimedMethod[] {new TimedMethod(0, "CharLogDelay", new object[] {"IMMUNE", self.partyIndex, "goop", self.GetPlayer()}),
    		new TimedMethod("Null")};
	}
	
	public TimedMethod[] Blind (int amount) {
		if (!blindImmune) {
			blinded += amount;
			return new TimedMethod[] {new TimedMethod(0, "Audio", new object[] {"Blind"}),
			    new TimedMethod(0, "CharLogDelay", new object[] {"Blind", self.partyIndex, "blind", self.GetPlayer()})};
		}
		return new TimedMethod[] {new TimedMethod(0, "CharLogDelay", new object[] {"IMMUNE", self.partyIndex,  "blid", self.GetPlayer()}),
    		new TimedMethod("Null")};
	}
	
	public TimedMethod[] Pass () {
		passing = true;
		//return new TimedMethod[0];
		return new TimedMethod[] {new TimedMethod(0, "CharLogDelay", new object[] {"SKIP", self.partyIndex, "skip", self.GetPlayer()})}; 
	}
	
	public TimedMethod[] CauseApathy (int amount) {
		if (apathetic >= amount) {
			return new TimedMethod[] {new TimedMethod("Null"), new TimedMethod("Null")};
		}
		apathetic = amount;
		return new TimedMethod[] {new TimedMethod(0, "Audio", new object[] {"Skip Turn"}), 
		    new TimedMethod(0, "CharLogDelay", new object[] {"Apathy", self.partyIndex, "apathy", self.GetPlayer()})};
	}
	
	public TimedMethod[] Possess () {
		if (!self.GetChampion() && possessed == 0) {
			possessed = 5;
			return new TimedMethod[] { new TimedMethod(0, "Audio", new object[] {"Poison"}),
			    new TimedMethod(0, "CharLogDelay", new object[] {"WEALTHY", self.partyIndex, "stun", self.GetPlayer()})};
		}
		if (self.GetChampion()) {
		    return new TimedMethod[] {new TimedMethod(0, "CharLogDelay", new object[] {"Strong Will", self.partyIndex, "stun", self.GetPlayer()}),
			    new TimedMethod("Null")};
		}
		return new TimedMethod[] {new TimedMethod("Null"), new TimedMethod("Null")};
	}
	
	public void Coffee () {
		caffeine = 6; //self.SetPower(self.GetPower() + 5); self.GainAccuracy(5);
		//return new TimedMethod[] { new TimedMethod(0, "Audio", new object[] {"Tazer"}),
		//	new TimedMethod(0, "CharLog", new object[] {"Caffeine", self.partyIndex})};
	}
	
	public int CoffeeEffect() {
		if (caffeine > 0) {
			return 5;
		} else if (caffeine < 0) {
			return -5;
		} else {
			return 0;
	    }
	}
	
	public string DescriptorText() {
		string s = "";
		if (asleep > 0) {
			s += "Asleep - Can not take actions until awoken (randomly or through damage)\n";
		}
		if (stunned > 0) {
			s += "Stunned - Can not take actions for " + stunned.ToString() + " turns\n";
		}
		if (poisoned > 0) {
		    s += "Poisoned - While in the lead, take " + poisoned.ToString() + " damage at the start of their turn\n";
	    }
		if (gooped) {
			s += "Gooped - Can not take defensive actions or switch out. Skip turn to clean\n";
		}
		if (blinded > 0) {
			s += "Blinded - Accuracy is reduced by " + blinded.ToString() + ", recovers by 1 each turn\n";
		}
		if (apathetic > 0) {
			s += "Apathy - Chance to skip turns\n";
		}
		if (regeneration > 0) {
			s += "Regeneration - Recover " + regeneration.ToString() + " hp every turn\n";
		}
		if (possessed > 0) {
			s += "Possessed - Unusable and attacks you each turn";
		}
		return s;
	}
	
	public string BarText() {
		string s = "";
		if (asleep > 0) {
			s += "Asleep ";
		}
		if (stunned > 0) {
			s += "Stunned ";
		}
		if (poisoned > 0) {
		    s += "Poisoned ";
	    }
		if (gooped) {
			s += "Gooped ";
		}
		if (blinded > 0) {
			s += "Blinded ";
		}
		if (apathetic > 0) {
			s += "Apathy ";
		}
		if (regeneration > 0) {
			s += "Regeneration ";
		}
		if (possessed > 0) {
			s += "Possessed ";
		}
		return s;
	}
	
	public void PostBattle() {
		asleep = 0; poisoned = 0; stunned = 0; gooped = false; blinded = 0; waking = false; catalyst = 0; regeneration = 0; passing = false;
		possessed = 0; apathetic = 0; sleepImmune = false; poisonImmune = false; stunImmune = false; goopImmune = false; blindImmune = false;
		firewall = false;
	}
}