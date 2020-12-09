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

        public DataConvert() { SetupDefault(); }

        public void SetupDefault()
        {
            // Default methods
        }

        public void Setup()
        {
            // Setup methods
        }

        #endregion

        #region Public and private methods

        public Cat[] ToCats(BaseEntity[] entities)
        {
            var result = new Cat[0];
            if (!(entities is null) && entities.Length > 0)
            {
                result = new Cat[entities.Length];
                var i = 0;
                foreach (var entity in entities)
                {
                    result[i] = (Cat)entity;
                    i++;
                }
            }
            return result;
        }

        #endregion
    }
}
