namespace LittleNHibernateProject.Model
{
    public class MotherBanan : Banan
    {
        public virtual Banan HusbandBanan { get; set; }
    }
}