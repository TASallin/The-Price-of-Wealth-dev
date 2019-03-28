public class Exam : Item {
	
	public Exam () {
		name = "Exam"; description = "Whoever writes their name on this exam will feel Serene"; price = 11; usableOut = true;
	}
	
	public override TimedMethod[] Use () {
		Party.AddItem(this);
		return new TimedMethod[] {new TimedMethod(60, "Log", new object[] {"No time for this"}),
    		new TimedMethod(0, "Audio", new object[] {"Skip Turn"}),
			new TimedMethod(0, "CharLogSprite", new object[] {"SKIP", Party.playerSlot - 1, "skip", true})};
	}
	
	public override void UseOutOfCombat (int index) {
		Party.members[index].SetQuirk(new Serene(Party.members[index]));
	}
	
}