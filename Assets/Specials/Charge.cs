public class Charge : Special {
	
	public Charge () {name = "Charge"; description = "Attack the front enemy with high power and other enemies with low power";
    	baseCost = 3; modifier = 0;}
	
	public override TimedMethod[] Use () {
		Attacks.SetAudio("Blunt Hit", 15);
		if (Attacks.EvasionCheck(Party.GetEnemy(), Party.GetPlayer().GetAccuracy()) && Party.enemyCount > 1) {
			//Character second = Party.enemies[0];
			TimedMethod[] moves = new TimedMethod[Party.enemyCount + 1];
			moves[0] = new TimedMethod(0, "Audio", new object[] {"Running"});
			int index = 1;
			for (int i = 0; i < 4; i++) {
				if (i != Party.enemySlot - 1 && Party.enemies[i] != null && Party.enemies[i].GetAlive()) {
					moves[index] = new TimedMethod(0, "AttackAny", new object[] {Party.GetPlayer(), Party.enemies[i],
			    	Party.GetPlayer().GetStrength()/2, (Party.GetPlayer().GetStrength() + 5)/2, Party.GetPlayer().GetAccuracy(), true, false, false});
					//break;
				index++;
				}
			}
			moves[index] = new TimedMethod(0, "StagnantAttack", new object[] {
		        true, Party.GetPlayer().GetStrength(), Party.GetPlayer().GetStrength() + 5, Party.GetPlayer().GetAccuracy(), true, true, false});
		    return moves;
		} else {
			return new TimedMethod[] {new TimedMethod(0, "Audio", new object[] {"Running"}), new TimedMethod(0, "StagnantAttack", new object[] {
		        true, Party.GetPlayer().GetStrength(), Party.GetPlayer().GetStrength() + 5, Party.GetPlayer().GetAccuracy(), true, true, false})};
		}
	}
}