public class Feast : Special {
	
	public Feast () {name = "Feast"; description = "Restores 5 health to all party members"; baseCost = 7; modifier = 0; usableOut = true;}
	
	public override TimedMethod[] UseSupport (int i) {
		foreach (Character c in Party.members) {
			if (c != null && c.GetAlive()) {
				c.Heal(5);
			}
		}
	    return new TimedMethod[] {new TimedMethod(0, "Audio", new object[] {"Eat"}), new TimedMethod(0, "Audio", new object[] {"Heal"}),
		    new TimedMethod(0, "Audio", new object[] {"Skill2"}),
			new TimedMethod(0, "CharLogSprite", new object[] {"5", 0, "healing", true}),
			new TimedMethod(0, "CharLogSprite", new object[] {"5", 1, "healing", true}),
			new TimedMethod(0, "CharLogSprite", new object[] {"5", 2, "healing", true}),
			new TimedMethod(0, "CharLogSprite", new object[] {"5", 3, "healing", true}),
			new TimedMethod(30, "Log", new object[] {"Party was healed"})};
	}
	
	public override void UseOutOfCombat () {
		Party.UseSP(GetCost());
		foreach (Character c in Party.members) {
			if (c != null) {
				c.Heal(5);
			}
		}
	}
	
}