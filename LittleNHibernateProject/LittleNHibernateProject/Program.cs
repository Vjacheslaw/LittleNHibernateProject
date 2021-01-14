using LittleNHibernateProject.Model;
using LittleNHibernateProject.NHibernateConfig;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.SqlCommand;
using NHibernate.Transform;
using System.Linq;

namespace LittleNHibernateProject
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new DbConfigurationFactory();
            var factorySession = factory.CreateFactory("connection string");

            using (var session = factorySession.OpenSession())
            {
                if (!session.Query<FatherBanan>().Any())//if empty - first start
                    InitializeData(session);
            }

            using (var session = factorySession.OpenSession())
            {
                //join on abstract class
                JobBanan job = null;
                FatherBanan fatherBanan = null;
                CompanyBanan companyBanan = null;
                var list =
                        session
                            .QueryOver<JobBanan>((() => job))
                            .Where(wh => wh.Banan.GetType().IsIn(new[] { typeof(FatherBanan).FullName }))
                            .JoinEntityAlias(
                                () => fatherBanan,
                                Restrictions.EqProperty("fatherBanan.Id", "job.Banan.id"),
                                JoinType.LeftOuterJoin
                            )
                            .JoinEntityAlias(
                                () => companyBanan,
                                Restrictions.EqProperty("companyBanan.FatherBanan.Id", "fatherBanan.Id"),
                                JoinType.LeftOuterJoin
                            )
                            .TransformUsing(Transformers.AliasToBean<JobAndFather>())
                            .List<JobAndFather>()
                    ;

                var banan = session.QueryOver<JobBanan>()
                    .Where(wh => wh.Banan != null)
                    .Where(Restrictions.Eq("Banan.id", (long)1))
                    .List();
            }
        }

        private static void InitializeData(ISession session)
        {
            session.Save(new FatherBanan { Variety = "Yellow", Name = "Отец" });
            session.Save(new FatherBanan { Variety = "Black", Name = "Отец2" });
            session.Save(new FatherBanan { Variety = "Red", Name = "Отец3" });
            session.Save(new FatherBanan { Variety = "White", Name = "Отец4" });

            session.Save(new MotherBanan { Variety = "Yellow" });
            session.Save(new MotherBanan { Variety = "Red" });
            session.Save(new MotherBanan { Variety = "Black" });
            session.Save(new MotherBanan { Variety = "White" });

            session.Flush();

            var fathers = session.Query<FatherBanan>().ToList();
            var mothers = session.Query<MotherBanan>().ToList();

            foreach (var father in fathers)
            {
                father.WifeBanan = mothers.First(wh => wh.Variety == father.Variety);
            }

            foreach (var mother in mothers)
            {
                mother.HusbandBanan = mothers.First(wh => wh.Variety == mother.Variety);
            }

            session.Flush();

            foreach (var father in fathers.Take(2))
            {
                session.Save(new JobBanan() { Banan = father, JobName = "Father job" });
                session.Save(new CompanyBanan() { FatherBanan = father, Name = "Banana company" });
            }

            foreach (var mother in mothers.Take(2))
            {
                session.Save(new JobBanan() { Banan = mother, JobName = "Mother job" });
            }

            session.Flush();
        }

        public class JobAndFather
        {
            public JobBanan job { get; set; }

            public FatherBanan fatherBanan { get; set; }

            public CompanyBanan companyBanan { get; set; }
        }
    }
}
