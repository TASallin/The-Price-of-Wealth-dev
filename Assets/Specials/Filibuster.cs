public class Filibuster : Special {
	
	public Filibuster() {name = "Filibuster"; description = "put everyone to sleep, even yourself"; baseCost = 4; modifier = 0;}
	
	public override TimedMethod[] UseSupport(int i) {
		TimedMethod[] totalSleep = new TimedMethod[] {new TimedMethod("Null"), new TimedMethod("Null"), new TimedMethod("Null"),
	    	new TimedMethod("Null"), new TimedMethod("Null"), new TimedMethod("Null"), new TimedMethod("Null"), new TimedMethod("Null")};
		int index = 0;
		TimedMethod[] sleepPart;
		foreach (Character c in Party.members) {
			if (c != null && c.GetAlive()) {
				sleepPart = c.status.Sleep();
				totalSleep[index] = sleepPart[0];
				totalSleep[index + 1] = sleepPart[1];
			}
			index += 2;
		}
		foreach (Character c in Party.enemies) {
			if (c != null && c.GetAlive()) {
				c.status.Sleep();
				sleepPart = c.status.Sleep();
				totalSleep[index] = sleepPart[0];
				totalSleep[index + 1] = sleepPart[1];
			}
			index += 2;
		}
		return new TimedMethod[] {new TimedMethod(0, "Audio", new object[] {"Filibuster"}),
		    new TimedMethod(60, "Log", new object[] {Party.members[i].ToString() + " wouldn't stop talking"}),
			totalSleep[0], totalSleep[1], totalSleep[2], totalSleep[3], totalSleep[4], totalSleep[5], totalSleep[6], totalSleep[7]};
	}
}