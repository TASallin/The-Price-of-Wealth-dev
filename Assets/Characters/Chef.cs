public class Chef : Character {
    
	int cycle;
	
    public Chef() {
		health = 25; maxHP = 25; strength = 4; power = 0; charge = 0; defense = 0; guard = 0;
		baseAccuracy = 11; accuracy = 11; dexterity = 2; evasion = 0; type = "Chef"; passive = new Passive(this);
		quirk = Quirk.GetQuirk(this); special = null; player = false; champion = false; recruitable = false;
		cycle = 0; CreateDrops();
	}
	
	public override TimedMethod[] AI () {
		if (cycle == 0) {
			cycle++;
			return Prepare();
		} else if (cycle == 1) {
			cycle++;
			return Cook();
		} else {
		    System.Random rng = new System.Random();
	    	int num = rng.Next(10);
    		if (num < 5) {
		    	return Attack();
	    	} else if (num == 5) {
    			return Meal("eat");
		    } else if (num == 6) {
	    		return Meal("poison");
    		} else if (num == 7) {
			    return Meal("attack");
		    } else if (num == 8) {
	    		return Meal("defense");
    		} else {
			    return Meal("stun");
		    }
		}
	}
	
	public TimedMethod[] Prepare() {
	    return new TimedMethod[] {new TimedMethod(0, "Audio", new object[] {"Nullify"}), new TimedMethod(60, "Log", new object[] {
			ToString() + " took out a selection of ingredients, tools, and a portable stove"}),
			new TimedMethod(0, "CharLogSprite", new object[] {"SKIP", Party.enemySlot - 1, "skip", false})};
    }
	
	public TimedMethod[] Cook() {
		GainGuard(5);
	    return new TimedMethod[] {new TimedMethod(0, "Audio", new object[] {"Firewall"}),
		    new TimedMethod(60, "Log", new object[] {
			ToString() + " cooked all the food behind the stove"})};
    }
	
	
	public TimedMethod[] Attack () {
		Attacks.SetAudio("Metal Hit", 10);
		return new TimedMethod[] {new TimedMethod(60, "Log", new object[] {ToString() + " swung a frying pan"}),
		    new TimedMethod(0, "Audio", new object[] {"Big Swing"}),
			new TimedMethod(0, "Attack", new object[] {false})};
	}
	
	public TimedMethod[] Meal (string food) {
		string message = "";
		TimedMethod[] statusPart = new TimedMethod[] {new TimedMethod(60, "Log", new object[] {"Miss"}), new TimedMethod("Null")};
		TimedMethod audioPart = new TimedMethod("Null");
		switch (food) {
		    case "eat": message = ToString() + " ate some food";
    			Heal(8); statusPart = new TimedMethod[] {new TimedMethod(0, "CharLogSprite", new object[] {"8", Party.enemySlot - 1, "healing", false}),
				    new TimedMethod("Null")};
				audioPart = new TimedMethod(0, "Audio", new object[] {"Eat"});
			    break;
			case "poison": message = ToString() + " chucked raw chicken"; 
			     if (Attacks.EvasionCycle(this, Party.GetPlayer())) {statusPart = Party.GetPlayer().status.Poison(1);}
				 audioPart = new TimedMethod(0, "Audio", new object[] {"Missile"});
			    break;
			case "attack": message = ToString() + " chucked bread";
			    if (Attacks.EvasionCycle(this, Party.GetPlayer())) {Party.GetPlayer().GainCharge(-6);
				    statusPart[0] =  new TimedMethod(0, "CharLogSprite", new object[] {"-6", Party.playerSlot - 1, "charge", true}); 
					statusPart[1] = new TimedMethod(0, "Audio", new object[] {"Nullify"});}
				audioPart = new TimedMethod(0, "Audio", new object[] {"Missile"});
			    break;
			case "defense": message = ToString() + " chucked jello";
			    if (Attacks.EvasionCycle(this, Party.GetPlayer())) {Party.GetPlayer().GainGuard(-6);
				    statusPart[0] = new TimedMethod(0, "CharLogSprite", new object[] {"-6", Party.playerSlot - 1, "guard", true});
					statusPart[1] = new TimedMethod(0, "Audio", new object[] {"Slime"});}
				audioPart = new TimedMethod(0, "Audio", new object[] {"Missile"});
				break;
			case "stun": message = ToString() + " chucked pepper";
			    if (Attacks.EvasionCycle(this, Party.GetPlayer())) {statusPart = Party.GetPlayer().status.Stun(3);}
				audioPart = new TimedMethod(0, "Audio", new object[] {"Powder"});
				break;
		}
		return new TimedMethod[] {audioPart, new TimedMethod(60, "Log", new object[] {message}), statusPart[0], statusPart[1]};
	}
	
	public override void CreateDrops() {
		drops = ItemDrops.AnyFood(ItemDrops.Amount(2, 4));
	}
	
	public override Item[] Loot () {
		System.Random rng = new System.Random();
		int sp = 4 + rng.Next(3);
		Party.UseSP(sp * -1);
		Item[] dropped = drops;
		drops = new Item[0];
		return dropped;
	}
	
	public override string[] CSDescription () {
		return new string[] {"Chef - They cook ''food''. Their ''food'' has a variety of nasty effects",
		    "They have a lot of health, but they need time to set up their meals. Use it wisely"};
	}
}