public class ForceField : Passive {
	
	public ForceField(Character c) {self = c; name = "Force field"; description = "Gain 2 block every turn";}
	
	public override TimedMethod[] CheckLead(bool player) {
		self.GainGuard(2);
		return new TimedMethod[] {new TimedMethod(0, "CharLogSprite", new object[] {"2", self.partyIndex, "guard", player})};
	}
	
	public override TimedMethod[] Initialize(bool player) {
		return new TimedMethod[] {new TimedMethod(60, "Log", new object[] {self.ToString() + " is protected by a force field"})};
	}

}