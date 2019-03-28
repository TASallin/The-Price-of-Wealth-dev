public class SlimeGoo : Item {
	
	public SlimeGoo () {name = "Slime Goo"; description = "Goops the enemy"; price = 2;}
	
	public override TimedMethod[] Use() {
		if (Attacks.EvasionCycle(Party.GetPlayer(), Party.GetEnemy())) {
			TimedMethod[] goopPart = Party.GetEnemy().status.Goop();
			return new TimedMethod[] {new TimedMethod(0, "Audio", new object[] {"Slime"}), goopPart[0], goopPart[1]};
		} else {
		    return new TimedMethod[] {new TimedMethod(0, "Audio", new object[] {"Slime"}), new TimedMethod(60, "Log", new object[] {"miss"})};
		}
	}
}