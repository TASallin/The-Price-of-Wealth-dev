using System.Collections.Generic;

public class WinGameEvent : Event {
	
	public WinGameEvent() {
		
	}
	
	public override void Enact() {
		text = "You have defeated the creator of the hypnotizing drug, and now the machine creating it is destroyed";
		options1 = new LinkedList<TimedMethod>();
		options1.AddLast(new TimedMethod("Win"));
		optionText1 = "Win";
	}
	
}