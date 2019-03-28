public class MusicMajor : Character {
	
	public MusicMajor () {
		health = 16; maxHP = 16; strength = 3; power = 0; charge = 0; defense = 0; guard = 0; 
		baseAccuracy = 14; accuracy = 14; dexterity = 4; evasion = 0; type = "Music Major"; passive = new Performance(this);
		quirk = Quirk.GetQuirk(this); special = new Trumpet(); special2 = new Warsong(); 
		player = false; champion = false; recruitable = true; CreateDrops(); attackEffect = "enemy loses 1 charge";
	}	
	
	public override TimedMethod[] AI() {
		System.Random rng = new System.Random();
		int num = rng.Next(10);
		if (num < 4) {
			return Trumpet();
		} else if (num < 7) {
			return Keyboard();
		} else {
			return Violin();
		}
	}
	
	public override TimedMethod[] BasicAttack() {
		TimedMethod statPart = new TimedMethod("Null");
		if (Attacks.EvasionCheck(Party.GetEnemy(), GetAccuracy())) {
			Party.GetEnemy().GainCharge(-1);
			statPart = new TimedMethod(0, "CharLogDelay", new object[] {"-1", Party.enemySlot - 1, "charge", false});
		}
		TimedMethod[] attackPart;
		Attacks.SetAudio("Piano Hit", 10);
		if (Party.BagContains(new Metronome())) {
			attackPart = Attacks.Attack(this, Party.GetEnemy(), strength + 3, strength + 3, GetAccuracy(), true, true, false);
		} else {
		    attackPart = Attacks.Attack(this, Party.GetEnemy());
		}
		TimedMethod[] moves = new TimedMethod[attackPart.Length + 2];
		moves[0] = new TimedMethod(0, "Audio", new object[] {"Big Swing"});
		moves[moves.Length - 1] = statPart;
		attackPart.CopyTo(moves, 1);
		return moves;
	}
	
	public TimedMethod[] Trumpet() {
		if (Attacks.EvasionCycle(this, Party.GetPlayer())) {
		    Status.NullifyAttack(Party.GetPlayer());
    		if (Party.playerCount > 1 && !Party.GetPlayer().GetGooped()) {
	    		int steps = new System.Random().Next(Party.playerCount - 1);
		    	for (int i = 0; i < 4; i++) {
			    	if (Party.members[i] != null && Party.members[i].GetAlive() && i != Party.playerSlot - 1) {
				    	if (steps == 0) {
					    	return new TimedMethod[] {new TimedMethod(60, "Log", new object[] {ToString() 
						        + " played a trumpet"}), new TimedMethod(60, "SwitchTo", new object[] {i + 1}),
								new TimedMethod(0, "CharLogSprite", new object[] {"atk reset", Party.playerSlot - 1, "nullAttack", true})};
    					} else {
	    					steps--;
		    			}
			    	}
    			}
     		}
	    	return new TimedMethod[] {new TimedMethod(60, "Log", new object[] {ToString() + " played a trumpet"}),
			    new TimedMethod(0, "Audio", new object[] {"Trumpet"}), new TimedMethod(0, "AudioAfter", new object[] {"Nullify", 15}),
				new TimedMethod(0, "CharLogSprite", new object[] {"atk reset", Party.playerSlot - 1, "nullAttack", true})};
		} else {
			return new TimedMethod[] {new TimedMethod(60, "Log", new object[] {ToString() + " played a trumpet. Ineffective"}),
			    new TimedMethod(0, "Audio", new object[] {"Trumpet"})};
		}
	}
	
	public TimedMethod[] Keyboard() {
		Attacks.SetAudio("Piano Hit", 10);
		return new TimedMethod[] {new TimedMethod(60, "Log", new object[] {ToString() + " swung a keyboard"}),
		    new TimedMethod(0, "Audio", new object[] {"Big Swing"}),
		    new TimedMethod(0, "StagnantAttack", new object[] {false, 6, 6, GetAccuracy(), true, true, false})};
	}
	
	public TimedMethod[] Violin() {
		TimedMethod[] sleepPart;
		if (Attacks.EvasionCycle(this, Party.GetPlayer())) {
		    sleepPart = Party.GetPlayer().status.Sleep();
		} else {
			sleepPart = new TimedMethod[] {new TimedMethod(60, "Log", new object[] {"Ineffective"}), new TimedMethod("Null")};
		}
		return new TimedMethod[] {new TimedMethod(0, "Audio", new object[] {"Violin"}),
		    new TimedMethod(60, "Log", new object[] {ToString() + " played a slow violin piece"}), sleepPart[0], sleepPart[1]};
	}
	
	public override void CreateDrops() {
		drops = ItemDrops.GuaranteedDrop(new Item[] {new Tuba(), new Tuba(), new Heels(), new Smartphone(), new Rice(), new Metronome()},
		    ItemDrops.Amount(1, 2), new Tuba());	
	}
	
	public override Item[] Loot () {
		System.Random rnd = new System.Random();
		int sp = 2 + rnd.Next(3);
		Party.UseSP(sp * -1);
		Item[] dropped = drops;
		drops = new Item[0];
		return dropped;
	}
	
	public override string[] CSDescription () {
		return new string[] {"Music Major - Who would ever want to carry a keyboard and a giant trumpet-looking thing at once?",
		    "The most important thing to know is that REEEE = lose all your attack buffs. And you get forcibly pushed to the backline",
		    "And don't get lulled to sleep by their violin"};
	}
}