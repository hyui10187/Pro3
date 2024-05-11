using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackendGameData {

    private static BackendGameData instance = null;

    public static BackendGameData Instance {
        get {
            if(instance == null) {
                instance = new BackendGameData();
            }

            return instance;
        }
    }

    private UserGameData userGameData = new UserGameData();
    public UserGameData UserGameData => userGameData;

    private string gameDataRowInDate = string.Empty;


}
