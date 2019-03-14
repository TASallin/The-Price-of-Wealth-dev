using System.Collections.Generic;

public class SketchyMachineO : Event {
	
	public SketchyMachineO () {}
	
	public override void Enact () {
		text = "You enter a room with a huge device that apparently enhances power. There is enough electricity for 1 person";
		TimedMethod[] contents = new TimedMethod[3];
		TimedMethod strength = new TimedMethod(0, "StatChange", new object[] {"GainStrength", 2});
		TimedMethod accuracy = new TimedMethod(0, "StatChange", new object[] {"GainAccuracy", 3});
		TimedMethod health = new TimedMethod(0, "StatChange", new object[] {"GainMaxHP", -3});
		TimedMethod dexterity = new TimedMethod(0, "StatChange", new object[] {"GainDexterity", -1});
		options1 = new LinkedList<TimedMethod>();
		string result = "The machine increased ";
		if (new System.Random().Next(2) == 0) {
			contents[0] = strength;
			result = result + "strength by 2 but reduced ";
		} else {
			contents[0] = accuracy;
			result = result + "accuracy by 3 but reduced ";
		}
		if (new System.Random().Next(2) == 0) {
			contents[1] = health;
			result = result + "max HP by 3";
		} else {
			contents[1] = dexterity;
			result = result + "dexterity by 1";
		}
		contents[2] = new TimedMethod(0, "CauseEvent", new object[] {new TextEvent(result)});
		options1.AddLast(new TimedMethod(0, "CauseEvent", new object[] {new SelectMember("Who will be altered?", contents)}));
		optionText1 = "Send someone to the machine";
		options2 = new LinkedList<TimedMethod>();
		options2.AddLast(new TimedMethod("Resolve"));
		optionText2 = "No unethical science";
	}
}