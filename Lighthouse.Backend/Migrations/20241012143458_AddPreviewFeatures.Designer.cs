﻿// <auto-generated />
using System;
using Lighthouse.Backend.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Lighthouse.Backend.Migrations
{
    [DbContext(typeof(LighthouseAppContext))]
    [Migration("20241012143458_AddPreviewFeatures")]
    partial class AddPreviewFeatures
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.10");

            modelBuilder.Entity("FeatureProject", b =>
                {
                    b.Property<int>("FeaturesId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ProjectsId")
                        .HasColumnType("INTEGER");

                    b.HasKey("FeaturesId", "ProjectsId");

                    b.HasIndex("ProjectsId");

                    b.ToTable("FeatureProject");
                });

            modelBuilder.Entity("Lighthouse.Backend.Models.AppSetting", b =>
                {
                    b.Property<string>("Key")
                        .HasColumnType("TEXT");

                    b.Property<int>("Id")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Key");

                    b.ToTable("AppSettings");
                });

            modelBuilder.Entity("Lighthouse.Backend.Models.Feature", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsUnparentedFeature")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Order")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("ReferenceId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Url")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Features");
                });

            modelBuilder.Entity("Lighthouse.Backend.Models.FeatureWork", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("FeatureId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("RemainingWorkItems")
                        .HasColumnType("INTEGER");

                    b.Property<int>("TeamId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("TotalWorkItems")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("FeatureId");

                    b.HasIndex("TeamId");

                    b.ToTable("FeatureWork");
                });

            modelBuilder.Entity("Lighthouse.Backend.Models.Forecast.ForecastBase", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("TEXT");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasMaxLength(34)
                        .HasColumnType("TEXT");

                    b.Property<int>("TotalTrials")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("ForecastBase");

                    b.HasDiscriminator().HasValue("ForecastBase");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("Lighthouse.Backend.Models.Forecast.IndividualSimulationResult", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("ForecastId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Key")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Value")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("ForecastId");

                    b.ToTable("IndividualSimulationResult");
                });

            modelBuilder.Entity("Lighthouse.Backend.Models.History.FeatureHistoryEntry", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("FeatureId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("FeatureReferenceId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateOnly>("Snapshot")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("FeatureHistory");
                });

            modelBuilder.Entity("Lighthouse.Backend.Models.History.FeatureWorkHistoryEntry", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("FeatureHistoryEntryId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("RemainingWorkItems")
                        .HasColumnType("INTEGER");

                    b.Property<int>("TeamId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("TotalWorkItems")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("FeatureHistoryEntryId");

                    b.ToTable("FeatureWorkHistoryEntry");
                });

            modelBuilder.Entity("Lighthouse.Backend.Models.Milestone", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("Date")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("ProjectId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("ProjectId");

                    b.ToTable("Milestone");
                });

            modelBuilder.Entity("Lighthouse.Backend.Models.Preview.PreviewFeature", b =>
                {
                    b.Property<string>("Key")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("Enabled")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Id")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Key");

                    b.ToTable("PreviewFeatures");
                });

            modelBuilder.Entity("Lighthouse.Backend.Models.Project", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("DefaultAmountOfWorkItemsPerFeature")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("ProjectUpdateTime")
                        .HasColumnType("TEXT");

                    b.Property<string>("SizeEstimateField")
                        .HasColumnType("TEXT");

                    b.Property<string>("UnparentedItemsQuery")
                        .HasColumnType("TEXT");

                    b.Property<string>("WorkItemQuery")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("WorkItemTypes")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("WorkTrackingSystemConnectionId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("WorkTrackingSystemConnectionId");

                    b.ToTable("Projects");
                });

            modelBuilder.Entity("Lighthouse.Backend.Models.Team", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("AdditionalRelatedField")
                        .HasColumnType("TEXT");

                    b.Property<int>("FeatureWIP")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("RawThroughput")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("ThroughputHistory")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("ThroughputUpdateTime")
                        .HasColumnType("TEXT");

                    b.Property<string>("WorkItemQuery")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("WorkItemTypes")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("WorkTrackingSystemConnectionId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("WorkTrackingSystemConnectionId");

                    b.ToTable("Teams");
                });

            modelBuilder.Entity("Lighthouse.Backend.Models.WorkTrackingSystemConnection", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("WorkTrackingSystem")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("WorkTrackingSystemConnections");
                });

            modelBuilder.Entity("Lighthouse.Backend.Models.WorkTrackingSystemConnectionOption", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsSecret")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Key")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("WorkTrackingSystemConnectionId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("WorkTrackingSystemConnectionId");

                    b.ToTable("WorkTrackingSystemConnectionOption");
                });

            modelBuilder.Entity("Lighthouse.Backend.Models.Forecast.WhenForecast", b =>
                {
                    b.HasBaseType("Lighthouse.Backend.Models.Forecast.ForecastBase");

                    b.Property<int>("FeatureId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("NumberOfItems")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("TeamId")
                        .HasColumnType("INTEGER");

                    b.HasIndex("FeatureId");

                    b.HasIndex("TeamId");

                    b.HasDiscriminator().HasValue("WhenForecast");
                });

            modelBuilder.Entity("Lighthouse.Backend.Models.History.WhenForecastHistoryEntry", b =>
                {
                    b.HasBaseType("Lighthouse.Backend.Models.Forecast.ForecastBase");

                    b.Property<int>("FeatureHistoryEntryId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("NumberOfItems")
                        .HasColumnType("INTEGER");

                    b.Property<int>("TeamId")
                        .HasColumnType("INTEGER");

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
                    b.HasOne("Lighthouse.Backend.Models.WorkTrackingSystemConnection", "WorkTrackingSystemConnection")
                        .WithMany()
                        .HasForeignKey("WorkTrackingSystemConnectionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

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
