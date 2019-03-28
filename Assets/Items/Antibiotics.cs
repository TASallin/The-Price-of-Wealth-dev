public class Antibiotics : Item {
	
	public Antibiotics () {name = "Antibiotics"; description = "Cures poison and makes you immune for the rest of the fight";
	    price = 3; usableOut = true; selects = true;}
	
	public override TimedMethod[] UseSelected (int i) {
		Party.members[i].status.poisoned = 0;
		Party.members[i].status.poisonImmune = true;
		return new TimedMethod[] {new TimedMethod(0, "Audio", new object[] {"Powder"}),
		    new TimedMethod(0, "CharLogSprite", new object[] {"IMMUNE", i, "poison", true})};
	}
	
	public override void UseOutOfCombat(int i) {
		Party.members[i].status.poisonImmune = true;
		Party.members[i].status.poisoned = 0;
	}
}