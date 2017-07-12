﻿namespace Qvision.Umbraco.PollIt
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;

    using AutoMapper;

    using global::Umbraco.Core;
    using global::Umbraco.Core.Logging;
    using global::Umbraco.Core.Persistence;
    using global::Umbraco.Core.Persistence.Migrations;
    using global::Umbraco.Web;
    using global::Umbraco.Web.UI.JavaScript;


    using Qvision.Umbraco.PollIt.Constants;
    using Qvision.Umbraco.PollIt.Controllers.ApiControllers;
    using Qvision.Umbraco.PollIt.Mapping.Profile;
    using Qvision.Umbraco.PollIt.Models.Pocos;

    using Semver;

    public class Startup : ApplicationEventHandler
    {
        /// <summary>
        /// The application started.
        /// </summary>
        /// <param name="umbracoApplication">
        /// The umbraco application.
        /// </param>
        /// <param name="applicationContext">
        /// The application context.
        /// </param>
        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            using (ApplicationContext.Current.ProfilingLogger.TraceDuration<Startup>("Begin ApplicationStarted", "End ApplicationStarted"))
            {
                this.SetupSections(applicationContext);
                this.SetupMigration();

                // Add mapping
                Mapper.AddProfile<QuestionProfile>();
                Mapper.AddProfile<AnswerProfile>();
                Mapper.AddProfile<ResponseProfile>();

                // setup server variables
                ServerVariablesParser.Parsing += this.ServerVariablesParserParsing;
            }
        }

        /// <summary>
        /// Setup sections.
        /// </summary>
        /// <param name="applicationContext">
        /// The application Context.
        /// </param>
        private void SetupSections(ApplicationContext applicationContext)
        {
            // Gets a reference to the section (if already added)
            var section = applicationContext.Services.SectionService.GetByAlias(ApplicationConstants.SectionAlias);
            if (section != null)
            {
                return;
            }

            // Add a new "Polls" section
            applicationContext.Services.SectionService.MakeNew("Poll It", ApplicationConstants.SectionAlias, ApplicationConstants.SectionIcon);

            // Grant all existing users access to the new section
            applicationContext.Services.UserService.AddSectionToAllUsers(ApplicationConstants.SectionAlias);
        }

        /// <summary>
        /// Setup database.
        /// </summary>
        /// <param name="applicationContext">
        /// The application Context.
        /// </param>
        private void SetupMigration()
        {            
            var currentVersion = new SemVersion(0, 0, 0);

            var migrations = ApplicationContext.Current.Services.MigrationEntryService.GetAll(ApplicationConstants.ProductName);
            
            var latestMigration = migrations.OrderByDescending(x => x.Version).FirstOrDefault();

            if (latestMigration != null)
            {
                currentVersion = latestMigration.Version;
            }

            var targetVersion = new SemVersion(0, 5, 0);
            if (targetVersion != currentVersion)
            {
                var migrationsRunner = new MigrationRunner(
                    ApplicationContext.Current.Services.MigrationEntryService,
                    ApplicationContext.Current.ProfilingLogger.Logger,
                    currentVersion,
                    targetVersion,
                    ApplicationConstants.ProductName);

                try
                {
                    migrationsRunner.Execute(UmbracoContext.Current.Application.DatabaseContext.Database);
                }
                catch (Exception e)
                {
                    LogHelper.Error<Startup>("Error running Statistics migration", e);
                }
            }
        }

        /// <summary>
        /// The server variables parser parsing.
        /// </summary>
        private void ServerVariablesParserParsing(object sender, Dictionary<string, object> e)
        {
            if (HttpContext.Current == null)
            {
                throw new InvalidOperationException("HttpContext is null");
            }

            if (!e.Keys.Contains("pollIt"))
            {
                var urlHelper = new UrlHelper(new RequestContext(new HttpContextWrapper(HttpContext.Current), new RouteData()));

                var urlDictionairy = new Dictionary<string, object>
                {
                    { "getOverview", urlHelper.GetUmbracoApiService<OverviewApiController>("Get") },

                    { "getQuestions", urlHelper.GetUmbracoApiService<QuestionApiController>("Get") },
                    { "getQuestionById", urlHelper.GetUmbracoApiService<QuestionApiController>("GetById") },
                    { "saveQuestion", urlHelper.GetUmbracoApiService<QuestionApiController>("Post") },
                    { "deleteQuestion", urlHelper.GetUmbracoApiService<QuestionApiController>("Delete") },
                    { "getQuestionAnswersById", urlHelper.GetUmbracoApiService<QuestionApiController>("GetAnswers") },
                    { "postQuestionAnswer", urlHelper.GetUmbracoApiService<QuestionApiController>("PostAnswer") },
                    { "getQuestionResponsesById", urlHelper.GetUmbracoApiService<QuestionApiController>("GetResponses") },

                    { "saveAnswer", urlHelper.GetUmbracoApiService<AnswerApiController>("Post") },
                    { "updateSort", urlHelper.GetUmbracoApiService<AnswerApiController>("UpdateSort") },
                    { "deleteAnswer", urlHelper.GetUmbracoApiService<AnswerApiController>("Delete") }
                };

                e.Add("pollIt", urlDictionairy);
            }
        }
    }
}
