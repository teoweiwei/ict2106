//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TrafficReport.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class tblTrafficSpeed
    {
        public int tsID { get; set; }
        public System.DateTime tsDateTime { get; set; }
        public Nullable<int> tsRoadName { get; set; }
        public Nullable<int> tsMinSpeed { get; set; }
        public Nullable<int> tsMaxSpeed { get; set; }
        public Nullable<double> tsStartLat { get; set; }
        public Nullable<double> tsStartLong { get; set; }
        public Nullable<double> tsEndLong { get; set; }
    
        public virtual tblRoadName tblRoadName { get; set; }
    }
}
