public class Footwork : Passive {
	
	public Footwork(Character c) {self = c; name = "Footwork"; description = "Gain 3 evasion each turn";}
	
	public override TimedMethod[] CheckLead(bool player) {
		self.GainEvasion(3);
		return new TimedMethod[0];if (!self.GetGooped()) {
    		self.GainEvasion(3);
			return new TimedMethod[] {new TimedMethod(0, "CharLogSprite", new object[] {"3", self.partyIndex, "evasion", player})};
		}
		return new TimedMethod[0];
	}
	
	
	public override TimedMethod[] Initialize(bool player) {
		return new TimedMethod[] {new TimedMethod(0, "CharLogSprite", new object[] {"Dodgy 3", self.partyIndex, "dexterity", player})};
	}
}