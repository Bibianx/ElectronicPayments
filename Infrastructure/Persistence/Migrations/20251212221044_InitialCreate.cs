using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ElectronicPayments.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EPAYCO",
                columns: table => new
                {
                    id_pago = table.Column<Guid>(type: "uuid", nullable: false),
                    ref_payco = table.Column<int>(type: "integer", nullable: false),
                    factura = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    descripcion = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    valor = table.Column<decimal>(type: "numeric(16,2)", nullable: false),
                    estado = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    transactionID = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    token = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EPAYCO", x => x.id_pago);
                });

            migrationBuilder.CreateTable(
                name: "INTENTOSZP",
                columns: table => new
                {
                    id_pago = table.Column<Guid>(type: "uuid", nullable: false),
                    flt_total_con_iva = table.Column<decimal>(type: "numeric(16,2)", nullable: false),
                    flt_valor_iva = table.Column<decimal>(type: "numeric(16,2)", nullable: false),
                    str_id_pago = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    str_descripcion_pago = table.Column<string>(type: "character varying(70)", maxLength: 70, nullable: false),
                    str_email = table.Column<string>(type: "character varying(70)", maxLength: 70, nullable: true),
                    str_id_cliente = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    str_tipo_id = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: true),
                    str_nombre_cliente = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    str_apellido_cliente = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    str_telefono_cliente = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    str_opcional1 = table.Column<string>(type: "character varying(70)", maxLength: 70, nullable: true),
                    str_opcional2 = table.Column<string>(type: "character varying(70)", maxLength: 70, nullable: true),
                    str_opcional3 = table.Column<string>(type: "character varying(70)", maxLength: 70, nullable: true),
                    str_opcional4 = table.Column<string>(type: "character varying(70)", maxLength: 70, nullable: true),
                    str_opcional5 = table.Column<string>(type: "character varying(70)", maxLength: 70, nullable: true),
                    int_id_comercio = table.Column<int>(type: "integer", nullable: false),
                    str_usuario = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    str_clave = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    int_modalidad = table.Column<int>(type: "integer", nullable: false),
                    numero_ticket_cobol = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    estado_intento = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: true),
                    fecha_intento = table.Column<DateOnly>(type: "date", nullable: true),
                    hora_intento = table.Column<TimeOnly>(type: "time without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_INTENTOSZP", x => x.id_pago);
                });

            migrationBuilder.CreateTable(
                name: "RECAUDOSBANCOS",
                columns: table => new
                {
                    id_recaudo = table.Column<Guid>(type: "uuid", nullable: false),
                    codigo_recaudo = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    id_transaccion = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    total_recaudo = table.Column<decimal>(type: "numeric(16,2)", nullable: false),
                    json_notificacion_recaudo = table.Column<string>(type: "jsonb", nullable: true),
                    es_reversado = table.Column<bool>(type: "boolean", nullable: false),
                    codigo_comercio_recaudo = table.Column<int>(type: "integer", nullable: false),
                    codigo_proveedor_recaudo = table.Column<int>(type: "integer", nullable: false),
                    codigo_estado_recaudo = table.Column<int>(type: "integer", nullable: false),
                    fecha_recaudo = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    fecha_reverso_recaudo = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    estado_recaudo_cobol = table.Column<string>(type: "text", nullable: true),
                    detalle_recaudo_cobol = table.Column<string>(type: "text", nullable: true),
                    recaudo_contabilizado_ok = table.Column<bool>(type: "boolean", nullable: true),
                    ticket_cobol_ok = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RECAUDOSBANCOS", x => x.id_recaudo);
                });

            migrationBuilder.CreateTable(
                name: "HISTORIALZP",
                columns: table => new
                {
                    id_historial = table.Column<Guid>(type: "uuid", nullable: false),
                    cod_estado_ini = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    descrip_estado_ini = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    cod_estado_fin = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    descrip_estado_fin = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    detalle_pago = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    origen_cambio = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    json_respuesta = table.Column<string>(type: "jsonb", nullable: true),
                    fecha_ini = table.Column<DateOnly>(type: "date", nullable: true),
                    fecha_fin = table.Column<DateOnly>(type: "date", nullable: true),
                    hora_ini = table.Column<TimeOnly>(type: "time without time zone", nullable: true),
                    hora_fin = table.Column<TimeOnly>(type: "time without time zone", nullable: true),
                    contabilizado_cobol_ok = table.Column<bool>(type: "boolean", nullable: true),
                    ticket_cobol_ok = table.Column<bool>(type: "boolean", nullable: true),
                    estado_cobol_ini = table.Column<string>(type: "text", nullable: true),
                    detalle_cobol_ini = table.Column<string>(type: "text", nullable: true),
                    estado_cobol_fin = table.Column<string>(type: "text", nullable: true),
                    detalle_cobol_fin = table.Column<string>(type: "text", nullable: true),
                    id_pago = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HISTORIALZP", x => x.id_historial);
                    table.ForeignKey(
                        name: "FK_HISTORIALZP_INTENTOSZP_id_pago",
                        column: x => x.id_pago,
                        principalTable: "INTENTOSZP",
                        principalColumn: "id_pago",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EPAYCO_transactionID",
                table: "EPAYCO",
                column: "transactionID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HISTORIALZP_id_pago",
                table: "HISTORIALZP",
                column: "id_pago");

            migrationBuilder.CreateIndex(
                name: "IX_INTENTOSZP_str_id_pago",
                table: "INTENTOSZP",
                column: "str_id_pago");

            migrationBuilder.CreateIndex(
                name: "IX_RECAUDOSBANCOS_codigo_recaudo",
                table: "RECAUDOSBANCOS",
                column: "codigo_recaudo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EPAYCO");

            migrationBuilder.DropTable(
                name: "HISTORIALZP");

            migrationBuilder.DropTable(
                name: "RECAUDOSBANCOS");

            migrationBuilder.DropTable(
                name: "INTENTOSZP");
        }
    }
}
