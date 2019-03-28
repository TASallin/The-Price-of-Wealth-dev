public class USB : Item {
	
	public USB () {name = "USB"; description = "Attack for 1 damage without removing charge. If you miss, gain 2 charge. Repeatable"; price = 2;}
	
	public override TimedMethod[] Use() {
		Party.AddItem(this);
		Attacks.SetAudio("Knife", 10);
		TimedMethod statPart = new TimedMethod("Null");
		if (!Attacks.EvasionCheck(Party.GetEnemy(), Party.GetPlayer().GetAccuracy())) {
			Party.GetPlayer().SetCharge(Party.GetPlayer().GetCharge() + 2);
			statPart = new TimedMethod(0, "CharLogSprite", new object[] {"2", Party.playerSlot - 1, "charge", true});
		}
		return new TimedMethod[] {new TimedMethod(0, "Audio", new object[] {"Small Swing"}), new TimedMethod(0, "StagnantAttack", new object[] {
		    true, 1, 1, Party.GetPlayer().GetAccuracy(), false, false, false}), statPart};
	}
}