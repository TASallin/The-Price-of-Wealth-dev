public class CoffeeDetector : Passive {
	
	bool used;
	
	public CoffeeDetector(Character c) {self = c; name = "CoffeeDetector"; description = "If the enemy drinks coffee, gain 10 power";}
	
	public override TimedMethod[] Check(bool player) {
		Character enemy;
		if (player) {enemy = Party.GetEnemy();} else {enemy = Party.GetPlayer();}
		if (!used && enemy.status.caffeine > 0) {
			used = true; self.GainPower(10);
			return new TimedMethod[] {new TimedMethod(0, "Audio", new object[] {"Recursion"}),
			   new TimedMethod(60, "Log", new object[] {self.ToString() + " became HYPER at the smell of coffee"}),
			   new TimedMethod(0, "CharLogSprite", new object[] {"10", self.partyIndex, "power", self.GetPlayer()})};
		}
		return new TimedMethod[0];
	}
	
	public override TimedMethod[] CheckLead(bool player) {
		return Check(player);
	}
	
	public override TimedMethod[] Initialize(bool player) {
		used = false;
		return new TimedMethod[0];
	}
}