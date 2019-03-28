public class Janitor : Character {
	
	public Janitor() {
		health = 24; maxHP = 24; strength = 2; power = 0; charge = 0; defense = 0; guard = 0;
		baseAccuracy = 12; accuracy = 12; dexterity = 2; evasion = 0; type = "Janitor"; passive = new Passive(this);
		quirk = Quirk.GetQuirk(this); special = null; player = false; champion = false; recruitable = false; CreateDrops();
	}
	
	public override TimedMethod[] AI () {
		if ((GetPoisoned() || GetGooped() || GetBlinded()) && new System.Random().Next(2) == 0) {
			return Clean();
		}
		System.Random rng = new System.Random();
		int num = rng.Next(10);
		if (num < 4) {
			return Broom();
		} else if (num < 7) {
			return Chemicals();
		} else {
			return Exhaust();
		}
	}
	
	public TimedMethod[] Clean() {
		TimedMethod[] statusPart = new TimedMethod[] {new TimedMethod("Null"), new TimedMethod("Null"), new TimedMethod("Null")};
		if (GetGooped()) {
			statusPart[0] = new TimedMethod(0, "CharLogSprite", new object[] {"Cleaned", Party.enemySlot - 1, "goop", false});
		}
		if (GetBlinded()) {
			statusPart[1] = new TimedMethod(0, "CharLogSprite", new object[] {"Restored", Party.enemySlot - 1, "blind", false});
		}
		if (GetPoisoned()) {
			statusPart[2] = new TimedMethod(0, "CharLogSprite", new object[] {"Cured", Party.enemySlot - 1, "poison", false});
		}
		status.gooped = false;
		status.blinded = 0;
		status.poisoned = 0;
		return new TimedMethod[] {new TimedMethod(0, "Audio", new object[] {"Clean"}),
	    	new TimedMethod(60, "Log", new object[] {ToString() + " cleaned negative effects"}), statusPart[0], statusPart[1], statusPart[2]};
	}
	
	public TimedMethod[] Broom() {
		Attacks.SetAudio("Blunt Hit", 10);
		return new TimedMethod[] {new TimedMethod(60, "Log", new object[] {ToString() + " swung a broom"}),
		    new TimedMethod(0, "Audio", new object[] {"Big Swing"}),
		    new TimedMethod(0, "Attack", new object[] {false})};
	}
	
	public TimedMethod[] Chemicals() {
		TimedMethod[] poisonPart;
		TimedMethod[] statPart = new TimedMethod[] {new TimedMethod("Null"), new TimedMethod("Null")};
		if (Attacks.EvasionCycle(this, Party.GetPlayer())) {
			poisonPart = Party.GetPlayer().status.StackPoison(1);
			Party.GetPlayer().GainPower(-1);
			Party.GetPlayer().GainDefense(-1);
			statPart[0] = new TimedMethod(0, "CharLogSprite", new object[] {"-1", Party.playerSlot - 1, "power", true});
			statPart[1] = new TimedMethod(0, "CharLogSprite", new object[] {"-1", Party.playerSlot - 1, "defense", true});
		} else {
			poisonPart = new TimedMethod[] {new TimedMethod(60, "Log", new object[] {"They missed"}), new TimedMethod("Null")};
		}
		return new TimedMethod[] {new TimedMethod(60, "Log", new object[] {ToString() + " sprayed toxic, weakening chemicals"}),
		    new TimedMethod(0, "Audio", new object[] {"Acid"}),
			poisonPart[0], poisonPart[1], statPart[0], statPart[1]};
	}
	
	public TimedMethod[] Exhaust() {
		TimedMethod[] poisonPart;
		TimedMethod[] blindPart;
		if (Attacks.EvasionCycle(this, Party.GetPlayer())) {
			poisonPart = Party.GetPlayer().status.StackPoison(1);
			blindPart = Party.GetPlayer().status.Blind(5);
		} else {
			poisonPart = new TimedMethod[] {new TimedMethod(60, "Log", new object[] {"They missed"}), new TimedMethod("Null")};
			blindPart = new TimedMethod[] {new TimedMethod("Null"), new TimedMethod("Null")};
		}
		return new TimedMethod[] {new TimedMethod(60, "Log", new object[] {ToString() + " released exhaust fumes"}),
 		    new TimedMethod(0, "Audio", new object[] {"Fumes"}), poisonPart[0], poisonPart[1], blindPart[0], blindPart[1]};
	}
	
	public override void CreateDrops() {
		drops = ItemDrops.GuaranteedDrop(new Item[] {new Sanitizer(), new SlimeGoo(), new ToxicSolution(), new Antibiotics(), new Milk()},
		   ItemDrops.Amount(1, 3), new Sanitizer());
	}
	
	public override Item[] Loot () {
	    System.Random rng = new System.Random();
		int sp = 4 + rng.Next(4);
		Party.UseSP(sp * -1);
		Item[] dropped = drops;
		drops = new Item[0];
		return dropped;
    }	
	
	public override string[] CSDescription () {
		return new string[] {"Janitor - Tired of cleaning up after you",
            "Works with poisons and other debuffs",
		    "Unlike other poisons, the janitors' stack in severity as they are used",
		    "Since switching out inexcplicably halts the toxins, utilize that if it gets too severe"};
	}
}