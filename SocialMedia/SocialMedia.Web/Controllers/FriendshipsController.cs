﻿namespace SocialMedia.Web.Controllers
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using SocialMedia.Services.Friendship;
    using SocialMedia.Services.User;

    [Authorize]
    public class FriendshipsController : Controller
    {
        private readonly IFriendshipService _friendshipService;
        private readonly IUserService _userService;

        public FriendshipsController(
            IFriendshipService friendshipService,
            IUserService userService)
        {
            this._friendshipService = friendshipService;
            this._userService = userService;
        }

        public async Task<IActionResult> UserFriends(string userId)
        {
            var friends = await this._friendshipService
                .GetFriendsAsync(userId);

            return View("Friends", friends);
        }

        public async Task<IActionResult> Friends()
        {
            TempData.Clear();
            var currentUserId = await this._userService
                .GetUserIdByNameAsync(User.Identity.Name);

            var friends = await this._friendshipService
                .GetFriendsAsync(currentUserId);

            return View(friends);
        }

        public async Task<IActionResult> NonFriends()
        {
            TempData.Clear();
            var currentUserId = await this._userService
                .GetUserIdByNameAsync(User.Identity.Name);

            var nonFriends = await this._friendshipService
                .GetNonFriendsAsync(currentUserId);

            return View(nonFriends);
        }

        [HttpGet]
        public async Task<IActionResult> FriendshipStatus(string userId, [FromQuery]string invokedFrom)
        {
            if (userId == null)
            {
                return NotFound();
            }

            var currentUserId = await this._userService
                .GetUserIdByNameAsync(User.Identity.Name);

            if (currentUserId == userId)
            {
                return RedirectToAction("Index", "Profile",
                    new { friendshipStatus = ServiceModelFriendshipStatus.CurrentUser });
            }

            var friendshipStatus = await this._friendshipService
                .GetFriendshipStatusAsync(currentUserId, userId);

            //Check is it invoked from pendings or requests collections
            if (invokedFrom != null &&
                friendshipStatus == ServiceModelFriendshipStatus.Pending)
            {
                if (invokedFrom == "Requests")
                {
                    friendshipStatus = ServiceModelFriendshipStatus.Request;
                }
            }

            return RedirectToAction("Index", "Profile", new { userId = userId, friendshipStatus = friendshipStatus });
        }

        public async Task<IActionResult> FriendRequests()
        {
            var model = new FriendshipServiceModel();

            var currentUserId = await this._userService
                .GetUserIdByNameAsync(User.Identity.Name);

            model.Requests = await this._friendshipService
                .GetFriendRequestsAsync(currentUserId);

            model.PendingRequests = await this._friendshipService
                .GetPendingRequestsAsync(currentUserId);

            return View(model);
        }

        public async Task<IActionResult> SendRequestAsync(string addresseeId)
        {
            if (addresseeId == null)
            {
                return NotFound();
            }

            var currentUserId = await this._userService
                .GetUserIdByNameAsync(User.Identity.Name);

            await this._friendshipService.SendRequestAsync(currentUserId, addresseeId);

            return RedirectToAction(nameof(FriendRequests));
        }

        public async Task<IActionResult> AcceptAsync(string requesterId)
        {
            var currentUserId = await this._userService
                .GetUserIdByNameAsync(User.Identity.Name);

            await this._friendshipService.AcceptRequestAsync(currentUserId, requesterId);

            return RedirectToAction(nameof(FriendRequests));
        }

        public async Task<IActionResult> RejectAsync(string requesterId)
        {
            var currentUserId = await this._userService
                .GetUserIdByNameAsync(User.Identity.Name);

            await this._friendshipService.RejectRequestAsync(currentUserId, requesterId);

            return RedirectToAction(nameof(FriendRequests));
        }

        public async Task<IActionResult> CancelInvitationAsync(string addresseeId)
        {
            var currentUserId = await this._userService
                .GetUserIdByNameAsync(User.Identity.Name);

            await this._friendshipService.CancelInvitationAsync(currentUserId, addresseeId);

            return RedirectToAction(nameof(FriendRequests));
        }

        public async Task<IActionResult> UnfriendAsync(string friendId)
        {
            var currentUserId = await this._userService
                .GetUserIdByNameAsync(User.Identity.Name);

            await this._friendshipService.UnfriendAsync(currentUserId, friendId);

            return RedirectToAction(nameof(Friends));
        }

        public async Task<JsonResult> GetUserFriendsByPartName(string partName)
        {
            var currentUserId = await this._userService
                .GetUserIdByNameAsync(User.Identity.Name);

            var friends = await this._friendshipService
                .GetFriendsByPartNameAsync(partName, currentUserId);

            return new JsonResult(friends.ToList());
        }

        public async Task<JsonResult> GetFriendById(string friendId)
        {
            var user = await this._userService
                .GetUserByIdAsync(friendId);
            
            return new JsonResult(user);
        }
    }
}