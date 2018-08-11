using System;
using System.Collections.Generic;
using System.Text;
using Lab4.BLL.Models;
using Lab4.BLL.Constants;
using System.Data.SqlClient;
using Dapper;
using Lab4.BLL.Utils;

namespace Lab4.BLL.Services
{
    public class FriendService
    {
        protected readonly string ConnectionString;

        public FriendService(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public FriendRequest SendFriendRequest(int fromUserId, int toUserId)
        {
            return SendFriendRequest(new FriendRequest() {
                FromUserId = fromUserId,
                ToUserId = toUserId
            });
        }

        public FriendRequest SendFriendRequest(FriendRequest friendRequest)
        {
            friendRequest.StateId = FriendRequestStateConstants.New;

            using (var db = new SocialNetDbContext(ConnectionString)) {
                db.FriendRequests.Add(friendRequest);
                db.SaveChanges();
            }

            return friendRequest;
        }

        public UserFriend AcceptFriendRequest(FriendRequest friendRequest)
        {
            UserFriend userFriend = null;
            using (var db = new SocialNetDbContext(ConnectionString)) {
                db.FriendRequests.Attach(friendRequest);
                friendRequest.StateId = FriendRequestStateConstants.Accepted;

                userFriend = db.UserFriends.Add(new UserFriend {
                    UserId = friendRequest.FromUserId,
                    FriendId = friendRequest.ToUserId
                }).Entity;

                db.UserFriends.Add(new UserFriend {
                    UserId = friendRequest.ToUserId,
                    FriendId = friendRequest.FromUserId
                });

                db.SaveChanges();
            }

            return userFriend;
        }

        public FriendRequest PostponeFriendRequest(FriendRequest friendRequest)
        {
            using (var db = new SocialNetDbContext(ConnectionString)) {
                db.FriendRequests.Attach(friendRequest);
                friendRequest.StateId = FriendRequestStateConstants.Postponed;
                db.SaveChanges();
            }

            return friendRequest;
        }

        public FriendRequest RejectFriendRequest(FriendRequest friendRequest)
        {
            using (var db = new SocialNetDbContext(ConnectionString)) {
                db.FriendRequests.Attach(friendRequest);
                friendRequest.StateId = FriendRequestStateConstants.Rejected;
                db.SaveChanges();
            }

            return friendRequest;
        }

        public void RemoveFriend(UserFriend userFriend)
        {
            using (var db = new SqlConnection(ConnectionString)) {
                db.ExecuteScalar(
                    @"delete from UserFriend
                    where UserId = @UserId and FriendId = @FriendId
                        or UserId = @FriendId and FriendId = @UserId",
                    new {UserId = userFriend.UserId, FriendId = userFriend.FriendId});
            }
        }
    }
}