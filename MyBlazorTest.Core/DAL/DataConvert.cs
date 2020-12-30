namespace MyBlazorTest.Core.DAL
{
    public class DataConvert
    {
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
