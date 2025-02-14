﻿// <auto-generated />
using System;
using System.Collections.Generic;
using Lighthouse.Backend.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Lighthouse.Migrations.Postgres.Migrations
{
    [DbContext(typeof(LighthouseAppContext))]
    [Migration("20250210105742_CascadeDeleteForFeatureWork")]
    partial class CascadeDeleteForFeatureWork
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("FeatureProject", b =>
                {
                    b.Property<int>("FeaturesId")
                        .HasColumnType("integer");

                    b.Property<int>("ProjectsId")
                        .HasColumnType("integer");

                    b.HasKey("FeaturesId", "ProjectsId");

                    b.HasIndex("ProjectsId");

                    b.ToTable("FeatureProject");
                });

            modelBuilder.Entity("Lighthouse.Backend.Models.AppSetting", b =>
                {
                    b.Property<string>("Key")
                        .HasColumnType("text");

                    b.Property<int>("Id")
                        .HasColumnType("integer");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Key");

                    b.ToTable("AppSettings");
                });

            modelBuilder.Entity("Lighthouse.Backend.Models.Feature", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<bool>("IsUnparentedFeature")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsUsingDefaultFeatureSize")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Order")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ReferenceId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("State")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Url")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Features");
                });

            modelBuilder.Entity("Lighthouse.Backend.Models.FeatureWork", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("FeatureId")
                        .HasColumnType("integer");

                    b.Property<int>("RemainingWorkItems")
                        .HasColumnType("integer");

                    b.Property<int>("TeamId")
                        .HasColumnType("integer");

                    b.Property<int>("TotalWorkItems")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("FeatureId");

                    b.HasIndex("TeamId");

                    b.ToTable("FeatureWork");
                });

            modelBuilder.Entity("Lighthouse.Backend.Models.Forecast.ForecastBase", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasMaxLength(34)
                        .HasColumnType("character varying(34)");

                    b.Property<int>("TotalTrials")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("ForecastBase");

                    b.HasDiscriminator().HasValue("ForecastBase");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("Lighthouse.Backend.Models.Forecast.IndividualSimulationResult", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("ForecastId")
                        .HasColumnType("integer");

                    b.Property<int>("Key")
                        .HasColumnType("integer");

                    b.Property<int>("Value")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ForecastId");

                    b.ToTable("IndividualSimulationResult");
                });

            modelBuilder.Entity("Lighthouse.Backend.Models.History.FeatureHistoryEntry", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("FeatureId")
                        .HasColumnType("integer");

                    b.Property<string>("FeatureReferenceId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateOnly>("Snapshot")
                        .HasColumnType("date");

                    b.Property<string>("State")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("FeatureReferenceId", "Snapshot");

                    b.ToTable("FeatureHistory");
                });

            modelBuilder.Entity("Lighthouse.Backend.Models.History.FeatureWorkHistoryEntry", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("FeatureHistoryEntryId")
                        .HasColumnType("integer");

                    b.Property<int>("RemainingWorkItems")
                        .HasColumnType("integer");

                    b.Property<int>("TeamId")
                        .HasColumnType("integer");

                    b.Property<int>("TotalWorkItems")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("FeatureHistoryEntryId");

                    b.ToTable("FeatureWorkHistoryEntry");
                });

            modelBuilder.Entity("Lighthouse.Backend.Models.Milestone", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("ProjectId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ProjectId");

                    b.ToTable("Milestone");
                });

            modelBuilder.Entity("Lighthouse.Backend.Models.Preview.PreviewFeature", b =>
                {
                    b.Property<string>("Key")
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("Enabled")
                        .HasColumnType("boolean");

                    b.Property<int>("Id")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Key");

                    b.ToTable("PreviewFeatures");
                });

            modelBuilder.Entity("Lighthouse.Backend.Models.Project", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("DefaultAmountOfWorkItemsPerFeature")
                        .HasColumnType("integer");

                    b.Property<int>("DefaultWorkItemPercentile")
                        .HasColumnType("integer");

                    b.PrimitiveCollection<List<string>>("DoingStates")
                        .IsRequired()
                        .HasColumnType("text[]");

                    b.PrimitiveCollection<List<string>>("DoneStates")
                        .IsRequired()
                        .HasColumnType("text[]");

                    b.Property<string>("FeatureOwnerField")
                        .HasColumnType("text");

                    b.Property<string>("HistoricalFeaturesWorkItemQuery")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.PrimitiveCollection<List<string>>("OverrideRealChildCountStates")
                        .IsRequired()
                        .HasColumnType("text[]");

                    b.Property<int?>("OwningTeamId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("ProjectUpdateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("SizeEstimateField")
                        .HasColumnType("text");

                    b.PrimitiveCollection<List<string>>("ToDoStates")
                        .IsRequired()
                        .HasColumnType("text[]");

                    b.Property<string>("UnparentedItemsQuery")
                        .HasColumnType("text");

                    b.Property<bool>("UsePercentileToCalculateDefaultAmountOfWorkItems")
                        .HasColumnType("boolean");

                    b.Property<string>("WorkItemQuery")
                        .IsRequired()
                        .HasColumnType("text");

                    b.PrimitiveCollection<List<string>>("WorkItemTypes")
                        .IsRequired()
                        .HasColumnType("text[]");

                    b.Property<int>("WorkTrackingSystemConnectionId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("OwningTeamId");

                    b.HasIndex("WorkTrackingSystemConnectionId");

                    b.ToTable("Projects");
                });

            modelBuilder.Entity("Lighthouse.Backend.Models.Team", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("AdditionalRelatedField")
                        .HasColumnType("text");

                    b.Property<bool>("AutomaticallyAdjustFeatureWIP")
                        .HasColumnType("boolean");

                    b.PrimitiveCollection<List<string>>("DoingStates")
                        .IsRequired()
                        .HasColumnType("text[]");

                    b.PrimitiveCollection<List<string>>("DoneStates")
                        .IsRequired()
                        .HasColumnType("text[]");

                    b.Property<int>("FeatureWIP")
                        .HasColumnType("integer");

                    b.PrimitiveCollection<List<string>>("FeaturesInProgress")
                        .IsRequired()
                        .HasColumnType("text[]");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.PrimitiveCollection<int[]>("RawThroughput")
                        .IsRequired()
                        .HasColumnType("integer[]");

                    b.Property<DateTime>("TeamUpdateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("ThroughputHistory")
                        .HasColumnType("integer");

                    b.Property<DateTime?>("ThroughputHistoryEndDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("ThroughputHistoryStartDate")
                        .HasColumnType("timestamp with time zone");

                    b.PrimitiveCollection<List<string>>("ToDoStates")
                        .IsRequired()
                        .HasColumnType("text[]");

                    b.Property<bool>("UseFixedDatesForThroughput")
                        .HasColumnType("boolean");

                    b.Property<string>("WorkItemQuery")
                        .IsRequired()
                        .HasColumnType("text");

                    b.PrimitiveCollection<List<string>>("WorkItemTypes")
                        .IsRequired()
                        .HasColumnType("text[]");

                    b.Property<int>("WorkTrackingSystemConnectionId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("WorkTrackingSystemConnectionId");

                    b.ToTable("Teams");
                });

            modelBuilder.Entity("Lighthouse.Backend.Models.WorkTrackingSystemConnection", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("WorkTrackingSystem")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("WorkTrackingSystemConnections");
                });

            modelBuilder.Entity("Lighthouse.Backend.Models.WorkTrackingSystemConnectionOption", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<bool>("IsOptional")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsSecret")
                        .HasColumnType("boolean");

                    b.Property<string>("Key")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("WorkTrackingSystemConnectionId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("WorkTrackingSystemConnectionId");

                    b.ToTable("WorkTrackingSystemConnectionOption");
                });

            modelBuilder.Entity("ProjectTeam", b =>
                {
                    b.Property<int>("ProjectsId")
                        .HasColumnType("integer");

                    b.Property<int>("TeamsId")
                        .HasColumnType("integer");

                    b.HasKey("ProjectsId", "TeamsId");

                    b.HasIndex("TeamsId");

                    b.ToTable("ProjectTeam");
                });

            modelBuilder.Entity("Lighthouse.Backend.Models.Forecast.WhenForecast", b =>
                {
                    b.HasBaseType("Lighthouse.Backend.Models.Forecast.ForecastBase");

                    b.Property<int>("FeatureId")
                        .HasColumnType("integer");

                    b.Property<int>("NumberOfItems")
                        .HasColumnType("integer");

                    b.Property<int?>("TeamId")
                        .HasColumnType("integer");

                    b.HasIndex("FeatureId");

                    b.HasIndex("TeamId");

                    b.HasDiscriminator().HasValue("WhenForecast");
                });

            modelBuilder.Entity("Lighthouse.Backend.Models.History.WhenForecastHistoryEntry", b =>
                {
                    b.HasBaseType("Lighthouse.Backend.Models.Forecast.ForecastBase");

                    b.Property<int>("FeatureHistoryEntryId")
                        .HasColumnType("integer");

                    b.Property<int>("NumberOfItems")
                        .HasColumnType("integer");

                    b.Property<int>("TeamId")
                        .HasColumnType("integer");

                    b.HasIndex("FeatureHistoryEntryId");

                    b.ToTable("ForecastBase", t =>
                        {
                            t.Property("NumberOfItems")
                                .HasColumnName("WhenForecastHistoryEntry_NumberOfItems");

                            t.Property("TeamId")
                                .HasColumnName("WhenForecastHistoryEntry_TeamId");
                        });

                    b.HasDiscriminator().HasValue("WhenForecastHistoryEntry");
                });

            modelBuilder.Entity("FeatureProject", b =>
                {
                    b.HasOne("Lighthouse.Backend.Models.Feature", null)
                        .WithMany()
                        .HasForeignKey("FeaturesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Lighthouse.Backend.Models.Project", null)
                        .WithMany()
                        .HasForeignKey("ProjectsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Lighthouse.Backend.Models.FeatureWork", b =>
                {
                    b.HasOne("Lighthouse.Backend.Models.Feature", "Feature")
                        .WithMany("FeatureWork")
                        .HasForeignKey("FeatureId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Lighthouse.Backend.Models.Team", "Team")
                        .WithMany()
                        .HasForeignKey("TeamId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Feature");

                    b.Navigation("Team");
                });

            modelBuilder.Entity("Lighthouse.Backend.Models.Forecast.IndividualSimulationResult", b =>
                {
                    b.HasOne("Lighthouse.Backend.Models.Forecast.ForecastBase", "Forecast")
                        .WithMany("SimulationResults")
                        .HasForeignKey("ForecastId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Forecast");
                });

            modelBuilder.Entity("Lighthouse.Backend.Models.History.FeatureWorkHistoryEntry", b =>
                {
                    b.HasOne("Lighthouse.Backend.Models.History.FeatureHistoryEntry", "FeatureHistoryEntry")
                        .WithMany("FeatureWork")
                        .HasForeignKey("FeatureHistoryEntryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("FeatureHistoryEntry");
                });

            modelBuilder.Entity("Lighthouse.Backend.Models.Milestone", b =>
                {
                    b.HasOne("Lighthouse.Backend.Models.Project", "Project")
                        .WithMany("Milestones")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Project");
                });

            modelBuilder.Entity("Lighthouse.Backend.Models.Project", b =>
                {
                    b.HasOne("Lighthouse.Backend.Models.Team", "OwningTeam")
                        .WithMany()
                        .HasForeignKey("OwningTeamId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("Lighthouse.Backend.Models.WorkTrackingSystemConnection", "WorkTrackingSystemConnection")
                        .WithMany()
                        .HasForeignKey("WorkTrackingSystemConnectionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("OwningTeam");

                    b.Navigation("WorkTrackingSystemConnection");
                });

            modelBuilder.Entity("Lighthouse.Backend.Models.Team", b =>
                {
                    b.HasOne("Lighthouse.Backend.Models.WorkTrackingSystemConnection", "WorkTrackingSystemConnection")
                        .WithMany()
                        .HasForeignKey("WorkTrackingSystemConnectionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("WorkTrackingSystemConnection");
                });

            modelBuilder.Entity("Lighthouse.Backend.Models.WorkTrackingSystemConnectionOption", b =>
                {
                    b.HasOne("Lighthouse.Backend.Models.WorkTrackingSystemConnection", "WorkTrackingSystemConnection")
                        .WithMany("Options")
                        .HasForeignKey("WorkTrackingSystemConnectionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("WorkTrackingSystemConnection");
                });

            modelBuilder.Entity("ProjectTeam", b =>
                {
                    b.HasOne("Lighthouse.Backend.Models.Project", null)
                        .WithMany()
                        .HasForeignKey("ProjectsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Lighthouse.Backend.Models.Team", null)
                        .WithMany()
                        .HasForeignKey("TeamsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Lighthouse.Backend.Models.Forecast.WhenForecast", b =>
                {
                    b.HasOne("Lighthouse.Backend.Models.Feature", "Feature")
                        .WithMany("Forecasts")
                        .HasForeignKey("FeatureId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Lighthouse.Backend.Models.Team", "Team")
                        .WithMany()
                        .HasForeignKey("TeamId");

                    b.Navigation("Feature");

                    b.Navigation("Team");
                });

            modelBuilder.Entity("Lighthouse.Backend.Models.History.WhenForecastHistoryEntry", b =>
                {
                    b.HasOne("Lighthouse.Backend.Models.History.FeatureHistoryEntry", "FeatureHistoryEntry")
                        .WithMany("Forecasts")
                        .HasForeignKey("FeatureHistoryEntryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("FeatureHistoryEntry");
                });

            modelBuilder.Entity("Lighthouse.Backend.Models.Feature", b =>
                {
                    b.Navigation("FeatureWork");

                    b.Navigation("Forecasts");
                });

            modelBuilder.Entity("Lighthouse.Backend.Models.Forecast.ForecastBase", b =>
                {
                    b.Navigation("SimulationResults");
                });

            modelBuilder.Entity("Lighthouse.Backend.Models.History.FeatureHistoryEntry", b =>
                {
                    b.Navigation("FeatureWork");

                    b.Navigation("Forecasts");
                });

            modelBuilder.Entity("Lighthouse.Backend.Models.Project", b =>
                {
                    b.Navigation("Milestones");
                });

            modelBuilder.Entity("Lighthouse.Backend.Models.WorkTrackingSystemConnection", b =>
                {
                    b.Navigation("Options");
                });
#pragma warning restore 612, 618
        }
    }
}
