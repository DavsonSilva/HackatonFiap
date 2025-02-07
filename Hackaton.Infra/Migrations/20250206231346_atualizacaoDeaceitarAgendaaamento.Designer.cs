﻿// <auto-generated />
using System;
using Hackaton.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Hackaton.Infra.Migrations
{
    [DbContext(typeof(FiapDbContext))]
    [Migration("20250206231346_atualizacaoDeaceitarAgendaaamento")]
    partial class atualizacaoDeaceitarAgendaaamento
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Hackaton.Domain.Entities.AgendaEntity.Agenda", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("DataHora")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("Disponivel")
                        .HasColumnType("boolean");

                    b.Property<int>("MedicoId")
                        .HasColumnType("integer");

                    b.Property<int?>("PacienteId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("MedicoId");

                    b.HasIndex("PacienteId");

                    b.ToTable("Agenda");
                });

            modelBuilder.Entity("Hackaton.Domain.Entities.ConsultaEntity.Consulta", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("AgendaId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("DataHora")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("MedicoId")
                        .HasColumnType("integer");

                    b.Property<int>("PacienteId")
                        .HasColumnType("integer");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("AgendaId");

                    b.HasIndex("MedicoId");

                    b.HasIndex("PacienteId");

                    b.ToTable("Consulta");
                });

            modelBuilder.Entity("Hackaton.Domain.Entities.NotificacaoEntity.Notificacao", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("DataEnvio")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Mensagem")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("UsuarioId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("UsuarioId");

                    b.ToTable("Notificacao");
                });

            modelBuilder.Entity("Hackaton.Domain.Entities.UsuarioEntity.Usuario", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasMaxLength(8)
                        .HasColumnType("character varying(8)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("Senha")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Usuario");

                    b.HasDiscriminator().HasValue("Usuario");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("Hackaton.Domain.Entities.MedicoEntity.Medico", b =>
                {
                    b.HasBaseType("Hackaton.Domain.Entities.UsuarioEntity.Usuario");

                    b.Property<string>("CRM")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)");

                    b.HasDiscriminator().HasValue("Medico");
                });

            modelBuilder.Entity("Hackaton.Domain.Entities.PacienteEntity.Paciente", b =>
                {
                    b.HasBaseType("Hackaton.Domain.Entities.UsuarioEntity.Usuario");

                    b.Property<string>("CPF")
                        .IsRequired()
                        .HasMaxLength(14)
                        .HasColumnType("character varying(14)");

                    b.HasDiscriminator().HasValue("Paciente");
                });

            modelBuilder.Entity("Hackaton.Domain.Entities.AgendaEntity.Agenda", b =>
                {
                    b.HasOne("Hackaton.Domain.Entities.MedicoEntity.Medico", "Medico")
                        .WithMany("HorariosDisponiveis")
                        .HasForeignKey("MedicoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Hackaton.Domain.Entities.PacienteEntity.Paciente", "Paciente")
                        .WithMany()
                        .HasForeignKey("PacienteId");

                    b.Navigation("Medico");

                    b.Navigation("Paciente");
                });

            modelBuilder.Entity("Hackaton.Domain.Entities.ConsultaEntity.Consulta", b =>
                {
                    b.HasOne("Hackaton.Domain.Entities.AgendaEntity.Agenda", "Agenda")
                        .WithMany()
                        .HasForeignKey("AgendaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Hackaton.Domain.Entities.MedicoEntity.Medico", "Medico")
                        .WithMany()
                        .HasForeignKey("MedicoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Hackaton.Domain.Entities.PacienteEntity.Paciente", "Paciente")
                        .WithMany("Consultas")
                        .HasForeignKey("PacienteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Agenda");

                    b.Navigation("Medico");

                    b.Navigation("Paciente");
                });

            modelBuilder.Entity("Hackaton.Domain.Entities.NotificacaoEntity.Notificacao", b =>
                {
                    b.HasOne("Hackaton.Domain.Entities.UsuarioEntity.Usuario", "Usuario")
                        .WithMany()
                        .HasForeignKey("UsuarioId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("Hackaton.Domain.Entities.MedicoEntity.Medico", b =>
                {
                    b.Navigation("HorariosDisponiveis");
                });

            modelBuilder.Entity("Hackaton.Domain.Entities.PacienteEntity.Paciente", b =>
                {
                    b.Navigation("Consultas");
                });
#pragma warning restore 612, 618
        }
    }
}
