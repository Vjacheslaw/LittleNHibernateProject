namespace LittleNHibernateProject.Model
{
    public abstract class Banan
    {
        public virtual long Id { get; set; }

        public virtual string Variety { get; set; }

        public virtual long VersionObject { get; set; }
    }
}