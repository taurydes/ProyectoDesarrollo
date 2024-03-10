using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UCABPagaloTodoMS.Infrastructure.Migrations
{
    public partial class InicialEntregaII : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ArchivoDeudas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ServicioPrestadoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ConsumidorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArchivoDeudas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ConfiguracionPagos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ServicioPrestadoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfiguracionPagos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NombreUsuario = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Clave = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Correo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NombreAdministrador = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Cedula = table.Column<int>(type: "int", nullable: true),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Apellido = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Telefono = table.Column<int>(type: "int", nullable: true),
                    Direccion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Consumidor_EstatusCuenta = table.Column<bool>(type: "bit", nullable: true),
                    ArchivoDeudaId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    NombreEmpresa = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Rif = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EstatusCuenta = table.Column<bool>(type: "bit", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Campos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NombreCampo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tipo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Longitud = table.Column<int>(type: "int", nullable: true),
                    Requerido = table.Column<bool>(type: "bit", nullable: false),
                    ConfiguracionPagoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Campos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Campos_ConfiguracionPagos_ConfiguracionPagoId",
                        column: x => x.ConfiguracionPagoId,
                        principalTable: "ConfiguracionPagos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ServiciosPrestados",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PrestadorServicioId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ConfiguracionPagoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Costo = table.Column<float>(type: "real", nullable: false),
                    EstadoServicio = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TipoPago = table.Column<bool>(type: "bit", nullable: false),
                    EstatusServicio = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiciosPrestados", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServiciosPrestados_Usuarios_PrestadorServicioId",
                        column: x => x.PrestadorServicioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ArchivoConciliacions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ServicioPrestadoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Confirmado = table.Column<bool>(type: "bit", nullable: false),
                    UrlDescarga = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArchivoConciliacions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArchivoConciliacions_ServiciosPrestados_ServicioPrestadoId",
                        column: x => x.ServicioPrestadoId,
                        principalTable: "ServiciosPrestados",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Pagos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ConsumidorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ServicioPrestadoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Referencia = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EstadoPago = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pagos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pagos_ServiciosPrestados_ServicioPrestadoId",
                        column: x => x.ServicioPrestadoId,
                        principalTable: "ServiciosPrestados",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Pagos_Usuarios_ConsumidorId",
                        column: x => x.ConsumidorId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onUpdate: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CamposPago",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PagoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NombreCampo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tipo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Longitud = table.Column<int>(type: "int", nullable: true),
                    Valor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CamposPago", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CamposPago_Pagos_PagoId",
                        column: x => x.PagoId,
                        principalTable: "Pagos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArchivoConciliacions_ServicioPrestadoId",
                table: "ArchivoConciliacions",
                column: "ServicioPrestadoId");

            migrationBuilder.CreateIndex(
                name: "IX_Campos_ConfiguracionPagoId",
                table: "Campos",
                column: "ConfiguracionPagoId");

            migrationBuilder.CreateIndex(
                name: "IX_CamposPago_PagoId",
                table: "CamposPago",
                column: "PagoId");

            migrationBuilder.CreateIndex(
                name: "IX_Pagos_ConsumidorId",
                table: "Pagos",
                column: "ConsumidorId");

            migrationBuilder.CreateIndex(
                name: "IX_Pagos_ServicioPrestadoId",
                table: "Pagos",
                column: "ServicioPrestadoId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiciosPrestados_PrestadorServicioId",
                table: "ServiciosPrestados",
                column: "PrestadorServicioId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArchivoConciliacions");

            migrationBuilder.DropTable(
                name: "ArchivoDeudas");

            migrationBuilder.DropTable(
                name: "Campos");

            migrationBuilder.DropTable(
                name: "CamposPago");

            migrationBuilder.DropTable(
                name: "ConfiguracionPagos");

            migrationBuilder.DropTable(
                name: "Pagos");

            migrationBuilder.DropTable(
                name: "ServiciosPrestados");

            migrationBuilder.DropTable(
                name: "Usuarios");
        }
    }
}
