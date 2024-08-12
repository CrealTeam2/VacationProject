using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISavable {
    void LoadData(Database data);
    void SaveData(ref Database data);
}
