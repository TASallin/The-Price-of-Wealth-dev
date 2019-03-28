public class Dodgy : Passive {
	
	public Dodgy(Character c) {self = c; name = "Dodgy"; description = "Gain 2 evasion each turn";}
	
	public override TimedMethod[] CheckLead(bool player) {
		if (!self.GetGooped()) {
    		self.GainEvasion(2);
			return new TimedMethod[] {new TimedMethod(0, "CharLogSprite", new object[] {"3", self.partyIndex, "evasion", player})};
		}
		return new TimedMethod[0];
	}
		
	public override TimedMethod[] Initialize(bool player) {
		return new TimedMethod[] {new TimedMethod(0, "CharLogSprite", new object[] {"Dodgy 2", self.partyIndex, "dexterity", player})};
	}
}