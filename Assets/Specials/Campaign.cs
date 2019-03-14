public class Campaign : Special {
	
	public Campaign() {
		name = "Campaign"; description = "Gain 1 power, 1 defense, 2 charge, and 2 guard. If you don't regular attack next turn, face penalties";
	    baseCost = 1; modifier = 0;
	}
	
	public override TimedMethod[] Use () {
		Character self = Party.GetPlayer();
		self.GainPower(1); self.GainDefense(1);
		self.GainCharge(2); self.GainGuard(2);
		Democracy castPassive = (Democracy)self.GetPassive();
		if (castPassive.promise == 0) {
			castPassive.promise = 2;
		}
		return new TimedMethod[] {new TimedMethod(0, "Audio", new object[] {"Blah"}), 
		    new TimedMethod(60, "Log", new object[] {self.ToString() + " promised action"})};
	}
}