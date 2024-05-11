using System;

[Serializable]
public class UserGameData {

    public int level;
    public float exp;
    public int gold;

    public void Reset() {
        level = 1;
        exp = 0;
        gold = 0;
    }
    
}
