public class Curry : Item {
	
	public Curry() {name = "Curry"; description = "Restores 5 hp. You gain 5 charge"; selects = true; price = 2; usableOut = true;}
	
	public override TimedMethod[] UseSelected(int i) {
		Party.members[i].Heal(5);
		Party.members[i].SetCharge(Party.members[i].GetCharge() + 5);
		return new TimedMethod[] {new TimedMethod(0, "Audio", new object[] {"Eat"}),
		    new TimedMethod(60, "Log", new object[] {Party.members[i].GetName() + " ate the curry. It was spicy."}),
			new TimedMethod(0, "CharLogSprite", new object[] {"5", i, "healing", true}),
			new TimedMethod(0, "CharLogSprite", new object[] {"5", i, "charge", true})};
	}

    public override void UseOutOfCombat(int i) {
		UseSelected(i);
	}
}