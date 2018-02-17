using LightBDD.Framework;
using LightBDD.Framework.Scenarios.Basic;
using LightBDD.XUnit2;

namespace ImageGallery.Client.Test.UI.Features
{
    /// <summary>
    ///
    /// </summary>
    [FeatureDescription(
        @"In order to access personal data
As an user
I want to login into system")]
    [Label("Story-1")]
    public partial class Login_feature
    {
        /// <summary>
        /// This is a sample scenario.
        /// When executed, it would appear in the report as "Successful login" and it would consists of following steps:
        ///
        /// GIVEN the user is about to login
        /// GIVEN the user entered valid login
        /// GIVEN the user entered valid password
        /// WHEN the user clicks login button
        /// THEN the login operation should be successful
        /// THEN a welcome message containing user name should be returned
        /// </summary>
        [Scenario]
        [Label("Ticket-1")]
        [ScenarioCategory(Categories.Security)]
        public void Successful_login()
        {
            Runner.RunScenario(
                Given_the_user_is_about_to_login,
                Given_the_user_entered_valid_login,
                Given_the_user_entered_valid_password,
                When_the_user_clicks_login_button,
                Then_the_login_operation_should_be_successful,
                Then_a_welcome_message_containing_user_name_should_be_returned);
        }
    }
}