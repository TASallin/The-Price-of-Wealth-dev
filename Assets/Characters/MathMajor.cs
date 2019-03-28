public class MathMajor : Character {
	
	int factorial;
	int answer;
	
	public MathMajor() {
        health = 18; maxHP = 18; strength = 2; power = 0; charge = 0; defense = 0; guard = 0;
		baseAccuracy = 20; accuracy = 20; dexterity = 3; evasion = 0; type = "Math Major"; passive = new Observant(this);
		quirk = Quirk.GetQuirk(this); special = new Factorial(); special2 = new Pi(); player = false; champion = false; recruitable = true;
		factorial = 0; answer = 1; CreateDrops(); attackEffect = "gain 1 accuracy";
	}
	
	public override TimedMethod[] AI () {
		if (factorial == 0) {
			return Prepare();
		} else {
			return Attack();
		}
	}
	
	public override TimedMethod[] BasicAttack() {
		TimedMethod[] attackPart;
		Attacks.SetAudio("Fire Hit", 6);
		if (Party.BagContains(new Metronome())) {
			attackPart = Attacks.Attack(this, Party.GetEnemy(), strength + 3, strength + 3, GetAccuracy(), true, true, false);
		} else {
		    attackPart = Attacks.Attack(this, Party.GetEnemy());
		}
		GainAccuracy(1);
		TimedMethod[] moves = new TimedMethod[attackPart.Length + 2];
		moves[0] = new TimedMethod(0, "Audio", new object[] {"Laser Shot"});
		moves[1] = new TimedMethod(0, "CharLogSprite", new object[] {"1", Party.playerSlot - 1, "accuracy", true});
		attackPart.CopyTo(moves, 2);
		return moves;
	}
	
	public override TimedMethod[] EnemyTurn () {
		//TimedMethod[] extra = status.Check();
		if (GetAsleep() || GetStunned() || GetPassing()) {
			factorial = 0;
			answer = 1;
			return new TimedMethod[] {new TimedMethod(0, "CharLogSprite", new object[] {"SKIP", Party.enemySlot - 1, "skip", false}),
			    new TimedMethod(0, "Audio", new object[] {"Skip Turn"})};
		} else { 
		    return AI();
		}
	}
	
	public TimedMethod[] Prepare () {
		factorial = 1;
		return new TimedMethod[] {new TimedMethod(0, "Audio", new object[] {"Recursion"}), 
			new TimedMethod(60, "Log", new object[] {ToString() + " initiated the factorial function"})};
	}
	
	public TimedMethod[] Attack () {
		Attacks.SetAudio("Fire Hit", 6);
		answer = answer * factorial;
		factorial++;
		return new TimedMethod[] {new TimedMethod(60, "Log", new object[] {ToString() + " attacked"}),
		    new TimedMethod(0, "Audio", new object[] {"Laser Shot"}),
		    new TimedMethod(0, "StagnantAttack", new object[] {false, answer, answer, GetAccuracy(), true, true, false})};
	}
	
	public override void OnRecruit () {
		Party.AddLoot (new Calculator());
	}
	
	public override void CreateDrops() {
		drops = ItemDrops.GuaranteedDrop(new Item[] {new Pencil(), new Textbook(), new Pendulum(), new Celery()},
    		ItemDrops.Amount(1, 2), new Calculator()); 
	}
	
	public override Item[] Loot () {
		System.Random rnd = new System.Random();
		int sp = 2 + rnd.Next(3);
		Party.UseSP(sp * -1);
		Item[] dropped = drops;
		drops = new Item[0];
		return dropped;
	}
	
	public override string[] CSDescription () {
		return new string[] {"Math Major - Likes math way too much",
            "knowledge of functions lets them scale their attack power excessively", 
		"In addition, their team will gain accuracy as the fight goes on. Take them all out quickly"};
	}
}