public class Average : Quirk {
	
	public Average (Character c) {self = c; name = "Average"; description = "Attack and defense never change";}
	
	public override TimedMethod[] Check (bool player) {
		Status.NullifyAttack(self); Status.NullifyDefense(self);
		return new TimedMethod[] {new TimedMethod(0, "CharLogSprite", new object[] {"atk reset", self.partyIndex, "nullAttack", self.GetPlayer()}),
		    new TimedMethod(0, "CharLogSprite", new object[] {"def reset", self.partyIndex, "nullDefense", self.GetPlayer()})};
	}
	
	public override TimedMethod[] CheckLead (bool player) {return Check(player);}
}