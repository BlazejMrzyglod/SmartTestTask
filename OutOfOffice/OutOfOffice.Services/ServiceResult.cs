using System.ComponentModel;

namespace OutOfOffice.Services
{
    public enum ServiceResultStatus
    {
        [Description("Error")]
        Error = 0,
        [Description("Success")]
        Succes = 1,
        [Description("Warning")]
        Warrnig,
        [Description("Information")]
        Info,
    }
    public class ServiceResult
    {
        public ServiceResultStatus Result { get; set; }
        public ICollection<String> Messages { get; set; }

        public ServiceResult()
        {
            Result = ServiceResultStatus.Succes;
            Messages = new List<string>();
        }

        public static Dictionary<string, ServiceResult> CommonResults { get; set; } = new Dictionary<string, ServiceResult>()
        {
              {"NotFound" , new ServiceResult() {
                  Result =ServiceResultStatus.Error,
                  Messages = new List<string>( new string[] { "Object not gound" })  } },
              {"OK" , new ServiceResult() {
                  Result =ServiceResultStatus.Succes,
                  Messages = new List<string>()  } }
        };
    }
}