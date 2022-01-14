using UnityEngine;
using UnityEngine.Events;

namespace KSaveDataMan
{
    //cloud, bigdata etc module desc static data so that we can query operation for each modules
    //CDATA style thing or get/set or both or completely new?
    public static class SaveDataManager
    {
        public static bool IsInitialized { get { return systemInitialised; } }
        public static System.Action OnInitialized;
        public static UnityEvent OnInitializedEvent;
        internal static SaveDataOperationManager operationManager = null;
        static bool systemInitialised = false;
        static void CheckManager()
        {
            if (operationManager == null)
            {
                var g = new GameObject("_SaveDataOperationManager_Gen_");
                operationManager = g.AddComponent<SaveDataOperationManager>();
            }
        }

        static void CheckInitOfSys()
        {
            if (IsInitialized == false && Config.data != null && Config.data.DebugMessage)
            {
                Debug.LogError("You must initialized the save data manager first to use any of it's feature!");
            }
        }

        public static void InitSystem(SaveDataConfig config, System.Action OnComplete = null)
        {
            CheckManager();
            Config.data = config;
            AtomicSaveInternalController.LoadMasterSaveToMemory();
            ClassSave.LoadMasterSaveToMemory();
            //todo do cloud initialization stuffs
            //todo do any big data or small class data json initialization stuffs
            systemInitialised = true;//this should ideally be inside the callback when everything is actually initialized.
            OnInitialized?.Invoke();
            OnInitializedEvent?.Invoke();
        }

        //Gameobject save class(asset bundle replacement?)

        /// <summary>
        /// Dengerous! This will wipe out all atomic, small class/struct data and cloud data(if user credential is set properly during initialization)
        /// </summary>
        public static void DeleteAllData()
        {
            CheckInitOfSys();
            CheckManager();
            AtomicSaveInternalController.Clear(null);
            ClassSave.Clear(null);
        }

        #region Atomic
        /// <summary>
        /// Deletes corresponding atomic data with all identifiers provided.
        /// <para>If no identifiers is provided, then will wipe out all atomic data</para>
        /// </summary>
        /// <param name="identifiers">List of labels attributed to the atomic data</param>
        public static void DeleteAtomic(params string[] identifiers)
        {
            CheckInitOfSys();
            CheckManager();
            AtomicSaveInternalController.Clear(identifiers);
        }

        /// <summary>
        /// Write all atomic save data to device.
        /// <para>The system automatically writes at sceneload/app exit.</para>
        /// </summary>
        public static void SaveAtomicToDevice()
        {
            CheckInitOfSys();
            CheckManager();
            AtomicSaveInternalController.WriteMasterSaveToDevice();
        }

        /// <summary>
        /// Load all atomic data to memory
        /// <para>The system automatically loads atomic save file to memory at first usage if it not loaded already during initialisation.</para>
        /// </summary>
        public static void LoadAtomicFromDevice()
        {
            CheckInitOfSys();
            CheckManager();
            AtomicSaveInternalController.LoadMasterSaveToMemory();
        }

        /// <summary>
        /// Using the identifiers provided, returns a data handle to user.
        /// <para>If the data already exists then the handle represents current data.</para>
        /// <para>Otherwise, a fresh atomic data will be created.</para>
        /// </summary>
        /// <typeparam name="T">Data type</typeparam>
        /// <param name="identifiers">List of labels attributed to the atomic data</param>
        /// <returns></returns>
        public static AtomicSaveData<T> GetOrCreateAtomic<T>(params string[] identifiers) where T : struct
        {
            CheckInitOfSys();
            CheckManager();
            return AtomicSaveData<T>.CreateHandle(identifiers);
        }
        #endregion

        #region BigData
        /// <summary>
        /// Permanently deletes all big data(typically big one. i.e. several megabytes to gigabytes) identified by all[NOT just any but 'all'] the 'identifier's.
        /// <para>If no identifiers are provided then this will permanently delete all local data</para>
        /// </summary>
        /// <param name="OnComplete">Operation completion callback</param>
        /// <param name="identifiers">List of labels attributed to the data</param>
        public static void ClearBigLocalData(System.Action<LocalDataCode> OnComplete, params string[] identifiers)
        {
            CheckInitOfSys();
            CheckManager();
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Store the byte array data(typically big one. i.e. several megabytes to gigabytes) into device storage with 'identifier's.
        /// <para>If there is any data present with 'all' 'identifiers', then this will overwrite the local data.</para>
        /// <para>Otherwise it will create a new data in the local storage.</para>
        /// </summary>
        /// <param name="data">Byte array data</param>
        /// <param name="OnComplete">Operation completion callback</param>
        /// <param name="identifiers">List of labels attributed to the data</param>
        public static void SaveBigDataToDevice(byte[] data, System.Action<LocalDataCode> OnComplete, params string[] identifiers)
        {
            CheckInitOfSys();
            CheckManager();
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Loads the local data(typically big one. i.e. several megabytes to gigabytes) in byte array into memory with 'identifier's.
        /// <para>If no such data with identifiers are present, default or null will be given in the callback</para>
        /// </summary>
        /// <param name="OnComplete">Operation completion callback</param>
        /// <param name="identifiers">List of labels attributed to the data</param>
        public static void LoadBigDataFromDevice(System.Action<LocalDataCode, byte[]> OnComplete, params string[] identifiers)
        {
            CheckInitOfSys();
            CheckManager();
            throw new System.NotImplementedException();
        }
        #endregion

        #region SmallClassOrStructData
        /// <summary>
        /// Permanently deletes all data identified by all the 'identifier's. 
        /// <para>If no identifiers are provided then this will permanently delete all local class/struct data</para>
        /// <para>Only work for the data that is possible to serialize/deserialize by unity </para>
        /// </summary>
        /// <param name="identifiers">List of labels attributed to the class data</param>
        public static void ClearLocalData(params string[] identifiers)
        {
            CheckInitOfSys();
            CheckManager();
            ClassSave.Clear(identifiers);
        }

        /// <summary>
        /// Store the data into device storage with 'identifier's.
        /// <para>If there is any data present with 'all' 'identifiers', then this will overwrite the local class/struct data.</para>
        /// <para>Otherwise it will create a new data in the local storage.</para>
        /// <para>Only work for the data that is possible to serialize/deserialize by unity </para>
        /// </summary>
        /// <typeparam name="T">Data Type</typeparam>
        /// <param name="data">Data</param>
        /// <param name="OnComplete">Operation completion callback</param>
        /// <param name="identifiers">List of labels attributed to the data</param>
        public static void SaveDataToDevice<T>(T data, System.Action<LocalDataCode> OnComplete, params string[] identifiers)
        {
            CheckInitOfSys();
            CheckManager();
            ClassSave.Save(data, identifiers, OnComplete);
        }

        /// <summary>
        /// Loads the local data into memory with 'identifier's.
        /// <para>If no such data with identifiers are present, default or null will be given in the callback</para>
        /// <para>Only work for the data that is possible to serialize/deserialize by unity </para>
        /// </summary>
        /// <typeparam name="T">Data type</typeparam>
        /// <param name="OnComplete">Operation completion callback</param>
        /// <param name="identifiers">List of labels attributed to the data</param>
        public static void LoadDataFromDevice<T>(System.Action<LocalDataCode, T> OnComplete, params string[] identifiers)
        {
            CheckInitOfSys();
            CheckManager();
            ClassSave.Load(identifiers, OnComplete);
        }
        #endregion

        #region Cloud
        /// <summary>
        /// Permanently deletes all data identified by all[NOT just any but 'all'] the 'identifier's.
        /// <para>If no identifiers are provided then this will permanently delete all cloud data associated with the 'user' setup earlier</para>
        /// </summary>
        /// <param name="OnComplete">Operation completion callback</param>
        /// <param name="identifiers">List of labels attributed to the data</param>
        public static void ClearCloudData(System.Action<CloudDataCode> OnComplete, params string[] identifiers)
        {
            CheckInitOfSys();
            CheckManager();
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Store the data into cloud with 'identifier's.
        /// <para>If there is any data present with 'all' 'identifiers', then this will overwrite the server data.</para>
        /// <para>Otherwise it will create a new data in server and store there.</para>
        /// </summary>
        /// <typeparam name="T">Data type</typeparam>
        /// <param name="data">Data</param>
        /// <param name="OnComplete">Operation completion callback</param>
        /// <param name="identifiers">List of labels attributed to the data</param>
        public static void SaveDataToCloud<T>(T data, System.Action<CloudDataCode> OnComplete, params string[] identifiers)
        {
            CheckInitOfSys();
            CheckManager();
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Loads the cloud data into memory with 'identifier's.
        /// <para>If no such data with identifiers are present, default or null will be given in the callback</para>
        /// </summary>
        /// <typeparam name="T">Data type</typeparam>
        /// <param name="OnComplete">Operation completion callback</param>
        /// <param name="identifiers">List of labels attributed to the data</param>
        public static void LoadDataFromCloud<T>(System.Action<CloudDataCode, T> OnComplete, params string[] identifiers)
        {
            CheckInitOfSys();
            CheckManager();
            throw new System.NotImplementedException();
        }
        #endregion
    }
}