using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;
using LittleNHibernateProject.Model;

namespace LittleNHibernateProject.NHibernateConfig
{

    /// <summary>
    /// Для маппинга абстрактных типов
    /// </summary>
    public class OvverideMappingsFatherBanan : IAutoMappingOverride<FatherBanan>
    {
        public void Override(AutoMapping<FatherBanan> mapping)
        {
            mapping.ReferencesAny(x => x.WifeBanan).EntityTypeColumn("Banan_Type")
                .EntityIdentifierColumn("Banan_Id").IdentityType<long>()
                .AddMetaValue<MotherBanan>("MotherBanan")
                .AddMetaValue<FatherBanan>("FatherBanan")
                ;
        }
    }

    public class OvverideMappingsMotherBanan : IAutoMappingOverride<MotherBanan>
    {
        public void Override(AutoMapping<MotherBanan> mapping)
        {
            mapping.ReferencesAny(x => x.HusbandBanan).EntityTypeColumn("Banan_Type")
                .EntityIdentifierColumn("Banan_Id").IdentityType<long>()
                .AddMetaValue<MotherBanan>("MotherBanan")
                .AddMetaValue<FatherBanan>("FatherBanan")
                ;
        }
    }

    public class OvverideMappingsJobBanan : IAutoMappingOverride<JobBanan>
    {
        public void Override(AutoMapping<JobBanan> mapping)
        {
            mapping.ReferencesAny(x => x.Banan).EntityTypeColumn("Banan_Type")
                .EntityIdentifierColumn("Banan_Id").IdentityType<long>()
                .AddMetaValue<MotherBanan>("MotherBanan")
                .AddMetaValue<FatherBanan>("FatherBanan")
                ;
        }
    }
}
