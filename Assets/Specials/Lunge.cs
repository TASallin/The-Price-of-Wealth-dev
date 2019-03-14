public class Lunge : Special {
	
	public Lunge() {name = "Lunge"; description = "Switch in and attack"; baseCost = 2; modifier = 0;}
	
	public override TimedMethod[] UseSupport (int i) {
		Attacks.SetAudio("Slap", 15);
		return new TimedMethod[] {new TimedMethod(0, "SwitchTo", new object[] {i + 1}), new TimedMethod(0, "Audio", new object[] {"Small Swing"}),
		    new TimedMethod(60, "AttackAny", new object[] {
			Party.members[i], Party.GetEnemy(), Party.members[i].GetStrength(), Party.members[i].GetStrength() + 5,
			Party.members[i].GetAccuracy(), true, true, false})};
	}
}