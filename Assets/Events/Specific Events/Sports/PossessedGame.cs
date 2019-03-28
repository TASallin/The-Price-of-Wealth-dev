using System.Collections.Generic;

public class PossessedGame : Event {
	
	public PossessedGame () {}
	
	public override void Enact () {
		text = "As you take the field, the gates at both end open. A hoard of possessed football players charge directly at each other with you" 
		    + " in the middle";
		Character[] enemies = new Character[] {new FootballPlayer(), new FootballPlayer(), new FootballPlayer(), new FootballPlayer()};
		enemies[0].SetRecruitable(false); enemies[1].SetRecruitable(false); enemies[2].SetRecruitable(false); enemies[3].SetRecruitable(false);
		options1 = new LinkedList<TimedMethod>();
		options1.AddLast(new TimedMethod(0, "Battle", new object[] {enemies}));
		optionText1 = "Play along (fight)";
		options2 = new LinkedList<TimedMethod>();
	    options2.AddLast(new TimedMethod(0, "DamageAll", new object[] {5}));
		options2.AddLast(new TimedMethod("Resolve"));
		optionText2 = "Cower to the sidelines (5 damage to party)";
		if (Party.PartyContains(new FootballPlayer()) >= 0) {
			options3 = new LinkedList<TimedMethod>();
			options3.AddLast(new TimedMethod(0, "ChooseMember", new object[] {Party.PartyContains(new FootballPlayer())}));
			if (Party.members[Party.PartyContains(new FootballPlayer())].GetStrength() > 5) {
				options3.AddLast(new TimedMethod(0, "StatChange", new object[] {"GainStrength", 1}));
				options3.AddLast(new TimedMethod(0, "CauseEvent", new object[] {new TextEvent(Party.members[Party.PartyContains(
				    new FootballPlayer())].ToString() + " breaks the line, clearing a path for the rest of the party. They even gained 1 strength!")}));
			} else {
			    Event fail = new Event();
		    	fail.text = "With only an average amount of strength, " + Party.members[Party.PartyContains(new FootballPlayer())].ToString()
			        + " couldn't hold up against the charge (-10 HP). The party will have to fight";
		    	fail.options1 = new LinkedList<TimedMethod>();
	    		fail.options1.AddLast(new TimedMethod(0, "Damage", new object[] {10}));
    			fail.options1.AddLast(new TimedMethod(0, "Battle", new object[] {enemies}));
 			    fail.optionText1 = "Oh no";
			    options3.AddLast(new TimedMethod(0, "CauseEvent", new object[] {fail}));
			}
			optionText3 = "Football Player: Test your strength";
		}
		if (Party.PartyContains(new AerospaceEngineer()) >= 0) {
			options4 = new LinkedList<TimedMethod>();
			options4.AddLast(new TimedMethod(0, "ChooseMember", new object[] {Party.PartyContains(new AerospaceEngineer())}));
			options4.AddLast(new TimedMethod(0, "StatChange", new object[] {"GainPower", -5}));
			options4.AddLast(new TimedMethod(0, "CauseEvent", new object[] {new TextEvent(Party.members[
			    Party.PartyContains(new AerospaceEngineer())].ToString() + " Keeps the party above the chaos. The drone was heavily strained"
				+ " in the process, limiting the engineer's power next fight")}));
			optionText4 = "Aerospace Engineer: Use the drone to hold the party in the air";
		}
	}
}