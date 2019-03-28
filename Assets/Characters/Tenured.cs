public class Tenured : Instructor {
	
	
	public Tenured () {
		health = 45; maxHP = 45; strength = 3; dexterity = 4; type = "Tenured"; passive = new Leader(this);
		quirk = Quirk.GetQuirk(this); champion = true; recruitable = false;
		cycle = true; turn1 = true; CreateDrops();
	}
	
	public override TimedMethod[] AI () {
		if (turn1) {
			turn1 = false;
		    return Lecture();
		} else if (cycle && health < maxHP / 2) {
		    cycle = false;
			return Coffee();
		} else {
			System.Random rng = new System.Random();
			int num = rng.Next(10);
			if (num < 4) {
				return Attack();
			} else if (num < 6) {
				return Insult();
			} else if (num < 8) {
				return HighHorse();
			} else {
				return Lecture();
			}
		}
	}
	
	public TimedMethod[] Attack() {
		Attacks.SetAudio("Slap", 6);
		return new TimedMethod[] {new TimedMethod(60, "Log", new object[] {ToString() + " attacked"}),
		    new TimedMethod(0, "Audio", new object[] {"Small Swing"}),
		    new TimedMethod(0, "StagnantAttack", new object[] {false, 3, 3, GetAccuracy(), true, true, false})};
	}
	
	public TimedMethod[] Coffee() {
		status.Coffee();
		return new TimedMethod[] {new TimedMethod(0, "Audio", new object[] {"Drink"}), new TimedMethod(0, "AudioAfter", new object[] {"Fire", 15}),
		    new TimedMethod(60, "Log", new object[] {ToString() + " drank the coffee"})};
	}
	
	public TimedMethod[] HighHorse () {
		TimedMethod evadePart = new TimedMethod("Null");
		if (!GetGooped()) {
			evadePart = new TimedMethod(0, "CharLogSprite", new object[] {"4", Party.enemySlot - 1, "evasion", false});
		}
		GainPower(3); GainGuard(4); GainEvasion(4);
		return new TimedMethod[] {new TimedMethod(0, "Audio", new object[] {"Whistle"}),
		    new TimedMethod(60, "Log", new object[] {ToString() + " gloated about her position"}),
			new TimedMethod(0, "CharLogSprite", new object[] {"3", Party.enemySlot - 1, "power", false}),
			new TimedMethod(0, "CharLogSprite", new object[] {"4", Party.enemySlot - 1, "guard", false}), evadePart};
	}
	
	public TimedMethod[] Insult () {
		TimedMethod[] statPart = new TimedMethod[] {new TimedMethod("Null"), new TimedMethod("Null")};
		if (Attacks.EvasionCycle(this, Party.GetPlayer())) {
			Party.GetPlayer().GainGuard(-6);
			Party.GetPlayer().GainCharge(-6);
			statPart[0] = new TimedMethod(0, "CharLogSprite", new object[] {"-6", Party.playerSlot - 1, "charge", true});
			statPart[1] = new TimedMethod(0, "CharLogSprite", new object[] {"-6", Party.playerSlot - 1, "guard", true});
		}
		GainCharge(5);
		return new TimedMethod[] {new TimedMethod(0, "Audio", new object[] {"Poison"}),
		    new TimedMethod(60, "Log", new object[] {ToString() + " called you a failure, worthless, ect"}),
			new TimedMethod(0, "CharLogSprite", new object[] {"5", Party.enemySlot - 1, "charge", false}), statPart[0], statPart[1]};
	}
	
	public override TimedMethod[] Lecture () {
		TimedMethod[] sleepPart;
		TimedMethod[] totalSleep = new TimedMethod[] {new TimedMethod("Null"), new TimedMethod("Null"), new TimedMethod("Null"),
	    	new TimedMethod("Null"), new TimedMethod("Null"), new TimedMethod("Null"), new TimedMethod("Null"), new TimedMethod("Null"),
			new TimedMethod("Null"), new TimedMethod("Null"), new TimedMethod("Null"), new TimedMethod("Null"), new TimedMethod("Null"),
			new TimedMethod("Null"), new TimedMethod("Null"), new TimedMethod("Null")};
		int index = 0;
		foreach (Character c in Party.members) {
			if (c != null && c.GetAlive() && Attacks.EvasionCycle(this, c)) {
		        sleepPart = c.status.Sleep();
				Status.NullifyAttack(c); Status.NullifyDefense(c);
				totalSleep[index + 2] = new TimedMethod(0, "CharLogSprite", new object[] {"atk reset", c.partyIndex, "nullAttack",  true});
				totalSleep[index + 3] = new TimedMethod(0, "CharLogSprite", new object[] {"def reset", c.partyIndex, "nullDefense",  true});
	    	} else {
    			sleepPart = new TimedMethod[] {new TimedMethod("Null"), new TimedMethod("Null")};
		    }
			totalSleep[index] = sleepPart[0];
			totalSleep[index + 1] = sleepPart[1];
			index += 4;
		}
		return new TimedMethod[] {new TimedMethod(0, "Audio", new object[] {"Filibuster"}),
		    new TimedMethod(60, "Log", new object[] {ToString() + " lectured"}), totalSleep[0], totalSleep[1], totalSleep[2], totalSleep[3],
			    totalSleep[4], totalSleep[5], totalSleep[6], totalSleep[7], totalSleep[8], totalSleep[9], totalSleep[10], totalSleep[11],
				totalSleep[12], totalSleep[13], totalSleep[14], totalSleep[15]};
	}
	
	public override void CreateDrops() {
		drops = ItemDrops.GuaranteedDrop(new Item[] {new Textbook(), new Pencil(), new Textbook(), new Pencil(), new Textbook(), new Pencil(),
		    new Textbook(), new Pencil(), new Textbook(), new Pencil(), new USB(), new VotedBadge()}, ItemDrops.Amount(1, 4), new Coffee());
	}
	
	public override Item[] Loot () {
		System.Random rnd = new System.Random();
		int sp = 4 + rnd.Next(8);
		Party.UseSP(sp * -1);
		Item[] dropped = drops;
		drops = new Item[0];
		return dropped;
	}
	
	
	public override string[] CSDescription () {
		return new string[] {"Tenured - An instructor that has more job security",
		    "They're much more agressive than regular instructors and have coffee privilages",
		    "This battle could get very out of hand if it isn't ended quickly"};
	}
}