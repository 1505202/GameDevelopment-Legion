/// <summary>
/// Legion Actor
/// </summary>
public class Legion : AActor
{
    private AController inputController;
    public AController Controller
    {
        get
        {
            return inputController;
        }
        set
        {
            inputController = value;
        }
    }


    /// <summary>
    /// Updates Assimilated Rogue Camera, Skill.
    /// </summary>
    /// <param name="rogue"> Rogue Element Of Which To Edit </param>
    public void UpdateAssimilatedRogueSkill(Rogue rogue)
    {

    }
}
