//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Domain
{
    using System;
    
    
    public interface IUser
    {
        
        UserUpdateAgeEvent UpdateAge(Int32 Age);
        
        UserUpdateNameEvent UpdateName(String Name);
    }
    
    public partial class User : IUser
    {
        
        private Guid _Id;
        
        private String _Name;
        
        private Int32 _Age;
        
        private User(Guid Id, String Name, Int32 Age)
        {
            this._Id = Id;
            this._Name = Name;
            this._Age = Age;
        }
        
        private User()
        {
        }
        
        public Guid Id
        {
            get
            {
                return this._Id;
            }
        }
        
        public String Name
        {
            get
            {
                return this._Name;
            }
        }
        
        public Int32 Age
        {
            get
            {
                return this._Age;
            }
        }
    }
}
