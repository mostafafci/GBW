﻿using GBW.Controllers;
using GBW.DataAccess;
using GBW.Models;
using GBW.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace GBW.Service
{
    public class UsersService : Repository<ApplicationUser>
    {
        private readonly ApplicationDbContext _context;
        public UsersService(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public ApplicationUser GetLoginByEmail(string email)
        {
            return dbSet.FirstOrDefault(s => s.Email == email);
        }

        public ApplicationUser GetByEmail(string email)
        {
            return dbSet.FirstOrDefault(s => s.Email == email);
        }

        public ApplicationUser GetUserById(string UserId)
        {
            return dbSet.FirstOrDefault(s => s.Id == UserId);
        }

        public ApplicationUser ActiveUser(string UserId)
        {
            var user= dbSet.FirstOrDefault(s => s.Id == UserId);
            user.IsActive = true;
            _context.SaveChanges();
            return user;
        }

        public ApplicationUser DisActiveUser(string UserId)
        {
            var user = dbSet.FirstOrDefault(s => s.Id == UserId);
            user.IsActive = false;
            _context.SaveChanges();
            return user;
        }

        public ApplicationUser UpdateUserImage(string UserId,string Image)
        {
            var user = dbSet.FirstOrDefault(s => s.Id == UserId);
            user.Image = Image;
            _context.SaveChanges();
            return user;
        }

        public List<UsersListViewModel> GetUsersList()
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(_context));
            List<string> Super_Role = roleManager.FindByName("SuperAdmin").Users.ToList().Select(x=>x.UserId).ToList();
            return dbSet.Where(x=> !Super_Role.Contains(x.Id)).ToList().Select(l=>new UsersListViewModel() {
                Id=l.Id,
                Email=l.Email,
                Phone=l.PhoneNumber,
                Image=l.Image,
                Statues=l.IsActive
            }).ToList();

        }


        public UsersDataViewModel GetUserDataDetails(string UserId)
        {
            var user= dbSet.Where(x =>x.Id==UserId && x.IsActive==true).ToList().Select(l => new UsersDataViewModel()
            {
                Id = l.Id,
                Email = l.Email,
                Phone = l.PhoneNumber,
                ImageBase64 = l.Image,
                ReferralLink = l.ReferralLink,
                Statues = l.IsActive
            }).FirstOrDefault();
            user.ReferralLink = ReturnFullReferralLink(user.Email);
            return user;
        }

        public string ReturnFullReferralLink(string Email)
        {
            List<string> Lst = Email.Split('@').ToList();
            if (Lst.Count > 0)
            {
                string Domain = WebConfigurationManager.AppSettings["ApplicationFrontDomain"];
                return Domain + Lst.FirstOrDefault();
            }
            return null;
        }

        public List<UserInvitedTree> GetUserReferralByUserId(string UserId)
        {
            var user = dbSet.FirstOrDefault(s => s.Id == UserId && s.IsActive==true);
            if (user == null)
            {
                return null;
            }
            UserInvitedTree ParentItem = new UserInvitedTree();
            ParentItem.ChildUserId = user.Id;
            ParentItem.ChildUserEmail = user.Email;
            ParentItem.ParnetUserId = user.InvitedUserId;

            List<UserInvitedTree> parents = new List<UserInvitedTree>();
            List<UserInvitedTree> Users = new List<UserInvitedTree>();
            parents.Add(ParentItem);
            Users.Add(ParentItem);
            return getUserRefferralChilds(parents,ref Users);
        }

        private List<UserInvitedTree> getUserRefferralChilds(List<UserInvitedTree> parents, ref List<UserInvitedTree> Users)
        {
            if (parents.Count > 0)
            {
                List<UserInvitedTree> paLst = new List<UserInvitedTree>();
                foreach (var par in parents)
                {
                    var parent = dbSet.Where(s => s.InvitedUserId == par.ChildUserId && s.IsActive == true).ToList();
                    if (parent.Count > 0)
                    {
                        paLst.AddRange(parent.Select(x=>new UserInvitedTree() { ChildUserId=x.Id,ChildUserEmail=x.Email,ParnetUserId=x.InvitedUserId}).ToList());
                        Users.AddRange(parent.Select(x => new UserInvitedTree() { ChildUserId = x.Id, ChildUserEmail = x.Email, ParnetUserId = x.InvitedUserId }).ToList());
                    }
                }
                getUserRefferralChilds(paLst, ref Users);
            }
            return Users;
        }

    }

    public class UserInvitedTree
    {
        public string ChildUserId { get; set; }
        public string ChildUserEmail { get; set; }
        public string ParnetUserId { get; set; }
    }

}