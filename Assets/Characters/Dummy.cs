public class Dummy : Character {
   
    public Dummy() {
	    health = 100; maxHP = 100; strength = 1; power = 0; charge = 0; defense = 0; guard = 0;
		baseAccuracy = 0; accuracy = 0; dexterity = 0; evasion = 0; type = "Dummy"; passive = new Passive(this);
		quirk = Quirk.GetQuirk(this); special = null; player = false; champion = false; recruitable = false; status.goopImmune = true;
        CreateDrops();
    }	
	
	public override TimedMethod[] EnemyTurn () {
	    Robber robber = (Robber) Party.GetEnemy(1);
		return robber.Ambush();
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
		return new string[] {"Dummy - The robber placed this here",
		    "It's completely harmless. Ignore it"};
        //substitution jutsu
    }

}