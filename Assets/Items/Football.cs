public class Football : Item {
	
	public Football() {name = "Football"; description = "Attack for moderate damage. If you hit, get this item back"; price = 4;}
	
	public override TimedMethod[] Use () {
		if (Attacks.EvasionCheck(Party.GetEnemy(), Party.GetPlayer().GetAccuracy())) {
			Party.AddItem(this);
		}
		Attacks.SetAudio("Blunt Hit", 20);
		return new TimedMethod[] {new TimedMethod(0, "Audio", new object[] {"Big Swing"}), new TimedMethod(0, "StagnantAttack", new object[] {
			true, Party.GetPlayer().GetStrength() + 4, Party.GetPlayer().GetStrength() + 4, Party.GetPlayer().GetAccuracy(), true, true, false})};
	}
}