public class Reading : Passive {
	
	public Reading (Character c) {self = c; name = "Reading"; description = "On each turn not in the lead, gain 1 HP and 2 charge";}
	
	public override TimedMethod[] Check(bool player) {
		self.Heal(1); self.GainCharge(2);
		return new TimedMethod[] {new TimedMethod(0, "CharLogSprite", new object[] {"1", self.partyIndex, "healing", player}),
	    	new TimedMethod(0, "CharLogSprite", new object[] {"2", self.partyIndex, "charge", player})};
	}
}