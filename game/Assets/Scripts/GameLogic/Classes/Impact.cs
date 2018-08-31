public class Impact {

    public string playerId;
    public int life;
    public bool death;

    public Impact(JSONObject data)
    {
        playerId = data["playerId"] != null ? data["playerId"].str : null;
        life = data["life"] != null ? (int) data["life"].n : 0;
        death = data["death"] != null ? data["death"].b : false;

    }
}
