//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Application.Users
{
    using System;
    using System.Collections.Generic;
    
    
    public class UserAddPostApiCommand
    {
        
        public Guid NewPostId { get; private set; }
        
        public Guid PostToDeleteId { get; private set; }
        
        public UserAddPostApiCommand(Guid NewPostId, Guid PostToDeleteId)
        {
            this.NewPostId = NewPostId;
            this.PostToDeleteId = PostToDeleteId;
        }
    }
}