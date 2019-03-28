public class Tumble : Special {
	
	public Tumble() {name = "Tumble"; description = "Double your evasion"; baseCost = 3; modifier = 0;}
	
	public override TimedMethod[] Use () {
		TimedMethod evadePart = new TimedMethod("Null");
		if (!Party.GetPlayer().GetGooped()) {
			evadePart = new TimedMethod(0, "CharLogSprite", new object[] {
				Party.GetPlayer().GetEvasion().ToString(), Party.playerSlot - 1, "evasion", true});
		}
		Party.GetPlayer().GainEvasion(Party.GetPlayer().GetEvasion());
		return new TimedMethod[] {new TimedMethod(0, "Audio", new object[] {"Skill1"}), new TimedMethod(0, "Audio", new object[] {"Running"}),
		    new TimedMethod(60, "Log", new object[] {Party.GetPlayer().ToString() + " used advance moves"}), evadePart};
	}
}