public class Triage : Special {
	
	public Triage () {name = "Triage"; description = "Restore or revive a party member to 1/2 max hp"; baseCost = 8; modifier = 0;
	    useDead = true; selects = true;}
	
	public override TimedMethod[] UseSelects (int i) {
		if (Party.members[i] != null) {
			if (!Party.members[i].GetAlive()) {
				Party.playerCount++;
				Party.members[i].SetAlive(true);
			}
			Party.members[i].SetHealth(System.Math.Max(Party.members[i].GetHealth(), Party.members[i].GetMaxHP() / 2));
		}
		return new TimedMethod[] {new TimedMethod(0, "Audio", new object[] {"Skill1"}), new TimedMethod(0, "Audio", new object[] {"Heal"}),
		    new TimedMethod(60, "Log", new object[] {Party.GetPlayer().ToString() + " performed tiring healing"}),
			new TimedMethod(0, "CharLogSprite", new object[] {"1/2 MAX", i, "regeneration", true})};
	}
	
	//public override void UseOutOfCombat () {
		//Party.UseSP(GetCost());
		//Use();
	//}
}