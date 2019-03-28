public class Pi : Special {
	
	public Pi () {name = "Pi"; description = "Cause the enemy to become apathetic"; baseCost = 3; modifier = 0;}
	
	public override TimedMethod[] UseSupport(int i) {
		TimedMethod[] apathyPart = new TimedMethod[] {new TimedMethod(60, "Log", new object[] {"miss"}), new TimedMethod("Null")};
		if (Attacks.EvasionCycle(Party.members[i], Party.GetEnemy())) {
		    apathyPart = Party.GetEnemy().status.CauseApathy(3);
		}
		return new TimedMethod[] {new TimedMethod(0, "Audio", new object[] {"Filibuster"}), new TimedMethod(60, "Log", new object[] {
			Party.members[i].ToString() + " recited so much Pi it could lower self-esteem"}), apathyPart[0],  apathyPart[1]};
	}
}