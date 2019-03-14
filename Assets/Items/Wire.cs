public class Wire : Item {
	
	public Wire () {name = "Wire"; description = "Electrocutes an enemy, nullifying their defense"; price = 2;}
	
	public override TimedMethod[] Use () {
		if (Attacks.EvasionCheck(Party.GetEnemy(), Party.GetPlayer().GetAccuracy())) {
			Status.NullifyDefense(Party.GetEnemy());
		}
		Attacks.SetAudio("Shock", 10);
		return new TimedMethod[] {new TimedMethod(0, "Audio", new object[] {"Small Swing"}), new TimedMethod(0, "StagnantAttack", new object[] {
			true, Party.GetPlayer().GetStrength(), Party.GetPlayer().GetStrength() + 2, Party.GetPlayer().GetAccuracy(), true, true, false})};
	}
}