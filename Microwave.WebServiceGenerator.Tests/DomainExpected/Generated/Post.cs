//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Domain.Posts
{
    using System;
    using System.Collections.Generic;
    
    
    public partial class Post
    {
        
        public static CreationResult<Post> Create(PostCreateCommand command)
        {
            // TODO: Implement this method;
            var newGuid = Guid.NewGuid();
            var entity = new Post(newGuid, command);
            return CreationResult<Post>.OkResult(new List<DomainEventBase> { new PostCreateEvent(entity, newGuid) }, entity);
        }
        
        public override ValidationResult UpdateTitle(PostUpdateTitleCommand command)
        {
            // TODO: Implement this method;
            return ValidationResult.ErrorResult(new List<string>{"The Method \"UpdateTitle\" in Class \"Post\" that is not implemented was called, aborting..."});
        }
    }
}
