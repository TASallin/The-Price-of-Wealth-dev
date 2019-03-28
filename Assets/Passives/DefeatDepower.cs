public class DefeatDepower : Passive {
	
	int previousCount;
	
	public DefeatDepower(Character c) {self = c; name = "Defeat Depower"; description = "Team loses power when a member is defeated";}
	
	public override TimedMethod[] Check(bool player) {
		int count; Character[] team;
		if (player) {count = Party.playerCount; team = Party.members;} else {count = Party.enemyCount; team = Party.enemies;}
		int amount = -4 * (previousCount - count);
		previousCount = count;
		if (previousCount > count) {
			foreach (Character c in team) {
				if (c != null && c.GetAlive()) {
					c.GainPower(amount);
				}
			}
		    return new TimedMethod[] {new TimedMethod(60, "Log", new object[] {"Morale has fallen after a team-member's defeat"}),
			new TimedMethod(0, "CharLogSprite", new object[] {amount.ToString(), 0, "power", player}),
			new TimedMethod(0, "CharLogSprite", new object[] {amount.ToString(), 1, "power", player}),
			new TimedMethod(0, "CharLogSprite", new object[] {amount.ToString(), 2, "power", player}),
			new TimedMethod(0, "CharLogSprite", new object[] {amount.ToString(), 3, "power", player})};
		}
		previousCount = count;
		return new TimedMethod[0];
	}
	
	public override TimedMethod[] CheckLead(bool player) {
		return Check(player);
	}
	
	public override TimedMethod[] Deactivate(bool player) {
		return Check(player);
	}
	
	public override TimedMethod[] Initialize(bool player) {
		if (player) {previousCount = Party.playerCount;} else {previousCount = Party.enemyCount;}
		return new TimedMethod[0];
	}
}

