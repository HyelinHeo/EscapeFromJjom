using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MPXFile;

public class TileExporter : Singleton<TileExporter>
{
    public CreatorTile Creator;

    private string folder;

    public string Error;

    void Start()
    {
        folder = PathManager.DUNGEON_DIR + "/";
    }

    public void Load(string fileName)
    {
        string fullPath = folder + fileName;

        DungeonFile file = MPXFileManager.Load<DungeonFile>(fullPath);
        if (file != null)
        {
            Debug.Log("Dungeon file load ok. " + fullPath);
        }
        else
        {
            Debug.LogError("Dungeon file load filed. " + fullPath);
        }
    }



    public void Save(string dungeonName = "none")
    {
        TileCreator tileC = Creator.Creator;
        if (tileC != null)
        {
            DungeonFile saveFile = new DungeonFile(tileC);
            string fileName = saveFile.ID;
            string fullPath = folder + fileName;

            saveFile.Name = dungeonName;

            if (MPXFileManager.Save(saveFile, fullPath, ref Error))
            {
                Debug.LogFormat("Save File OK. {0}", fullPath);
            }
            else
            {
                Debug.LogErrorFormat("Save File Failed. {0}", fullPath);
            }
        }
    }
}
