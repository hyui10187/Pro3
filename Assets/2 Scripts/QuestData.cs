using System.Collections;
using System.Collections.Generic;

public class QuestData { // 구조체

    public string questName;
    public int[] npcId;

    public QuestData(string name, int[] npc) {
        questName = name;
        npcId = npc;
    }
    
}