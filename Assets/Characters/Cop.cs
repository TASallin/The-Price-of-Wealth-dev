public class Cop : Character {
	
    //bool attacked;
	
	public Cop() {
		health = 18; maxHP = 18; strength = 4; power = 0; charge = 0; defense = 0; guard = 0;
		baseAccuracy = 12; accuracy = 12; dexterity = 2; evasion = 0; type = "Cop"; passive = new Outgun(this);
		quirk = Quirk.GetQuirk(this); special = null; player = false; champion = false; recruitable = false; CreateDrops();
		//attacked = false;
	}
	
	public override TimedMethod[] AI () {
		System.Random rng = new System.Random();
		int num = rng.Next(10);
		if (health < maxHP && num > 2 && num < 5) {
			return Shoot();
		} else if (num < 4) {
			return Donut();
		} else if (num < 7) {
			return Whistle();
		} else {
			return Tazer();
		}
	}
	
	public TimedMethod[] Tazer() {
		Attacks.SetAudio("Tazer", 6);
		TimedMethod[] stunPart;
		if (Attacks.EvasionCheck(Party.GetPlayer(), GetAccuracy())) {
		    stunPart = Party.GetPlayer().status.Stun(2);
		} else {
			stunPart = new TimedMethod[] {new TimedMethod("Null"), new TimedMethod("Null")};
		}
	    return new TimedMethod[] {new TimedMethod(60, "Log", new object[] {ToString() + " fired a tazer"}),
            new TimedMethod(0, "Audio", new object[] {"Button"}),
		    new TimedMethod(0, "StagnantAttack", new object[] {false, 6, 6, GetAccuracy(), true, true, true}), stunPart[0], stunPart[1]};
	}
	
	public TimedMethod[] Shoot() {
		Attacks.SetAudio("Blunt Hit", 6);
		return new TimedMethod[] {new TimedMethod(60, "Log", new object[] {ToString() + " shot a pistol"}), 
		    new TimedMethod(0, "Audio", new object[] {"Gunfire"}),
		    new TimedMethod(0, "StagnantAttack", new object[] {false, 10, 10, GetAccuracy(), true, true, false})};
	}
	
	public TimedMethod[] Donut() {
		Heal(15); GainPower(-1); GainDefense(-1);
		return new TimedMethod[] {new TimedMethod(0, "Audio", new object[] {"Eat"}), new TimedMethod(0, "AudioAfter", new object[] {"Heal", 30}),
		    new TimedMethod(60, "Log", new object[] {ToString() + " ate a donut"}),
			new TimedMethod(0, "CharLogSprite", new object[] {"15", Party.enemySlot - 1,  "healing", false}),
			new TimedMethod(0, "CharLogSprite", new object[] {"-1", Party.enemySlot - 1,  "power", false}),
			new TimedMethod(0, "CharLogSprite", new object[] {"-1", Party.enemySlot - 1,  "defense", false})};
	}
	
	public TimedMethod[] Whistle() {
		if (Attacks.EvasionCycle(this, Party.GetPlayer())) {
			Status.NullifyAttack(Party.GetPlayer());
			return new TimedMethod[] {new TimedMethod(0, "Audio", new object[] {"Whistle"}),
    			new TimedMethod(0, "AudioAfter", new object[] {"Nullify", 60}), 
			    new TimedMethod(60, "Log", new object[] {ToString() + " blew a whistle"}),
				new TimedMethod(0, "CharLogSprite", new object[] {"Atk Reset", Party.playerSlot - 1,  "nullAttack", true})};
		} else {
			return new TimedMethod[] {new TimedMethod(0, "Audio", new object[] {"Whistle"}),
			    new TimedMethod(60, "Log", new object[] {ToString() + " blew a whistle, and nothing happened"})};
		}
	}
	
	public override void CreateDrops () {
		drops = ItemDrops.FromPool(new Item[] {new Donut(), new Donut(), new Donut(), new Tazer(), new Tazer(), new Whistle()},
		    ItemDrops.Amount(1, 3));
	}
	
	public override Item[] Loot () {
		System.Random rng = new System.Random();
		int sp = 3 + rng.Next(3);
		Party.UseSP(sp * -1);
		Item[] dropped = drops;
		drops = new Item[0];
		return dropped;
	}
	
	public override string[] CSDescription () {
		return new string[] {"Cop - Not really serving and protecting anymore",
		    "They can't use their gun until we deal damage to them, and their donut supply will lower their stats"};
        //Make sure you aren't black
	}
}