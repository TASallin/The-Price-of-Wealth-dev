public class PoliticalScientist : Character {
	
	public PoliticalScientist() {
		health = 21; maxHP = 21; strength = 3; power = 0; charge = 0; defense = 0; guard = 0;
		baseAccuracy = 13; accuracy = 13; dexterity = 4; evasion = 0; type = "Political Science Major"; passive = new Democracy(this);
		quirk = Quirk.GetQuirk(this); special = new Campaign(); special2 = new Filibuster();
		player = false; champion = false; recruitable = true; CreateDrops(); attackEffect = "chance to stun";
	}
	
	public override TimedMethod[] AI () {
		System.Random rng = new System.Random();
		int num = rng.Next(10);
		Democracy castPassive = (Democracy)passive;
		if (num < 4 || castPassive.promise == 1) {
			castPassive.promise = 0;
			return Attack();
		} else if (num < 8) {
			castPassive.promise = 2;
			return Campaign();
		} else {
		    return Debate();
		}
	}
	
	public override TimedMethod[] BasicAttack() {
		System.Random rng = new System.Random();
		TimedMethod[] stunPart;
		if (rng.Next(10) < 5 && Attacks.EvasionCheck(Party.GetEnemy(), GetAccuracy())) {
			stunPart = Party.GetEnemy().status.Stun(2);
		} else {
			stunPart = new TimedMethod[] {new TimedMethod("Null"), new TimedMethod("Null")};
		}
		Democracy castPassive = (Democracy)passive;
		castPassive.attacked = true;
		TimedMethod[] attackPart;
		Attacks.SetAudio("Blunt Hit", 10);
		if (Party.BagContains(new Metronome())) {
			attackPart = Attacks.Attack(this, Party.GetEnemy(), strength + 3, strength + 3, GetAccuracy(), true, true, false);
		} else {
		    attackPart = Attacks.Attack(this, Party.GetEnemy());
		}
		TimedMethod[] moves = new TimedMethod[attackPart.Length + 3];
		moves[0] = new TimedMethod(0, "Audio", new object[] {"Big Swing"});
		attackPart.CopyTo(moves, 1);
		moves[moves.Length - 2] = stunPart[0];
		moves[moves.Length - 1] = stunPart[1];
		return moves;
	}
	
	public TimedMethod[] Attack () {
		Attacks.SetAudio("Blunt Hit", 10);
		return new TimedMethod[] {new TimedMethod(60, "Log", new object[] {ToString() + " swung the campaign sign"}),
		    new TimedMethod(0, "Audio", new object[] {"Big Swing"}),
		    new TimedMethod(0, "StagnantAttack", new object[] {false, 4, 4, GetAccuracy(), true, true, false})};
	}
	
	public TimedMethod[] Campaign () {
		GainPower(1); GainDefense(1); GainCharge(2); GainGuard(2);
		return new TimedMethod[] {new TimedMethod(0, "AudioAfter", new object[] {"Recruit", 45}),
		    new TimedMethod(60, "Log", new object[] {ToString() + " campaigned with the promise of action"}),
			new TimedMethod(0, "CharLogSprite", new object[] {"1", Party.enemySlot - 1, "power", false}),
			new TimedMethod(0, "CharLogSprite", new object[] {"1", Party.enemySlot - 1, "defense", false}),
			new TimedMethod(0, "CharLogSprite", new object[] {"2", Party.enemySlot - 1, "charge", false}),
			new TimedMethod(0, "CharLogSprite", new object[] {"2", Party.enemySlot - 1, "guard", false})};
	}
	
	public TimedMethod[] Debate () {
	    GainCharge(4); Party.GetPlayer().GainCharge(4);
		return new TimedMethod[] {new TimedMethod(0, "AudioAfter", new object[] {"Nullify", 15}),
		    new TimedMethod(60, "Log", new object[] {ToString() + " began debating"}),
			new TimedMethod(0, "CharLogSprite", new object[] {"4", Party.enemySlot - 1, "charge", false}),
			new TimedMethod(0, "CharLogSprite", new object[] {"4", Party.playerSlot - 1, "charge", true})};
	}
	
	public override void CreateDrops () {
		drops = ItemDrops.GuaranteedDrop(new Item[] {new Coffee(), new Sanitizer(), new Celery(), new Pencil()}, ItemDrops.Amount(1, 2),
		    new VotedBadge()); 
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
		return new string[] {"Political Science Major - Frequently makes promises to attack in exchange for support",
	    	"So if they don't attack, they lose their support. They're also stronger if their team outnumbers us"};
	}
		
}