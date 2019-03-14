using System.Collections.Generic;

public class EvilVisitors : Event {
	
	public EvilVisitors () {}
	
	public override void Enact () {
		text = "A criminal looking lot is blocking the way forward. \"AHA! MORE WORTHLESS CRETINS IN THIS INFERIOR INSTITUTION! YOUR"
            + " PRESENCE IS WHY OUR VERY GOOD FOOTBALL TEAM LOST THIS YEAR!\""
		    + " You now notice their visible support for Unathletic University, home to the worst football team ever";
		Character[] enemies = new Character[] {new BusinessMajor(), new FootballPlayer(), new MathMajor(), new MusicMajor()};
		enemies[0].SetRecruitable(false); enemies[1].SetRecruitable(false); enemies[2].SetRecruitable(false); enemies[3].SetRecruitable(false);
		options1 = new LinkedList<TimedMethod>();
		options1.AddLast(new TimedMethod(0, "Battle", new object[] {enemies}));
		optionText1 = "Fight?";
		options2 = new LinkedList<TimedMethod>();
		if (new System.Random().Next(3) == 0) {
		options2.AddLast(new TimedMethod(0, "CauseEvent", new object[] {new TextEvent("You convince them that they took a wrong turn on the way here,"
    		+ " and that this is the basement to Our Bucks.")}));
		} else {
			options2.AddLast(new TimedMethod(0, "CauseEvent", new object[] {new BattleEvent(
			    enemies, "You try to be reasonable but they shout about not falling for their insidious speech. Whatever")}));
		}
		optionText2 = "Try to talk them down";
		if (Party.PartyContains(new FootballPlayer()) >= 0) {
			options3 = new LinkedList<TimedMethod>();
			options3.AddLast(new TimedMethod(0, "CauseEvent", new object[] {new TextEvent(Party.members[
			    Party.PartyContains(new FootballPlayer())].ToString() + "'s presence alone sends them running in terror")}));
			optionText3 = "Football Player: Intimidate them";
		}
		if (Party.ContainsQuirk(new Vengeful(null)) >= 0) {
			options4 = new LinkedList<TimedMethod>();
			options4.AddLast(new TimedMethod(0, "ChooseMember", new object[] {Party.ContainsQuirk(new Vengeful(null))}));
			options4.AddLast(new TimedMethod(0, "StatChange", new object[] {"GainPower", 10}));
			options4.AddLast(new TimedMethod(0, "CauseEvent", new object[] {new BattleEvent(enemies, Party.members[
			    Party.ContainsQuirk(new Vengeful(null))].ToString() + " has had enough of their crimes and gains a surge of power")}));
			optionText4 = "Vengeful: Become engraged at their misdeeds";
		}
	}
}