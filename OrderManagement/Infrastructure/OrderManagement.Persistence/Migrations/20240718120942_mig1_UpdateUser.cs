using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderManagement.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class mig1_UpdateUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AuthenticatorType",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Users",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Users",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "Users",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<byte[]>(
                name: "PasswordHash",
                table: "Users",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "PasswordSalt",
                table: "Users",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<bool>(
                name: "Status",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: new Guid("9450a960-4f4c-4967-84c7-1e9f5781dd00"),
                columns: new[] { "CreatedDate", "LastUpdateDate" },
                values: new object[] { new DateTime(2024, 7, 18, 12, 9, 42, 6, DateTimeKind.Utc).AddTicks(7318), new DateTime(2024, 7, 18, 12, 9, 42, 6, DateTimeKind.Utc).AddTicks(7318) });

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: new Guid("ee423d32-2a02-49c0-a70d-eeaeadfcf5c2"),
                columns: new[] { "CreatedDate", "LastUpdateDate" },
                values: new object[] { new DateTime(2024, 7, 18, 12, 9, 42, 8, DateTimeKind.Utc).AddTicks(8346), new DateTime(2024, 7, 18, 12, 9, 42, 8, DateTimeKind.Utc).AddTicks(8347) });

            migrationBuilder.UpdateData(
                table: "ProductCategories",
                keyColumn: "Id",
                keyValue: new Guid("5e378b2e-0b75-4a4f-b4f1-881419799561"),
                columns: new[] { "CreatedDate", "LastUpdateDate" },
                values: new object[] { new DateTime(2024, 7, 18, 12, 9, 42, 9, DateTimeKind.Utc).AddTicks(157), new DateTime(2024, 7, 18, 12, 9, 42, 9, DateTimeKind.Utc).AddTicks(158) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("9e60eb9a-9e7f-4a3d-8bbc-611bd5798a18"),
                columns: new[] { "CreatedDate", "LastUpdateDate" },
                values: new object[] { new DateTime(2024, 7, 18, 12, 9, 42, 9, DateTimeKind.Utc).AddTicks(5328), new DateTime(2024, 7, 18, 12, 9, 42, 9, DateTimeKind.Utc).AddTicks(5328) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("3e7d477c-7b2b-4bd1-b120-89b3f189340e"),
                columns: new[] { "AuthenticatorType", "CreatedDate", "Email", "FirstName", "LastName", "LastUpdateDate", "PasswordHash", "PasswordSalt", "Status" },
                values: new object[] { 0, new DateTime(2024, 7, 18, 12, 9, 42, 9, DateTimeKind.Utc).AddTicks(7325), "Test@gmail.com", "Test", "Test", new DateTime(2024, 7, 18, 12, 9, 42, 9, DateTimeKind.Utc).AddTicks(7326), new byte[0], new byte[0], false });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AuthenticatorType",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PasswordHash",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PasswordSalt",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Users");

            migrationBuilder.UpdateData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: new Guid("9450a960-4f4c-4967-84c7-1e9f5781dd00"),
                columns: new[] { "CreatedDate", "LastUpdateDate" },
                values: new object[] { new DateTime(2024, 7, 17, 11, 26, 56, 516, DateTimeKind.Utc).AddTicks(1356), new DateTime(2024, 7, 17, 11, 26, 56, 516, DateTimeKind.Utc).AddTicks(1356) });

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: new Guid("ee423d32-2a02-49c0-a70d-eeaeadfcf5c2"),
                columns: new[] { "CreatedDate", "LastUpdateDate" },
                values: new object[] { new DateTime(2024, 7, 17, 11, 26, 56, 517, DateTimeKind.Utc).AddTicks(3303), new DateTime(2024, 7, 17, 11, 26, 56, 517, DateTimeKind.Utc).AddTicks(3304) });

            migrationBuilder.UpdateData(
                table: "ProductCategories",
                keyColumn: "Id",
                keyValue: new Guid("5e378b2e-0b75-4a4f-b4f1-881419799561"),
                columns: new[] { "CreatedDate", "LastUpdateDate" },
                values: new object[] { new DateTime(2024, 7, 17, 11, 26, 56, 517, DateTimeKind.Utc).AddTicks(4376), new DateTime(2024, 7, 17, 11, 26, 56, 517, DateTimeKind.Utc).AddTicks(4377) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("9e60eb9a-9e7f-4a3d-8bbc-611bd5798a18"),
                columns: new[] { "CreatedDate", "LastUpdateDate" },
                values: new object[] { new DateTime(2024, 7, 17, 11, 26, 56, 517, DateTimeKind.Utc).AddTicks(7350), new DateTime(2024, 7, 17, 11, 26, 56, 517, DateTimeKind.Utc).AddTicks(7351) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("3e7d477c-7b2b-4bd1-b120-89b3f189340e"),
                columns: new[] { "CreatedDate", "LastUpdateDate" },
                values: new object[] { new DateTime(2024, 7, 17, 11, 26, 56, 517, DateTimeKind.Utc).AddTicks(8411), new DateTime(2024, 7, 17, 11, 26, 56, 517, DateTimeKind.Utc).AddTicks(8411) });
        }
    }
}
