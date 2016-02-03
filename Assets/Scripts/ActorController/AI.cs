/// <summary>
/// AI Controller, Decides What To Do And When
/// </summary>
public enum EAIDifficulty { Easy, Normal, Difficult }
public class AI : AController
{
    private EAIDifficulty aiDifficulty;
    public EAIDifficulty AIDifficulty
    {
        get { return aiDifficulty;  }
        set { aiDifficulty = value; }
    }

    private void Start()
    {

    }

    public override void UpdateController()
    {

    }
}
