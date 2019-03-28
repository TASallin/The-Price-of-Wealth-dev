public class CulinaryMajor : Character {

    public CulinaryMajor() {
		health = 22; maxHP = 22; strength = 3; power = 0; charge = 0; defense = 0; guard = 0;
		baseAccuracy = 12; accuracy = 12; dexterity = 2; evasion = 0; type = "Culinary Major"; passive = new Cooking(this);
		quirk = Quirk.GetQuirk(this); special2 = new Feast(); special = new SpicyEscape();
		player = false; champion = false; recruitable = true; CreateDrops(); attackEffect = "recover from poison and goop";
	}
	
	public override TimedMethod[] AI () {
		System.Random rng = new System.Random();
		int num = rng.Next(10);
		if (num < 4) {
			return Attack();
		} else if (num == 4) {
			return Eat("steak");
		} else if (num == 5) {
			return Eat("fish");
		} else if (num == 6) {
			return Eat("bread");
		} else {
			return Feast();
		}
	}
	
	public override TimedMethod[] BasicAttack() {
		TimedMethod[] statusPart = new TimedMethod[] {new TimedMethod("Null"), new TimedMethod("Null")};
		if (Attacks.EvasionCheck(Party.GetEnemy(), GetAccuracy())) {
			if (GetPoisoned()) {
				statusPart[0] = new TimedMethod(0, "CharLogSprite", new object[] {"Cured", Party.playerSlot - 1,  "poison", true});
			}
			if (GetGooped()) {
				statusPart[1] = new TimedMethod(0, "CharLogSprite", new object[] {"Cleaned", Party.playerSlot - 1,  "goop", true});
			}
			status.poisoned = 0;
			status.gooped = false;
		}
		TimedMethod[] attackPart;
		Attacks.SetAudio("Knife", 20);
		if (Party.BagContains(new Metronome())) {
			attackPart = Attacks.Attack(this, Party.GetEnemy(), strength + 3, strength + 3, GetAccuracy(), true, true, false);
		} else {
		    attackPart = Attacks.Attack(this, Party.GetEnemy());
		}
		TimedMethod[] moves = new TimedMethod[attackPart.Length + 3];
		moves[0] = new TimedMethod(0, "Audio", new object[] {"Knife Throw"});
		moves[1] = statusPart[0];
		moves[2] = statusPart[1];
		attackPart.CopyTo(moves, 3);
		return moves;
	}
	
	public TimedMethod[] Attack () {
		Attacks.SetAudio("Knife", 20);
		return new TimedMethod[] {new TimedMethod(60, "Log", new object[] {ToString() + " threw a cooking knife"}),
		    new TimedMethod(0, "Audio", new object[] {"Knife Throw"}),
		    new TimedMethod(0, "Attack", new object[] {false})};
	}
	
	public TimedMethod[] Eat (string food) {
		string message = "";
		TimedMethod statPart = new TimedMethod("Null");
		switch (food) {
			case "steak": message = ToString() + " ate the steak. Attack increased + health up"; GainPower(1);
			    statPart = new TimedMethod(0, "CharLogSprite", new object[] {"1", Party.enemySlot - 1, "power", false}); 
			    break;
			case "fish": message = ToString() + " ate the fish. Accuracy increased + health up"; GainAccuracy(1);
			    statPart = new TimedMethod(0, "CharLogSprite", new object[] {"2", Party.enemySlot - 1, "accuracy", false}); 
			    break;
			case "bread": message = ToString() + " ate the bread. Defense increased + health up"; GainDefense(1);
			    statPart = new TimedMethod(0, "CharLogSprite", new object[] {"1", Party.enemySlot - 1, "defense", false}); 
			    break;
		}
		Heal(4);
		return new TimedMethod[] {new TimedMethod(0, "Audio", new object[] {"Eat"}), new TimedMethod(0, "AudioAfter", new object[] {"Heal", 30}),
		    new TimedMethod(60, "Log", new object[] {message}), 
			new TimedMethod(0, "CharLogSprite", new object[] {"4", Party.enemySlot - 1, "healing", false}), statPart};
	}
	
	public TimedMethod[] Feast () {
		foreach (Character c in Party.enemies) {
			if (c != null) {
				c.Heal(3);
			}
		}
		return new TimedMethod[] {new TimedMethod(60, "Log", new object[] {ToString() + " distributed snacks. Party health up"}),
		    new TimedMethod(0, "AudioAfter", new object[] {"Eat", 10}),
		    new TimedMethod(0, "AudioAfter", new object[] {"Heal", 30}),
			new TimedMethod(6, "CharLogSprite", new object[] {"3", 0, "healing", false}),
			new TimedMethod(6, "CharLogSprite", new object[] {"3", 1, "healing", false}),
			new TimedMethod(6, "CharLogSprite", new object[] {"3", 2, "healing", false}),
			new TimedMethod(6, "CharLogSprite", new object[] {"3", 3, "healing", false})};
	}
	
	public override void CreateDrops() {
		drops = ItemDrops.AnyFood(ItemDrops.Amount(1, 3));
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
		return new string[] {"Culinary Major - They're good at cooking and love eating",
		   "Don't question how they cook without any nearby heat source, especially in the middle of combat",
		   "Or their endless supply of food"};
		
	}
}