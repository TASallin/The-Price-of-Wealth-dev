public class Textbook : Item {
	
	public Textbook() {name = "Textbook"; description = "Heavy and inaccurate as a weapon"; price = 2;}
	
	public override TimedMethod[] Use() {
		Attacks.SetAudio("Blunt Hit", 15);
		return new TimedMethod[] {new TimedMethod(60, "Log", new object[] {
			Party.GetPlayer().GetName() + " swung the textbook, sending pages everywhere"}),
    		new TimedMethod(0, "Audio", new object[] {"Big Swing"}), new TimedMethod(0, "StagnantAttack", new object[] {
			true, Party.GetPlayer().GetStrength() + 5, Party.GetPlayer().GetStrength() + 9, Party.GetPlayer().GetAccuracy() / 2, true, true, false})};
	}
}