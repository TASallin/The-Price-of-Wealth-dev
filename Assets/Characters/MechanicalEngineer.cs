public class MechanicalEngineer : Character {
	
	public MechanicalEngineer() {
		health = 14; maxHP = 14; strength = 3; power = 0; charge = 0; defense = 0; guard = 0;
		baseAccuracy = 16; accuracy = 16; dexterity = 3; evasion = 0; type = "Mechanical Engineering Major"; passive = new Car(this);
		quirk = Quirk.GetQuirk(this); special2 = new TeamAttack(); special = new OilDump();
		player = false; champion = false; recruitable = true; CreateDrops(); attackEffect = "enemy loses 1 guard";
	}
	
	public override TimedMethod[] AI () {
		System.Random rng = new System.Random();
		int num = rng.Next(10);
		if (num < 1 + Party.enemyCount) {
			return TeamAttack();
		} else if (num < 8) {
			return Wrench();
		} else {
			return Oil();
		}
	}
	
	public override TimedMethod[] BasicAttack() {
		TimedMethod guardPart = new TimedMethod("Null");
		if (Attacks.EvasionCheck(Party.GetEnemy(), GetAccuracy())) {
			guardPart = new TimedMethod(0, "CharLogDelay", new object[] {"-1", Party.enemySlot - 1, "guard", false});
			Party.GetEnemy().GainGuard(-1);
		}
		TimedMethod[] attackPart;
		Attacks.SetAudio("Metal Hit", 10);
		if (Party.BagContains(new Metronome())) {
			attackPart = Attacks.Attack(this, Party.GetEnemy(), strength + 3, strength + 3, GetAccuracy(), true, true, false);
		} else {
		    attackPart = Attacks.Attack(this, Party.GetEnemy());
		}
		TimedMethod[] moves = new TimedMethod[attackPart.Length + 2];
		moves[0] = new TimedMethod(0, "Audio", new object[] {"Big Swing"});
		moves[moves.Length - 1] = guardPart;
		attackPart.CopyTo(moves, 1);
		return moves;
	}
	
	public TimedMethod[] TeamAttack() {
		Attacks.SetAudio("Blunt Hit", 15);
		TimedMethod[] moves = new TimedMethod[Party.enemyCount + 1];
		moves[0] = new TimedMethod(60, "Log", new object[] {ToString() + " led a team attack"});
		int index = 0;
		int count = 1;
		Character current;
		while (count < Party.enemyCount) {
			current = Party.enemies[index];
		    if (index != Party.enemyCount - 1 && current != null && current.GetAlive()) {
				moves[count] = new TimedMethod(0, "AttackAny", new object[] {
					current, Party.GetPlayer(), current.GetStrength(), current.GetStrength() + 4, current.GetAccuracy(), true, false, false});
			    count++;
			}
		    index++;
		}
		moves[moves.Length - 1] =  new TimedMethod(0, "Attack", new object[] {false});
		return moves;
	}
	
	public TimedMethod[] Wrench() {
		Attacks.SetAudio("Metal Hit", 10);
		return new TimedMethod[] {new TimedMethod(60, "Log", new object[] {ToString() + " swung a wrench"}),
    		new TimedMethod(0, "Audio", new object[] {"Big Swing"}), new TimedMethod(60, "Attack", new object[] {false})};
	}
	
	public TimedMethod[] Oil() {
		if (Attacks.EvasionCycle(this, Party.GetPlayer())) {
			Party.GetPlayer().GainDefense(- 2);
		    return new TimedMethod[] {new TimedMethod(0, "AudioAfter", new object[] {"Oil", 10}),
			    new TimedMethod(60, "Log", new object[] {ToString() + " dumped oil. Defense down"}),
				new TimedMethod(0, "CharLogSprite", new object[] {-2, Party.playerSlot - 1, "defense",true})};
		} else {
			return new TimedMethod[] {new TimedMethod(0, "AudioAfter", new object[] {"Drink", 10}),
			    new TimedMethod(60, "Log", new object[] {ToString() + " dumped oil. It missed"})};
		}		
	}
	
	public override void CreateDrops() {
		drops = ItemDrops.GuaranteedDrop(new Item[] {new Oil(), new Oil(), new Shuttle(), new Wire(), new Smartphone()},
    		ItemDrops.Amount(1, 2), new Oil());
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
		return new string[] {"Mechanical Engineering Major - The most common engineer to be",
            "Relies on teamwork and oil",
		    "This makes them particularly dangerous in a group. Running can be a good decision in that case"};
	}

}