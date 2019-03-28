public class Pizza : Item {
	
	public Pizza() {name = "Pizza"; description = "Restores 10 hp"; selects = true; price = 2; usableOut = true;}
	
	public override TimedMethod[] UseSelected(int i) {
		Party.members[i].Heal(10);
		return new TimedMethod[] {new TimedMethod(0, "Audio", new object[] {"Eat"}), new TimedMethod(0, "Audio", new object[] {"Heal"}),
		    new TimedMethod(60, "Log", new object[] {
			Party.members[i].GetName() + " ate a pizza. Technically it was free pizza!"}),
			new TimedMethod(0, "CharLogSprite", new object[] {"10", i, "healing", true})};
	}

    public override void UseOutOfCombat(int i) {
		UseSelected(i);
	}
}