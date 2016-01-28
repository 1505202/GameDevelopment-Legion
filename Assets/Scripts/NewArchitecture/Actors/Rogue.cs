/// <summary>
/// 
/// </summary>
public class Rogue : AActor
{
    private int rogueSkillsUnlocked = 0;
    private ASkill[] rogueSkills = new ASkill[3];
    private ASkill legionSkill = null;

    private AController inputContoller;

    /// <summary>
    /// 
    /// </summary>
    public void UpdateRogueSkillCount()
    {
        rogueSkillsUnlocked++;
    }
}
