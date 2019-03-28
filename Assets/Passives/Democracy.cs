public class Democracy : Passive {
	
	public int promise;
	public bool attacked;
	
	public Democracy (Character c) {
		self = c; name = "Democracy"; description = "Each turn, gain charge and guard equal to your party size minus the enemy party size";
	}
	
	public override TimedMethod[] CheckLead(bool player) {
		TimedMethod[] promisePart = new TimedMethod[] {new TimedMethod("Null"), new TimedMethod("Null"), new TimedMethod("Null")};
		if (promise == 2) {
			promise--;
		} else if (promise == 1 && attacked) {
			promise--;
		} else if (promise == 1) {
			self.GainPower(-4);
			self.GainDefense(-4);
			promise--;
			self.GetSpecial().modifier += 10;
			promisePart = new TimedMethod[] {new TimedMethod(60, "Log", new object[] {"The promise was broken! Voter support is zero!"}),
			    new TimedMethod(0, "CharLogSprite", new object[] {"-4", self.partyIndex, "power", player}),
			    new TimedMethod(0, "CharLogSprite", new object[] {"-4", self.partyIndex, "defense", player})};
		}
		attacked = false;
		int advantage;
		if (player) {
			advantage = Party.playerCount - Party.enemyCount;
		} else {
			advantage = Party.enemyCount - Party.playerCount;
		}
		self.GainCharge(advantage);
		self.GainGuard(advantage);
		return new TimedMethod[] {new TimedMethod(60, "Log", new object[] {
			ToString() + " has an advantage of " + advantage.ToString()}),
			new TimedMethod(0, "CharLogSprite", new object[] {advantage.ToString(), self.partyIndex, "charge", player}),
			new TimedMethod(0, "CharLogSprite", new object[] {advantage.ToString(), self.partyIndex, "guard", player}),
			promisePart[0], promisePart[1], promisePart[2]};
	}
	
	public override TimedMethod[] Initialize(bool player) {
		promise = 0; attacked = false;
		return new TimedMethod[0];
	}
}