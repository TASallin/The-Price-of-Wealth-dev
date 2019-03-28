public class Hypnotize : Special {
	
	public Hypnotize() {name = "Hypnotize"; description = "Put the enemy to sleep. It's very accurate"; baseCost = 4; modifier = 0;}
	
	public override TimedMethod[] Use () {
		TimedMethod[] sleepPart = new TimedMethod[] {new TimedMethod(60, "Log", new object[] {"Miss"}), new TimedMethod("Null")};
		if (Attacks.EvasionCycle(Party.GetPlayer().GetAccuracy() * 2, Party.GetEnemy())) {
		    sleepPart = Party.GetEnemy().status.Sleep();
		}
		return new TimedMethod[] {new TimedMethod(0, "Audio", new object[] {"Hypnotize"}),
		    sleepPart[0], sleepPart[1]};
	}
}