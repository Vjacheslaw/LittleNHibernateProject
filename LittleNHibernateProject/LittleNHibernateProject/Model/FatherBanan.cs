namespace LittleNHibernateProject.Model
{
    public class FatherBanan : Banan
    {
        public virtual string Name { get; set; }

        public virtual Banan WifeBanan { get; set; }
    }
}