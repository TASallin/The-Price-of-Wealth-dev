public class Doctor : Character {
	
	public Doctor() {
		health = 17; maxHP = 17; strength = 3; power = 0; charge = 0; defense = 0; guard = 0;
		baseAccuracy = 18; accuracy = 18; dexterity = 2; evasion = 0; type = "Doctor"; passive = new Vaccinated(this);
		quirk = Quirk.GetQuirk(this); special = null; player = false; champion = false; recruitable = false; CreateDrops();
	}
	
	public override TimedMethod[] AI () {
		System.Random rng = new System.Random();
		int num = rng.Next(10);
		if (num < 4) {
			return Heal();
		} else if (num < 7) {
			return Attack();
		} else {
			return Anasthesia();
		}
	}
	
	public TimedMethod[] Heal () {
	    int index = 0;
		int minHP = 255;
		for (int i = 0; i < 4; i++) {
			if (Party.enemies[i] != null && Party.enemies[i].GetHealth() < minHP) {
				minHP = Party.enemies[i].GetHealth();
				index = i;
			}
		}
		if (Party.enemies[index].GetAlive()) {
			Party.enemies[index].Heal(5);
			return new TimedMethod[] {new TimedMethod(0, "Audio", new object[] {"Heal"}),
			    new TimedMethod(60, "Log", new object[] {ToString() + " treated " + Party.enemies[index].ToString()}),
				new TimedMethod(0, "CharLogSprite", new object[] {"5", Party.enemySlot - 1, "healing", false})};
		} else {
			Party.enemies[index].SetAlive(true);
			Party.enemies[index].Heal(1);
			Party.enemyCount++;
			return new TimedMethod[] {new TimedMethod(0, "AudioAfter", new object[] {"Tazer", 20}),
			    new TimedMethod(60, "Log", new object[] {ToString() + " revived " + Party.enemies[index].ToString()}),
				new TimedMethod(0, "CharLogSprite", new object[] {"1", Party.enemySlot - 1, "healing", false})};
		}
	}
	
	public TimedMethod[] Attack() {
		Attacks.SetAudio("Knife", 6);
		return new TimedMethod[] {new TimedMethod(60, "Log", new object[] {ToString() + " used operating tools"}), 
		    new TimedMethod(0, "Audio", new object[] {"Small Swing"}),
		    new TimedMethod(0, "StagnantAttack", new object[] {false, 4, 8, GetAccuracy(), true, true, true})};
	}
	
	public TimedMethod[] Anasthesia() {
		if (Attacks.EvasionCycle(this, Party.GetPlayer())) {
			TimedMethod[] sleepPart = Party.GetPlayer().status.Sleep();
			return new TimedMethod[] {new TimedMethod(60, "Log", new object[] {ToString() + " injected anasthesia"}),
			    new TimedMethod(0, "Audio", new object[] {"Clean"}), sleepPart[0], sleepPart[1]}; 
		} else {
			return new TimedMethod[] {new TimedMethod(60, "Log", new object[] {ToString() + " missed anasthesia"}),
			    new TimedMethod(0, "Audio", new object[] {"Clean"})}; 
		}
	}
	
	public override void CreateDrops () {
		drops = ItemDrops.FromPool(new Item[] {new Defibrilator(), new Defibrilator(), new Antibiotics(), new Antibiotics(),
		    new Sanitizer(), new Sanitizer(), new Milk()}, ItemDrops.Amount(1, 2));
	}
	
	public override Item[] Loot () {
		System.Random rng = new System.Random();
		int sp = 3 + rng.Next(4);
		Party.UseSP(sp * -1);
		Item[] dropped = drops;
		drops = new Item[0];
		return dropped;
	}
	
	public override string[] CSDescription () {
		return new string[] {"Doctor - Always go for the healer first, right?",
            "Can and will drug you",
		    "They can put us to sleep as well to buy them more time for healing"};
	}
}