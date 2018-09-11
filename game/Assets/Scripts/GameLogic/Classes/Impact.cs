public class Impact {

    public string playerId;
    public int life;
    public bool death;

    public Impact(ImpactResponse impactResponse)
    {
        playerId = impactResponse.playerId;
        life = impactResponse.life;
        death = impactResponse.death;
    }
}
