using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class FileDataHandler
{
    private string _dataDirectoryPath = "";
    private string _dataFileName = "";

    private bool _useEncryption = false;
    private readonly string _encryptionCodeword = "Omega";

    private readonly string _backupExtension = ".bak";

    public FileDataHandler(string dataDirPath, string dataFileName, bool useEncryption)
    {
        this._dataDirectoryPath = dataDirPath;
        this._dataFileName = dataFileName;
        this._useEncryption = useEncryption;
    }

    public void Save(GameData data, string profileId)
    {
        if(profileId == null)
        {
            return;
        }

        string fullPath = Path.Combine(_dataDirectoryPath, profileId, _dataFileName);
        string backupPath = fullPath + _backupExtension;

        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            string dataToStore = JsonUtility.ToJson(data, true);

            if (_useEncryption)
            {
                dataToStore = _EncryptDecrypt(dataToStore);
            }

            using(FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using(StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }

            GameData verifiedGameData = Load(profileId);

            if(verifiedGameData != null)
            {
                File.Copy(fullPath, backupPath, true);
            }
            else
            {
                throw new Exception("Save file couldn't be verified and back-up could not be created!");
            }

        } catch(Exception e)
        {
            Debug.LogError("Error occurred when trying to save data to file: " + fullPath + "\n" + e);
        }

    }

    public GameData Load(string profileId, bool allowRestoreFromBack = true)
    {
        //Load the game
        if(profileId == null)
        {
            return null;
        }

        string fullPath = Path.Combine(_dataDirectoryPath, profileId, _dataFileName);

        GameData loadedData = null;
        if (File.Exists(fullPath))
        {
            try
            {
                string dataToLoad = "";

                using(FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using(StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                if (_useEncryption)
                {
                    dataToLoad = _EncryptDecrypt(dataToLoad);
                }

                loadedData = JsonUtility.FromJson<GameData>(dataToLoad);

            } catch(Exception e)
            {
                if (allowRestoreFromBack)
                {
                    //Attempt rollback of file. If successful, then call this method recursively once
                    Debug.LogWarning("Failed to load data file. Attempting to roll back.\n" + e);
                    bool rollbackSuccess = _AttemptRollbackOfData(fullPath);

                    if (rollbackSuccess)
                    {
                        loadedData = Load(profileId, false);
                    }
                }
                else
                {
                    Debug.LogError("Error occurred when trying to load file at path " + fullPath + " and backup failed to work.\n" + e);
                }
            }
        }

        return loadedData;
    }

    public void Delete(string profileId)
    {
        if(profileId == null)
        {
            return;
        }

        string fullPath = Path.Combine(_dataDirectoryPath, profileId, _dataFileName);

        try
        {
            if (File.Exists(fullPath))
            {
                //delete the profile's folder
                Directory.Delete(Path.GetDirectoryName(fullPath), true);
            }
            else
            {
                Debug.LogWarning("Tried to delete profile data, but data was not found at: " + fullPath);
            }
        } catch(Exception e)
        {
            Debug.LogError("Failed to delete data for profileId: " + profileId + " at path: " + fullPath + "\n" + e);
        }
    }

    public Dictionary<string, GameData> LoadAllProfiles()
    {
        Dictionary<string, GameData> profileDictionary = new Dictionary<string, GameData>();

        IEnumerable<DirectoryInfo> dirInfos = new DirectoryInfo(_dataDirectoryPath).EnumerateDirectories();

        foreach(DirectoryInfo dirInfo in dirInfos)
        {
            string profileId = dirInfo.Name;

            string fullPath = Path.Combine(_dataDirectoryPath, profileId, _dataFileName);

            if (!File.Exists(fullPath))
            {
                continue;
            }

            GameData profileData = Load(profileId);
            if(profileData != null)
            {
                profileDictionary.Add(profileId, profileData);
            }
            else
            {
                Debug.LogError("Tried to load profile but something went wrong.");
            }
        }

        return profileDictionary;
    }

    public string GetLastSavedScene(string selectedId)
    {
        GameData dataToCheck = Load(selectedId);

        return dataToCheck.lastSceneSaved;
    }

    public string GetMostRecentProfile()
    {
        string mostRecentProfile = null;
        Dictionary<string, GameData> profilesData = LoadAllProfiles();

        foreach(KeyValuePair<string, GameData> pair in profilesData)
        {
            string profileId = pair.Key;
            GameData gameData = pair.Value;

            if(gameData == null)
            {
                continue;
            }

            if(mostRecentProfile == null)
            {
                mostRecentProfile = profileId;
            }
            else
            {
                DateTime mostRecentDateTime = DateTime.FromBinary(profilesData[mostRecentProfile].lastSaved);
                DateTime newDateTime = DateTime.FromBinary(gameData.lastSaved);

                if(newDateTime > mostRecentDateTime)
                {
                    mostRecentProfile = profileId;
                }

            }
        }

        return mostRecentProfile;
    }

    string _EncryptDecrypt(string data)
    {
        string modifiedData = "";

        for(int i = 0; i < data.Length; i++)
        {
            modifiedData += (char)(data[i] ^ _encryptionCodeword[i & _encryptionCodeword.Length]);
        }

        return modifiedData;
    }

    bool _AttemptRollbackOfData(string fullPath)
    {
        bool success = false;

        string backupPath = fullPath + _backupExtension;

        try
        {
            if (File.Exists(backupPath))
            {
                File.Copy(backupPath, fullPath, true);
                success = true;
                Debug.LogWarning("Data had to be rolled back to backup file at: " + backupPath);
            }
            else
            {
                throw new Exception("Tried to roll back data, but no backup file exists to roll data back to");
            }
        } catch(Exception e)
        {
            Debug.Log("Error occurred while trying to roll back to backup file at: " + backupPath + "\n" + e);
        }

        return success;
    }
}
