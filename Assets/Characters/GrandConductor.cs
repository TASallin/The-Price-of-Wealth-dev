public class GrandConductor : Conductor {
	
	public GrandConductor() {
		health = 25; maxHP = 25; dexterity = 1; type = "Grand Conductor"; passive = new Directive(this);
		champion = true; cycle = 0; CreateDrops();
	}
	
	public override TimedMethod[] AI () {
		System.Random rng = new System.Random();
		int num = rng.Next(4);
		if (Party.enemyCount <= 2) {
			return Summon();
		}
		if (cycle == 0) {
			cycle++;
			return CountIn();
		} else if (num == 0) {
			return Forte();
		} else if (num == 1) {
			return Piano();
		} else if (num == 2) {
			return Allegro();
		} else {
			return Largo();
		}
	}
	
	public override TimedMethod[] CountIn () {
		foreach (Character c in Party.enemies) {
			if (c != null && c.GetAlive()) {
				c.GainCharge(10);
			}
		}
		return new TimedMethod[] {new TimedMethod(0, "Audio", new object[] {"ConductorCount"}),  new TimedMethod(0, "Audio", new object[] {"Button"}),
		    new TimedMethod(0, "AudioAfter", new object[] {"Button", 15}),  new TimedMethod(0, "AudioAfter", new object[] {"Button", 15}),
			new TimedMethod(0, "AudioAfter", new object[] {"Button", 15}),
		    new TimedMethod(60, "Log", new object[] {ToString() + " initiated the performance. All charge up"}),
			new TimedMethod(15, "CharLogSprite", new object[] {"10", 0, "charge", false}),
			new TimedMethod(15, "CharLogSprite", new object[] {"10", 1, "charge", false}),
			new TimedMethod(15, "CharLogSprite", new object[] {"10", 2, "charge", false}),
	        new TimedMethod(15, "CharLogSprite", new object[] {"10", 3, "charge", false})};
	}
	
	public TimedMethod[] Forte() {
		foreach (Character c in Party.enemies) {
			if (c != null && c.GetAlive()) {
				c.GainPower(3);
			}
		}
		return new TimedMethod[] { new TimedMethod(0, "Audio", new object[] {"Trumpet"}),
		    new TimedMethod(60, "Log", new object[] {ToString() + " called upon a forte. All power up"}),
			new TimedMethod(15, "CharLogSprite", new object[] {"3", 0, "power", false}),
			new TimedMethod(15, "CharLogSprite", new object[] {"3", 1, "power", false}),
			new TimedMethod(15, "CharLogSprite", new object[] {"3", 2, "power", false}),
	        new TimedMethod(15, "CharLogSprite", new object[] {"3", 3, "power", false})};
	}
	
	public TimedMethod[] Piano() {
		foreach (Character c in Party.enemies) {
			if (c != null && c.GetAlive()) {
				c.Heal(3);
			}
		}
		return new TimedMethod[] { new TimedMethod(0, "Audio", new object[] {"Piano"}),
		    new TimedMethod(60, "Log", new object[] {ToString() + " called upon a piano. Team was healed"}),
			new TimedMethod(15, "CharLogSprite", new object[] {"3", 0, "healing", false}),
			new TimedMethod(15, "CharLogSprite", new object[] {"3", 1, "healing", false}),
			new TimedMethod(15, "CharLogSprite", new object[] {"3", 2, "healing", false}),
	        new TimedMethod(15, "CharLogSprite", new object[] {"3", 3, "healing", false})};
	}
	
	public TimedMethod[] Allegro() {
		foreach (Character c in Party.enemies) {
			if (c != null && c.GetAlive()) {
				c.GainEvasion(5); c.GainAccuracy(1);
			}
		}
		return new TimedMethod[] {new TimedMethod(0, "Audio", new object[] {"Allegro"}),
		    new TimedMethod(60, "Log", new object[] {ToString() + " called upon an allegro. All speed up"}),
			new TimedMethod(15, "CharLogSprite", new object[] {"5", 0, "evasion", false}),
			new TimedMethod(15, "CharLogSprite", new object[] {"5", 1, "evasion", false}),
			new TimedMethod(15, "CharLogSprite", new object[] {"5", 2, "evasion", false}),
	        new TimedMethod(15, "CharLogSprite", new object[] {"5", 3, "evasion", false}),
			new TimedMethod(15, "CharLogSprite", new object[] {"1", 0, "accuracy", false}),
			new TimedMethod(15, "CharLogSprite", new object[] {"1", 1, "accuracy", false}),
			new TimedMethod(15, "CharLogSprite", new object[] {"1", 2, "accuracy", false}),
	        new TimedMethod(15, "CharLogSprite", new object[] {"1", 3, "accuracy", false})};
	}
	
	public TimedMethod[] Largo() {
		foreach (Character c in Party.enemies) {
			if (c != null && c.GetAlive()) {
				c.GainGuard(3); c.GainDefense(1);
			}
		}
		return new TimedMethod[] { new TimedMethod(0, "Audio", new object[] {"Violin"}),
		    new TimedMethod(60, "Log", new object[] {ToString() + " called upon a largo. All defense up"}),
			new TimedMethod(15, "CharLogSprite", new object[] {"3", 0, "guard", false}),
			new TimedMethod(15, "CharLogSprite", new object[] {"3", 1, "guard", false}),
			new TimedMethod(15, "CharLogSprite", new object[] {"3", 2, "guard", false}),
	        new TimedMethod(15, "CharLogSprite", new object[] {"3", 3, "guard", false}),
			new TimedMethod(15, "CharLogSprite", new object[] {"1", 0, "defense", false}),
			new TimedMethod(15, "CharLogSprite", new object[] {"1", 1, "defense", false}),
			new TimedMethod(15, "CharLogSprite", new object[] {"1", 2, "defense", false}),
	        new TimedMethod(15, "CharLogSprite", new object[] {"1", 3, "defense", false})};
	}
	
	public TimedMethod[] Summon() {
	System.Random rng = new System.Random();
		int seed;
		Character current;
		for (int i = 0; i < 1; i++) {
			seed = rng.Next(6);
			if (seed == 0) {
				current = new DanceMajor();
			} else if (seed == 1) {
				current = new PizzaCultist();
			} else if (seed == 2) {
				current = new HistoryMajor();
			} else {
				current = new MusicMajor();
			}
			current.SetRecruitable(false);
			Party.AddEnemy(current);
		}
		return new TimedMethod[] { new TimedMethod(0, "Audio", new object[] {"Recruit"}),
		    new TimedMethod(60, "Log", new object[] {ToString() + " led more performers to the scene"})};
    }
	
	public override void CreateDrops () {
		drops = ItemDrops.GuaranteedDrop(new Item[] {new Metronome()}, ItemDrops.Amount(1, 2), new Baton()); 
	}
	
	public override Item[] Loot () {
		System.Random rng = new System.Random();
		int sp = 6 + rng.Next(5);
		Party.UseSP(sp * -1);
		Item[] dropped = drops;
		drops = new Item[0];
		return dropped;
	}
	
	public override string[] CSDescription () {
		return new string[] {"Grand Conductor - Forget the neverending underlings, just take out this one",
		    "If we can power up while they switch positions it'll be better for us"};
	}
}