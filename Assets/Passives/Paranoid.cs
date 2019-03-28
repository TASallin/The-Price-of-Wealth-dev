public class Paranoid : Quirk {
	
	public Paranoid(Character c) {
		self = c;	name = "Paranoid"; description = "You run away immediately instead of after 1 turn. Gain 5 evasion on turn 1";
	}

	public override TimedMethod[] Initialize(bool player) {
		self.GainEvasion(5);
		return new TimedMethod[] {new TimedMethod(0, "CharLogSprite", new object[] {"5", self.partyIndex, "evasion", player})};
	}

}