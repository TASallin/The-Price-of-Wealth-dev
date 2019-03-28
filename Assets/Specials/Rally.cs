public class Rally : Special {
    
    public Rally() {name = "Rally"; description = "team gains 1 power"; baseCost = 2; modifier = 0;}

    public override TimedMethod[] UseSupport(int i) {
		foreach (Character c in Party.members) {
		    if (c != null && c.GetAlive()) {
		        c.SetPower(c.GetPower() + 1);
			}
		}
		return new TimedMethod[] {new TimedMethod(0, "Log", new object[] {Party.members[i].ToString() + " rallied"}),
		    new TimedMethod(0, "Audio", new object[] {"Whistle"}),
			new TimedMethod(15, "CharLogSprite", new object[] {"1", 0, "power", true}),
			new TimedMethod(15, "CharLogSprite", new object[] {"1", 1, "power", true}),
			new TimedMethod(15, "CharLogSprite", new object[] {"1", 2, "power", true}),
			new TimedMethod(15, "CharLogSprite", new object[] {"1", 3, "power", true})};
    }		
} 