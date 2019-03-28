public class Cooking : Passive {
	
	int turn;
	
	public Cooking(Character c) {self = c; name = "Cooking"; description = "party restores 2 hp on the 3rd turn of a battle";}
	
	public override TimedMethod[] Initialize (bool player) {
		turn = 0;
		return new TimedMethod[0];
	}
	
	public override TimedMethod[] Check (bool player) {
		turn++;
	    if (turn == 3) {
    		Character[] current;
		    if (player) {
	     		current = Party.members;
    		} else {
			    current = Party.enemies;
		    }
	    	foreach (Character c in current) {
    			if (c != null) {
				    c.Heal(2);
			    }
		    }
	    	return new TimedMethod[] {new TimedMethod(0, "Audio", new object[] {"Heal"}),
			    new TimedMethod(60, "Log", new object[] {self.ToString() + "'s cooking restored health"}),
				new TimedMethod(0, "CharLogSprite", new object[] {"2", 0, "healing", player}),
				new TimedMethod(0, "CharLogSprite", new object[] {"2", 1, "healing", player}),
				new TimedMethod(0, "CharLogSprite", new object[] {"2", 2, "healing", player}),
				new TimedMethod(0, "CharLogSprite", new object[] {"2", 3, "healing", player})};
		}
		return new TimedMethod[0];
	}
	
	public override TimedMethod[] CheckLead(bool player) {
		return Check(player);
	}
}