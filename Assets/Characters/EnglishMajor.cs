public class EnglishMajor : Character {
	
	public EnglishMajor() {
		health = 19; maxHP = 19; strength = 4; power = 0; charge = 0; defense = 0; guard = 0;
		baseAccuracy = 14; accuracy = 14; dexterity = 4; evasion = 0; type = "English Major"; passive = new Reading(this);
		quirk = Quirk.GetQuirk(this); special = new Quote(); special2 = new Read(); 
		player = false; champion = false; recruitable = true; CreateDrops(); attackEffect = "switch out";
	}
	
	public override TimedMethod[] AI () {
		System.Random rng = new System.Random();
		int num = rng.Next(10);
		if (num < 4) {
			return Attack();
		} else if (num < 7) {
			return Insult();
		} else {
			return Argument();
		}
	}
	
	public override TimedMethod[] BasicAttack() {
		TimedMethod[] attackPart;
		Attacks.SetAudio("Blunt Hit", 15);
		if (Party.BagContains(new Metronome())) {
			attackPart = Attacks.Attack(this, Party.GetEnemy(), strength + 3, strength + 3, GetAccuracy(), true, true, false);
		} else {
		    attackPart = Attacks.Attack(this, Party.GetEnemy());
		}
		TimedMethod switchPart = new TimedMethod(0, "Log", new object[] {""});
		if (Party.playerCount > 1) {
	    	int steps = new System.Random().Next(Party.playerCount - 1);
		    for (int i = 0; i < 4; i++) {
			    if (Party.members[i] != null && Party.members[i].GetAlive() && i != Party.playerSlot - 1) {
				    if (steps == 0) {
					   	switchPart = new TimedMethod(60, "SwitchTo", new object[] {i + 1});
    				} else {
	    				steps--;
		    		}
		    	}
    		}
     	}
		TimedMethod[] moves = new TimedMethod[attackPart.Length + 2];
		moves[0] = new TimedMethod(0, "AudioAfter", new object[] {"Big Swing", 10});
		attackPart.CopyTo(moves, 1);
		moves[moves.Length - 1] = switchPart;
		return moves;
	}
	
	public TimedMethod[] Attack () {
		Attacks.SetAudio("Blunt Hit", 15);
		return new TimedMethod[] {new TimedMethod(60, "Log", new object[] {ToString() + " swung a dictionary"}),
		new TimedMethod(0, "AudioAfter", new object[] {"Big Swing", 10}),
		new TimedMethod(0, "Attack", new object[] {false})};
	}
	
	public TimedMethod[] Insult() {
		if (Attacks.EvasionCycle(this, Party.GetPlayer())) {
	    	Party.GetPlayer().GainCharge(3);
		    Party.GetPlayer().GainDefense(-2);
			return new TimedMethod[] {new TimedMethod(0, "Audio", new object[] {"Fire"}), 
			    new TimedMethod(60, "Log", new object[] {ToString() + " insulted in old English. Defense down and charge up"}),
				new TimedMethod(0, "Audio", new object[] {"Nullify"}),
				new TimedMethod(0, "CharLogSprite", new object[] {"3", Party.playerSlot - 1, "charge", true}),
				new TimedMethod(0, "CharLogSprite", new object[] {"-2", Party.playerSlot - 1, "defense", true})};
		} else {
			return new TimedMethod[] {new TimedMethod(0, "Audio", new object[] {"Fire"}), 
			    new TimedMethod(60, "Log", new object[] {ToString() + " insulted in old English. It went over your head"})};
		}
	}
	
	public TimedMethod[] Argument() {
		power = System.Math.Max(power, 0); charge = System.Math.Min(charge + 5, 5);
		return new TimedMethod[] {new TimedMethod(60, "Log", new object[] {
			ToString() + " constructed an argument. Attack debuffs removed and charge up"}),
			new TimedMethod(0, "Audio", new object[] {"Clean"}),
			new TimedMethod(0, "CharLogSprite", new object[] {"Atk Reset", Party.enemySlot - 1, "nullAttack", false}),
			new TimedMethod(0, "CharLogSprite", new object[] {"5", Party.enemySlot - 1, "charge", false})};
	}
	
	public override void CreateDrops() {
		drops = ItemDrops.FromPool(new Item[] {new Pencil(), new Pencil(), new Pencil(), new Textbook(), new Curry(), new Coffee()},
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
		return new string[] {"English Major - That guy who likes to find a reason in books that don't have meaning, like why is the door blue, because the author liked blue",
	    	"Can remove attack debuffs with a pointless arguement and debuff you with a dead version of a language"};
	}
		
}