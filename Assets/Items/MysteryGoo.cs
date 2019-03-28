public class MysteryGoo : Item {
	
	public MysteryGoo () {name = "Mystery Goo"; description = "Throw it at the enemy!"; price = 1;}
	
	public override TimedMethod[] Use () {
		System.Random rng = new System.Random();
		int seed = rng.Next(11);
		int magnitude = rng.Next(10) + 1;
		Character targ = Party.GetEnemy();
		if (!Attacks.EvasionCycle(Party.GetPlayer(), targ)) {
			return new TimedMethod[] {new TimedMethod(0, "Audio", new object[] {"Slime"}),
			    new TimedMethod(60, "Log", new object[] {"The goo fell sadly to the ground, splattering uselessly"})};
		}
		
		if (seed == 0) {
			targ.GainPower(-1* magnitude / 2 - 1);
			return new TimedMethod[] {new TimedMethod(0, "Audio", new object[] {"Slime"}),
			    new TimedMethod(60, "Log", new object[] {"The goo disrupted power by " + (magnitude/2 + 1).ToString() + " points"}),
				new TimedMethod(0, "CharLogSprite", new object[] {(magnitude/2 + 1).ToString(), Party.enemySlot - 1, "power", false})};
		} else if (seed == 1) {
			targ.GainDefense(-1 * magnitude / 2 - 1);
			return new TimedMethod[] {new TimedMethod(0, "Audio", new object[] {"Slime"}),
			    new TimedMethod(60, "Log", new object[] {"The goo drained defense by " + (magnitude/2 + 1).ToString()}),
				new TimedMethod(0, "CharLogSprite", new object[] {(magnitude/2 + 1).ToString(), Party.enemySlot - 1, "defense", false})};
		} else if (seed == 2) {
			targ.GainAccuracy(-1 * magnitude / 2 - 1);
			return new TimedMethod[] {new TimedMethod(0, "Audio", new object[] {"Slime"}),
			    new TimedMethod(60, "Log", new object[] {"The goo reduced accuracy by " + (magnitude/2 + 1).ToString()}),
				new TimedMethod(0, "CharLogSprite", new object[] {(magnitude/2 + 1).ToString(), Party.enemySlot - 1, "accuracy", false})};
		} else if (seed == 3) {
			targ.GainCharge(-1 * magnitude);
			return new TimedMethod[] {new TimedMethod(0, "Audio", new object[] {"Slime"}),
			    new TimedMethod(60, "Log", new object[] {"The goo stuck to the target's hands. Charge -" + magnitude.ToString()}),
				new TimedMethod(0, "CharLogSprite", new object[] {magnitude.ToString(), Party.enemySlot - 1, "charge", false})};
		} else if (seed == 4) {
			targ.GainGuard(-1 * magnitude);
			return new TimedMethod[] {new TimedMethod(0, "Audio", new object[] {"Slime"}),
			    new TimedMethod(60, "Log", new object[] {"The goo broke the target's guard by " + magnitude.ToString()}),
				new TimedMethod(0, "CharLogSprite", new object[] {magnitude.ToString(), Party.enemySlot - 1, "guard", false})};
		} else if (seed == 5) {
			TimedMethod[] goopPart = targ.status.Goop();
			return new TimedMethod[] {new TimedMethod(0, "Audio", new object[] {"Slime"}),
			    new TimedMethod(60, "Log", new object[] {"The goo revealed to be slime goop, trapping the target"}), goopPart[0], goopPart[1]};
		} else if (seed == 6) {
			TimedMethod[] poisonPart = targ.status.Poison(magnitude / 3 + 1);
			return new TimedMethod[] {new TimedMethod(0, "Audio", new object[] {"Slime"}), new TimedMethod(60, "Log", new object[] {
				"The goo was a deadly poison with a degree of " + (magnitude/3 + 1).ToString()}), poisonPart[0], poisonPart[1]};
		} else if (seed == 7) {
			TimedMethod[] sleepPart = targ.status.Sleep();
			return new TimedMethod[] {new TimedMethod(0, "Audio", new object[] {"Slime"}),
			    new TimedMethod(60, "Log", new object[] {"The goo caused the target to fall asleep"}), sleepPart[0], sleepPart[1]};
		} else if (seed == 8) {
			TimedMethod[] blindPart = targ.status.Blind(magnitude);
			return new TimedMethod[] {new TimedMethod(0, "Audio", new object[] {"Slime"}), new TimedMethod(
			    60, "Log", new object[] {"The goo stuck to the target's eyes with a power of " + magnitude.ToString()}), blindPart[0], blindPart[1]};
		} else if (seed == 9) {
			Status.NullifyAttack(targ); Status.NullifyDefense(targ);
			return new TimedMethod[] {new TimedMethod(0, "Audio", new object[] {"Slime"}),
			    new TimedMethod(60, "Log", new object[] {"The goo countered buffs to attack and defense"}),
				new TimedMethod(0, "CharLogSprite", new object[] {"atk reset", Party.enemySlot - 1, "nullAttack", false}),
				new TimedMethod(0, "CharLogSprite", new object[] {"def reset", Party.enemySlot - 1, "nullDefense", false})};
		} else {
			targ.GainEvasion(-1 * targ.GetDexterity());
			Attacks.SetAudio("Slime", 0);
			return new TimedMethod[] {new TimedMethod(60, "Log", new object[] {"The goo solidified midair and turned into a rock!"}),
			new TimedMethod(60, "StagnantAttack", new object[] {true, magnitude, magnitude, Party.GetPlayer().GetAccuracy(), true, true, false})};
		}
	}
	
}