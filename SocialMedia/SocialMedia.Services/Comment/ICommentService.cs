﻿using Microsoft.EntityFrameworkCore;
using SocialMedia.Services.Models;
using System.Threading.Tasks;

namespace SocialMedia.Services.Comment
{
    public interface ICommentService
    {
        Task<EntityState> AddComment(CommentServiceModel commentServiceModel);
    }
}