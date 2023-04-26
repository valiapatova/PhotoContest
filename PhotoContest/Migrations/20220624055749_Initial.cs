using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PhotoContest.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Contests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    IsOpen = table.Column<bool>(type: "bit", nullable: false),
                    Phase1Start = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Phase2Start = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PhaseName = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Contests_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PictureUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PictureName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Rank = table.Column<int>(type: "int", nullable: false),
                    RankPoints = table.Column<int>(type: "int", nullable: false),
                    PasswordHash = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    PasswordSalt = table.Column<byte[]>(type: "varbinary(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ContestUser",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ContestId = table.Column<int>(type: "int", nullable: false),
                    IsJury = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContestUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContestUser_Contests_ContestId",
                        column: x => x.ContestId,
                        principalTable: "Contests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ContestUser_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PhotoPosts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Story = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ContestId = table.Column<int>(type: "int", nullable: false),
                    TotalRating = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhotoPosts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PhotoPosts_Contests_ContestId",
                        column: x => x.ContestId,
                        principalTable: "Contests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PhotoPosts_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Ratings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RatingValue = table.Column<int>(type: "int", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    PhotoPostId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ratings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ratings_PhotoPosts_PhotoPostId",
                        column: x => x.PhotoPostId,
                        principalTable: "PhotoPosts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Ratings_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Nature" },
                    { 2, "Flora" },
                    { 3, "Human" },
                    { 4, "Story" }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "admin" },
                    { 2, "junkie" }
                });

            migrationBuilder.InsertData(
                table: "Contests",
                columns: new[] { "Id", "CategoryId", "EndDate", "IsDeleted", "IsOpen", "Phase1Start", "Phase2Start", "PhaseName", "Title" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2015, 6, 14, 12, 45, 0, 0, DateTimeKind.Unspecified), false, true, new DateTime(2015, 5, 15, 13, 45, 0, 0, DateTimeKind.Unspecified), new DateTime(2015, 6, 13, 13, 45, 0, 0, DateTimeKind.Unspecified), 3, "Art of Nature" },
                    { 3, 1, new DateTime(2015, 5, 31, 13, 45, 0, 0, DateTimeKind.Unspecified), false, true, new DateTime(2015, 5, 15, 13, 45, 0, 0, DateTimeKind.Unspecified), new DateTime(2015, 5, 30, 13, 45, 0, 0, DateTimeKind.Unspecified), 3, "Colourful Flowers" },
                    { 4, 1, new DateTime(2020, 5, 31, 13, 45, 0, 0, DateTimeKind.Unspecified), false, true, new DateTime(2020, 5, 15, 13, 45, 0, 0, DateTimeKind.Unspecified), new DateTime(2020, 5, 30, 13, 45, 0, 0, DateTimeKind.Unspecified), 3, "Vladi's Dog" },
                    { 5, 1, new DateTime(2022, 7, 24, 7, 57, 49, 196, DateTimeKind.Local).AddTicks(3333), false, true, new DateTime(2022, 6, 24, 8, 57, 49, 196, DateTimeKind.Local).AddTicks(3328), new DateTime(2022, 7, 23, 8, 57, 49, 196, DateTimeKind.Local).AddTicks(3331), 1, "Nature Landscapes" },
                    { 2, 2, new DateTime(2022, 7, 15, 4, 57, 49, 196, DateTimeKind.Local).AddTicks(3315), false, false, new DateTime(2022, 6, 24, 8, 57, 49, 194, DateTimeKind.Local).AddTicks(4775), new DateTime(2022, 7, 14, 8, 57, 49, 196, DateTimeKind.Local).AddTicks(3300), 3, "Art of Human" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "FirstName", "IsDeleted", "LastName", "PasswordHash", "PasswordSalt", "PictureName", "PictureUrl", "Rank", "RankPoints", "RoleId", "Username" },
                values: new object[,]
                {
                    { 12, "severozapad@example.com", "Krasi", false, "Radkov", new byte[] { 16, 48, 141, 8, 71, 72, 147, 130, 198, 186, 100, 83, 154, 182, 236, 139, 206, 46, 180, 170, 13, 252, 76, 177, 237, 149, 196, 149, 124, 194, 215, 227, 206, 209, 217, 61, 149, 134, 89, 58, 50, 92, 13, 138, 135, 66, 144, 119, 13, 11, 150, 127, 32, 135, 175, 65, 236, 72, 132, 114, 46, 247, 62, 138 }, new byte[] { 90, 130, 80, 24, 123, 17, 18, 212, 210, 79, 153, 250, 122, 24, 170, 192, 255, 95, 252, 53, 207, 73, 132, 42, 186, 198, 228, 58, 131, 101, 131, 230, 210, 61, 53, 157, 146, 52, 174, 5, 172, 150, 35, 173, 77, 0, 113, 82, 9, 132, 13, 201, 119, 161, 139, 29, 79, 249, 248, 81, 150, 41, 152, 143, 32, 190, 165, 42, 10, 250, 216, 165, 94, 145, 131, 114, 13, 136, 192, 65, 224, 227, 0, 10, 68, 10, 144, 193, 253, 106, 37, 253, 106, 36, 125, 216, 238, 165, 4, 129, 92, 218, 217, 156, 219, 88, 37, 155, 7, 88, 192, 186, 15, 0, 152, 174, 255, 81, 84, 68, 27, 172, 54, 224, 43, 132, 158, 107 }, null, null, 0, 0, 2, "bace" },
                    { 11, "tina.t@example.com", "Tina", false, "Turner", new byte[] { 241, 191, 170, 46, 249, 7, 106, 152, 218, 178, 43, 33, 90, 67, 90, 181, 34, 56, 173, 52, 204, 131, 66, 61, 161, 68, 74, 221, 112, 103, 129, 62, 225, 152, 23, 201, 232, 88, 228, 65, 79, 244, 87, 103, 97, 81, 112, 165, 22, 123, 198, 153, 3, 170, 100, 247, 222, 129, 48, 197, 87, 5, 13, 40 }, new byte[] { 54, 92, 57, 12, 15, 102, 186, 52, 238, 246, 90, 177, 223, 4, 18, 187, 75, 95, 52, 59, 207, 237, 162, 234, 236, 7, 6, 189, 138, 154, 145, 221, 222, 194, 38, 154, 98, 19, 161, 31, 172, 199, 5, 100, 194, 97, 195, 113, 84, 146, 154, 183, 233, 227, 172, 162, 235, 161, 44, 20, 182, 212, 108, 60, 37, 142, 102, 202, 229, 139, 138, 19, 38, 211, 247, 193, 94, 239, 101, 182, 245, 107, 213, 4, 23, 136, 184, 121, 213, 227, 182, 208, 90, 19, 167, 181, 155, 92, 76, 197, 129, 63, 239, 117, 32, 56, 9, 100, 159, 104, 129, 87, 185, 160, 189, 220, 202, 225, 56, 63, 49, 156, 168, 72, 61, 175, 139, 64 }, null, null, 0, 0, 2, "tina67" },
                    { 10, "peacemaker@example.com", "Dalai", false, "Lama", new byte[] { 114, 174, 76, 11, 34, 225, 203, 177, 5, 103, 19, 160, 61, 238, 36, 81, 117, 10, 21, 18, 95, 69, 208, 29, 14, 242, 225, 120, 205, 154, 253, 125, 204, 32, 81, 118, 107, 163, 172, 225, 146, 71, 50, 94, 178, 196, 20, 47, 213, 69, 171, 41, 97, 126, 30, 238, 57, 155, 183, 77, 206, 104, 62, 202 }, new byte[] { 171, 170, 25, 158, 175, 38, 220, 71, 103, 193, 132, 183, 56, 150, 42, 229, 67, 251, 117, 137, 30, 238, 122, 235, 88, 105, 12, 70, 164, 240, 244, 229, 108, 160, 128, 178, 61, 163, 42, 4, 247, 0, 107, 84, 15, 239, 127, 100, 129, 244, 186, 166, 192, 99, 103, 232, 108, 191, 2, 181, 145, 183, 197, 24, 82, 165, 26, 145, 93, 117, 160, 161, 166, 49, 91, 53, 189, 84, 172, 253, 209, 220, 3, 133, 56, 22, 212, 251, 128, 139, 112, 118, 64, 160, 75, 178, 196, 205, 106, 132, 80, 159, 124, 91, 240, 32, 57, 149, 54, 34, 66, 213, 108, 237, 162, 36, 227, 126, 12, 162, 0, 110, 247, 166, 238, 123, 147, 24 }, null, null, 0, 0, 2, "peace" },
                    { 9, "ironman@example.com", "Tony", false, "Stark", new byte[] { 152, 208, 190, 76, 19, 73, 6, 128, 79, 30, 83, 209, 91, 122, 215, 103, 33, 90, 49, 102, 22, 9, 226, 169, 10, 200, 102, 14, 224, 74, 56, 150, 206, 87, 61, 94, 67, 218, 114, 41, 193, 180, 195, 97, 127, 173, 92, 254, 209, 189, 230, 199, 213, 94, 45, 245, 24, 99, 182, 2, 25, 30, 97, 107 }, new byte[] { 191, 126, 134, 8, 157, 124, 126, 11, 136, 87, 230, 38, 67, 219, 153, 3, 98, 139, 140, 94, 132, 0, 54, 47, 61, 45, 40, 245, 184, 28, 251, 13, 60, 54, 178, 231, 28, 62, 167, 151, 141, 125, 136, 239, 79, 29, 144, 196, 237, 215, 9, 0, 142, 59, 117, 51, 67, 204, 205, 76, 252, 170, 166, 38, 250, 251, 55, 241, 116, 161, 79, 199, 158, 148, 86, 152, 101, 176, 123, 16, 28, 89, 249, 119, 176, 119, 205, 249, 194, 109, 244, 228, 14, 96, 239, 31, 240, 101, 212, 203, 122, 28, 9, 67, 109, 150, 67, 10, 174, 142, 40, 28, 79, 159, 132, 139, 239, 188, 87, 73, 129, 140, 205, 57, 0, 1, 38, 186 }, null, null, 0, 0, 2, "irin" },
                    { 8, "strategy442@example.com", "Dimitur", false, "Penev", new byte[] { 46, 68, 99, 217, 168, 219, 178, 156, 211, 238, 1, 151, 196, 56, 204, 112, 187, 157, 126, 150, 238, 104, 105, 236, 205, 253, 199, 142, 176, 70, 227, 219, 85, 87, 104, 68, 91, 67, 27, 137, 166, 147, 151, 133, 66, 83, 187, 177, 203, 41, 110, 216, 206, 167, 236, 139, 139, 107, 188, 48, 187, 74, 77, 209 }, new byte[] { 242, 247, 88, 23, 238, 223, 241, 12, 174, 132, 28, 134, 249, 205, 68, 3, 223, 122, 89, 126, 45, 204, 216, 84, 81, 176, 244, 161, 128, 174, 246, 239, 109, 42, 97, 168, 137, 175, 198, 96, 242, 20, 7, 45, 152, 19, 55, 135, 70, 73, 251, 244, 240, 171, 177, 124, 243, 125, 180, 209, 53, 26, 16, 38, 158, 17, 53, 147, 109, 224, 210, 138, 154, 12, 28, 179, 19, 151, 207, 224, 74, 246, 130, 114, 67, 38, 91, 244, 181, 254, 240, 171, 208, 57, 225, 50, 187, 225, 235, 39, 82, 21, 188, 176, 145, 103, 207, 135, 123, 144, 179, 30, 35, 48, 250, 237, 181, 85, 200, 87, 89, 164, 167, 193, 185, 91, 238, 157 }, null, null, 0, 0, 2, "stratega" },
                    { 7, "popking@example.com", "Michael", false, "Jackson", new byte[] { 49, 35, 239, 221, 202, 3, 151, 197, 252, 246, 129, 11, 121, 169, 13, 25, 92, 109, 253, 210, 87, 241, 238, 234, 76, 25, 236, 248, 151, 129, 115, 61, 5, 164, 111, 220, 127, 140, 2, 60, 67, 145, 5, 62, 138, 158, 57, 154, 13, 171, 77, 97, 184, 174, 203, 199, 36, 21, 39, 146, 215, 44, 40, 183 }, new byte[] { 17, 156, 248, 185, 28, 120, 206, 190, 214, 170, 120, 98, 104, 157, 173, 47, 228, 2, 233, 252, 136, 44, 26, 110, 194, 141, 56, 128, 62, 1, 143, 39, 48, 118, 118, 156, 162, 154, 61, 234, 9, 162, 47, 145, 86, 123, 184, 198, 162, 206, 115, 109, 158, 129, 80, 142, 68, 158, 189, 0, 18, 191, 175, 200, 134, 65, 201, 59, 207, 145, 180, 137, 84, 2, 27, 153, 156, 229, 168, 191, 175, 176, 136, 199, 205, 240, 247, 118, 149, 194, 105, 100, 104, 248, 24, 37, 218, 128, 8, 135, 234, 236, 177, 105, 47, 187, 99, 1, 100, 195, 245, 70, 212, 165, 125, 58, 248, 36, 193, 203, 131, 25, 104, 197, 128, 250, 11, 0 }, null, null, 0, 0, 2, "jako" },
                    { 5, "putin@example.com", "Vladimir", false, "Putin", new byte[] { 19, 155, 245, 10, 114, 144, 220, 71, 99, 240, 181, 127, 210, 15, 68, 222, 141, 62, 196, 119, 205, 119, 221, 54, 198, 168, 32, 176, 251, 64, 246, 28, 116, 18, 169, 148, 29, 106, 182, 10, 183, 111, 79, 45, 62, 29, 183, 150, 115, 46, 7, 126, 130, 139, 24, 239, 12, 207, 13, 161, 20, 139, 152, 19 }, new byte[] { 43, 230, 151, 121, 250, 70, 4, 70, 92, 119, 112, 94, 61, 51, 207, 6, 201, 126, 105, 252, 107, 2, 33, 121, 56, 150, 97, 148, 156, 104, 237, 187, 217, 81, 246, 102, 27, 100, 175, 10, 116, 102, 50, 1, 107, 147, 118, 11, 205, 94, 26, 74, 221, 162, 0, 53, 21, 197, 238, 236, 195, 178, 55, 15, 66, 41, 196, 47, 187, 37, 19, 184, 4, 210, 240, 89, 253, 222, 226, 247, 21, 222, 12, 28, 92, 159, 229, 168, 241, 15, 3, 210, 137, 169, 23, 183, 148, 55, 225, 187, 142, 36, 53, 212, 55, 20, 220, 5, 140, 57, 228, 120, 230, 157, 157, 199, 123, 168, 62, 249, 103, 45, 7, 95, 2, 74, 127, 133 }, null, null, 0, 0, 2, "russia" },
                    { 13, "kiril@example.com", "Kiril", false, "Master", new byte[] { 221, 113, 47, 182, 188, 68, 207, 235, 181, 188, 49, 152, 244, 116, 63, 107, 58, 201, 42, 206, 69, 250, 132, 99, 103, 132, 124, 25, 115, 79, 37, 94, 111, 235, 203, 13, 202, 122, 26, 212, 153, 126, 128, 99, 117, 3, 44, 0, 59, 254, 8, 53, 103, 219, 47, 180, 103, 165, 31, 145, 228, 82, 211, 161 }, new byte[] { 175, 194, 147, 98, 200, 239, 156, 153, 57, 180, 64, 39, 184, 221, 183, 39, 158, 168, 209, 241, 121, 213, 57, 88, 158, 96, 68, 178, 140, 151, 109, 116, 167, 37, 135, 140, 97, 5, 243, 242, 130, 5, 38, 216, 183, 59, 158, 6, 224, 90, 6, 117, 92, 44, 152, 35, 56, 136, 8, 116, 235, 128, 137, 48, 243, 92, 144, 156, 106, 192, 229, 220, 238, 145, 163, 101, 126, 244, 175, 247, 18, 193, 169, 222, 235, 22, 170, 197, 143, 110, 230, 55, 249, 106, 42, 154, 2, 189, 112, 5, 9, 75, 42, 89, 92, 180, 219, 72, 128, 251, 194, 168, 236, 39, 16, 142, 90, 158, 191, 118, 248, 28, 205, 30, 55, 140, 223, 191 }, null, null, 0, 0, 2, "kiro" },
                    { 4, "barak@example.com", "Barak", false, "Obama", new byte[] { 43, 125, 63, 75, 123, 228, 164, 251, 6, 219, 29, 204, 102, 10, 161, 235, 131, 220, 217, 74, 80, 23, 2, 191, 46, 63, 29, 120, 118, 182, 119, 202, 74, 74, 49, 162, 131, 45, 179, 198, 37, 48, 97, 99, 82, 143, 230, 154, 16, 69, 6, 111, 253, 197, 33, 119, 110, 33, 169, 241, 13, 81, 40, 90 }, new byte[] { 3, 131, 112, 85, 167, 62, 107, 111, 80, 36, 36, 84, 98, 144, 164, 243, 130, 97, 11, 100, 127, 156, 17, 180, 169, 106, 70, 104, 80, 137, 191, 34, 19, 175, 110, 101, 52, 109, 84, 237, 200, 11, 209, 55, 30, 32, 10, 76, 19, 104, 108, 168, 138, 201, 131, 154, 119, 118, 233, 240, 74, 7, 167, 22, 108, 62, 252, 169, 169, 102, 228, 87, 198, 191, 133, 20, 5, 234, 10, 159, 76, 152, 228, 13, 162, 94, 200, 242, 158, 213, 216, 220, 215, 251, 215, 84, 133, 254, 214, 93, 101, 85, 226, 192, 60, 125, 74, 65, 229, 133, 186, 79, 130, 137, 248, 100, 70, 219, 86, 18, 118, 43, 172, 135, 251, 133, 172, 5 }, null, null, 0, 0, 2, "president" },
                    { 3, "valia@example.com", "Valentina", false, "Patova", new byte[] { 168, 142, 154, 208, 206, 35, 113, 216, 223, 186, 250, 231, 10, 49, 114, 114, 191, 30, 97, 123, 148, 234, 126, 191, 31, 62, 165, 148, 55, 237, 33, 122, 222, 22, 233, 0, 241, 36, 46, 212, 132, 241, 0, 45, 53, 44, 56, 166, 56, 166, 191, 223, 144, 186, 247, 46, 225, 105, 248, 146, 142, 252, 198, 143 }, new byte[] { 63, 80, 209, 71, 63, 71, 90, 84, 122, 63, 146, 192, 176, 183, 187, 143, 13, 101, 68, 182, 189, 74, 50, 160, 171, 8, 129, 161, 37, 138, 102, 211, 36, 165, 155, 94, 111, 76, 228, 219, 162, 0, 118, 109, 112, 187, 8, 184, 86, 251, 39, 128, 141, 232, 189, 8, 212, 239, 224, 224, 46, 24, 143, 94, 146, 255, 1, 76, 186, 239, 199, 248, 134, 201, 167, 203, 135, 182, 40, 77, 23, 159, 229, 94, 79, 100, 106, 23, 87, 88, 199, 216, 79, 24, 16, 138, 160, 62, 15, 9, 48, 157, 111, 125, 3, 8, 44, 87, 68, 199, 225, 176, 85, 93, 209, 71, 108, 131, 57, 90, 13, 53, 239, 99, 145, 84, 31, 123 }, null, null, 0, 0, 1, "valia" },
                    { 2, "vladi@example.com", "Vladimir", false, "Panev", new byte[] { 165, 102, 50, 170, 110, 232, 242, 176, 25, 11, 147, 35, 197, 170, 214, 207, 146, 174, 135, 156, 130, 18, 2, 114, 216, 7, 48, 242, 102, 33, 118, 67, 232, 96, 159, 220, 168, 166, 0, 144, 165, 133, 80, 73, 102, 39, 188, 247, 132, 63, 141, 80, 226, 197, 193, 228, 230, 112, 61, 216, 11, 252, 197, 51 }, new byte[] { 196, 80, 23, 122, 252, 249, 205, 41, 72, 88, 127, 200, 157, 221, 79, 162, 201, 178, 25, 115, 253, 167, 171, 27, 62, 159, 6, 64, 185, 196, 243, 95, 122, 63, 166, 201, 145, 182, 255, 126, 230, 1, 12, 179, 185, 19, 47, 17, 44, 55, 168, 182, 59, 44, 22, 221, 139, 95, 17, 209, 177, 19, 233, 176, 124, 24, 247, 8, 62, 216, 151, 77, 81, 248, 247, 160, 58, 37, 232, 229, 57, 125, 219, 222, 115, 112, 76, 115, 212, 169, 170, 215, 57, 103, 114, 134, 56, 244, 136, 121, 253, 157, 31, 57, 124, 182, 212, 59, 4, 213, 140, 42, 135, 118, 85, 250, 242, 250, 155, 132, 12, 88, 207, 49, 181, 70, 175, 249 }, null, null, 0, 0, 1, "vladi" },
                    { 1, "joro@example.com", "Georgi", false, "Vachev", new byte[] { 148, 227, 166, 35, 15, 74, 151, 46, 179, 136, 213, 169, 158, 92, 244, 54, 59, 18, 130, 21, 114, 3, 17, 65, 227, 59, 6, 16, 65, 167, 15, 71, 99, 202, 114, 207, 31, 217, 169, 48, 130, 98, 234, 178, 48, 26, 219, 219, 174, 28, 128, 13, 175, 42, 74, 185, 108, 118, 160, 97, 147, 222, 176, 189 }, new byte[] { 52, 142, 195, 251, 139, 35, 51, 155, 180, 99, 202, 147, 75, 176, 141, 183, 15, 132, 78, 217, 92, 19, 49, 202, 168, 86, 41, 87, 5, 128, 31, 179, 239, 169, 51, 234, 211, 233, 255, 23, 160, 191, 230, 239, 34, 121, 116, 126, 133, 237, 91, 186, 229, 246, 88, 255, 191, 180, 29, 32, 215, 71, 8, 2, 162, 70, 32, 236, 33, 139, 31, 56, 168, 198, 30, 146, 216, 91, 34, 34, 20, 70, 81, 26, 215, 165, 8, 123, 148, 89, 101, 32, 47, 249, 216, 100, 94, 45, 198, 154, 164, 53, 10, 194, 218, 105, 3, 67, 84, 148, 164, 198, 193, 108, 23, 101, 204, 114, 228, 209, 67, 63, 165, 48, 220, 193, 246, 147 }, null, null, 0, 0, 1, "joro" },
                    { 6, "catlover@example.com", "Valeri", false, "Bojinov", new byte[] { 251, 29, 212, 14, 54, 153, 239, 38, 241, 61, 84, 152, 73, 32, 100, 173, 56, 222, 207, 208, 91, 32, 18, 174, 12, 243, 227, 34, 18, 80, 206, 188, 87, 82, 23, 19, 114, 164, 106, 130, 104, 47, 189, 64, 249, 223, 196, 217, 109, 238, 38, 219, 93, 90, 160, 82, 154, 87, 204, 157, 179, 55, 214, 204 }, new byte[] { 136, 31, 182, 107, 20, 247, 27, 174, 197, 227, 77, 88, 39, 221, 82, 1, 18, 56, 87, 38, 115, 189, 41, 94, 137, 76, 57, 27, 196, 254, 115, 213, 99, 140, 206, 177, 29, 3, 41, 189, 219, 95, 55, 151, 151, 169, 80, 60, 211, 85, 91, 211, 61, 49, 104, 107, 94, 42, 54, 40, 197, 179, 71, 112, 44, 49, 98, 147, 227, 234, 100, 115, 186, 224, 203, 186, 99, 84, 97, 230, 56, 210, 250, 7, 233, 26, 41, 7, 202, 94, 100, 109, 93, 53, 236, 103, 193, 13, 141, 207, 155, 242, 238, 7, 165, 61, 96, 30, 46, 141, 221, 216, 123, 68, 227, 63, 87, 109, 208, 23, 124, 217, 186, 82, 72, 134, 225, 207 }, null, null, 0, 0, 2, "catlover" },
                    { 14, "chuknorris@example.com", "Chuck", false, "Norris", new byte[] { 40, 31, 111, 155, 201, 86, 56, 27, 63, 239, 83, 28, 3, 137, 14, 50, 167, 17, 149, 202, 15, 149, 215, 132, 56, 79, 147, 230, 100, 77, 147, 113, 44, 110, 145, 159, 215, 221, 13, 192, 175, 166, 206, 78, 168, 71, 149, 243, 227, 22, 48, 61, 241, 176, 60, 44, 151, 84, 163, 33, 159, 198, 21, 76 }, new byte[] { 246, 101, 171, 197, 218, 93, 17, 20, 247, 163, 86, 230, 63, 125, 217, 129, 201, 126, 102, 23, 207, 87, 55, 3, 188, 22, 107, 251, 224, 158, 29, 33, 53, 97, 6, 141, 74, 255, 39, 8, 89, 1, 249, 30, 25, 60, 165, 139, 123, 55, 42, 153, 225, 148, 212, 35, 223, 229, 128, 101, 155, 71, 9, 59, 76, 250, 17, 98, 41, 78, 95, 137, 162, 101, 133, 118, 80, 41, 241, 38, 33, 203, 14, 72, 0, 164, 195, 53, 66, 195, 55, 12, 195, 212, 150, 208, 16, 18, 23, 56, 26, 54, 207, 189, 134, 137, 181, 43, 111, 19, 124, 169, 76, 231, 192, 239, 247, 123, 166, 71, 18, 33, 237, 174, 106, 212, 210, 39 }, null, null, 0, 0, 2, "chuknorris" }
                });

            migrationBuilder.InsertData(
                table: "ContestUser",
                columns: new[] { "Id", "ContestId", "IsDeleted", "IsJury", "UserId" },
                values: new object[,]
                {
                    { 1, 1, false, true, 1 },
                    { 12, 1, false, false, 12 },
                    { 11, 1, false, false, 11 },
                    { 10, 1, false, false, 10 },
                    { 9, 1, false, false, 9 },
                    { 13, 1, false, false, 13 },
                    { 7, 1, false, false, 7 },
                    { 8, 1, false, false, 8 },
                    { 5, 1, false, false, 5 },
                    { 4, 1, false, false, 4 },
                    { 3, 1, false, true, 3 },
                    { 2, 1, false, true, 2 },
                    { 6, 1, false, false, 6 }
                });

            migrationBuilder.InsertData(
                table: "PhotoPosts",
                columns: new[] { "Id", "ContestId", "ImageName", "IsDeleted", "Story", "Title", "TotalRating", "Url", "UserId" },
                values: new object[,]
                {
                    { 3, 1, "Landscape", false, "You just have to stop adn look at this.", "In the mountain", 0.0, "https://res.cloudinary.com/doifgnlmu/image/upload/v1655922720/PhotoContest/bailey-zindel-NRQV-hBF10M-unsplash_bfjumk.jpg", 6 },
                    { 2, 1, "Landscape", false, "When you are out and see this...", "By the sea", 0.0, "https://res.cloudinary.com/doifgnlmu/image/upload/v1655922722/PhotoContest/pascal-debrunner-1WQ5RZuH9xo-unsplash_x69y9x.jpg", 5 },
                    { 4, 1, "Landscape", false, "It is like a fairytail when you walk through places like this one.", "Beautiful landscape", 0.0, "https://res.cloudinary.com/doifgnlmu/image/upload/v1655922719/PhotoContest/hendrik-cornelissen--qrcOR33ErA-unsplash_ahabg0.jpg", 7 },
                    { 5, 1, "Landscape", false, "Sometimes I just want to liva on a place like this.", "By the lake", 0.0, "https://res.cloudinary.com/doifgnlmu/image/upload/v1655922719/PhotoContest/luca-bravo-zAjdgNXsMeg-unsplash_xw0gag.jpg", 8 },
                    { 1, 1, "Landscape", false, "It was a beautiful sunset and I was on the right spot.", "Purple landscape", 0.0, "https://res.cloudinary.com/doifgnlmu/image/upload/v1655922726/PhotoContest/mark-harpur-K2s_YE031CA-unsplash_j6j5x5.jpg", 4 },
                    { 6, 1, "Landscape", false, "This i actually nearby where I live and when I have a photo with me I just have to catch it.", "Sunny view", 0.0, "https://res.cloudinary.com/doifgnlmu/image/upload/v1655922716/PhotoContest/jasper-boer-LJD6U920zVo-unsplash_qx9x3m.jpg", 9 },
                    { 7, 1, "Landscape", false, "I went to the beach and fell in love in what I saw.", "Purple sunset", 0.0, "https://res.cloudinary.com/doifgnlmu/image/upload/v1655922716/PhotoContest/rodrigo-soares-c6SciRp2kaQ-unsplash_n2y8sv.jpg", 10 },
                    { 8, 1, "Landscape", false, "This time I took my photo to the mauntain instead of the ski equipment and this is the result.", "Chill", 0.0, "https://res.cloudinary.com/doifgnlmu/image/upload/v1655922714/PhotoContest/danyu-wang-sR7_ImYvt1Q-unsplash_phcvru.jpg", 11 },
                    { 9, 1, "Landscape", false, "Everyone loves to go the seaside and I never miss my photo in case I see something like this.", "Sunday sunset", 0.0, "https://res.cloudinary.com/doifgnlmu/image/upload/v1655922715/PhotoContest/christian-joudrey-DuD5D3lWC3c-unsplash_ipdl1q.jpg", 12 },
                    { 10, 1, "Landscape", false, "This is where I met my wife and I aways come here in the sprin... sommetimes witha photo too.", "Sharana lake", 0.0, "https://res.cloudinary.com/doifgnlmu/image/upload/v1655922715/PhotoContest/ken-cheung-KonWFWUaAuk-unsplash_nu3p9g.jpg", 14 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Contests_CategoryId",
                table: "Contests",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ContestUser_ContestId",
                table: "ContestUser",
                column: "ContestId");

            migrationBuilder.CreateIndex(
                name: "IX_ContestUser_UserId",
                table: "ContestUser",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PhotoPosts_ContestId",
                table: "PhotoPosts",
                column: "ContestId");

            migrationBuilder.CreateIndex(
                name: "IX_PhotoPosts_UserId",
                table: "PhotoPosts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_PhotoPostId",
                table: "Ratings",
                column: "PhotoPostId");

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_UserId",
                table: "Ratings",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                table: "Users",
                column: "RoleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContestUser");

            migrationBuilder.DropTable(
                name: "Ratings");

            migrationBuilder.DropTable(
                name: "PhotoPosts");

            migrationBuilder.DropTable(
                name: "Contests");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
