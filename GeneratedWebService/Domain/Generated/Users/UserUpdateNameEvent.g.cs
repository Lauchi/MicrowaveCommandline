//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Domain.Users
{
    using System;
    
    
    public class UserUpdateNameEvent : DomainEventBase
    {
        
        private Guid _UserId;
        
        private String _Name;
        
        private UserUpdateNameEvent()
        {
        }
        
        public UserUpdateNameEvent(Guid UserId, String Name)
        {
            this._UserId = UserId;
            this._Name = Name;
        }
        
        public Guid UserId
        {
            get
            {
                return this._UserId;
            }
        }
        
        public String Name
        {
            get
            {
                return this._Name;
            }
        }
    }
}
