using FluentNHibernate.Mapping;

namespace MyBlazorTest.Core.Models
{
    public class CatMap : ClassMap<CatEntity>
    {
        public CatMap()
        {
            Table("Cat");
            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.Name).Length(16).Not.Nullable();
            Map(x => x.Sex).Length(1).Nullable();
            Map(x => x.Weight).Nullable();
        }
    }
}
