namespace LittleNHibernateProject.Model
{
    public class JobBanan
    {
        public virtual long Id { get; set; }

        public virtual string JobName { get; set; }

        public virtual Banan Banan { get; set; }

        public virtual long VersionObject { get; set; }
    }
}