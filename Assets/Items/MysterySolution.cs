public class MysterySolution : Item {
	
	public MysterySolution () {name = "Mystery Solution"; description = "Drink it!"; selects = true; price = 1;}
	
	public override TimedMethod[] UseSelected (int i) {
		System.Random rng = new System.Random();
		int seed = rng.Next(9);
		int magnitude = rng.Next(10) + 1;
		Character self = Party.members[i];
		if (seed == 0) {
			self.GainPower(magnitude / 2 + 1);
			return new TimedMethod[] {new TimedMethod(0, "Audio", new object[] {"Drink"}),
			    new TimedMethod(60, "Log", new object[] {"It made you stronger by " + (magnitude/2 + 1).ToString() + " points"}),
				new TimedMethod(0, "CharLogSprite", new object[] {(magnitude/2 + 1).ToString(), Party.playerSlot - 1, "power", true})};
		} else if (seed == 1) {
			self.GainDefense(magnitude / 2 + 1);
			return new TimedMethod[] {new TimedMethod(0, "Audio", new object[] {"Drink"}),
			    new TimedMethod(60, "Log", new object[] {"High iron raised defense by " + (magnitude/2 + 1).ToString()}),
				new TimedMethod(0, "CharLogSprite", new object[] {(magnitude/2 + 1).ToString(), Party.playerSlot - 1, "defense", true})};
		} else if (seed == 2) {
			self.GainAccuracy(magnitude / 2 + 1);
			return new TimedMethod[] {new TimedMethod(0, "Audio", new object[] {"Drink"}),
			    new TimedMethod(60, "Log", new object[] {"It helps with focus. Accuracy +  " + (magnitude/2 + 1).ToString()}),
				new TimedMethod(0, "CharLogSprite", new object[] {(magnitude/2 + 1).ToString(), Party.playerSlot - 1, "accuracy", true})};
		} else if (seed == 3) {
			self.GainCharge(magnitude);
			return new TimedMethod[] {new TimedMethod(0, "Audio", new object[] {"Drink"}),
			    new TimedMethod(60, "Log", new object[] {"It was spicy. Charge + " + magnitude.ToString()}),
				new TimedMethod(0, "CharLogSprite", new object[] {magnitude.ToString(), Party.playerSlot - 1, "charge", true})};
		} else if (seed == 4) {
			self.GainGuard(magnitude);
			return new TimedMethod[] {new TimedMethod(0, "Audio", new object[] {"Drink"}),
			    new TimedMethod(60, "Log", new object[] {"It gave guard somehow, increasing by " + magnitude.ToString()}),
				new TimedMethod(0, "CharLogSprite", new object[] {magnitude.ToString(), Party.playerSlot - 1, "guard", true})};
		} else if (seed == 5) {
			self.GainEvasion(magnitude);
			return new TimedMethod[] {new TimedMethod(0, "Audio", new object[] {"Drink"}),
			    new TimedMethod(60, "Log", new object[] {"Caffeine increased evasion by " + magnitude.ToString()}),
				new TimedMethod(0, "CharLogSprite", new object[] {magnitude.ToString(), Party.playerSlot - 1, "evasion", true})};
		} else if (seed == 6) {
			self.status.blinded = 0; self.status.poisoned = 0; self.status.asleep = 0; self.status.stunned = 0; self.status.gooped = false;
			return new TimedMethod[] {new TimedMethod(0, "Audio", new object[] {"Drink"}),
			    new TimedMethod(60, "Log", new object[] {"It was a panacea. Negative effects removed"}),
				new TimedMethod(0, "CharLogSprite", new object[] {"Panacea", Party.playerSlot - 1, "regeneration", true})};
		} else if (seed == 7) {
			Status.NullifyAttack(self); Status.NullifyDefense(self);
			return new TimedMethod[] {new TimedMethod(0, "Audio", new object[] {"Drink"}),
			    new TimedMethod(60, "Log", new object[] {"Attack and defense were reset"}),
				new TimedMethod(0, "CharLogSprite", new object[] {"atk reset", Party.playerSlot - 1, "nullAttack", true}),
				new TimedMethod(0, "CharLogSprite", new object[] {"def reset", Party.playerSlot - 1, "nullDefense", true})};
		} else {
			self.Heal(magnitude);
			return new TimedMethod[] {new TimedMethod(0, "Audio", new object[] {"Drink"}),
			    new TimedMethod(60, "Log", new object[] {"It's a healing elixer. HP + " + magnitude.ToString()}),
				new TimedMethod(0, "CharLogSprite", new object[] {magnitude.ToString(), Party.playerSlot - 1, "healing", true})};
		}
	}
	
}