using System.Collections.Generic;
using TrafficReport.Models;

namespace TrafficReport.Services
{
    interface ILTADataMallGateway
    {
        List<LTADataMallModel.AccidentData> GetLTAAccidentData();
        List<LTADataMallModel.SpeedData> GetLTASpeedData();
    }
}
