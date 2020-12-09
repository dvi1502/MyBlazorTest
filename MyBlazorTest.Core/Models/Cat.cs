using FluentNHibernate.Mapping;

namespace MyBlazorTest.Core.Models
{
    public class Cat : BaseEntity
    {
        public virtual string Name { get; set; }

        public virtual char Sex { get; set; }

        public virtual float Weight { get; set; }
    }
    
    public class CatMap : ClassMap<Cat>
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
