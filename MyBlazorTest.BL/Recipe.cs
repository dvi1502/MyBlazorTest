using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyBlazorTest.BL
{
    public class Recipe : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public int Time { get; set; }
        public IEnumerable<Ingredient> Ingredients { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }

    }

    public class RecipeMap : ClassMap<Recipe>
    {
        public RecipeMap()
        {

            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.Name);
            Map(x => x.Description);
            Map(x => x.ImagePath);
            Map(x => x.Time);
            Map(x => x.Ingredients);
            Map(x => x.UserId);
            Map(x => x.User);

        }
    }

}
