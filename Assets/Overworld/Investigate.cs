using System.Collections.Generic;

public static class Investigate {
	
	public static List<Event> tower;
	public static List<Event> dining;
	public static List<Event> research;
	public static List<Event> sports;
	public static List<Event> art;
	public static List<Event> health;
	public static List<Event> lecture;
	public static Dictionary<string, List<Event>> all;
	public static Dictionary<string, int>  indexes;
	public static System.Random rng = new System.Random();
	
	
	public static void Initialize() {
		tower = new List<Event>(new Event[] {new TowerEasyBattleP(), new TowerEasyBattleR(), new TowerMediumBattle(), new TowerHardBattle(),
		    new TowerEasyBattleP(), new TowerEasyBattleR(), new TowerMediumBattle(), new TowerHardBattle(), new ItemEvent(new Item[] {new PinkSlip(),
			new Briefcase()}, "The investigation reveals loot"), new ItemEvent(new Item[] {new Smartphone(), new PaperPlane()},
 			"The investigation reveals loot"), new RecruitEvent(new PoliticalScientist(), "You meet someone friendly"), new RecruitEvent(
		    new PsychMajor(), "You meet someone friendly"), new AngryAdmin(), new BuzzBot(), new Demonstration()});
		dining = new List<Event>(new Event[] {new DiningEasyBattleP(), new DiningEasyBattleR(), new DiningMediumBattle(), new DiningHardBattle(),
		    new DiningEasyBattleP(), new DiningEasyBattleR(), new DiningMediumBattle(), new DiningHardBattle(), new ItemEvent(new Item[] {new Donut(),
			new Celery()}, "The investigation reveals loot"), new ItemEvent(new Item[] {new Curry(), new Rice()},
 			"The investigation reveals loot"), new RecruitEvent(new CulinaryMajor(), "You meet someone friendly"), new RecruitEvent(
		    new HistoryMajor(), "You meet someone friendly"), new CultHideout(), new PoisonCoffee(), new ExplodingKitchen()});
		research = new List<Event>(new Event[] {new ResearchEasyBattleP(), new ResearchEasyBattleR(), new ResearchMediumBattle(),
    		new ResearchHardBattle(), new ResearchEasyBattleP(), new ResearchEasyBattleR(), new ResearchMediumBattle(), new ResearchHardBattle(),
			new ItemEvent(new Item[] {new MysteryGoo(), new MysterySolution()}, "The investigation reveals loot"),
			new ItemEvent(new Item[] {new ToxicSolution(), new ExplosiveBrew()}, "The investigation reveals loot"), new RecruitEvent(
			new AerospaceEngineer(), "You meet someone friendly"), new RecruitEvent(new MechanicalEngineer(), "You meet someone friendly"),
			new Mutagens(), new SketchyMachineO(), new SketchyMachineD()});
		sports = new List<Event>(new Event[] {new SportsEasyBattleP(), new SportsEasyBattleR(), new SportsMediumBattle(), new SportsHardBattle(),
		    new SportsEasyBattleP(), new SportsEasyBattleR(), new SportsMediumBattle(), new SportsHardBattle(), new ItemEvent(
			new Item[] {new Football(), new Pizza()}, "The investigation reveals loot"), new ItemEvent(new Item[] {new ProteinBar(), new Tazer()},
 			"The investigation reveals loot"), new RecruitEvent(new FootballPlayer(), "You meet someone friendly"), new RecruitEvent(
		    new CJMajor(), "You meet someone friendly"), new EvilVisitors(), new FoodStand(), new StandCult()});
		art = new List<Event>(new Event[] {new ArtEasyBattleP(), new ArtEasyBattleR(), new ArtMediumBattle(), new ArtHardBattle(),
		    new ArtEasyBattleP(), new ArtEasyBattleR(), new ArtMediumBattle(), new ArtHardBattle(), new ItemEvent(new Item[] {new Tuba(),
			new Baton()}, "The investigation reveals loot"), new ItemEvent(new Item[] {new Heels(), new Metronome()},
 			"The investigation reveals loot"), new RecruitEvent(new MusicMajor(), "You meet someone friendly"), new RecruitEvent(
		    new DanceMajor(), "You meet someone friendly"), new LaserGallery(), new PossessedOrchestra(), new ArtAppreciator()});
		health = new List<Event>(new Event[] {new HealthEasyBattleP(), new HealthEasyBattleR(), new HealthMediumBattle(), new HealthHardBattle(),
		    new HealthEasyBattleP(), new HealthEasyBattleR(), new HealthMediumBattle(), new HealthHardBattle(), new ItemEvent(
			new Item[] {new Sanitizer(), new Defibrilator()}, "The investigation reveals loot"), new ItemEvent(
			new Item[] {new Antibiotics(), new SlimeGoo()}, "The investigation reveals loot"), new RecruitEvent(
			new PreMed(), "You meet someone friendly"), new RecruitEvent(new MathMajor(), "You meet someone friendly"),
			new MedicalSlime(), new ReviveChoice(), new TheDentist()});
		lecture = new List<Event>(new Event[] {new LectureEasyBattleP(), new LectureEasyBattleR(), new LectureMediumBattle(), new LectureHardBattle(),
		    new LectureEasyBattleP(), new LectureEasyBattleR(), new LectureMediumBattle(), new LectureHardBattle(), new ItemEvent(
			new Item[] {new Textbook(), new Pencil()}, "The investigation reveals loot"), new ItemEvent(new Item[] {new USB(), new Wire()},
 			"The investigation reveals loot"), new RecruitEvent(new EnglishMajor(), "You meet someone friendly"), new RecruitEvent(
		    new ChemistryMajor(), "You meet someone friendly"), new FacultyCoven(), new HallwaySkater(), new OfficeHours()});
		Shuffle(tower); Shuffle(dining); Shuffle(research); Shuffle(sports); Shuffle(art); Shuffle(health); Shuffle(lecture);
		all = new Dictionary<string, List<Event>>(); all.Add("tower", tower); all.Add("dining", dining); all.Add("research", research);
		    all.Add("sports", sports); all.Add("art", art); all.Add("health", health); all.Add("lecture", lecture);
		indexes = new Dictionary<string, int>(); indexes.Add("tower", 0); indexes.Add("dining", 0); indexes.Add("research", 0); 
		indexes.Add("sports", 0); indexes.Add("art", 0); indexes.Add("health", 0); indexes.Add("lecture", 0);
	}
	
	public static Event Get(string location) {
		int index = indexes[location];
		List<Event> events = all[location];
		Event current;
		if (index < events.Count) {
			current = events[index];
	        indexes[location]++;
			Time.Increment();
		} else {
			current = new TextEvent("This area has been cleared out");
		}
	    return current;
	}
	
	public static void Shuffle(List<Event> list) {
		int n = list.Count;  
        while (n > 1) {  
            n--;  
            int k = rng.Next(n + 1);  
            Event val = list[k];  
            list[k] = list[n];  
            list[n] = val;  
    	}
	}
}