public class Read : Special {
	
	public Read() {name = "Read"; description = "Gain 2 regeneration and fall asleep"; baseCost = 5; modifier = 0;}
	
	public override TimedMethod[] UseSupport (int i) {
		TimedMethod[] regenPart = Party.members[i].status.Regenerate(2);
		TimedMethod[] sleepPart = Party.members[i].status.Sleep();
		return new TimedMethod[] {new TimedMethod(0, "Audio", new object[] {"Heal"}),
    		new TimedMethod(60, "Log", new object[] {Party.members[i].ToString() + " is reading"}), regenPart[0], sleepPart[0], sleepPart[1]};
	}
}