namespace TrafficReport.DAL
{
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using Models;

    public partial class TrafficReportContext : DbContext
    {
        public TrafficReportContext() : base("name=trafficreportEntities2")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }

        public virtual DbSet<tblLocationName> tblLocationNames { get; set; }
        public virtual DbSet<tblRainfall> tblRainfalls { get; set; }
        public virtual DbSet<tblRoadName> tblRoadNames { get; set; }
        public virtual DbSet<tblTrafficAccident> tblTrafficAccidents { get; set; }
        public virtual DbSet<tblTrafficSpeed> tblTrafficSpeeds { get; set; }
    }
}
