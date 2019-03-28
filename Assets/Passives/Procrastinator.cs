using UnityEngine;
public class Procrastinator : Quirk {
	
	int turns;
	
    public Procrastinator (Character c) {
	    self = c; name = "Procrastinator"; description = "Lose 5 power and accuracy at the start of combat, and gain 1 of each per turn, up to 15 times";
    }

    public override TimedMethod[] Check (bool player) {
		if (turns <= 15) {
		    turns++;
			self.GainPower(1); self.GainAccuracy(1);
		}
		return new TimedMethod[] {new TimedMethod(0, "CharLogSprite", new object[] {"1", self.partyIndex, "power", player}),
		    new TimedMethod(0, "CharLogSprite", new object[] {"1", self.partyIndex, "accuracy", player})};
	}
	
	public override TimedMethod[] CheckLead (bool player) {
		return Check(player);
	}
	
	public override TimedMethod[] Initialize (bool player) {
		self.GainPower(-6); self.GainAccuracy(-6);
		turns = 0;
		return new TimedMethod[] {new TimedMethod(0, "CharLogSprite", new object[] {"-6", self.partyIndex, "power", player}),
		    new TimedMethod(0, "CharLogSprite", new object[] {"-6", self.partyIndex, "accuracy", player})};
	}

}