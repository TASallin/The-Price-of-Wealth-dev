public class Baton : Item {
	
	public Baton () {
		name = "Baton"; description = "Turns a music major's passive into Directive, causing party to rotate each turn. Use out of combat";
		price = 4; usableOut = true;
	}
	
	public override TimedMethod[] Use () {
		Party.AddItem(this);
		return new TimedMethod[] {new TimedMethod(60, "Log", new object[] {"No time for this"}),
		    new TimedMethod(0, "Audio", new object[] {"Skip Turn"}),
			new TimedMethod(0, "CharLogSprite", new object[] {"SKIP", Party.playerSlot - 1 , "skip", true})};
	}
	
	public override void UseOutOfCombat (int index) {
		if (Party.members[index].GetPassive().GetName() == "Performance") {
			Party.members[index].SetPassive(new Directive(Party.members[index]));
		} else {
			Party.AddItem(this);
		}
	}
	
}