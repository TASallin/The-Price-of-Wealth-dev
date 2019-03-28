public class Warsong : Special {
	
	public Warsong () {name = "War Song"; description = "Party gains 3 charge and guard"; baseCost = 3; modifier = 0;}
	
	public override TimedMethod[] UseSupport(int i) {
		foreach (Character c in Party.members) {
			if (c != null && c.GetAlive()) {
				c.GainCharge(3); c.GainGuard(3);
			}
		}
		return new TimedMethod[] {new TimedMethod(0, "Audio", new object[] {"Allegro"}),
		    new TimedMethod(0, "Log", new object[] {Party.members[i].ToString() + " gave an inspiring song"}),
			new TimedMethod(15, "CharLogSprite", new object[] {"3", 0, "charg", true}),
			new TimedMethod(15, "CharLogSprite", new object[] {"3", 1, "charge", true}),
			new TimedMethod(15, "CharLogSprite", new object[] {"3", 2, "charge", true}),
			new TimedMethod(15, "CharLogSprite", new object[] {"3", 3, "charge", true}),
			new TimedMethod(15, "CharLogSprite", new object[] {"3", 0, "guard", true}),
			new TimedMethod(15, "CharLogSprite", new object[] {"3", 1, "guard", true}),
			new TimedMethod(15, "CharLogSprite", new object[] {"3", 2, "guard", true}),
			new TimedMethod(15, "CharLogSprite", new object[] {"3", 3, "guard", true})};
	}

}