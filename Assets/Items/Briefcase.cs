public class Briefcase : Item {
	
	public Briefcase() {name = "Briefcase"; description = "Your items may not be stolen"; price = 2;}
	
	public override TimedMethod[] Use () {
        return new TimedMethod[] {new TimedMethod(60, "Log", new object[] {
			Party.GetPlayer().ToString() + " set the briefcase down, opened it, and took"
		    + "all the items out of it, placing them back in the bag before ABANDONING THE BRIEFCASE FOREVER"}),
			new TimedMethod(0, "Audio", new object[] {"Skip Turn"}),
			new TimedMethod(0, "CharLogSprite", new object[] {"SKIP", Party.playerSlot - 1, "skip", true})};
	}
}