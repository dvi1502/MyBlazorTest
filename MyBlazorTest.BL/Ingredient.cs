using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyBlazorTest.BL
{
    public class Ingredient : BaseEntity
    {
        public string Name { get; set; }
        public int Amount { get; set; }
        public int RecipeId { get; set; }
        public Recipe Recipe { get; set; }
    }

    public class IngredientMap : ClassMap<Ingredient>
    {
        public IngredientMap()
        {

            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.Name);
            Map(x => x.Amount);
            Map(x => x.RecipeId);
            Map(x => x.Recipe);

        }
    }


}
