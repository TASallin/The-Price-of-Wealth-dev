public class Meatloaf : Item {
	
	public Meatloaf() {name = "Meatloaf"; description = "Restores 15 hp. You gain 5 power"; selects = true; price = 11; usableOut = true;}
	
	public override TimedMethod[] UseSelected(int i) {
		Party.members[i].Heal(15);
		Party.members[i].SetPower(Party.members[i].GetPower() + 5);
		return new TimedMethod[] {new TimedMethod(0, "Audio", new object[] {"Eat"}), new TimedMethod(0, "Audio", new object[] {"Fire"}),
		    new TimedMethod(60, "Log", new object[] {Party.members[i].GetName() + " ate a meatloaf. You feel fulfilled"}),
			new TimedMethod(0, "CharLogSprite", new object[] {"15", i, "healing", true}),
			new TimedMethod(0, "CharLogSprite", new object[] {"5", i, "power", true})};
	}

    public override void UseOutOfCombat(int i) {
		UseSelected(i);
	}
}