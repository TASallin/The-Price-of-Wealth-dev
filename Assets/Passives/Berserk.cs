public class Berserk : Quirk {
	
	public Berserk (Character c) {self = c; name = "Berserk"; description = "Has 5 power and -5 defense";}
	
	public override TimedMethod[] Initialize (bool player) {
		self.GainPower(5);
		self.GainDefense(-5);
		return new TimedMethod[] {new TimedMethod(0, "CharLogSprite", new object[] {"5", self.partyIndex, "power", self.GetPlayer()}),
		    new TimedMethod(0, "CharLogSprite", new object[] {"-5", self.partyIndex, "defense", self.GetPlayer()})};
	}
	
}