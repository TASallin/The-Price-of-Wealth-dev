public class Watermelon : Character {
   
    public Watermelon() {
	    health = 7; maxHP = 7; strength = 1; power = 0; charge = 0; defense = 0; guard = 0;
		baseAccuracy = 0; accuracy = 0; dexterity = 0; evasion = 0; type = "Watermelon"; passive = new Passive(this);
		quirk = Quirk.GetQuirk(this); special = null; player = false; champion = false; recruitable = false;
		status.goopImmune = true; CreateDrops();
    }	
	
	public override TimedMethod[] EnemyTurn () {
	    FratLord f = (FratLord) Party.GetEnemy(1);
		return f.Fail();
	}
	
	public override Item[] Loot () {
		System.Random rng = new System.Random();
		int sp = 1;
		Party.UseSP(sp * -1);
		Item[] dropped = drops;
		drops = new Item[0];
		return dropped;
	}
	
	public override string[] CSDescription () {
		return new string[] {"Watermelon - It's a watermelon why are you afraid of it attack it",
		    "Do that next time"};
	}
	
}