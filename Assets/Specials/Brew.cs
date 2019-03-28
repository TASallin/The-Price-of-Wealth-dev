public class Brew : Special {
	
	public Brew () {name = "Brew"; description = "if you have a flask, creates a toxic brew. Must be out of combat"; baseCost = 4;
    usableOut = true; modifier = 0;}
	
	public override TimedMethod[] Use () {
		Party.UseSP(GetCost() * -1);
		return new TimedMethod[] {new TimedMethod(60, "Log", new object[] {"You don't have time to do this"}),
		    new TimedMethod(0, "Audio", new object[] {"Skip Turn"}),
			new TimedMethod(0, "CharLogSprite", new object[] {"SKIP", Party.playerSlot - 1, "skip", true})};
	}
	
	public override void UseOutOfCombat () {
		for (int i = 0; i < 10; i++) {
			if (Party.GetItems()[i] != null && Party.GetItems()[i].GetType().Equals(typeof(Flask))) {
				Party.GetItems()[i] = new ToxicSolution();
				Party.UseSP(GetCost());
				break;
			}
		}
	}
}