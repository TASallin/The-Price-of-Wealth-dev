public class Politician : Character {
	
    public int cycle;
	public bool broken;
	
	public Politician() {
	    health = 60; maxHP = 60; strength = 3; power = 0; charge = 0; defense = 0; guard = 0; 
		baseAccuracy = 12; accuracy = 12; dexterity = 4; evasion = 0; type = "The Politician"; passive = new Passive(this);
		quirk = Quirk.GetQuirk(this); special = null; player = false; champion = true; recruitable = false;
		cycle = 0; broken = false; CreateDrops();
	}
	
	public override string ToString() {return "The Politician";}
	
	public override TimedMethod[] AI () {
		if (broken) {
			if (Party.enemyCount > 1) {
				return Switch();
			}
			if (cycle == 0) {
				cycle++;
				return Weak();
			} else if (cycle == 1) {
				cycle ++;
				return SaveFace();
		    } else {
				cycle = 0;
				return Manager();
			}
		} else {
		    if (cycle == 0) {
		    	cycle++;
	    		return CampaignDefense();
    		} else if (cycle == 1) {
		    	cycle++;
	    		return Filibuster();
     		} else if (cycle == 2) {
		    	cycle++;
	    	    return CampaignBalance();			
    		} else if (cycle == 3) {
		    	cycle++;
	    		return Veto();
    		} else if (cycle == 4) {
		    	cycle++;
	    		return CampaignOffense();
    		} else {
			    cycle = 0;
			    return Attack();
		    }
		}
	}
	
	public override TimedMethod[] EnemyTurn () {
		//TimedMethod[] extra = status.Check(this);
		if (GetAsleep() || GetStunned() || GetPassing()) {
			status.passing = false;
			if (!broken && (cycle == 1 || cycle == 3 || cycle == 5)) {
				broken = true; cycle = 0; defense = System.Math.Min(-1, defense - 2); power = System.Math.Min(-1, power - 2); evasion = 0;
				return new TimedMethod[] {new TimedMethod(60, "Log", new object[] {"The promise has been broken! Voter support is 0!"}),
				    new TimedMethod(0, "CharLogSprite", new object[] {"SKIP", Party.enemySlot - 1, "skip", false})};
			}
			return new TimedMethod[] {new TimedMethod(0, "CharLogSprite", new object[] {"SKIP", Party.enemySlot - 1, "skip", false}),
			    new TimedMethod(0, "Audio", new object[] {"Skip Turn"})};
		} else { 
		    return AI();
		}
	}
	
	public TimedMethod[] CampaignDefense() {
		TimedMethod statPart = new TimedMethod("Null");
		if (!GetGooped()) {
			statPart = new TimedMethod(0, "CharLogSprite", new object[] {"10", Party.enemySlot - 1, "evasion", false});
		}
		GainGuard(10); GainEvasion(10);
		return new TimedMethod[] {new TimedMethod(60, "Log", new object[] {
			"The Politician campaigned with the promise of stalling votes"}), new TimedMethod(60, "Log", new object[] {
		    "It yielded defensive results"}),  new TimedMethod(0, "CharLogSprite", new object[] {"10", Party.enemySlot - 1, "guard", false}), statPart};
	}
	
	public TimedMethod[] CampaignBalance() {
	    GainPower(1); GainDefense(1);
		return new TimedMethod[] {new TimedMethod(60, "Log", new object[] {
			"The Politician campaigned with the promise of denying opposition"}), new TimedMethod(60, "Log", new object[] {
		    "It yielded balanced results"}), new TimedMethod(0, "CharLogSprite", new object[] {"1", Party.enemySlot - 1, "power", false}),
		    new TimedMethod(0, "CharLogSprite", new object[] {"1", Party.enemySlot - 1, "defense", false})};
	}
	
	public TimedMethod[] CampaignOffense() {
	    GainCharge(9); GainAccuracy(2);
		return new TimedMethod[] {new TimedMethod(60, "Log", new object[] {
			"The Politician campaigned with the promise of destruction"}), new TimedMethod(60, "Log", new object[] {
		    "It yielded offensive results"}), new TimedMethod(0, "CharLogSprite", new object[] {"9", Party.enemySlot - 1, "charge", false}),
			new TimedMethod(0, "CharLogSprite", new object[] {"2", Party.enemySlot - 1, "accuracy", false})};
	}
	
	public TimedMethod[] Filibuster() {
		TimedMethod[] totalSleep = new TimedMethod[] {new TimedMethod("Null"), new TimedMethod("Null"), new TimedMethod("Null"),
	    	new TimedMethod("Null"), new TimedMethod("Null"), new TimedMethod("Null"), new TimedMethod("Null"), new TimedMethod("Null")};
		int index = 0;
		TimedMethod[] sleepPart;
		foreach (Character c in Party.members) {
			if (c != null && c.GetAlive() && Attacks.EvasionCycle(this, c)) {
		        sleepPart = c.status.Sleep();
	    	} else {
    			sleepPart = new TimedMethod[] {new TimedMethod(0, "CharLog", new object[] {"miss", c.partyIndex, true}), new TimedMethod("Null")};
		    }
			totalSleep[index] = sleepPart[0];
			totalSleep[index + 1] = sleepPart[1];
			index += 2;
		}
		return new TimedMethod[] {new TimedMethod(0, "Audio", new object[] {"Filibuster"}),
		    new TimedMethod(60, "Log", new object[] {"The Politician filibustered"}), totalSleep[0], totalSleep[1], totalSleep[2], totalSleep[3],
			totalSleep[4], totalSleep[5], totalSleep[6], totalSleep[7]};
	}
	
	public TimedMethod[] Veto() {
		if (Attacks.EvasionCycle(this, Party.GetPlayer())) {
		    Status.NullifyAttack(Party.GetPlayer()); Status.NullifyDefense(Party.GetPlayer());
			return new TimedMethod[] {new TimedMethod(0, "Audio", new object[] {"Nullify"}),
		        new TimedMethod(60, "Log", new object[] {"The Politician used the veto. Stats were countered"}),
				new TimedMethod(0, "CharLogSprite", new object[] {"atk reset", Party.playerSlot - 1, "nullAttack", false}),
				new TimedMethod(0, "CharLogSprite", new object[] {"def reset", Party.playerSlot - 1, "nullDefense", false})};
		}
		return new TimedMethod[] {new TimedMethod(60, "Log", new object[] {"The Politician used the veto. It failed"})};
    }
	
	public TimedMethod[] Attack() {
		Attacks.SetAudio("Gunfire", 30);
		return new TimedMethod[] {new TimedMethod(60, "Log", new object[] {"The Politician delivered the promised destruction"}),
		    new TimedMethod(0, "StagnantAttack", new object[] {false, 3, 3, GetAccuracy(), true, false, false}),
			new TimedMethod(0, "StagnantAttack", new object[] {false, 5, 5, GetAccuracy(), true, false, false}),
		    new TimedMethod(0, "StagnantAttack", new object[] {false, 7, 7, GetAccuracy(), true, true, false})};
	}
	
	public TimedMethod[] Weak() {
		Attacks.SetAudio("Blunt Hit", 10);
		return new TimedMethod[] {new TimedMethod(60, "Log", new object[] {"The Politician attacked"}), 
		    new TimedMethod(0, "Audio", new object[] {"Small Swing"}),
		    new TimedMethod(0, "StagnantAttack", new object[] {false, 3, 3, GetAccuracy(), true, true, false})};
	}
	
	public TimedMethod[] SaveFace() {
		GainEvasion(4);
		TimedMethod statPart = new TimedMethod("Null");
		if (!GetGooped()) {
			statPart = new TimedMethod(0, "CharLogSprite", new object[] {"4", Party.enemySlot - 1, "evasion", false});
		}
		return new TimedMethod[] {new TimedMethod(60, "Log", new object[] {"The Politician tried to save face. Evasion up"}),
		    new TimedMethod(0, "Audio", new object[] {"Skip Turn"}), statPart};
	}
	
	public TimedMethod[] Manager() {
		if (GetGooped()) {
			status.gooped = false;
			return new TimedMethod[] {new TimedMethod(60, "Log", new object[] {ToString() + " escaped the goop"}),
			    new TimedMethod(0, "CharLogSprite", new object[] {"Cleaned", Party.enemySlot - 1,  "goop", false}),
			    new TimedMethod(0, "Audio", new object[] {"Clean"})};
		}
		Party.AddEnemy(new CampaignManager());
		Switch();
		return new TimedMethod[] {new TimedMethod(60, "Log", new object[] {"The Politician called upon the campaign manager"}),
		    new TimedMethod(0, "EnemySwitch", new object[] {1, 2})};
	}
	
	public TimedMethod[] Switch() {
		if (GetGooped()) {
			status.gooped = false;
			return new TimedMethod[] {new TimedMethod(60, "Log", new object[] {ToString() + " escaped the goop"}),
			    new TimedMethod(0, "CharLogSprite", new object[] {"Cleaned", Party.enemySlot - 1,  "goop", false}),
			    new TimedMethod(0, "Audio", new object[] {"Clean"})};
		}
		Party.enemySlot = 2;
		return new TimedMethod[] {new TimedMethod(60, "Log", new object[] {"The Politician is waiting for the campaign manager"}),
		    new TimedMethod(0, "EnemySwitch", new object[] {1, 2})};
	}
	
	public override void CreateDrops () {
		drops = ItemDrops.FromPool(new Item[] {new Smartphone(), new Pizza(), new Defibrilator(), new Briefcase(), new Antibiotics(), new Pendulum()},
		    ItemDrops.Amount(2, 4));
	}
	
	public override Item[] Loot () {
		System.Random rnd = new System.Random();
		int sp = 10 + rnd.Next(6);
		Party.UseSP(sp * -1);
		Item[] dropped = drops;
		drops = new Item[0];
		return dropped;
	}
	
	public override string[] CSDescription() {
		return new string[] {"This is The Politician. They lie for a living",
            "They'll frequently campaign to get stronger",
            "They can put you to sleep, negate your stats, and if you take too long, they'll use a triple attack with their cane",
			"Apparently their weakness is in all the promises they make", 
		"But this doesn't answer the most important question...what party are they from?"}; 
	}
	
}