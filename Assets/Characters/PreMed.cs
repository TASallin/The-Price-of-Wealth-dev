public class PreMed : Character {
	
	public PreMed() {
		health = 16; maxHP = 16; strength = 4; power = 0; charge = 0; defense = 0; guard = 0;
		baseAccuracy = 16; accuracy = 16; dexterity = 2; evasion = 0; type = "Pre-Med Student"; passive = new Precision(this);
		quirk = Quirk.GetQuirk(this); special = new Triage(); special2 = new Prescribe(); 
		player = false; champion = false; recruitable = true; CreateDrops(); attackEffect = "pierce defense";
	}
	
	public override TimedMethod[] AI () {
		System.Random rng = new System.Random();
		int num = rng.Next(10);
		if (num < 4) {
			return Treat();
		} else if (num < 7) {
			return Attack();
		} else {
			return Steroid();
		}
	}
	
	public override TimedMethod[] BasicAttack() {
		TimedMethod[] attackPart;
		Attacks.SetAudio("Knife", 6);
		if (Party.BagContains(new Metronome())) {
			attackPart = Attacks.Attack(this, Party.GetEnemy(), strength + 3, strength + 3, GetAccuracy(), true, true, true);
		} else {
		    attackPart = Attacks.Attack(this, Party.GetEnemy(), strength, strength + 5, GetAccuracy(), true, true, true);
		}
		TimedMethod[] moves = new TimedMethod[attackPart.Length + 1];
		moves[0] = new TimedMethod(0, "Audio", new object[] {"Small Swing"});
		attackPart.CopyTo(moves, 1);
		return moves;
	}
	
	public TimedMethod[] Treat() {
		System.Random rng = new System.Random();
		int amount = rng.Next(5) + 6;
		Heal(amount);
		return new TimedMethod[] {new TimedMethod(0, "Audio", new object[] {"Heal"}),
		    new TimedMethod(60, "Log", new object[] {ToString() + " healed themself"}), 
			new TimedMethod(0, "CharLogSprite", new object[] {amount.ToString(), Party.enemySlot - 1, "healing", false})};
	}
	
	public TimedMethod[] Attack () {
		Attacks.SetAudio("Knife", 6);
		return new TimedMethod[] {new TimedMethod(60, "Log", new object[] {ToString() + " used a surgical knife"}),
		    new TimedMethod(0, "Audio", new object[] {"Small Swing"}),
	    	new TimedMethod(0, "StagnantAttack", new object[] {false, 4, 4, GetAccuracy(), true, true, true})};
	}
	
	public TimedMethod[] Steroid() {
		GainPower(2); GainDefense(-1);
		return new TimedMethod[] {new TimedMethod(0, "Audio", new object[] {"Poison Damage"}),
		    new TimedMethod(60, "Log", new object[] {ToString() + " used steroids"}),
			new TimedMethod(0, "CharLogSprite", new object[] {"2", Party.enemySlot - 1, "power", false}),
			new TimedMethod(0, "CharLogSprite", new object[] {"-1", Party.enemySlot - 1, "defense", false})};
	}
	
	public override void CreateDrops() {
		drops = ItemDrops.FromPool(new Item[] {new Defibrilator(), new Antibiotics(), new Sanitizer(), new Antibiotics(), new Sanitizer()},
		    ItemDrops.Amount(1, 2));
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
		return new string[] {"Pre-Med Student - They are learing how to be a doctor and like cutting people a little too much",
            "Aside from the obvious healing, their attacks ignore most defenses",
	    	"They have ways to gain power as well, so try to beat them before they drag the fight out too long"};
	}
}