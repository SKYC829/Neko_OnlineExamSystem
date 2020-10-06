using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.Data.EntityFrameworkCore.Metadata;

namespace Neko.Data.Migrations
{
    public partial class InitDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tb_Menu",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    ParentId = table.Column<int>(nullable: false),
                    MenuIndex = table.Column<int>(nullable: false),
                    MenuName = table.Column<string>(nullable: true),
                    JumpUrl = table.Column<string>(nullable: true),
                    Remark = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tb_Menu", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tb_RoleMenu",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    RoleId = table.Column<int>(nullable: false),
                    MenuId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tb_RoleMenu", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tb_RoleMenu_Tb_Menu_MenuId",
                        column: x => x.MenuId,
                        principalTable: "Tb_Menu",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tb_User",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    WorkId = table.Column<string>(nullable: true),
                    UserName = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    Password_Hash = table.Column<string>(nullable: true),
                    RoleId = table.Column<int>(nullable: false),
                    IsRemove = table.Column<bool>(nullable: false),
                    IsLock = table.Column<bool>(nullable: false),
                    Remark = table.Column<string>(nullable: true),
                    CreateUserId = table.Column<int>(nullable: false),
                    CreateDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tb_User", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tb_User_Tb_User_CreateUserId",
                        column: x => x.CreateUserId,
                        principalTable: "Tb_User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tb_Role",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    RoleName = table.Column<string>(nullable: true),
                    RoleType = table.Column<int>(nullable: false),
                    IsRemove = table.Column<bool>(nullable: false),
                    Remark = table.Column<string>(nullable: true),
                    CreateUserId = table.Column<int>(nullable: false),
                    CreateDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tb_Role", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tb_Role_Tb_User_CreateUserId",
                        column: x => x.CreateUserId,
                        principalTable: "Tb_User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tb_Role_CreateUserId",
                table: "Tb_Role",
                column: "CreateUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Tb_RoleMenu_MenuId",
                table: "Tb_RoleMenu",
                column: "MenuId");

            migrationBuilder.CreateIndex(
                name: "IX_Tb_RoleMenu_RoleId",
                table: "Tb_RoleMenu",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Tb_User_CreateUserId",
                table: "Tb_User",
                column: "CreateUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Tb_User_RoleId",
                table: "Tb_User",
                column: "RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tb_RoleMenu_Tb_Role_RoleId",
                table: "Tb_RoleMenu",
                column: "RoleId",
                principalTable: "Tb_Role",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tb_User_Tb_Role_RoleId",
                table: "Tb_User",
                column: "RoleId",
                principalTable: "Tb_Role",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tb_Role_Tb_User_CreateUserId",
                table: "Tb_Role");

            migrationBuilder.DropTable(
                name: "Tb_RoleMenu");

            migrationBuilder.DropTable(
                name: "Tb_Menu");

            migrationBuilder.DropTable(
                name: "Tb_User");

            migrationBuilder.DropTable(
                name: "Tb_Role");
        }
    }
}
