public class Sword : Textbook {
	
	public Sword() {name = "Actual Sword"; description = "Wow! An actual sword"; price = 3;}
	
	public override TimedMethod[] Use() {
		Attacks.SetAudio("Sword", 15);
		return new TimedMethod[] {new TimedMethod(60, "Log", new object[] {
			Party.GetPlayer().GetName() + " attacked with the sword. It broke immediately"}),
		    new TimedMethod(0, "Audio", new object[] {"Big Swing"}), new TimedMethod(0, "StagnantAttack", new object[] {
			true, Party.GetPlayer().GetStrength() + 7, Party.GetPlayer().GetStrength() + 11, Party.GetPlayer().GetAccuracy(), true, true, false})};
	}
}