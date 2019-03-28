public class Milk : Item {
	
	public Milk() {name = "Milk"; description = "Restores 5 hp and cure poison"; selects = true; price = 2; usableOut = true;}
	
	public override TimedMethod[] UseSelected(int i) {
		TimedMethod curePart = new TimedMethod("Null");
		if (Party.members[i].GetPoisoned()) {
			curePart = new TimedMethod(0, "CharLogSprite", new object[] {"Cured", i, "poison", true});
		}
		Party.members[i].Heal(5);
		Party.members[i].status.poisoned = 0;
		return new TimedMethod[] {new TimedMethod(0, "Audio", new object[] {"Drink"}),
		    new TimedMethod(60, "Log", new object[] {Party.members[i].GetName() + " ate the milk. It tasted like milk."}),
			new TimedMethod(0, "CharLogSprite", new object[] {"5", i, "healing", true}), curePart};
	}

    public override void UseOutOfCombat(int i) {
		UseSelected(i);
	}
}