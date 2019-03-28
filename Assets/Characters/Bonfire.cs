public class Bonfire : Character {
   
    public Bonfire() {
	    health = 100; maxHP = 100; strength = 1; power = 0; charge = 0; defense = 0; guard = 0;
		baseAccuracy = 6; accuracy = 6; dexterity = 0; evasion = 0; type = "Bonfire"; passive = new Passive(this);
		quirk = Quirk.GetQuirk(this); special = null; player = false; champion = false; recruitable = false; status.goopImmune = true;
        CreateDrops();
    }	
	
	public override TimedMethod[] EnemyTurn () {
		FratLord f = (FratLord) Party.GetEnemy(1);
		if (Attacks.EvasionCheck(Party.GetPlayer(), GetAccuracy())) {
			TimedMethod[] firePart = new TimedMethod[] {new TimedMethod(60, "Log", new object[] {
				ToString() + " was compelled to jump the bonfire, and failed"}),
			    new TimedMethod(0, "StagnantAttack", new object[] {false, 5, 5, GetAccuracy(), true, false, false})};
			TimedMethod[] fratPart = f.Fail();
			TimedMethod[] moves = new TimedMethod[firePart.Length + fratPart.Length];
			firePart.CopyTo(moves, 0);
			fratPart.CopyTo(moves, firePart.Length);
			return moves;
		} else {
			Party.enemySlot = 1;
			TimedMethod[] firePart = new TimedMethod[] {new TimedMethod(60, "Log", new object[] {
				ToString() + " was compelled to jump the bonfire"}),
			    new TimedMethod(0, "AttackAny", new object[] {this, Party.GetPlayer(), 5, 5, GetAccuracy(), true, false, false})};
			TimedMethod[] fratPart = f.Third();
			TimedMethod[] moves = new TimedMethod[firePart.Length + fratPart.Length];
			firePart.CopyTo(moves, 0);
			fratPart.CopyTo(moves, firePart.Length);
			return moves;
		}
	}
	
	public override Item[] Loot () {
		System.Random rng = new System.Random();
		int sp = 5;
		Party.UseSP(sp * -1);
		Item[] dropped = drops;
		drops = new Item[0];
		return dropped;
	}
	
	public override string[] CSDescription () {
		return new string[] {"Bonfire - It is a large fire",
            "Do you have enough evasion to jump it?" };
	}
	
}