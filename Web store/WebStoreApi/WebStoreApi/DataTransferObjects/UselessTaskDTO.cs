using System;

namespace WebStore.Api.DataTransferObjects
{
    public class UselessTaskDTO : ICloneable
    {
        public int Id { get; set; }

        public UselessTaskStates Status { get; set; }

        public long Progress { get; set; }

        public long TotalWorkAmount { get; set; }

        public string ServerIdentifier { get; set; }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }

    public enum UselessTaskStates
    {
        New,
        Running,
        Canceled,
        Done
    }
}
