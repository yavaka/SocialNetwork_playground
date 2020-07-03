﻿namespace SocialMedia.Services.Models
{
    using SocialMedia.Data.Models;
    using SocialMedia.Models.ViewModels;
    using System;
    using System.Collections.Generic;

    public class PostServiceModel
    {
        public PostServiceModel()
        {
            this.CurrentUserFriends = new List<UserServiceModel>();
            this.TaggedFriends = new List<UserServiceModel>();
            this.Comments = new List<CommentTagFriendsViewModel>();
        }

        public PostServiceModel(Post post)
        {
            this.PostId = post.PostId;
            this.Content = post.Content;
            this.DatePosted = post.DatePosted;
            this.Author = new UserServiceModel(post.Author);
            this.Group = post.Group;

            this.TaggedFriends = new List<UserServiceModel>();
            this.Comments = new List<CommentTagFriendsViewModel>();
        }

        public int PostId{ get; set; }

        public string Content { get; set; }

        public DateTime DatePosted{ get; set; }
       
        public UserServiceModel Author { get; set; }
       
        public Group Group { get; set; }////// Change with GroupServiceModel

        public IEnumerable<UserServiceModel> CurrentUserFriends{ get; set; }

        public IEnumerable<UserServiceModel> TaggedFriends{ get; set; }

        public IEnumerable<CommentTagFriendsViewModel> Comments{ get; set; }////// Change with CommentServiceModel
    }
}
