namespace MediaLibrary
{
    using System.Data.Entity;

    public partial class TestEntities : DbContext
    {
        public TestEntities(string connectionString)
            : base(connectionString)
        {
 
        }
    }
}
