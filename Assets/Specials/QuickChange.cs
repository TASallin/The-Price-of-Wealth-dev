public class QuickChange : Special {
	
	public QuickChange() {name = "QuickChange"; description = "Switch out without consuming your turn"; baseCost = 5; modifier = 0; selects = true;}
	
	public override TimedMethod[] UseSelects(int i) {
		if (i == Party.playerSlot - 1) {
			Party.UseSP(GetCost() * -1);
		}
		return new TimedMethod[] {new TimedMethod(0, "SwitchTo", new object[] {i + 1}), new TimedMethod("RefreshStatusP"),
    		new TimedMethod("ContinueTurn")};
	}
}