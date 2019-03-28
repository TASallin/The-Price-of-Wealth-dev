public class Roll : Special {
	
	public Roll() {name = "Roll"; description = "Gain 8 evasion"; baseCost = 1; modifier = 0;}
	
	public override TimedMethod[] Use() {
		TimedMethod evadePart = new TimedMethod("Null");
		if (!Party.GetPlayer().GetGooped()) {
			evadePart = new TimedMethod(0, "CharLogSprite", new object[] {"8", Party.playerSlot - 1, "evasion", true});
		}
		Party.GetPlayer().GainEvasion(8);
		return new TimedMethod[] {new TimedMethod(0, "Audio", new object[] {"Big Swing"}),
		    new TimedMethod(60, "Log", new object[] {Party.GetPlayer().ToString() + " rolled"}), evadePart};
	}
}