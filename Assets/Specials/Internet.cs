public class Internet : Special {
	
	public Internet() {name = "Internet"; description = "Learn about the opponent and gain evasion"; baseCost = 1; modifier = 0;}
	
	public override TimedMethod[] UseSupport(int i) {
		TimedMethod evadePart = new TimedMethod("Null");
		if (!Party.members[i].GetGooped()) {
			evadePart = new TimedMethod(0, "CharLogSprite", new object[] {"8", i, "evasion", true});
		}
		Party.GetPlayer().GainEvasion(8);
		string[] lines = Party.GetEnemy().CSDescription();
		TimedMethod[] returned = new TimedMethod[lines.Length + 2];
		returned[0] = new TimedMethod(0, "Audio", new object[] {"Wikipedia"});
		returned[1] = new TimedMethod(0, "CharLogSprite", new object[] {"8", i, "evasion", true});
		int index = 2;
		foreach (string s in lines) {
			returned[index] = new TimedMethod(120, "Log", new object[] {s});
			index++;
		}
		return returned;
    }
}