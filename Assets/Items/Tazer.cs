public class Tazer : Item {
	
	public Tazer() {name = "Tazer";	description = "A piercing attack that stuns"; price = 3;}
	
	public override TimedMethod[] Use () {
		Attacks.SetAudio("Tazer", 6);
		TimedMethod[] stunPart;
		if (Attacks.EvasionCheck(Party.GetEnemy(), Party.GetPlayer().GetAccuracy())) {
		    stunPart = Party.GetEnemy().status.Stun(2);
		} else {
			stunPart = new TimedMethod[] {new TimedMethod("Null"), new TimedMethod("Null")};
		}
		//TimedMethod[] attackPart = new TimedMethod[] {new TimedMethod(0, "Audio", new object[] {"Button"}),
		//    new TimedMethod(0, "StagnantAttack", new object[] {true, 6, 6, Party.GetPlayer().GetAccuracy(), true, true, true})};
		//TimedMethod[] total = new TimedMethod[2];
		//attackPart.CopyTo(total, 0);
		//stunPart.CopyTo(total, 1);
		return new TimedMethod[] {new TimedMethod(0, "Audio", new object[] {"Button"}), new TimedMethod(0, "StagnantAttack", new object[] {
			true, 6, 6, Party.GetPlayer().GetAccuracy(), true, true, true}), stunPart[0], stunPart[1]};
	}
	
}