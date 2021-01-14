namespace LittleNHibernateProject.Model
{
    public class CompanyBanan
    {
        public virtual long Id { get; set; }

        public virtual FatherBanan FatherBanan { get; set; }

        public virtual string Name { get; set; }

        public virtual long VersionObject { get; set; }
    }
}
