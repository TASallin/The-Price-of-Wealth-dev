public class Flask : Item {
	
    public Flask () {name = "Flask"; description = "Deals 1 damage. Can hold a liquid"; price = 1;}

    public override TimedMethod[] Use () {
		Attacks.SetAudio("Glass Break", 20);
	    return new TimedMethod[] {new TimedMethod(0, "Audio", new object[] {"Missile"}),
		    new TimedMethod(60, "StagnantAttack", new object[] {
			true, 1, 1, Party.GetPlayer().GetAccuracy(), true, true, false})};
	}	
}