using Domain.Core;

namespace Domain.Models
{
    public class UserInfo : EntityBase<UserInfo, int>
    {
        public string Name { get; set; }
        public string Pw { get; set; }
    }
}