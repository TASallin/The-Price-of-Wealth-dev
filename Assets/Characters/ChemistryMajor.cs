public class ChemistryMajor : Character {
	
	int flasks;
	
	public ChemistryMajor() {
		health = 17; maxHP = 17; strength = 4; power = 0; charge = 0; defense = 0; guard = 0;
		baseAccuracy = 11; accuracy = 11; dexterity = 2; evasion = 0; type = "Chemistry Major"; passive = new Catalyst(this);
		quirk = Quirk.GetQuirk(this); special = new Brew(); special2 = new UnstableLiquid(); player = false; champion = false; recruitable = true;
		flasks = 4; CreateDrops(); attackEffect = "chance to poison";
	}
	
	public override TimedMethod[] AI () {
		if (flasks == 0) {
			return Attack();
		} else {
			System.Random rng = new System.Random();
			int num = rng.Next(10);
			flasks--;
			if (num < 4) {
				return Acid();
			} else if (num < 7) {
				return Poison();
			} else {
				return Slime();
			}
		}
	}
	
	public override TimedMethod[] BasicAttack() {
		System.Random rng = new System.Random();
		TimedMethod[] poisonPart;
		if (rng.Next(10) < 3 && Attacks.EvasionCheck(Party.GetEnemy(), GetAccuracy())) {
			poisonPart = Party.GetEnemy().status.Poison(1);
		} else {
			poisonPart = new TimedMethod[] {new TimedMethod("Null"), new TimedMethod("Null")};
		}
		Attacks.SetAudio("Acid", 10);
		TimedMethod[] attackPart;
		if (Party.BagContains(new Metronome())) {
			attackPart = Attacks.Attack(this, Party.GetEnemy(), strength + 3, strength + 3, GetAccuracy(), true, true, false);
		} else {
		    attackPart = Attacks.Attack(this, Party.GetEnemy());
		}
		TimedMethod[] moves = new TimedMethod[attackPart.Length + 4];
		moves[0] = new TimedMethod(0, "Audio", new object[] {"Missile"});
		moves[1] = new TimedMethod(0, "AudioAfter", new object[] {"Glass Break", 20});
		attackPart.CopyTo(moves, 2);
		moves[moves.Length - 2] = poisonPart[0];
		moves[moves.Length - 1] = poisonPart[1];
		return moves;
	}
	
	public TimedMethod[] Acid () {
		Attacks.SetAudio("Acid", 10);
		return new TimedMethod[] {new TimedMethod(60, "Log", new object[] {ToString() + " threw an acid solution"}),
		    new TimedMethod(0, "Audio", new object[] {"Missile"}), new TimedMethod(0, "AudioAfter", new object[] {"Glass Break", 20}),
	    	new TimedMethod(0, "StagnantAttack", new object[] {false, 6, 6, GetAccuracy(), true, true, false})};
	}
	
	public TimedMethod[] Poison () {
		TimedMethod[] poisonPart;
		Attacks.SetAudio("Acid", 10);
		if (Attacks.EvasionCheck(Party.GetPlayer(), GetAccuracy())) {
			poisonPart = Party.GetPlayer().status.Poison(1);
		} else {
			poisonPart = new TimedMethod[] {new TimedMethod("Null"), new TimedMethod("Null")};
		}
		return new TimedMethod[] {new TimedMethod(60, "Log", new object[] {ToString() + " threw a toxic solution"}),
		    new TimedMethod(0, "Audio", new object[] {"Oil"}),
	    	new TimedMethod(0, "StagnantAttack", new object[] {false, 1, 1, GetAccuracy(), true, true, false}), poisonPart[0], poisonPart[1]};
    }
	
	public TimedMethod[] Slime () {
		TimedMethod[] goopPart;
		TimedMethod[] blindPart;
		Attacks.SetAudio("Slime", 10);
		if (Attacks.EvasionCheck(Party.GetPlayer(), GetAccuracy())) {
			goopPart = Party.GetPlayer().status.Goop();
			blindPart = Party.GetPlayer().status.Blind(3);
		} else {
			goopPart = new TimedMethod[] {new TimedMethod("Null"), new TimedMethod("Null")};
			blindPart = new TimedMethod[] {new TimedMethod("Null"), new TimedMethod("Null")};
		}
		return new TimedMethod[] {new TimedMethod(60, "Log", new object[] {ToString() + " threw a slime solution"}),
		    new TimedMethod(0, "Audio", new object[] {"Missile"}), new TimedMethod(0, "AudioAfter", new object[] {"Glass Break", 20}), 
	    	new TimedMethod(0, "StagnantAttack", new object[] {false, 1, 1, GetAccuracy(), true, true, false}),
			goopPart[0], goopPart[1], blindPart[0], blindPart[1]};
	}
	
	public TimedMethod[] Attack () {
		Attacks.SetAudio("Blunt Hit", 6);
		return new TimedMethod[] {new TimedMethod(60, "Log", new object[] {ToString() + " attacked normally"}),
		    new TimedMethod(0, "Audio", new object[] {"Small Swing"}),
	    	new TimedMethod(0, "StagnantAttack", new object[] {false, 1, 1, GetAccuracy(), true, true, false})};
	}
	
	public override string SpecificBarText () {
		return "flasks: " + flasks.ToString();
	}
	
	public override void OnRecruit () {
		Party.AddLoot (new Flask());
	}
	
	public override void CreateDrops() {
		drops = ItemDrops.FromPool(new Item[] {new Flask(), new Flask(), new Flask(), new ToxicSolution(), new ToxicSolution(), new ToxicSolution(),
		    new MysterySolution(), new SlimeGoo(), new MysteryGoo()}, ItemDrops.Amount(1, 2));
	}
	
	public override Item[] Loot () {
		System.Random rng = new System.Random();
		int sp = 2 + rng.Next(3);
		Party.UseSP(sp * -1);
	    Item[] dropped = drops;
		drops = new Item[0];
		return dropped;
	}
	
	public override string[] CSDescription () {
		return new string[] {"Chemistry Major - They want to be Walter White",
            "They throw flasks of liquid and bad things happen",
		    "They've also huffed too much poison so that it doesn't effect them anymore",
			"They only have so many flasks, after which their regular attacks are weak"};
        //They've also gotten so used to poison that it doesn't effect them anymore
    }
}