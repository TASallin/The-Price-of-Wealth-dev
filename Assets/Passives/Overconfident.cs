public class Overconfident : Quirk {
	
	bool powered;
	
	public Overconfident (Character c) {self = c; name = "Overconfident"; description = "Powered up at max hp";}
	
	public override TimedMethod[] Check(bool player) {
		if (powered && self.GetHealth() < self.GetMaxHP()) {
			powered = false;
			self.GainPower(-3);
			self.GainDefense(-2);
			return new TimedMethod[] {new TimedMethod(0, "CharLogSprite", new object[] {"-3", self.partyIndex, "power", player}),
			    new TimedMethod(0, "CharLogSprite", new object[] {"-2", self.partyIndex, "defense", player})};
		} else if (!powered && self.GetHealth() == self.GetMaxHP()) {
			powered = true;
			self.GainPower(3);
			self.GainDefense(2);
			return new TimedMethod[] {new TimedMethod(0, "CharLogSprite", new object[] {"3", self.partyIndex, "power", player}),
			    new TimedMethod(0, "CharLogSprite", new object[] {"2", self.partyIndex, "defense", player})};
		} else {
			return new TimedMethod[0];
		}
	}

	public override TimedMethod[] CheckLead(bool player) {
		return Check(player);
	} 
	
	public override TimedMethod[] Initialize (bool player) {
		powered = false;
		return new TimedMethod[0];
	}
}