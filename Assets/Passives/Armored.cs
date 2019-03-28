public class Armored : Passive {
	
    int formerHP;
	
	public Armored (Character c) {
		self = c; name = "Armored";
		description = "+1 defense. At the start of your turn, gain 1 guard if you didn't take damage last turn";
		formerHP = System.Int32.MaxValue;
	}
	
	public override TimedMethod[] CheckLead (bool player) {
		if (self.GetHealth() >= formerHP) {
			self.SetGuard(self.GetGuard() + 1);
			formerHP = self.GetHealth();
			return new TimedMethod[] {new TimedMethod(0, "CharLogSprite", new object[] {"1", self.partyIndex, "guard", self.GetPlayer()})};
		}
		formerHP = self.GetHealth();
		return new TimedMethod[0];
	}
	
	public override TimedMethod[] Initialize (bool player) {
		formerHP = System.Int32.MaxValue;
		self.GainDefense(1);
		return new TimedMethod[] {new TimedMethod(0, "CharLogSprite", new object[] {"1", self.partyIndex, "defense", self.GetPlayer()})};
	}
	
}