public class Rice : Item {
	
	public Rice() {name = "Rice"; description = "Restores 7 hp. Resets attack and defense"; selects = true; price = 2; usableOut = true;}
	
	public override TimedMethod[] UseSelected(int i) {
		Party.members[i].Heal(7);
		Status.NullifyAttack(Party.members[i]); Status.NullifyDefense(Party.members[i]);
		return new TimedMethod[] {new TimedMethod(0, "Audio", new object[] {"Eat"}), new TimedMethod(0, "Audio", new object[] {"Nullify"}),
		    new TimedMethod(60, "Log", new object[] {Party.members[i].GetName() + " ate the rice. It was plain"}),
			new TimedMethod(0, "CharLogSprite", new object[] {"7", i, "healing", true}),
			new TimedMethod(0, "CharLogSprite", new object[] {"atk reset", i, "nullAttack", true}),
			new TimedMethod(0, "CharLogSprite", new object[] {"def reset", i, "nullDefense", true})};
	}

    public override void UseOutOfCombat(int i) {
		UseSelected(i);
	}
}