public class Tuba : Item {
	
	public Tuba() {name = "Tuba"; description = "Reset attack and defense on opponent"; price = 2;}
	
	public override TimedMethod[] Use() {
		TimedMethod[] nullPart = new TimedMethod[] {new TimedMethod("Null"), new TimedMethod("Null")};
		if (Attacks.EvasionCycle(Party.GetPlayer(), Party.GetEnemy())) {
		    Status.NullifyAttack(Party.GetEnemy()); Status.NullifyDefense(Party.GetEnemy());
			nullPart[0] = new TimedMethod(0, "CharLogSprite", new object[] {"atk reset", Party.enemySlot - 1, "nullAttack", false});
			nullPart[1] = new TimedMethod(0, "CharLogSprite", new object[] {"def reset", Party.enemySlot - 1, "nullDefense", false});
		}
		string message = Party.GetPlayer().GetName() + " blasted the tuba!";
		return new TimedMethod[] {new TimedMethod(0, "Audio", new object[] {"Trumpet"}), new TimedMethod(60, "Log", new object[] {message}),
		    nullPart[0], nullPart[1]};
	}
}