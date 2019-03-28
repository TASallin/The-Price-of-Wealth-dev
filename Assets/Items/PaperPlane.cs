public class PaperPlane : Item {
	
	public PaperPlane () {name = "Paper Plane"; description = "A blinding projectile"; price = 2;}
	
	public override TimedMethod[] Use () {
		Attacks.SetAudio("Knife", 30);
		TimedMethod[] blindPart;
		if (Party.GetPlayer().GetAccuracy() > Party.GetEnemy().GetEvasion()) {
			blindPart = Party.GetEnemy().status.Blind(7);
		} else {
			blindPart = new TimedMethod[] {new TimedMethod("Null"), new TimedMethod("Null")};
		}
		TimedMethod[] attackPart = new TimedMethod[] {new TimedMethod(0, "Audio", new object[] {"Small Swing"}),
		    new TimedMethod(0, "StagnantAttack", new object[] {
			true, Party.GetPlayer().GetStrength(), Party.GetPlayer().GetStrength() + 2, Party.GetPlayer().GetAccuracy(), true, true, false})};
		TimedMethod[] total = new TimedMethod[4];
		attackPart.CopyTo(total, 0);
		blindPart.CopyTo(total, 2);
		return total;
	}
	
}