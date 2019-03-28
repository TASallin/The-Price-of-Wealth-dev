public class Celery : Item {
	
	public Celery() {name = "Celery"; description = "Restores 5 hp. You gain 1 defense"; selects = true; price = 2; usableOut = true;}
	
	public override TimedMethod[] UseSelected(int i) {
		Party.members[i].Heal(5);
		Party.members[i].SetDefense(Party.members[i].GetDefense() + 1);
		return new TimedMethod[] {new TimedMethod(0, "Audio", new object[] {"Eat"}),
		    new TimedMethod(60, "Log", new object[] {Party.members[i].GetName() + " ate the celery. It was fibrous."}),
			new TimedMethod(0, "CharLogSprite", new object[] {"5", i, "healing", true}),
			new TimedMethod(0, "CharLogSprite", new object[] {"1", i, "defense", true})};
	}

    public override void UseOutOfCombat(int i) {
		UseSelected(i);
	}
}