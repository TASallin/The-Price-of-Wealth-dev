public static class Names {
    
    static System.Random rnd = new System.Random();
	
	public static string Get () {
        string[] names = new string[] {"Jacob", "Emily", "Michael", "Hannah", "Matthew", "Alexis", "Joshua", "Sarah", "Nicholas", "Samantha", "Christopher", "Ashley", "Andrew", "Madison", "Joseph", "Taylor", "Daniel", "Jessica", "Tyler", "Elizabeth", "Brandon", "Alyssa", "Ryan", "Lauren", "Austin", "Kayla", "William", "Brianna", "John", "Megan", "David", "Victoria", "Zachary", "Emma", "Anthony", "Abigail", "James", "Rachel", "Justin", "Olivia", "Alexander", "Jennifer", "Jonathan", "Amanda", "Dylan", "Sydney", "Noah", "Morgan", "Christian", "Nicole", "Robert", "Jasmine", "Samuel", "Grace", "Kyle", "Destiny", "Benjamin", "Anna", "Jordan", "Julia", "Thomas", "Haley", "Nathan", "Alexandra", "Cameron", "Kaitlyn", "Kevin", "Natalie", "Jose", "Brittany", "Hunter", "Katherine", "Ethan", "Stephanie", "Aaron", "Rebecca", "Eric", "Maria", "Jason", "Allison", "Caleb", "Amber", "Logan", "Savannah", "Brian", "Danielle", "Adam", "Courtney", "Cody", "Mary", "Juan", "Gabrielle", "Steven", "Brooke", "Connor", "Jordan", "Timothy", "Sierra", "Charles", "Sara", "Tristan", "Jeffery"};
        int randName = rnd.Next(names.Length);
        return names[randName];
	}
}