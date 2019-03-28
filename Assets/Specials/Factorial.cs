public class Factorial : Special {
	
	int factorial;
	int answer;
	int lastUse;
	
	public Factorial() {name = "Factorial"; description = "Deals more damage when landed consecutively"; baseCost = 1; modifier = 0;
	    factorial = 1; answer = 1; lastUse = 0;}

    public override TimedMethod[] Use() {
		if (lastUse == Party.turn - 1) {
		    answer = answer * factorial;
			if (factorial < 8) {
		        factorial++;
		    }
		    lastUse++;
		} else {
			answer = 1; factorial = 2; lastUse = Party.turn; 
		}
		if (!Attacks.EvasionCheck(Party.GetEnemy(), Party.GetPlayer().GetAccuracy())) {
			answer = 1; factorial = 1;
		}
		Attacks.SetAudio("Fire Hit", 6);
		return new TimedMethod[] {
    		new TimedMethod(0, "Audio", new object[] {"Laser Shot"}),
		    new TimedMethod(0, "StagnantAttack", new object[] {true, answer, answer, Party.GetPlayer().GetAccuracy(), true, true, false})};
	}
	
}