public class HistoryMajor : Character {
    
    public HistoryMajor () {
		health = 22; maxHP = 22; strength = 4; power = 0; charge = 0; defense = 0; guard = 0; 
		baseAccuracy = 11; accuracy = 11; dexterity = 1; evasion = 0; type = "History Major"; passive = new Armored(this);
		quirk = Quirk.GetQuirk(this); special = new Joust(); special2 = new Tactics(); 
		player = false; champion = false; recruitable = true; CreateDrops(); attackEffect = "gain 1 guard";
	}	
	
	public override TimedMethod[] AI () {
		System.Random rng = new System.Random();
		int num = rng.Next(10);
		if (num < 5) {
			return Sword ();
		} else if (num < 8) {
			return Shield ();
		} else {
			return Tactics ();
		}
	}
	
	public override TimedMethod[] BasicAttack() {
		TimedMethod[] attackPart;
		Attacks.SetAudio("Sword", 10);
		if (Party.BagContains(new Metronome())) {
			attackPart = Attacks.Attack(this, Party.GetEnemy(), strength + 3, strength + 3, GetAccuracy(), true, true, false);
		} else {
		    attackPart = Attacks.Attack(this, Party.GetEnemy());
		}
		GainGuard(1);
		TimedMethod[] moves = new TimedMethod[attackPart.Length + 2];
		moves[0] = new TimedMethod(0, "Audio", new object[] {"Big Swing"});
		moves[1] = new TimedMethod(0, "CharLogSprite", new object[] {"1", Party.playerSlot - 1, "guard", true});
		attackPart.CopyTo(moves, 2);
		return moves;
	}
	
	public TimedMethod[] Sword () {
		Attacks.SetAudio("Sword", 10);
		return new TimedMethod[] {new TimedMethod(60, "Log", new object[] {ToString() + " swung the actual sword"}),
		    new TimedMethod(0, "Audio", new object[] {"Big Swing"}),
		    new TimedMethod(0, "Attack", new object[] {false})};
	}
	
	public TimedMethod[] Shield () {
		int amount = new System.Random().Next(4, 9);
		GainGuard(amount);
		Attacks.SetAudio("Metal Hit", 20);
		return new TimedMethod[] {new TimedMethod(60, "Log", new object[] {ToString() + " shield bashed"}),
		    new TimedMethod(0, "Audio", new object[] {"Big Swing"}),
		    new TimedMethod(0, "StagnantAttack", new object[] {false, 1, 3, GetAccuracy(), true, true, false}),
			new TimedMethod(0, "CharLogSprite", new object[] {amount.ToString(), Party.enemySlot - 1, "guard", false})};
	}
	
	public TimedMethod[] Tactics () {
		foreach (Character c in Party.enemies) {
			if (c != null && c.GetAlive()) {
				c.GainAccuracy(2);
				c.GainEvasion(4);
			}
		}
		return new TimedMethod[] {new TimedMethod(0, "Audio", new object[] {"Metal Hit"}),
		    new TimedMethod(60, "Log", new object[] {ToString() + " Implemented tactics. Party's " 
		    + "evasion and accuracy increased"}),
			new TimedMethod(0, "CharLogSprite", new object[] {"2", 0, "accuracy", false}),
			new TimedMethod(0, "CharLogSprite", new object[] {"2", 1, "accuracy", false}),
			new TimedMethod(0, "CharLogSprite", new object[] {"2", 2, "accuracy", false}),
	        new TimedMethod(0, "CharLogSprite", new object[] {"2", 3, "accuracy", false}),
			new TimedMethod(0, "CharLogSprite", new object[] {"4", 0, "evasion", false}),
			new TimedMethod(0, "CharLogSprite", new object[] {"4", 1, "evasion", false}),
			new TimedMethod(0, "CharLogSprite", new object[] {"4", 2, "evasion", false}),
	        new TimedMethod(0, "CharLogSprite", new object[] {"4", 3, "evasion", false})};
	}

    public override void CreateDrops() {
	drops = ItemDrops.GuaranteedDrop(new Item[] {new Textbook(), new Curry(), new Pencil(), new PaperPlane()},
    	ItemDrops.Amount(1, 2), new Sword());
	}
	
	public override Item[] Loot () {
		System.Random rnd = new System.Random();
		int sp = 2 + rnd.Next(3);
		Party.UseSP(sp * -1);
		Item[] dropped = drops;
		drops = new Item[0];
		return dropped;
	}
	
	public override string[] CSDescription() {
		return new string[] {"History Major - Buying tons of swords and shields actually became useful"
		, "They're strong in defense, and they know some weird tactics things. Use an item if you can't beat them normally"};
        //All those medieval knight sets they've had in a glass case are finally being used
    }
}