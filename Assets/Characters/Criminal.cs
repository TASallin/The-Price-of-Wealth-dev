public class Criminal : Character {
	
	bool running;
	
	public Criminal() {
	    health = 15; maxHP = 15; strength = 4; power = 0; charge = 0; defense = 0; guard = 0;
		baseAccuracy = 13; accuracy = 13; dexterity = 4; evasion = 0; type = "Criminal"; passive = new Passive(this);
		quirk = Quirk.GetQuirk(this); special = null; player = false; champion = false; recruitable = false;
        running = false; CreateDrops();
    }
	
	public override TimedMethod[] AI () {
		System.Random rng = new System.Random();
		int num = rng.Next(10);
		if (running) {
			return Run();
		} else if (num < 4) {
			return Steal();
		} else if (num < 7) {
			return Attack();
		} else {
			return Bluff();
		}
	}
	
	public TimedMethod[] Steal() {
		Attacks.SetAudio("Steal", 6);
		TimedMethod[] stealPart;
		if (Attacks.EvasionCheck(Party.GetPlayer(), GetAccuracy())) {
			stealPart = Party.StealItem();
		} else {
			stealPart = new TimedMethod[] {new TimedMethod(0, "Log", new object[] {""})};
		}
		return new TimedMethod[] {new TimedMethod(60, "Log", new object[] {ToString() + " tried to mug you"}),
		    new TimedMethod(0, "Audio", new object[] {"Small Swing"}),
		    new TimedMethod(0, "StagnantAttack", new object[] {false, 2, 5, GetAccuracy(), true, true, false}), stealPart[0]};
	}
	
	public TimedMethod[] Attack () {
		Attacks.SetAudio("knife", 6);
		return new TimedMethod[] {new TimedMethod(60, "Log", new object[] {ToString() + " used a knife"}),
		   new TimedMethod(0, "Audio", new object[] {"Small Swing"}),
	    	new TimedMethod(0, "StagnantAttack", new object[] {false, 6, 8, GetAccuracy(), true, true, false})};
	}
	
	public TimedMethod[] Bluff () {
	    if (Party.enemyCount == 1 && health < maxHP / 2 || drops.Length >= 4) {
			running = true;
			return new TimedMethod[] {new TimedMethod(60, "Log", new object[] {ToString() + " is running. You get the last move"}),
			    new TimedMethod(0, "Audio", new object[] {"Skip Turn"}),
				new TimedMethod(0, "CharLogSprite", new object[] {"SKIP", Party.enemySlot - 1, "skip",  false})};
		}
		TimedMethod bluffPart = new TimedMethod("Null");
		if (Attacks.EvasionCycle(this, Party.GetPlayer())) {
			Party.GetPlayer().GainCharge(-5);
			bluffPart = new TimedMethod(0, "CharLogSprite", new object[] {"-5", Party.playerSlot - 1,  "charge", true});
		}
		GainGuard(5);
		return new TimedMethod[] {new TimedMethod(0, "Audio", new object[] {"Skill2"}), new TimedMethod(0, "AudioAfter", new object[] {"Nullify", 15}),
			new TimedMethod(60, "Log", new object[] {ToString() + " bluffed. Guard up and opposing charge down"}),
			new TimedMethod(0, "CharLogSprite", new object[] {"5", Party.enemySlot - 1,  "guard", false}), bluffPart};
	}
	
	public TimedMethod[] Run () {
		if (GetGooped()) {
			running = false;
			status.gooped = false;
			return new TimedMethod[] {new TimedMethod(60, "Log", new object[] {ToString() + " is stuck! the escape plan failed"}),
			    new TimedMethod(0, "CharLogSprite", new object[] {"Cleaned", Party.enemySlot - 1,  "goop", false}),
			    new TimedMethod(0, "Audio", new object[] {"Clean"})};
		}
		return new TimedMethod[] {new TimedMethod(0, "Audio", new object[] {"Running"}),
		    new TimedMethod(60, "Log", new object[] {ToString() + " escaped..."}), new TimedMethod("Win")};
	}
	
	public override void CreateDrops() {
		drops = ItemDrops.FromPool(new Item[] {new Pizza(), new ProteinBar(), new Milk(), new Coffee(), new Textbook(), new Whistle(),
		    new Automatic(), new Tazer(), new Donut(), new Briefcase(), new Metronome(), new Tuba(), new USB(), new Antibiotics(),
	    	new MysteryGoo(), new MysterySolution(), new ToxicSolution()}, ItemDrops.Amount(1, 3));
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
		return new string[] {"Criminal - Steals your stuff and runs away like a coward",
		    "They can't run if affected by specific statuses, or if they're at 0 hp, so go for that"};
	}
}