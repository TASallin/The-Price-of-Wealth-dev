public class IronSpinach : Item {
	
	public IronSpinach() {name = "Iron Spinach"; description = "Restores 15 hp. You gain 5 defense"; selects = true; price = 11; usableOut = true;}
	
	public override TimedMethod[] UseSelected(int i) {
		Party.members[i].Heal(15);
		Party.members[i].SetDefense(Party.members[i].GetDefense() + 5);
		return new TimedMethod[] {new TimedMethod(0, "Audio", new object[] {"Eat"}), new TimedMethod(0, "Audio", new object[] {"Metal Hit"}),
    		new TimedMethod(60, "Log", new object[] {
			Party.members[i].GetName() + " ate the iron spinach. It tasted pretty bad"}),
			new TimedMethod(0, "CharLogSprite", new object[] {"15", i, "healing", true}),
			new TimedMethod(0, "CharLogSprite", new object[] {"5", i, "defense", true})};
	}

    public override void UseOutOfCombat(int i) {
		UseSelected(i);
	}
}