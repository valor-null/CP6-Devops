using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DimDim.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContaCorrente_Cliente_IdCliente",
                table: "ContaCorrente");

            migrationBuilder.DropForeignKey(
                name: "FK_Transacao_ContaCorrente_IdConta",
                table: "Transacao");

            migrationBuilder.DropIndex(
                name: "IX_Transacao_IdConta",
                table: "Transacao");

            migrationBuilder.RenameIndex(
                name: "IX_ContaCorrente_IdCliente",
                table: "ContaCorrente",
                newName: "IX_Conta_IdCliente");

            migrationBuilder.AlterColumn<string>(
                name: "Tipo",
                table: "Transacao",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DataHora",
                table: "Transacao",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "SYSUTCDATETIME()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETDATE()");

            migrationBuilder.AlterColumn<string>(
                name: "TipoConta",
                table: "ContaCorrente",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldDefaultValue: "Corrente");

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "ContaCorrente",
                type: "rowversion",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Nome",
                table: "Cliente",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Cliente",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(150)",
                oldMaxLength: 150);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DataCadastro",
                table: "Cliente",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "SYSUTCDATETIME()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETDATE()");

            migrationBuilder.AlterColumn<string>(
                name: "CPF",
                table: "Cliente",
                type: "char(11)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(11)",
                oldMaxLength: 11);

            migrationBuilder.CreateIndex(
                name: "IX_Transacao_IdConta_DataHora",
                table: "Transacao",
                columns: new[] { "IdConta", "DataHora" },
                descending: new[] { false, true });

            migrationBuilder.AddCheckConstraint(
                name: "CK_Transacao_Tipo",
                table: "Transacao",
                sql: "Tipo IN ('CREDITO','DEBITO','TRANSFERENCIA')");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Transacao_Valor",
                table: "Transacao",
                sql: "Valor > 0");

            migrationBuilder.CreateIndex(
                name: "UQ_ContaCorrente_NumeroConta",
                table: "ContaCorrente",
                column: "NumeroConta",
                unique: true);

            migrationBuilder.AddCheckConstraint(
                name: "CK_ContaCorrente_TipoConta",
                table: "ContaCorrente",
                sql: "TipoConta IN ('Corrente','Poupanca')");

            migrationBuilder.CreateIndex(
                name: "UQ_Cliente_CPF",
                table: "Cliente",
                column: "CPF",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ_Cliente_Email",
                table: "Cliente",
                column: "Email",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ContaCorrente_Cliente_IdCliente",
                table: "ContaCorrente",
                column: "IdCliente",
                principalTable: "Cliente",
                principalColumn: "IdCliente",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Transacao_ContaCorrente_IdConta",
                table: "Transacao",
                column: "IdConta",
                principalTable: "ContaCorrente",
                principalColumn: "IdConta",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContaCorrente_Cliente_IdCliente",
                table: "ContaCorrente");

            migrationBuilder.DropForeignKey(
                name: "FK_Transacao_ContaCorrente_IdConta",
                table: "Transacao");

            migrationBuilder.DropIndex(
                name: "IX_Transacao_IdConta_DataHora",
                table: "Transacao");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Transacao_Tipo",
                table: "Transacao");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Transacao_Valor",
                table: "Transacao");

            migrationBuilder.DropIndex(
                name: "UQ_ContaCorrente_NumeroConta",
                table: "ContaCorrente");

            migrationBuilder.DropCheckConstraint(
                name: "CK_ContaCorrente_TipoConta",
                table: "ContaCorrente");

            migrationBuilder.DropIndex(
                name: "UQ_Cliente_CPF",
                table: "Cliente");

            migrationBuilder.DropIndex(
                name: "UQ_Cliente_Email",
                table: "Cliente");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "ContaCorrente");

            migrationBuilder.RenameIndex(
                name: "IX_Conta_IdCliente",
                table: "ContaCorrente",
                newName: "IX_ContaCorrente_IdCliente");

            migrationBuilder.AlterColumn<string>(
                name: "Tipo",
                table: "Transacao",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(15)",
                oldMaxLength: 15);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DataHora",
                table: "Transacao",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "SYSUTCDATETIME()");

            migrationBuilder.AlterColumn<string>(
                name: "TipoConta",
                table: "ContaCorrente",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "Corrente",
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "Nome",
                table: "Cliente",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(150)",
                oldMaxLength: 150);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Cliente",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DataCadastro",
                table: "Cliente",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "SYSUTCDATETIME()");

            migrationBuilder.AlterColumn<string>(
                name: "CPF",
                table: "Cliente",
                type: "nvarchar(11)",
                maxLength: 11,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "char(11)");

            migrationBuilder.CreateIndex(
                name: "IX_Transacao_IdConta",
                table: "Transacao",
                column: "IdConta");

            migrationBuilder.AddForeignKey(
                name: "FK_ContaCorrente_Cliente_IdCliente",
                table: "ContaCorrente",
                column: "IdCliente",
                principalTable: "Cliente",
                principalColumn: "IdCliente",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transacao_ContaCorrente_IdConta",
                table: "Transacao",
                column: "IdConta",
                principalTable: "ContaCorrente",
                principalColumn: "IdConta",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
