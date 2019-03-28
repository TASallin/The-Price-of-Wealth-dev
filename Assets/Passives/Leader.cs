public class Leader : Passive {
	
	public Leader (Character c) {self = c; name = "Leader"; description = "Lead member other than you gains 1 power per turn";}
	
	public override TimedMethod[] Check(bool player) {
		Character lead;
		if (player) {
			lead = Party.GetPlayer();
		} else {
			lead = Party.GetEnemy();
		}
		lead.SetPower(lead.GetPower() + 1);
		return new TimedMethod[] {new TimedMethod(30, "Log", new object[] {self.ToString() + " is strengthening " + lead.ToString()}),
		    new TimedMethod(0, "CharLogSprite", new object[] {"1", Party.enemySlot, "power", player})};
	}
	
}