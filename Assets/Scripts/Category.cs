public class Category
{
    static private string[] possibleValues = new string[6]{"Archer", "Assassin", "Necromancian", "Nomad", "Warrior", "Wizard"};
    /* values btween 1 and 100 */ 
    // Separate Wisdom from Intelligence
    private int wisdom = 10; // More exp and drop rate ++
    private int energy = 10; // Attack ++
    private int constitution = 10; // health and endurance
    private int dexterity = 10; // Speed and attack speed

    public int GetWisdom(){ return wisdom; }
    public int GetEnergy(){ return energy; }
    public int GetConstitution(){ return constitution; }
    public int GetDexterity(){ return dexterity; }

    public void SetWisdom(int value){ wisdom = value; }
    public void SetEnergy(int value){ energy = value; }
    public void SetConsitution(int value){ constitution = value; }
    public void SetDexterity(int value){ dexterity = value; }
    public string[] GetPossibleValues(){ return possibleValues; }
}
