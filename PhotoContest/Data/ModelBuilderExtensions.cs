using Microsoft.EntityFrameworkCore;
using PhotoContest.Models;
using System;
using System.Collections.Generic;

namespace PhotoContest.Data
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            var roles = new List<Role>();
            roles.Add(new Role() { Id = 1, Name = "admin" });
            roles.Add(new Role() { Id = 2, Name = "junkie" });

            modelBuilder.Entity<Role>().HasData(roles);

            var categories = new List<Category>();
            categories.Add(new Category() { Id = 1, Name = "Nature" });
            categories.Add(new Category() { Id = 2, Name = "Flora" });
            categories.Add(new Category() { Id = 3, Name = "Human" });
            categories.Add(new Category() { Id = 4, Name = "Story" });

            modelBuilder.Entity<Category>().HasData(categories);

            // Creating Contests

            var contests = new List<Contest>();
            contests.Add(new Contest() { Id = 1, 
                Title = "Art of Nature", 
                CategoryId = 1, 
                IsOpen = true,
                Phase1Start = new DateTime(2015, 5, 15, 13, 45, 0),
                Phase2Start = new DateTime(2015, 5, 15, 13, 45, 0).AddDays(29),
                EndDate = new DateTime(2015, 5, 15, 13, 45, 0).AddDays(29).AddHours(23),
                PhaseName = Models.Enums.PhaseEnum.Finished });
            contests.Add(new Contest() { 
                Id = 2, 
                Title = "Art of Human", 
                CategoryId = 2, 
                IsOpen = false, 
                Phase1Start = DateTime.Now, 
                Phase2Start = DateTime.Now.AddDays(20), 
                EndDate = DateTime.Now.AddDays(20).AddHours(20), 
                PhaseName = Models.Enums.PhaseEnum.Finished });
            contests.Add(new Contest() { 
                Id = 3, 
                Title = "Colourful Flowers", 
                CategoryId = 1, 
                IsOpen = true,
                Phase1Start = new DateTime(2015, 5, 15, 13, 45, 0),
                Phase2Start = new DateTime(2015, 5, 15, 13, 45, 0).AddDays(15),
                EndDate = new DateTime(2015, 5, 15, 13, 45, 0).AddDays(16),
                PhaseName = Models.Enums.PhaseEnum.Finished });
            contests.Add(new Contest() { 
                Id = 4, 
                Title = "Vladi's Dog", 
                CategoryId = 1, 
                IsOpen = true,
                Phase1Start = new DateTime(2020, 5, 15, 13, 45, 0),
                Phase2Start = new DateTime(2020, 5, 15, 13, 45, 0).AddDays(15),
                EndDate = new DateTime(2020, 5, 15, 13, 45, 0).AddDays(16),
                PhaseName = Models.Enums.PhaseEnum.Finished });
            contests.Add(new Contest() {
                Id = 5,
                Title = "Nature Landscapes",
                CategoryId = 1,
                IsOpen = true,
                Phase1Start = DateTime.Now,
                Phase2Start = DateTime.Now.AddDays(29),
                EndDate = DateTime.Now.AddDays(29).AddHours(23),
                PhaseName = Models.Enums.PhaseEnum.One });

            modelBuilder.Entity<Contest>().HasData(contests);

            // Creating Users

            var user1 = new User()
            {
                Id = 1,
                FirstName = "Georgi",
                LastName = "Vachev",
                Username = "joro",
                Email = "joro@example.com",
                RoleId = 1
            };
            CreatePasswordHash("12345", out byte[] passwordHash1, out byte[] passwordSalt1);
            user1.PasswordHash = passwordHash1;
            user1.PasswordSalt = passwordSalt1;

            var user2 = new User()
            {
                Id = 2,
                FirstName = "Vladimir",
                LastName = "Panev",
                Username = "vladi",
                Email = "vladi@example.com",
                RoleId = 1
            };
            CreatePasswordHash("12345", out byte[] passwordHash2, out byte[] passwordSalt2);
            user2.PasswordHash = passwordHash2;
            user2.PasswordSalt = passwordSalt2;

            var user3 = new User()
            {
                Id = 3,
                FirstName = "Valentina",
                LastName = "Patova",
                Username = "valia",
                Email = "valia@example.com",
                RoleId = 1
            };
            CreatePasswordHash("12345", out byte[] passwordHash3, out byte[] passwordSalt3);
            user3.PasswordHash = passwordHash3;
            user3.PasswordSalt = passwordSalt3;

            var user4 = new User()
            {
                Id = 4,
                FirstName = "Barak",
                LastName = "Obama",
                Username = "president",
                Email = "barak@example.com",
                RoleId = 2
            };
            CreatePasswordHash("12345", out byte[] passwordHash4, out byte[] passwordSalt4);
            user4.PasswordHash = passwordHash4;
            user4.PasswordSalt = passwordSalt4;

            var user5 = new User()
            {
                Id = 5,
                FirstName = "Vladimir",
                LastName = "Putin",
                Username = "russia",
                Email = "putin@example.com",
                RoleId = 2
            };
            CreatePasswordHash("12345", out byte[] passwordHash5, out byte[] passwordSalt5);
            user5.PasswordHash = passwordHash5;
            user5.PasswordSalt = passwordSalt5;

            var user6 = new User()
            {
                Id = 6,
                FirstName = "Valeri",
                LastName = "Bojinov",
                Username = "catlover",
                Email = "catlover@example.com",
                RoleId = 2
            };
            CreatePasswordHash("12345", out byte[] passwordHash6, out byte[] passwordSalt6);
            user6.PasswordHash = passwordHash6;
            user6.PasswordSalt = passwordSalt6;

            var user7 = new User()
            {
                Id = 7,
                FirstName = "Michael",
                LastName = "Jackson",
                Username = "jako",
                Email = "popking@example.com",
                RoleId = 2
            };
            CreatePasswordHash("12345", out byte[] passwordHash7, out byte[] passwordSalt7);
            user7.PasswordHash = passwordHash7;
            user7.PasswordSalt = passwordSalt7;

            var user8 = new User()
            {
                Id = 8,
                FirstName = "Dimitur",
                LastName = "Penev",
                Username = "stratega",
                Email = "strategy442@example.com",
                RoleId = 2
            };
            CreatePasswordHash("12345", out byte[] passwordHash8, out byte[] passwordSalt8);
            user8.PasswordHash = passwordHash8;
            user8.PasswordSalt = passwordSalt8;

            var user9 = new User()
            {
                Id = 9,
                FirstName = "Tony",
                LastName = "Stark",
                Username = "irin",
                Email = "ironman@example.com",
                RoleId = 2
            };
            CreatePasswordHash("12345", out byte[] passwordHash9, out byte[] passwordSalt9);
            user9.PasswordHash = passwordHash9;
            user9.PasswordSalt = passwordSalt9;

            var user10 = new User()
            {
                Id = 10,
                FirstName = "Dalai",
                LastName = "Lama",
                Username = "peace",
                Email = "peacemaker@example.com",
                RoleId = 2
            };
            CreatePasswordHash("12345", out byte[] passwordHash10, out byte[] passwordSalt10);
            user10.PasswordHash = passwordHash10;
            user10.PasswordSalt = passwordSalt10;

            var user11 = new User()
            {
                Id = 11,
                FirstName = "Tina",
                LastName = "Turner",
                Username = "tina67",
                Email = "tina.t@example.com",
                RoleId = 2
            };
            CreatePasswordHash("12345", out byte[] passwordHash11, out byte[] passwordSalt11);
            user11.PasswordHash = passwordHash11;
            user11.PasswordSalt = passwordSalt11;

            var user12 = new User()
            {
                Id = 12,
                FirstName = "Krasi",
                LastName = "Radkov",
                Username = "bace",
                Email = "severozapad@example.com",
                RoleId = 2
            };
            CreatePasswordHash("12345", out byte[] passwordHash12, out byte[] passwordSalt12);
            user12.PasswordHash = passwordHash12;
            user12.PasswordSalt = passwordSalt12;

            var user13 = new User()
            {
                Id = 13,
                FirstName = "Kiril",
                LastName = "Master",
                Username = "kiro",
                Email = "kiril@example.com",
                RoleId = 2
            };
            CreatePasswordHash("12345", out byte[] passwordHash13, out byte[] passwordSalt13);
            user13.PasswordHash = passwordHash13;
            user13.PasswordSalt = passwordSalt13;

            var user14 = new User()
            {
                Id = 14,
                FirstName = "Chuck",
                LastName = "Norris",
                Username = "chuknorris",
                Email = "chuknorris@example.com",
                RoleId = 2
            };
            CreatePasswordHash("12345", out byte[] passwordHash14, out byte[] passwordSalt14);
            user14.PasswordHash = passwordHash14;
            user14.PasswordSalt = passwordSalt14;

            var users = new List<User>();
            users.Add(user1);
            users.Add(user2);
            users.Add(user3);
            users.Add(user4);
            users.Add(user5);
            users.Add(user6);
            users.Add(user7);
            users.Add(user8);
            users.Add(user9);
            users.Add(user10);
            users.Add(user11);
            users.Add(user12);
            users.Add(user13);
            users.Add(user14);

            modelBuilder.Entity<User>().HasData(users);

            // Creating PhotoPosts for Contest 1

            var photo1 = new PhotoPost()
            {
                Id = 1,
                Title = "Purple landscape",
                Story = "It was a beautiful sunset and I was on the right spot.",
                //Url = "",
                Url = @"https://res.cloudinary.com/doifgnlmu/image/upload/v1655922726/PhotoContest/mark-harpur-K2s_YE031CA-unsplash_j6j5x5.jpg",
                ImageName = "Landscape",
                ImageFile=default,
                IsDeleted = false,
                UserId = 4,
                ContestId = 1
            };

            var photo2 = new PhotoPost()
            {
                Id = 2,
                Title = "By the sea",
                Story = "When you are out and see this...",
                Url = @"https://res.cloudinary.com/doifgnlmu/image/upload/v1655922722/PhotoContest/pascal-debrunner-1WQ5RZuH9xo-unsplash_x69y9x.jpg",
                ImageFile = default,
                ImageName = "Landscape",
                IsDeleted = false,
                UserId = 5,
                ContestId = 1
            };

            var photo3 = new PhotoPost()
            {
                Id = 3,
                Title = "In the mountain",
                Story = "You just have to stop adn look at this.",
                Url = @"https://res.cloudinary.com/doifgnlmu/image/upload/v1655922720/PhotoContest/bailey-zindel-NRQV-hBF10M-unsplash_bfjumk.jpg",
                ImageFile = default,
                ImageName = "Landscape",
                IsDeleted = false,
                UserId = 6,
                ContestId = 1
            };

            var photo4 = new PhotoPost()
            {
                Id = 4,
                Title = "Beautiful landscape",
                Story = "It is like a fairytail when you walk through places like this one.",
                Url = @"https://res.cloudinary.com/doifgnlmu/image/upload/v1655922719/PhotoContest/hendrik-cornelissen--qrcOR33ErA-unsplash_ahabg0.jpg",
                ImageFile = default,
                ImageName = "Landscape",
                IsDeleted = false,
                UserId = 7,
                ContestId = 1
            };

            var photo5 = new PhotoPost()
            {
                Id = 5,
                Title = "By the lake",
                Story = "Sometimes I just want to liva on a place like this.",
                Url = @"https://res.cloudinary.com/doifgnlmu/image/upload/v1655922719/PhotoContest/luca-bravo-zAjdgNXsMeg-unsplash_xw0gag.jpg",
                ImageFile = default,
                ImageName = "Landscape",
                IsDeleted = false,
                UserId = 8,
                ContestId = 1
            };

            var photo6 = new PhotoPost()
            {
                Id = 6,
                Title = "Sunny view",
                Story = "This i actually nearby where I live and when I have a photo with me I just have to catch it.",
                Url = @"https://res.cloudinary.com/doifgnlmu/image/upload/v1655922716/PhotoContest/jasper-boer-LJD6U920zVo-unsplash_qx9x3m.jpg",
                ImageFile = default,
                ImageName = "Landscape",
                IsDeleted = false,
                UserId = 9,
                ContestId = 1
            };

            var photo7 = new PhotoPost()
            {
                Id = 7,
                Title = "Purple sunset",
                Story = "I went to the beach and fell in love in what I saw.",
                Url = @"https://res.cloudinary.com/doifgnlmu/image/upload/v1655922716/PhotoContest/rodrigo-soares-c6SciRp2kaQ-unsplash_n2y8sv.jpg",
                ImageFile = default,
                ImageName = "Landscape",
                IsDeleted = false,
                UserId = 10,
                ContestId = 1
            };

            var photo8 = new PhotoPost()
            {
                Id = 8,
                Title = "Chill",
                Story = "This time I took my photo to the mauntain instead of the ski equipment and this is the result.",
                Url = @"https://res.cloudinary.com/doifgnlmu/image/upload/v1655922714/PhotoContest/danyu-wang-sR7_ImYvt1Q-unsplash_phcvru.jpg",
                ImageFile = default,
                ImageName = "Landscape",
                IsDeleted = false,
                UserId = 11,
                ContestId = 1
            };

            var photo9 = new PhotoPost()
            {
                Id = 9,
                Title = "Sunday sunset",
                Story = "Everyone loves to go the seaside and I never miss my photo in case I see something like this.",
                Url = @"https://res.cloudinary.com/doifgnlmu/image/upload/v1655922715/PhotoContest/christian-joudrey-DuD5D3lWC3c-unsplash_ipdl1q.jpg",
                ImageFile = default,
                ImageName = "Landscape",
                IsDeleted = false,
                UserId = 12,
                ContestId = 1
            };

            var photo10 = new PhotoPost()
            {
                Id = 10,
                Title = "Sharana lake",
                Story = "This is where I met my wife and I aways come here in the sprin... sommetimes witha photo too.",
                Url = @"https://res.cloudinary.com/doifgnlmu/image/upload/v1655922715/PhotoContest/ken-cheung-KonWFWUaAuk-unsplash_nu3p9g.jpg",
                ImageFile = default,
                ImageName = "Landscape",
                IsDeleted = false,
                UserId = 14,
                ContestId = 1
            };

            var photoPosts = new List<PhotoPost>();
            photoPosts.Add(photo1);
            photoPosts.Add(photo2);
            photoPosts.Add(photo3);
            photoPosts.Add(photo4);
            photoPosts.Add(photo5);
            photoPosts.Add(photo6);
            photoPosts.Add(photo7);
            photoPosts.Add(photo8);
            photoPosts.Add(photo9);
            photoPosts.Add(photo10);

            modelBuilder.Entity<PhotoPost>().HasData(photoPosts);


            // Creating Jury for Contest 1

            var contestUser1 = new ContestUser()
            {
                Id = 1,
                UserId = 1,
                ContestId = 1,
                IsJury = true,
                IsDeleted = false
            };

            var contestUser2 = new ContestUser()
            {
                Id = 2,
                UserId = 2,
                ContestId = 1,
                IsJury = true,
                IsDeleted = false
            };

            var contestUser3 = new ContestUser()
            {
                Id = 3,
                UserId = 3,
                ContestId = 1,
                IsJury = true,
                IsDeleted = false
            };

            var contestUser4 = new ContestUser()
            {
                Id = 4,
                UserId = 4,
                ContestId = 1,
                IsJury = false,
                IsDeleted = false
            };

            var contestUser5 = new ContestUser()
            {
                Id = 5,
                UserId = 5,
                ContestId = 1,
                IsJury = false,
                IsDeleted = false
            };

            var contestUser6 = new ContestUser()
            {
                Id = 6,
                UserId = 6,
                ContestId = 1,
                IsJury = false,
                IsDeleted = false
            };

            var contestUser7 = new ContestUser()
            {
                Id = 7,
                UserId = 7,
                ContestId = 1,
                IsJury = false,
                IsDeleted = false
            };

            var contestUser8 = new ContestUser()
            {
                Id = 8,
                UserId = 8,
                ContestId = 1,
                IsJury = false,
                IsDeleted = false
            };

            var contestUser9 = new ContestUser()
            {
                Id = 9,
                UserId = 9,
                ContestId = 1,
                IsJury = false,
                IsDeleted = false
            };

            var contestUser10 = new ContestUser()
            {
                Id = 10,
                UserId = 10,
                ContestId = 1,
                IsJury = false,
                IsDeleted = false
            };

            var contestUser11 = new ContestUser()
            {
                Id = 11,
                UserId = 11,
                ContestId = 1,
                IsJury = false,
                IsDeleted = false
            };

            var contestUser12 = new ContestUser()
            {
                Id = 12,
                UserId = 12,
                ContestId = 1,
                IsJury = false,
                IsDeleted = false
            };

            var contestUser13 = new ContestUser()
            {
                Id = 13,
                UserId = 13,
                ContestId = 1,
                IsJury = false,
                IsDeleted = false
            };

            var contestUsers = new List<ContestUser>();
            contestUsers.Add(contestUser1);
            contestUsers.Add(contestUser2);
            contestUsers.Add(contestUser3);
            contestUsers.Add(contestUser4);
            contestUsers.Add(contestUser5);
            contestUsers.Add(contestUser6);
            contestUsers.Add(contestUser7);
            contestUsers.Add(contestUser8);
            contestUsers.Add(contestUser9);
            contestUsers.Add(contestUser10);
            contestUsers.Add(contestUser11);
            contestUsers.Add(contestUser12);
            contestUsers.Add(contestUser13);

            modelBuilder.Entity<ContestUser>().HasData(contestUsers);

        }



        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt) //Create the password with HMACSHA512 hash method
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512()) //Generate a new passwordsalt
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
    }
}
