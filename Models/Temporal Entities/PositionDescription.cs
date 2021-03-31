namespace CIS.HR.Models
{
    public class PositionDescription : TemporalEntity
    {
        public int PositionId { get; set; }
        public int ClassificationId { get; set; }
        public string Uid { get; set; }
        public string Title { get; set; }
        public virtual Position Position { get; set; }
        public virtual PositionClassification Classification { get; set; }
        //public virtual BenefitsDescription BenefitsDescription { get; set; }

        public int? LegacyId { get; set; }
        //public int PerformanceClassId { get; set; }
    }

    public class PositionDescriptionConfig : EntityConfig<PositionDescription>
    {
        public PositionDescriptionConfig()
        {
            this.HasRequired<Position>(e => e.Position)
                .WithMany(e => e.PositionDescriptionHistory)
                .HasForeignKey<int>(k => k.PositionId)
                .WillCascadeOnDelete(false);
        }
    }
}