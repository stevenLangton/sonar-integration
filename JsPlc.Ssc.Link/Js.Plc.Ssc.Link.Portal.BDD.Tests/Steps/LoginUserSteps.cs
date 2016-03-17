using System;

using TechTalk.SpecFlow;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using NUnit.Framework;
using System.Threading;


namespace Js.Plc.Ssc.Link.Portal.BDD.Tests.Steps
{
    [Binding]
    public sealed class LoginUserSteps
    {
        IWebDriver driver;

        [Given(@"(?i)I am (?:at|on|taken to|start on) the (.*?) page")]
        public void GivenIAmAtTheHomePage(String pageName)
        {
            driver = new ChromeDriver();
            driver.Navigate().GoToUrl("http://ci.link.js-devops.co.uk/Home/Welcome");
            Assert.AreEqual("Welcome to My performance - My performance", driver.Title);
        }

        [Then(@"I click the login button")]
        public void ThenIClickTheLoginButton()
        {
            driver.FindElement(By.Id("loginLink")).Click();
        }

        [Then(@"I should be at the login page")]
        public void ThenIShouldBeAtTheLoginPage()
        {
            Assert.AreEqual("Sign in to your account", driver.Title);
        }

        [Then(@"I fill in the following form")]
        public void ThenIFillInTheFollowingForm(Table table)
        {
            driver.FindElement(By.Id("cred_userid_inputtext")).SendKeys("steven.farkas@jstest3.onmicrosoft.com");
            driver.FindElement(By.Id("cred_password_inputtext")).SendKeys("Password1*");
            
        }

        [Then(@"I click the Azure login button")]
        public void ThenIClickTheLoginbButton()
        {
            driver.FindElement(By.Id("cred_sign_in_button")).Click();
            
        }
      

        [Given(@"I am loged in i should see the menu")]
        public void GivenIAmLogedInIShouldSeeTheMenu()
        {
          

            //Assert.AreEqual("Hello Steven!", driver.FindElement(By.ClassName("welcomeText")).Text);
            driver.Close();
        }



    }
}
