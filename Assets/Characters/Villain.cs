public class Villain : Character {
	
	int cycleSwitch;
	int cycleMain;
	
	public Villain () {
		health = 100; maxHP = 100; strength = 5; power = 0; charge = 0; defense = 0; guard = 0;
		baseAccuracy = 16; accuracy = 16; dexterity = 5; evasion = 0; type = "Villain"; passive = new Passive(this);
		quirk = Quirk.GetQuirk(this); special = null; player = false; champion = true; recruitable = false;
		CreateDrops(); cycleSwitch = 2; cycleMain = 0;
	}
	
	public override TimedMethod[] AI () {
		System.Random rng = new System.Random();
		int num = rng.Next(10);
		if (Party.enemyCount > 1) {
			if (cycleSwitch == 0) {
				cycleSwitch++;
				return Attack(num);
			} else if (cycleSwitch == 1) {
				cycleSwitch++;
				return Buff(num);
			} else {
				return Switch();
			}
		} else {
			if (cycleMain == 0) {
				cycleMain++;
				return Hypnotize();
			} else if (cycleMain == 1) {
				cycleMain++;
				return Attack(num);
			} else {
				cycleMain--;
				return Buff(num);
			}
		}
	}
	
	public TimedMethod[] Attack(int num) {
		if (num < 4) {
			return Knife();
		} else if (num < 8) {
			return Pistol();
		} else {
			return Acid();
		}
	}
	
	public TimedMethod[] Buff(int num) {
		if (num < 2) {
			return Steroid();
		} else if (num < 4) {
			return Poison();
		} else if (num < 6) {
			return Smoke();
		} else if (num < 8) {
			return Duel();
		} else {
			return Drug();
		}
	}
	
	public TimedMethod[] Knife () {
		Attacks.SetAudio("Knife", 6);
		return new TimedMethod[] {new TimedMethod(60, "Log", new object[] {"The Villain used the knife"}),
		    new TimedMethod(0, "AudioNumbered", new object[] {"Attack", 3, 4}), new TimedMethod(0, "Audio", new object[] {"Small Swing"}),
	    	new TimedMethod(0, "StagnantAttack", new object[] {false, 5, 5, GetAccuracy(), true, true, true})};
	}
	
	public TimedMethod[] Pistol () {
		Attacks.SetAudio("Blunt Hit", 6);
		return new TimedMethod[] {new TimedMethod(60, "Log", new object[] {"The Villain shot the pistol"}),
		    new TimedMethod(0, "AudioNumbered", new object[] {"Attack", 3, 4}), new TimedMethod(0, "Audio", new object[] {"Gunfire"}),
	    	new TimedMethod(0, "StagnantAttack", new object[] {false, 10, 10, GetAccuracy() / 2, true, true, false})};
	}
	
	public TimedMethod[] Acid () {
		Attacks.SetAudio("Poison Damage", 15);
		return new TimedMethod[] {new TimedMethod(60, "Log", new object[] {"The Villain threw an acid bomb"}),
		    new TimedMethod(0, "AudioNumbered", new object[] {"Attack", 3, 4}), new TimedMethod(0, "AudioAfter", new object[] {"S Explosion", 30}),
	    	new TimedMethod(0, "AttackAll", new object[] {false, 2, 2, GetAccuracy(), true, true, false})};
	}
	
	public TimedMethod[] Steroid () {
		GainPower(3);
		return new TimedMethod[] {new TimedMethod(0, "Audio", new object[] {"Knife"}),
		    new TimedMethod(60, "Log", new object[] {"The Villain applied steroids"}),
		    new TimedMethod(0, "CharLogSprite", new object[] {"3", Party.enemySlot - 1, "power"})};
	}
	
	public TimedMethod[] Poison() {
	    TimedMethod[] poisonPart = new TimedMethod[] {new TimedMethod(60, "Log", new object[] {"miss"}), new TimedMethod("Null")};
	    if (Attacks.EvasionCycle(this, Party.GetPlayer())) {
		    poisonPart = Party.GetPlayer().status.Poison(2);
	    }
		return new TimedMethod[] {new TimedMethod(0, "Audio", new object[] {"Fumes"}),
		    new TimedMethod(60, "Log", new object[] {"The Villain sprayed poison fumes"}), poisonPart[0], poisonPart[1]};
	}
	
	public TimedMethod[] Smoke() {
		TimedMethod evadePart = new TimedMethod("Null");
		GainEvasion(10);
		if (!GetGooped()) {
			evadePart = new TimedMethod(0, "CharLogSprite", new object[] {"10", Party.enemySlot - 1, "evasion", false});
		}
		TimedMethod[] blindPart = new TimedMethod[] {new TimedMethod(60, "Log", new object[] {"miss"}), new TimedMethod("Null")};
	    if (Attacks.EvasionCycle(this, Party.GetPlayer())) {
		    blindPart = Party.GetPlayer().status.Blind(7);
	    }
		return new TimedMethod[] {new TimedMethod(0, "AudioAfter", new object[] {"S Explosion", 20}),
		    new TimedMethod(60, "Log", new object[] {"The Villain threw a smoke bomb"}), blindPart[0], blindPart[1], evadePart};
	}
	
	public TimedMethod[] Duel() {
		int amount = Party.GetPlayer().GetEvasion() / 2;
		Party.GetPlayer().SetEvasion(Party.GetPlayer().GetEvasion() / 2);
		GainGuard(10);
		return new TimedMethod[] {new TimedMethod(0, "Audio", new object[] {"Poison"}),
		    new TimedMethod(60, "Log", new object[] {"The Villain gave an insulting taunt. Evasion halved"}),
			new TimedMethod(0, "CharLogSprite", new object[] {"10", Party.enemySlot - 1, "guard", false}),
			new TimedMethod(0, "CharLogSprite", new object[] {amount.ToString(), Party.playerSlot - 1, "evasion", true})};
	}
	
	public TimedMethod[] Drug() {
		if (Attacks.EvasionCycle(this, Party.GetEnemy())) {
			Status.NullifyAttack(Party.GetPlayer());
			Status.NullifyDefense(Party.GetPlayer());
			TimedMethod[] apathyPart = Party.GetPlayer().status.CauseApathy(2);
			return new TimedMethod[] {new TimedMethod(0, "AudioAfter", new object[] {"Knife", 15}), new TimedMethod(60, "Log", new object[] {
				"The Villain injected an unstable formula"}), apathyPart[0], apathyPart[1],
				new TimedMethod(0, "CharLogSprite", new object[] {"atk reset", Party.playerSlot - 1, "nullAttack", true}),
				new TimedMethod(0, "CharLogSprite", new object[] {"def reset", Party.playerSlot - 1, "nullDefense", true})};
		} else {
			return new TimedMethod[] {new TimedMethod(0, "Audio", new object[] {"Small Swing"}),
    			new TimedMethod(60, "Log", new object[] {"The Villain missed with an unstable formula"})};
		}
	}
	
	public TimedMethod[] Hypnotize() {
	    if (Attacks.EvasionCycle(GetAccuracy() * 2, Party.GetPlayer())) {
			TimedMethod[] possessPart = Party.GetPlayer().status.Possess();
			return new TimedMethod[] {new TimedMethod(0, "Audio", new object[] {"Possession"}),
			    new TimedMethod(60, "Log", new object[] {"The Villain activated the evil device"}), 
			    possessPart[0], possessPart[1], new TimedMethod(0, "SwitchTo", new object[] {1})};
	    }
		return new TimedMethod[] {new TimedMethod(0, "Audio", new object[] {"Possession"}),
		    new TimedMethod(60, "Log", new object[] {"The Villain activated the evil device and you dodged its nature"})};
	}
	
	public TimedMethod[] Switch() {
		if (GetGooped()) {
			status.gooped = false;
			GainEvasion(10);
			return new TimedMethod[] {new TimedMethod(0, "Audio", new object[] {"S Explosion"}),
			    new TimedMethod(60, "Log", new object[] {"The Villain escaped the goop with a smoke bomb"}),
				new TimedMethod(0, "CharLogSprite", new object[] {"Cleaned", Party.playerSlot - 1, "goop", false}),
				new TimedMethod(0, "CharLogSprite", new object[] {"10", Party.enemySlot - 1, "evasion", false})};
		}
		for (int i = 1; i < 4; i++) {
			if (Party.enemies[i] != null && Party.enemies[i].GetAlive()) {
				Party.enemySlot = i + 1;
				cycleSwitch = 0;
				return new TimedMethod[] {new TimedMethod(60, "Log", new object[] {"The Villain called upon his loyal cabinet"}),
				    new TimedMethod(0, "EnemySwitch", new object[] {Party.enemySlot, 1})};
			}
		}
		return Knife();
	}
	
	public override void CreateDrops() {
		drops = new Item[0];
	}
	
	public override Item[] Loot () {
		System.Random rnd = new System.Random();
		int sp = 10 + rnd.Next(6);
		Party.UseSP(sp * -1);
		Item[] dropped = drops;
		drops = new Item[0];
		return dropped;
	}
	
	public override string[] CSDescription() {
		return new string[] {"This is it. The final battle",
		"He seems to carry a full bag of tricks. Be ready for anything, and win"};
	}
}