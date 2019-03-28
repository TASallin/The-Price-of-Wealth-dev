public class Coffee : Item {
	
	public Coffee () {name = "Coffee"; description = "An amazing drink. Yields immediate results"; selects = true; price = 5; usableOut = true;}
	
	public override TimedMethod[] UseSelected (int i) {
		Party.members[i].status.Coffee();
		return new TimedMethod[] {new TimedMethod(0, "Audio", new object[] {"Drink"}), new TimedMethod(0, "AudioAfter", new object[] {"Fire", 15}),
		    new TimedMethod(60, "Log", new object[] {Party.members[i].ToString() + " gained the power of caffeine"}),
			new TimedMethod(0, "CharLogSprite", new object[] {"COFFEE", i, "power", true})};
	}
	
	public override void UseOutOfCombat (int i) {
		UseSelected(i);
	}
}