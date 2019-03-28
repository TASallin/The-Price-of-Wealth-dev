public class GoldenPizza : Item {
	
	public GoldenPizza() {name = "Golden Pizza"; description = "Restores 100 hp and cures poison"; selects = true; price = 6; usableOut = true;}
	
	public override TimedMethod[] UseSelected(int i) {
		TimedMethod curePart = new TimedMethod("Null");
		if (Party.members[i].GetPoisoned()) {
			curePart = new TimedMethod(0, "CharLogSprite", new object[] {"Cured", i, "poison", true});
		}
		Party.members[i].Heal(100);
		Party.members[i].status.poisoned = 0;
		return new TimedMethod[] {new TimedMethod(0, "Audio", new object[] {"Eat"}), new TimedMethod(0, "Audio", new object[] {"Heal"}),
		    new TimedMethod(60, "Log", new object[] {Party.members[i].GetName() + " ate the golden pizza. Despite the food coloring, it was exquisite"}),
			new TimedMethod(0, "CharLogSprite", new object[] {"100", i, "healing", true}), curePart};
	}

    public override void UseOutOfCombat(int i) {
		UseSelected(i);
	}
}