public class Sanitizer : Item {
	
	public Sanitizer() {name = "Sanitizer"; description = "Recover from poisoned, gooped, and blinded"; price = 2; usableOut = true; selects = true;}

	public override TimedMethod[] UseSelected (int i) {
		TimedMethod[] curePart = new TimedMethod[] {new TimedMethod("Null"), new TimedMethod("Null"), new TimedMethod("Null")};
		if (Party.members[i].GetPoisoned()) {
			curePart[0] = new TimedMethod(0, "CharLogSprite", new object[] {"Cured", i, "poison", true});
		}
		if (Party.members[i].GetPoisoned()) {
			curePart[1] = new TimedMethod(0, "CharLogSprite", new object[] {"Cleaned", i, "goop", true});
		}
		if (Party.members[i].GetPoisoned()) {
			curePart[2] = new TimedMethod(0, "CharLogSprite", new object[] {"Recover", i, "blind", true});
		}
		Party.members[i].status.poisoned = 0;
		Party.members[i].status.gooped = false;
		Party.members[i].status.blinded = 0;
		return new TimedMethod[] {new TimedMethod(0, "Audio", new object[] {"Clean"}),
    		new TimedMethod(60, "Log", new object[] {"Negative effects were removed"}), curePart[0], curePart[1], curePart[2]};
	}
	
	public override void UseOutOfCombat (int i) {
		UseSelected(i);
	}
	
}