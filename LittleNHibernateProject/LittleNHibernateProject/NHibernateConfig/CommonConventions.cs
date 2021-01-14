namespace LittleNHibernateProject
{
    #region using
    using FluentNHibernate.Conventions;
    using FluentNHibernate.Conventions.Instances;

    #endregion

    /// <summary>
    /// mapping on database convention
    /// </summary>
    public class CommonConventions : IConvention, IPropertyConvention, IIdConvention, IClassConvention, IHasManyConvention, IReferenceConvention, IVersionConvention
    {

        #region IReferenceConvention
        public void Apply(IManyToOneInstance instance)
        {
            instance.ForeignKey(GetForeignKeyName(instance.EntityType.Name, instance.Property.Name));
        }
        
        #endregion

        #region IHasManyConvention
        
        /// <summary>
        /// one to many
        /// </summary>
        /// <remarks>HasMany Map</remarks>
        public void Apply(IOneToManyCollectionInstance instance)
        {
            instance.Key.ForeignKey(GetForeignKeyName(instance.ChildType.Name, instance.EntityType.Name));
            instance.KeyNullable();
            instance.KeyUpdate();
            
            instance.Cache.ReadWrite();
            instance.Cache.Region(instance.EntityType.Name);
        }

        #endregion
                
        #region IPropertyConvention
        public void Apply(IPropertyInstance instance)
        {
            if (instance.Property.PropertyType == typeof(byte[]))
            {
                instance.CustomType("BinaryBlob");
                instance.Length(1048576);
            }
            else
            {
                instance.CustomType(instance.Property.PropertyType);

                if (instance.Property.PropertyType.IsEnum)
                    instance.Not.Nullable();
                                
                if (instance.Property.Name.Equals("Deleted"))
                    instance.Not.Nullable();
            }
        }
        #endregion

        #region IIdConvention
        public void Apply(IIdentityInstance instance)
        {
            instance.Column("Id");
            instance.GeneratedBy.Increment();
        }
        #endregion

        #region IClassConvention
        public void Apply(IClassInstance instance)
        {
            instance.Table(instance.EntityType.Name);
            instance.OptimisticLock.Version();
        }
        #endregion

        private static string GetForeignKeyName(string child, string parent)
        {
            return $"FK_{child}_{parent}";
        }

        public void Apply(IVersionInstance instance)
        {
            instance.Generated.Always();
            instance.UnsavedValue("0");
            instance.Not.Nullable();
            instance.Default(1);
        }
    }
}