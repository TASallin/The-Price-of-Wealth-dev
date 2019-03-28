public class StudentBodyPresident : Character {
	
	public StudentBodyPresident() {
	    health = 20; maxHP = 20; strength = 2; power = 0; charge = 0; defense = 0; guard = 0;
		baseAccuracy = 10; accuracy = 10; dexterity = 4; evasion = 0; type = "Student Body President"; passive = new PolicyMaker(this);
		quirk = Quirk.GetQuirk(this); special = null; player = false; champion = true; recruitable = false;
        CreateDrops();
    }
	
	public override TimedMethod[] AI () {
		System.Random rng = new System.Random();
		int num = rng.Next(10);
		if (Party.enemyCount == 1) {
			return Summon();
		} else if (GetGooped()) {
			if (num < 2) {
				return Panic();
			} else {
				return Clean();
			}
		} else {
			if (num < 4) {
				return Spotlight();
			} else if (num < 7) {
				return FreeFood();
			} else {
				return TeamAttack();
			}
		}
	}
	
	public TimedMethod[] Spotlight() {
	  	int steps = new System.Random().Next(Party.enemyCount - 1);
		for (int i = 0; i < 4; i++) {
			if (Party.enemies[i] != null && Party.enemies[i].GetAlive() && i != Party.enemySlot - 1) {
			    if (steps == 0) {
					Party.enemySlot = i + 1;
					Party.GetEnemy().GainPower(2);
					Party.GetEnemy().GainCharge(5);
					Party.GetEnemy().GainEvasion(3);
					Party.GetEnemy().GainAccuracy(1);
			    	return new TimedMethod[] {new TimedMethod(60, "Log", new object[] {ToString() 
				        + " put the spotlight on " + Party.GetEnemy().ToString()}), new TimedMethod(0, "EnemySwitch", new object[] {1, i + 1})};
    			} else {
	    			steps--;
	    		}
	    	}
    	}
		return Panic();
	}
	
	public TimedMethod[] FreeFood() {
		int steps = new System.Random().Next(Party.enemyCount - 1);
		for (int i = 0; i < 4; i++) {
			if (Party.enemies[i] != null && Party.enemies[i].GetAlive() && i != Party.enemySlot - 1) {
			    if (steps == 0) {
					Party.enemySlot = i + 1;
					Party.GetEnemy().Heal(20);
					Party.GetEnemy().GainGuard(1);
			    	return new TimedMethod[] {new TimedMethod(60, "Log", new object[] {ToString() 
				        + " enticed " + Party.GetEnemy().ToString() + " with free food"}), new TimedMethod(0, "EnemySwitch", new object[] {1, i + 1})};
    			} else {
	    			steps--;
	    		}
	    	}
    	}
		return Panic();
	}
	
	public TimedMethod[] TeamAttack() {
		TimedMethod[] moves = new TimedMethod[Party.enemyCount + 2];
		moves[0] = new TimedMethod(60, "Log", new object[] {ToString() + " led a team attack and slinked to the back in doing so"});
		moves[1] = new TimedMethod(0, "Audio", new object[] {"Skill1"});
		int index = 0;
		int count = 1;
		Character current;
		while (count < Party.enemyCount) {
			current = Party.enemies[index];
		    if (index != Party.enemyCount - 1 && current != null && current.GetAlive()) {
				moves[count + 1] = new TimedMethod(0, "AttackAny", new object[] {
					current, Party.GetPlayer(), current.GetStrength(), current.GetStrength() + 4, current.GetAccuracy(), true, false, false});
			    count++;
			}
		    index++;
		}
		int steps = new System.Random().Next(Party.enemyCount - 1);
		for (int i = 0; i < 4; i++) {
			if (Party.enemies[i] != null && Party.enemies[i].GetAlive() && i != Party.enemySlot - 1) {
			    if (steps == 0) {
					Party.enemySlot = i + 1;
			    	moves[moves.Length - 1] =  new TimedMethod(0, "EnemySwitch", new object[] {1, i});
    			} else {
	    			steps--;
	    		}
	    	}
    	}
		return moves;
	}
	
	public TimedMethod[] Summon() {
		System.Random rng = new System.Random();
		int seed;
		Character current;
		for (int i = 0; i < 3; i++) {
			seed = rng.Next(10);
			if (seed == 0) {
				current = new CSMajor();
			} else if (seed == 1) {
				current = new MusicMajor();
			} else if (seed == 2) {
				current = new PoliticalScientist();
			} else if (seed == 3) {
				current = new FootballPlayer();
			} else if (seed == 4) {
				current = new BusinessMajor();
			} else if (seed == 5) {
				current = new EnglishMajor();
			} else if (seed == 6) {
				current = new ChemistryMajor();
			} else if (seed == 7) {
				current = new CJMajor();
			} else if (seed == 8) {
				current = new CulinaryMajor();
			} else {
				current = new PsychMajor();
			}
			current.SetRecruitable(false);
			Party.AddEnemy(current);
		}
		return new TimedMethod[] {new TimedMethod(60, "Log", new object[] {ToString() + "Surrounded themself with minions"})};
	}
	
	public TimedMethod[] Panic () {
		return new TimedMethod[] {new TimedMethod(60, "Log", new object[] {ToString() + " freaked out, stuck in front"})};
	}
	
	public TimedMethod[] Clean () {
		return new TimedMethod[] {new TimedMethod(60, "Log", new object[] {ToString() + " escaped the goop"})};
	}
	
	public override void CreateDrops() {
		drops = ItemDrops.FromPool(new Item[] {new Smartphone(), new Coffee(), new PinkSlip(), new Briefcase(), new PaperPlane(), new VotedBadge(),
		    new Pizza()}, ItemDrops.Amount(1, 3));
	}
	
	public override Item[] Loot () {
		System.Random rnd = new System.Random();
		int sp = 3 + rnd.Next(5);
		Party.UseSP(sp * -1);
		Item[] dropped = drops;
		drops = new Item[0];
		return dropped;
	}
	
	public override string[] CSDescription() {
		return new string[] {"Student Body President - Exetremely pompous and overconfident but has extreme cowardice for phyiscal violence",
		    "When they switch out, they'll give buffs to their new bodyguards",
		    "They also change policies that affect all of us"};
	}

}