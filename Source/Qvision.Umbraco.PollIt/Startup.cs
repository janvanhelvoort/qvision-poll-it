namespace Qvision.Umbraco.PollIt
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
    using global::Umbraco.Web;
    using global::Umbraco.Web.UI.JavaScript;


    using Qvision.Umbraco.PollIt.Constants;
    using Qvision.Umbraco.PollIt.Controllers.ApiControllers;
    using Qvision.Umbraco.PollIt.Mapping.Profile;
    using Qvision.Umbraco.PollIt.Models.Pocos;

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
                this.SetupDatabase(applicationContext);

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
        private void SetupDatabase(ApplicationContext applicationContext)
        {
            var databaseContext = applicationContext.DatabaseContext;
            var databaseSchema = new DatabaseSchemaHelper(databaseContext.Database, applicationContext.ProfilingLogger.Logger, databaseContext.SqlSyntax);

            if (!databaseSchema.TableExist(TableConstants.Questions.TableName))
            {
                LogHelper.Info<Startup>("Setting up questions Table");
                databaseSchema.CreateTable<Question>(false);
            }

            if (!databaseSchema.TableExist(TableConstants.Answers.TableName))
            {
                LogHelper.Info<Startup>("Setting up answers Table");
                databaseSchema.CreateTable<Answer>(false);
            }

            if (!databaseSchema.TableExist(TableConstants.Responses.TableName))
            {
                LogHelper.Info<Startup>("Setting up responses Table");
                databaseSchema.CreateTable<Response>(false);
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
