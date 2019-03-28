public class Prescribe : Special {
	
	public Prescribe () {name = "Prescribe"; description = "A heal that cures poison"; baseCost = 3; modifier = 0; selects = true; usableOut = true;}
	
	public override TimedMethod[] UseSelects(int i) {
		TimedMethod curePart = new TimedMethod("Null");
		if (Party.members[i].GetPoisoned()) {
			curePart = new TimedMethod(0, "CharLogSprite", new object[] {"Cured", i, "poison", true});
		}
		System.Random rng = new System.Random();
		int amount = rng.Next(8) + 5;
		Party.members[i].Heal(amount);
		Party.members[i].status.poisoned = 0;
		return new TimedMethod[] {new TimedMethod(0, "Audio", new object[] {"Heal"}),
		    new TimedMethod(60, "Log", new object[] {Party.members[i].ToString() + " was healed for " + amount.ToString() + " hp"}),
			new TimedMethod(0, "CharLogSprite", new object[] {amount.ToString(), i, "healing", true}), curePart};
	}
	
}