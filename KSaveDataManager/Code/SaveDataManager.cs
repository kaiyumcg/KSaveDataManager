using UnityEngine;

namespace KSaveDataMan
{
    public enum CloudDataCode { Success = 0, NoInternet = 1, ServerConnectionError = 2, ServerStorageFull = 3, InvalidOperationError = 4 }
    public enum LocalDataCode { Success = 0, PermissionOrIOError = 1, InvalidOperationError = 2 }

    //Implements ieneumerators so that we can use it in coroutine as well
    public class AsyncDataOpHandle
    {
        //progress float and percent format
        //is completed
        //label

    }

    public class GameData<T>
    {
        private GameData() { }

        internal static GameData<T> CreateHandle(string[] identifiers)
        {
            throw new System.NotImplementedException();
        }

        T g_value;

        /// <summary>
        /// Get does I/O operation, update corresponding internal memory data and then returns it
        /// Set actually updates both internal memory data as well as the value of atomic in local storage
        /// </summary>
        public T Value
        {
            get 
            {
                return g_value;
            }
            set 
            {
                g_value = value;
            }
        }
    }

    public class EncryptionUsageDescription
    {
        
    }

    public class CloudUsageDescription
    {
        
    }

    public class SaveDataSetting
    {
        
    }

    //cloud, bigdata etc module desc static data so that we can query operation for each modules
    //CDATA style thing or get/set or both or completely new?
    public static class SaveDataManager
    {
        /// <summary>
        /// Initialize save data manager.
        /// </summary>
        /// <param name="encryptionSetting">Encryption setting. If null then it will not use any encryption.</param>
        /// <param name="cloudSetting">Cloud setting. If null then it will raise exception if code use any cloud feature</param>
        /// <param name="OnComplete">Applicable for cloud usage. Ignore if you do not use it</param>
        public static void InitSystem(SaveDataSetting mainSetting, System.Action OnComplete = null, 
            EncryptionUsageDescription encryptionSetting = null, CloudUsageDescription cloudSetting = null)
        {
            
        }

        /// <summary>
        /// Dengerous! This will wipe out all atomic, small class/struct data and cloud data(if user credential is set properly during initialization)
        /// </summary>
        public static void DeleteAllData()
        {
            throw new System.NotImplementedException();
        }

        #region Atomic
        /// <summary>
        /// Deletes corresponding atomic data with all identifiers provided.
        /// <para>If no identifiers is provided, then will wipe out all atomic data</para>
        /// </summary>
        /// <param name="identifiers">List of labels attributed to the atomic data</param>
        public static void DeleteAtomic(params string[] identifiers)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Using the identifiers provided, returns a data handle to user.
        /// <para>If the data already exists then the handle represents current data.</para>
        /// <para>Otherwise, a fresh atomic data will be created in device.</para>
        /// </summary>
        /// <typeparam name="T">Data type</typeparam>
        /// <param name="identifiers">List of labels attributed to the atomic data</param>
        /// <returns></returns>
        public static GameData<T> GetOrCreateAtomic<T>(params string[] identifiers)
        {
            return GameData<T>.CreateHandle(identifiers);
        }
        #endregion

        #region BigData
        /// <summary>
        /// Permanently deletes all big data(typically big one. i.e. several megabytes to gigabytes) identified by all[NOT just any but 'all'] the 'identifier's.
        /// <para>If no identifiers are provided then this will permanently delete all local data</para>
        /// </summary>
        /// <param name="OnComplete">Operation completion callback</param>
        /// <param name="identifiers">List of labels attributed to the data</param>
        public static AsyncDataOpHandle ClearBigLocalData(System.Action<LocalDataCode> OnComplete, params string[] identifiers)
        {
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
        public static AsyncDataOpHandle SaveBigDataToDevice(byte[] data, System.Action<LocalDataCode> OnComplete, params string[] identifiers)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Loads the local data(typically big one. i.e. several megabytes to gigabytes) in byte array into memory with 'identifier's.
        /// <para>If no such data with identifiers are present, default or null will be given in the callback</para>
        /// </summary>
        /// <param name="OnComplete">Operation completion callback</param>
        /// <param name="identifiers">List of labels attributed to the data</param>
        public static AsyncDataOpHandle LoadBigDataFromDevice(System.Action<LocalDataCode, byte[]> OnComplete, params string[] identifiers)
        {
            throw new System.NotImplementedException();
        }
        #endregion

        #region SmallClassOrStructData
        /// <summary>
        /// Permanently deletes all data identified by all[NOT just any but 'all'] the 'identifier's.
        /// <para>If no identifiers are provided then this will permanently delete all local data</para>
        /// </summary>
        /// <param name="OnComplete">Operation completion callback</param>
        /// <param name="identifiers">List of labels attributed to the data</param>
        public static AsyncDataOpHandle ClearLocalData(System.Action<LocalDataCode> OnComplete, params string[] identifiers)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Store the data into device storage with 'identifier's.
        /// <para>If there is any data present with 'all' 'identifiers', then this will overwrite the local data.</para>
        /// <para>Otherwise it will create a new data in the local storage.</para>
        /// </summary>
        /// <typeparam name="T">Data Type</typeparam>
        /// <param name="data">Data</param>
        /// <param name="OnComplete">Operation completion callback</param>
        /// <param name="identifiers">List of labels attributed to the data</param>
        public static AsyncDataOpHandle SaveDataToDevice<T>(T data, System.Action<LocalDataCode> OnComplete, params string[] identifiers)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Loads the local data into memory with 'identifier's.
        /// <para>If no such data with identifiers are present, default or null will be given in the callback</para>
        /// </summary>
        /// <typeparam name="T">Data type</typeparam>
        /// <param name="OnComplete">Operation completion callback</param>
        /// <param name="identifiers">List of labels attributed to the data</param>
        public static AsyncDataOpHandle LoadDataFromDevice<T>(System.Action<LocalDataCode, T> OnComplete, params string[] identifiers)
        {
            throw new System.NotImplementedException();   
        }
        #endregion

        #region Cloud
        /// <summary>
        /// Permanently deletes all data identified by all[NOT just any but 'all'] the 'identifier's.
        /// <para>If no identifiers are provided then this will permanently delete all cloud data associated with the 'user' setup earlier</para>
        /// </summary>
        /// <param name="OnComplete">Operation completion callback</param>
        /// <param name="identifiers">List of labels attributed to the data</param>
        public static AsyncDataOpHandle ClearCloudData(System.Action<CloudDataCode> OnComplete, params string[] identifiers)
        {
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
        public static AsyncDataOpHandle SaveDataToCloud<T>(T data, System.Action<CloudDataCode> OnComplete, params string[] identifiers)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Loads the cloud data into memory with 'identifier's.
        /// <para>If no such data with identifiers are present, default or null will be given in the callback</para>
        /// </summary>
        /// <typeparam name="T">Data type</typeparam>
        /// <param name="OnComplete">Operation completion callback</param>
        /// <param name="identifiers">List of labels attributed to the data</param>
        public static AsyncDataOpHandle LoadDataFromCloud<T>(System.Action<CloudDataCode, T> OnComplete, params string[] identifiers)
        {
            throw new System.NotImplementedException();
        }
        #endregion
    }
}