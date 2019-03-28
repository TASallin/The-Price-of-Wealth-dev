public class DanceMajor : Character {
	
	public DanceMajor() {
		health = 16; maxHP = 16; strength = 2; power = 0; charge = 0; defense = 0; guard = 0;
		baseAccuracy = 14; accuracy = 14; dexterity = 5; evasion = 0; type = "Dance Major"; passive = new Footwork(this);
		quirk = Quirk.GetQuirk(this); special = new Tumble(); special2 = new Lunge();
		player = false; champion = false; recruitable = true; CreateDrops(); attackEffect = "gain 3 evasion";
	}
	
	public override TimedMethod[] AI () {
		System.Random rng = new System.Random();
		int num = rng.Next(10);
		if (num < 5) {
			return Attack();
		} else if (num < 8) {
			return Dodge();
		} else {
			return Finish();
		}
	}
	
	public override TimedMethod[] BasicAttack() {
		TimedMethod[] attackPart;
		Attacks.SetAudio("Knife", 6);
		if (Party.BagContains(new Metronome())) {
			attackPart = Attacks.Attack(this, Party.GetEnemy(), strength + 3, strength + 3, GetAccuracy(), true, true, false);
		} else {
		    attackPart = Attacks.Attack(this, Party.GetEnemy());
		}
		GainEvasion(3);
		TimedMethod evadePart = new TimedMethod("Null");
		if (!GetGooped()) {
			evadePart = new TimedMethod(0, "CharLogSprite", new object[] {"3", Party.playerSlot - 1, "evasion", true});
		}
		TimedMethod[] moves = new TimedMethod[attackPart.Length + 2];
		moves[0] = new TimedMethod(0, "Audio", new object[] {"Small Swing"});
		moves[1] = evadePart;
		attackPart.CopyTo(moves, 2);
		return moves;
	}
	
	public TimedMethod[] Attack() {
		GainEvasion(4);
		TimedMethod evadePart = new TimedMethod("Null");
		if (!GetGooped()) {
			evadePart = new TimedMethod(0, "CharLogSprite", new object[] {"4", Party.enemySlot - 1, "evasion", false});
		}
		Attacks.SetAudio("Knife", 6);
		return new TimedMethod[] {new TimedMethod(60, "Log", new object[] {ToString() + " made a darting attack"}),
		    new TimedMethod(0, "Audio", new object[] {"Small Swing"}),
		    new TimedMethod(0, "StagnantAttack", new object[] {false, 2, 4, GetAccuracy(), true, true, false}),evadePart};
	}

	public TimedMethod[] Dodge() {
		GainEvasion(10);
		TimedMethod evadePart = new TimedMethod("Null");
		if (!GetGooped()) {
			evadePart = new TimedMethod(0, "CharLogSprite", new object[] {"3", Party.enemySlot - 1, "evasion", false});
		}
		return new TimedMethod[] {new TimedMethod(0, "AudioAfter", new object[] {"Big Swing", 30}),
		    new TimedMethod(60, "Log", new object[] {ToString() + " is dodging"}), evadePart};
	}
	
	public TimedMethod[] Finish() {
		int atk = evasion;
		evasion = 0;
		Attacks.SetAudio("Blunt Hit", 20);
		return new TimedMethod[] {new TimedMethod(60, "Log", new object[] {ToString() + " used their positioning for a big attack"}),
		    new TimedMethod(0, "Audio", new object[] {"Big Swing"}),
		    new TimedMethod(0, "StagnantAttack", new object[] {false, atk, atk, GetAccuracy(), true, true, false})};
	}
	
	public override void CreateDrops() {
		drops = ItemDrops.GuaranteedDrop(new Item[] {new Sanitizer(), new Smartphone(), new Whistle(), new Smartphone(), new Smartphone()},
		    ItemDrops.Amount(1, 2), new Heels());
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
		return new string[] {"Dance Major - This is a legit major",
            "Of everyone we could encounter, they seem to be the hardest to hit",
		    "They gain evasion with each passing turn. They can also convert it into attack power"};
	}
}