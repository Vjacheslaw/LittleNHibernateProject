using FluentNHibernate.Automapping;
using System;
using FluentNHibernate;

namespace LittleNHibernateProject.NHibernateConfig
{
    public class AutomappingConfiguration : DefaultAutomappingConfiguration
    {
        public override bool ShouldMap(Type type)
        {
            if (type.Namespace == "LittleNHibernateProject.Model")
            {
                return
                    base.ShouldMap(type);
            }

            return false;
        }

        public override bool IsVersion(Member member)
        {
            return member.Name == "VersionObject";
        }
    }
}