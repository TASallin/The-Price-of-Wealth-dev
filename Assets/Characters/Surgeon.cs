public class Surgeon : Doctor {
	
	public Surgeon() {
		health = 50; maxHP = 50; strength = 4; baseAccuracy = 21; accuracy = 21; dexterity = 3; type = "Surgeon";
	    champion = true; recruitable = false;
	}
	
	public override TimedMethod[] AI () {
		System.Random rng = new System.Random();
		int num = rng.Next(10);
		if (Party.GetPlayer().GetAsleep() && Party.GetPlayer().GetGooped()) {
			return Operate();
		}
		if (num < 1) {
			return Heal();
		} else if (num < 4) {
			return Attack();
		} else if (num < 6) {
			return Anasthesia();
		} else if (num < 8) {
			return Overdose();
		} else {
			return Restrain();
		}
	}
	
	public TimedMethod[] Operate() {
		Attacks.SetAudio("Knife", 10);
		return new TimedMethod[] {new TimedMethod(60, "Log", new object[] {ToString() + " performed deadly surgery under proper conditions"}),
		    new TimedMethod(0, "AudioNumbered", new object[] {"Attack", 1, 2}), new TimedMethod(0, "Audio", new object[] {"Finale"}),
		    new TimedMethod(0, "StagnantAttack", new object[] {false, 30, 30, GetAccuracy(), true, true, true})};
	}
	
	public TimedMethod[] Overdose() {
		if (Attacks.EvasionCycle(this, Party.GetPlayer())) {
			TimedMethod[] poisonPart = Party.GetPlayer().status.Poison(2);
			return new TimedMethod[] {new TimedMethod(0, "Audio", new object[] {"Skill1"}), new TimedMethod(0, "Audio", new object[] {"Drink"}),
			    new TimedMethod(60, "Log", new object[] {ToString() + " prescribed an overdose"}), poisonPart[0], poisonPart[1]}; 
		} else {
			return new TimedMethod[] {new TimedMethod(0, "Audio", new object[] {"Skill1"}), new TimedMethod(0, "Audio", new object[] {"Glass Break"}),
				new TimedMethod(60, "Log", new object[] {ToString() + " prescribed an overdose...but the handwriting was too poor"})}; 
		}
	}
	
	public TimedMethod[] Restrain() {
		if (Attacks.EvasionCycle(this, Party.GetPlayer())) {
			TimedMethod[] goopPart = Party.GetPlayer().status.Goop();
			return new TimedMethod[] {new TimedMethod(0, "AudioNumbered", new object[] {"Attack", 1, 2}), 
			    new TimedMethod(0, "Audio", new object[] {"Big Swing"}),
			    new TimedMethod(60, "Log", new object[] {ToString() + " used a straight jacket"}), goopPart[0], goopPart[1]}; 
		} else {
			return new TimedMethod[] {new TimedMethod(0, "AudioNumbered", new object[] {"Attack", 1, 2}),
    			new TimedMethod(0, "Audio", new object[] {"Big Swing"}),
			    new TimedMethod(60, "Log", new object[] {ToString() + " missed a straight jacket"})}; 
		}
	}
	
	public override void CreateDrops () {
		drops = ItemDrops.FromPool(new Item[] {new Defibrilator(), new Defibrilator(), new Antibiotics(), new Antibiotics(),
		    new Sanitizer(), new Sanitizer(), new Milk()}, ItemDrops.Amount(2, 3));
	}
	
	public override Item[] Loot () {
		System.Random rng = new System.Random();
		int sp = 5 + rng.Next(5);
		Party.UseSP(sp * -1);
		Item[] dropped = drops;
		drops = new Item[0];
		return dropped;
	}
	
	public override string[] CSDescription () {
		return new string[] {"Surgeon - Liked to cut open people way too much",
		    "Has debuffs such as poisen and goop"};
	}
}