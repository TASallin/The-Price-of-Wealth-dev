public class Metronome : Item {
	
	public Metronome () {name = "Metronome"; description = "While in inventory, your basic attack deals set damage. Use to discard";
    	price = 3; usableOut = true;}
	
	public override TimedMethod[] Use () {
		return new TimedMethod[] {new TimedMethod(60, "Log", new object[] {
			Party.GetPlayer().ToString() + " likes variance. The metronome was discarded"}), new TimedMethod(0, "Audio", new object[] {"Skip Turn"}),
			    new TimedMethod(0, "CharLogSprite", new object[] {"SKIP", Party.playerSlot - 1, "skip", true})};
	}
	
	public override void UseOutOfCombat (int i) {}
}