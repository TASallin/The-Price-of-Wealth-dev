public class Oil : Item {
	
	public Oil() {name = "Oil"; description = "Lowers defense"; price = 2;}
	
	public override TimedMethod[] Use () {
		if (Attacks.EvasionCycle(Party.GetPlayer(), Party.GetEnemy())) {
			Party.GetEnemy().SetDefense(Party.GetEnemy().GetDefense() - 2);
			return new TimedMethod[] {new TimedMethod(0, "Audio", new object[] {"Oil"}),
		        new TimedMethod(60, "Log", new object[] {Party.GetEnemy().ToString() + " was covered in oil"}),
			    new TimedMethod(0, "CharLogSprite", new object[] {"-2", Party.enemySlot - 1, "defense", false})};
		}
		return new TimedMethod[] {new TimedMethod(0, "Audio", new object[] {"Oil"}),
	    	new TimedMethod(60, "Log", new object[] {Party.GetEnemy().ToString() + " was not covered in oil"})};		
	}
}