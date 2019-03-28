public class Ninja : Quirk {
	
	public Ninja (Character c) {
		self = c; name = "Ninja"; description = "You are immune to blind and start fights with evasion equal to your accuracy";
	}
	
	public override TimedMethod[] Initialize (bool player) {
		self.status.blindImmune = true;
		self.GainEvasion(self.GetAccuracy());
		return new TimedMethod[] {new TimedMethod(0, "CharLogSprite", new object[] {self.GetAccuracy().ToString(), self.partyIndex, "evasion", player})};
	}
}