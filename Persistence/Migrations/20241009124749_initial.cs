using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
#pragma warning disable CS8981 // O nome do tipo contém apenas caracteres ascii em caixa baixa. Esses nomes podem ficar reservados para o idioma.
    public partial class initial : Migration
#pragma warning restore CS8981 // O nome do tipo contém apenas caracteres ascii em caixa baixa. Esses nomes podem ficar reservados para o idioma.
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "entregador",
                columns: table => new
                {
                    identificador = table.Column<string>(type: "text", nullable: false),
                    nome = table.Column<string>(type: "text", nullable: false),
                    cnpj = table.Column<string>(type: "text", nullable: false),
                    data_nascimento = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    numero_cnh = table.Column<string>(type: "text", nullable: false),
                    tipo_cnh = table.Column<string>(type: "text", nullable: false),
                    imagem_cnh_s3 = table.Column<string>(type: "text", nullable: false),
                    data_criada = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_entregador", x => x.identificador);
                });

            migrationBuilder.CreateTable(
                name: "locacao",
                columns: table => new
                {
                    identificador = table.Column<string>(type: "text", nullable: false),
                    entregador_id = table.Column<string>(type: "text", nullable: false),
                    moto_id = table.Column<string>(type: "text", nullable: false),
                    data_inicio = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    data_termino = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    data_previsao_termino = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    plano = table.Column<int>(type: "integer", nullable: false),
                    data_devolucao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    valor_diaria = table.Column<double>(type: "double precision", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_locacao", x => x.identificador);
                });

            migrationBuilder.CreateTable(
                name: "moto",
                columns: table => new
                {
                    identificador = table.Column<string>(type: "text", nullable: false),
                    ano = table.Column<int>(type: "integer", nullable: false),
                    modelo = table.Column<string>(type: "text", nullable: false),
                    placa = table.Column<string>(type: "text", nullable: false),
                    data_criada = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    data_atualizada = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_moto", x => x.identificador);
                });

            migrationBuilder.CreateIndex(
                name: "IX_entregador_identificador",
                table: "entregador",
                column: "identificador",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_entregador_numero_cnh",
                table: "entregador",
                column: "numero_cnh",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_moto_placa",
                table: "moto",
                column: "placa",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "entregador");

            migrationBuilder.DropTable(
                name: "locacao");

            migrationBuilder.DropTable(
                name: "moto");
        }
    }
}
