public class ProteinBar : Item {
	
	public ProteinBar() {name = "Protein Bar"; description = "Restores 5 hp. You gain 1 power"; selects = true; price = 2; usableOut = true;}
	
	public override TimedMethod[] UseSelected(int i) {
		Party.members[i].Heal(5);
		Party.members[i].SetPower(Party.members[i].GetPower() + 1);
		return new TimedMethod[] {new TimedMethod(0, "Audio", new object[] {"Eat"}),
		    new TimedMethod(60, "Log", new object[] {Party.members[i].GetName() + " ate a protein bar. Yay protein."}),
			new TimedMethod(0, "CharLogSprite", new object[] {"5", i, "healing", true}),
			new TimedMethod(0, "CharLogSprite", new object[] {"1", i, "power", true})};
	}

    public override void UseOutOfCombat(int i) {
		UseSelected(i);
	}
}