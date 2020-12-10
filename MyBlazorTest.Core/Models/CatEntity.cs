namespace MyBlazorTest.Core.Models
{
    public class CatEntity : BaseEntity
    {
        public virtual string Name { get; set; }

        public virtual char Sex { get; set; }

        public virtual float Weight { get; set; }

        public override string ToString()
        {
            return $"Name: {Name}. Sex: {Sex}. Weight: {Weight}";
        }
    }
}
