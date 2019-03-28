public class Trumpet : Special {
    
	public Trumpet() {name = "Trumpet"; description = "Reset enemy attack"; baseCost = 2; modifier = 0;}

    public override TimedMethod[] Use() {
		if (Attacks.EvasionCycle(Party.GetPlayer(), Party.GetEnemy())) {
            string message = Party.GetEnemy() + " lost attack";
		    Status.NullifyAttack(Party.GetEnemy());
			return new TimedMethod[] {new TimedMethod(0, "Audio", new object[] {"Trumpet"}),
    			new TimedMethod(60, "Log", new object[] {Party.GetEnemy() + " lost attack"}),
				new TimedMethod(0, "CharLogSprite", new object[] {"atk reset", Party.enemySlot - 1, "nullAttack", false})};
		}
		return new TimedMethod[] {new TimedMethod(0, "Audio", new object[] {"Trumpet"}), new TimedMethod(60, "Log", new object[] {"miss"})};
    }
}