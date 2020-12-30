using MyBlazorTest.Core.Models;
using System.Threading;

namespace MyBlazorTest.Core
{
    public class DataConvert
    {
        #region Design pattern "Lazy Singleton"

        private static DataConvert _instance;
        public static DataConvert Instance => LazyInitializer.EnsureInitialized(ref _instance);

        #endregion

        #region Constructor and destructor

        public DataConvert() { }

        #endregion

        #region Public and private methods

        public T[] ToChilds<T>(T[] entities) where T : class
        {
            var result = new T[0];
            if (!(entities is null) && entities.Length > 0)
            {
                result = new T[entities.Length];
                var i = 0;
                foreach (var entity in entities)
                {
                    result[i] = entity;
                    i++;
                }
            }
            return result;
        }

        #endregion
    }
}
