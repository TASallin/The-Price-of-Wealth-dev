public class Donut : Item {
	
	public Donut() {name = "Donut"; description = "Restores 15 hp. You lose 1 power and defense"; selects = true; price = 2; usableOut = true;}
	
	public override TimedMethod[] UseSelected(int i) {
		Party.members[i].Heal(15);
		Party.members[i].SetPower(Party.members[i].GetPower() - 1);
		Party.members[i].SetDefense(Party.members[i].GetDefense() - 1);
		return new TimedMethod[] {new TimedMethod(0, "Audio", new object[] {"Eat"}), new TimedMethod(0, "AudioAfter", new object[] {"Slime", 15}),
		    new TimedMethod(60, "Log", new object[] {Party.members[i].GetName() + " ate a donut. It was total junk."}),
			new TimedMethod(0, "CharLogSprite", new object[] {"15", i, "healing", true}),
			new TimedMethod(0, "CharLogSprite", new object[] {"-1", i, "power", true}),
			new TimedMethod(0, "CharLogSprite", new object[] {"-1", i, "defense", true})};
	}

    public override void UseOutOfCombat(int i) {
		UseSelected(i);
	}
}