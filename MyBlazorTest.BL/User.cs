using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyBlazorTest.BL
{
    public class User : BaseEntity
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public IEnumerable<Recipe> Recipes { get; set; }
    }

    public class UserMap : ClassMap<User>
    {
        public UserMap()
        {

            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.Name);
            Map(x => x.Email);
            Map(x => x.Recipes);

        }
    }
}
