using OnlineShopCore.Data.Enums;
using OnlineShopCore.Data.Interfaces;
using OnlineShopCore.Infrastructure.SharedKernel;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineShopCore.Data.Entities
{
    public class VoteLog : DomainEntity<int>
    {
        public int VoteForId { get; set; }

        public string UserName { get; set; }

        public float Vote { get; set; }

        public VoteLog()
        {

        }
        public VoteLog(int voteForId, string username, float vote)
        {
            VoteForId = voteForId;
            UserName = username;
            Vote = vote;
        }

        public VoteLog(int id, int voteForId, string username, float vote)
        {
            Id = id;
            VoteForId = voteForId;
            UserName = username;
            Vote = vote;
        }

    }
}
