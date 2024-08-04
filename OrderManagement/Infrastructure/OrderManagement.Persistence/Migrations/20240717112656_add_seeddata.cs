using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderManagement.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class add_seeddata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Products_ProductId",
                table: "Orders");

            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: new Guid("ee0e8df1-214b-43f7-8a37-440a2cc4675b"));

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: new Guid("16500acd-9f9b-427a-b8a3-c4137cc25553"));

            migrationBuilder.DeleteData(
                table: "ProductCategories",
                keyColumn: "Id",
                keyValue: new Guid("100a965f-1271-430e-8a04-f11b9207f2be"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("dbb96cbb-88c0-42ea-906b-1268af3bb626"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("0c4b2c80-0690-4f1e-a9d4-f19319723f71"));

            migrationBuilder.InsertData(
                table: "ProductCategories",
                columns: new[] { "Id", "CreatedDate", "Description", "LastUpdateDate", "Name" },
                values: new object[] { new Guid("5e378b2e-0b75-4a4f-b4f1-881419799561"), new DateTime(2024, 7, 17, 11, 26, 56, 517, DateTimeKind.Utc).AddTicks(4376), "Bu bir örnek kategoridir", new DateTime(2024, 7, 17, 11, 26, 56, 517, DateTimeKind.Utc).AddTicks(4377), "Örnek Kategori" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedDate", "Description", "LastUpdateDate", "Name" },
                values: new object[] { new Guid("3e7d477c-7b2b-4bd1-b120-89b3f189340e"), new DateTime(2024, 7, 17, 11, 26, 56, 517, DateTimeKind.Utc).AddTicks(8411), "Bu bir örnek kullanıcıdır", new DateTime(2024, 7, 17, 11, 26, 56, 517, DateTimeKind.Utc).AddTicks(8411), "Örnek Kullanıcı" });

            migrationBuilder.InsertData(
                table: "Companies",
                columns: new[] { "Id", "CreatedDate", "Description", "LastUpdateDate", "Name", "UserId" },
                values: new object[] { new Guid("9450a960-4f4c-4967-84c7-1e9f5781dd00"), new DateTime(2024, 7, 17, 11, 26, 56, 516, DateTimeKind.Utc).AddTicks(1356), "Bu bir örnek şirkettir", new DateTime(2024, 7, 17, 11, 26, 56, 516, DateTimeKind.Utc).AddTicks(1356), "Örnek Şirket", new Guid("3e7d477c-7b2b-4bd1-b120-89b3f189340e") });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CompanyId", "CreatedDate", "Description", "LastUpdateDate", "Name", "Price", "ProductCategoryId", "StockCount" },
                values: new object[] { new Guid("9e60eb9a-9e7f-4a3d-8bbc-611bd5798a18"), new Guid("9450a960-4f4c-4967-84c7-1e9f5781dd00"), new DateTime(2024, 7, 17, 11, 26, 56, 517, DateTimeKind.Utc).AddTicks(7350), "Bu bir örnek üründür", new DateTime(2024, 7, 17, 11, 26, 56, 517, DateTimeKind.Utc).AddTicks(7351), "Örnek Ürün", 50.0m, new Guid("5e378b2e-0b75-4a4f-b4f1-881419799561"), 100 });

            migrationBuilder.InsertData(
                table: "Orders",
                columns: new[] { "Id", "CompanyId", "CreatedDate", "LastUpdateDate", "Name", "OrderCount", "OrderStatus", "ProductId", "UnitPrice", "UserId" },
                values: new object[] { new Guid("ee423d32-2a02-49c0-a70d-eeaeadfcf5c2"), new Guid("9450a960-4f4c-4967-84c7-1e9f5781dd00"), new DateTime(2024, 7, 17, 11, 26, 56, 517, DateTimeKind.Utc).AddTicks(3303), new DateTime(2024, 7, 17, 11, 26, 56, 517, DateTimeKind.Utc).AddTicks(3304), "Örnek Sipariş", 5, 0, new Guid("9e60eb9a-9e7f-4a3d-8bbc-611bd5798a18"), 10.5m, new Guid("3e7d477c-7b2b-4bd1-b120-89b3f189340e") });

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Products_ProductId",
                table: "Orders",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Products_ProductId",
                table: "Orders");

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: new Guid("ee423d32-2a02-49c0-a70d-eeaeadfcf5c2"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("9e60eb9a-9e7f-4a3d-8bbc-611bd5798a18"));

            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: new Guid("9450a960-4f4c-4967-84c7-1e9f5781dd00"));

            migrationBuilder.DeleteData(
                table: "ProductCategories",
                keyColumn: "Id",
                keyValue: new Guid("5e378b2e-0b75-4a4f-b4f1-881419799561"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("3e7d477c-7b2b-4bd1-b120-89b3f189340e"));

            migrationBuilder.InsertData(
                table: "Companies",
                columns: new[] { "Id", "CreatedDate", "Description", "LastUpdateDate", "Name", "UserId" },
                values: new object[] { new Guid("ee0e8df1-214b-43f7-8a37-440a2cc4675b"), new DateTime(2024, 7, 17, 11, 14, 12, 514, DateTimeKind.Utc).AddTicks(9569), "Bu bir örnek şirkettir", new DateTime(2024, 7, 17, 11, 14, 12, 514, DateTimeKind.Utc).AddTicks(9570), "Örnek Şirket", new Guid("bfb96147-7a94-4d2b-bade-8926c6a99c7b") });

            migrationBuilder.InsertData(
                table: "Orders",
                columns: new[] { "Id", "CompanyId", "CreatedDate", "LastUpdateDate", "Name", "OrderCount", "OrderStatus", "ProductId", "UnitPrice", "UserId" },
                values: new object[] { new Guid("16500acd-9f9b-427a-b8a3-c4137cc25553"), new Guid("dc7f7d1d-ead8-4f27-91df-5dbadcc22945"), new DateTime(2024, 7, 17, 11, 14, 12, 515, DateTimeKind.Utc).AddTicks(8127), new DateTime(2024, 7, 17, 11, 14, 12, 515, DateTimeKind.Utc).AddTicks(8127), "Örnek Sipariş", 5, 0, new Guid("00000000-0000-0000-0000-000000000000"), 10.5m, new Guid("00000000-0000-0000-0000-000000000000") });

            migrationBuilder.InsertData(
                table: "ProductCategories",
                columns: new[] { "Id", "CreatedDate", "Description", "LastUpdateDate", "Name" },
                values: new object[] { new Guid("100a965f-1271-430e-8a04-f11b9207f2be"), new DateTime(2024, 7, 17, 11, 14, 12, 515, DateTimeKind.Utc).AddTicks(9194), "Bu bir örnek kategoridir", new DateTime(2024, 7, 17, 11, 14, 12, 515, DateTimeKind.Utc).AddTicks(9195), "Örnek Kategori" });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CompanyId", "CreatedDate", "Description", "LastUpdateDate", "Name", "Price", "ProductCategoryId", "StockCount" },
                values: new object[] { new Guid("dbb96cbb-88c0-42ea-906b-1268af3bb626"), new Guid("52621b75-cd7b-4f2b-81da-93bbb48b5b01"), new DateTime(2024, 7, 17, 11, 14, 12, 516, DateTimeKind.Utc).AddTicks(2049), "Bu bir örnek üründür", new DateTime(2024, 7, 17, 11, 14, 12, 516, DateTimeKind.Utc).AddTicks(2049), "Örnek Ürün", 50.0m, new Guid("589191bc-5449-48eb-92f4-70cfe3e33dee"), 100 });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedDate", "Description", "LastUpdateDate", "Name" },
                values: new object[] { new Guid("0c4b2c80-0690-4f1e-a9d4-f19319723f71"), new DateTime(2024, 7, 17, 11, 14, 12, 516, DateTimeKind.Utc).AddTicks(3101), "Bu bir örnek kullanıcıdır", new DateTime(2024, 7, 17, 11, 14, 12, 516, DateTimeKind.Utc).AddTicks(3102), "Örnek Kullanıcı" });

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Products_ProductId",
                table: "Orders",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id");
        }
    }
}
