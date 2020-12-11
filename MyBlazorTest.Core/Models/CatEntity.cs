namespace MyBlazorTest.Core.Models
{
    public class CatEntity : BaseEntity
    {
        public virtual string Name { get; set; }

        public virtual char Sex { get; set; }

        public virtual float Weight { get; set; }

        public CatEntity()
        {
            //
        }

        public CatEntity(string name, char sex, float weight)
        {
            Name = name;
            Sex = sex;
            Weight = weight;
        }

        public override string ToString()
        {
            return $"Name: {Name}. Sex: {Sex}. Weight: {Weight}";
        }
    }
}
