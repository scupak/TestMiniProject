using TechTalk.SpecFlow;
using Xunit;

namespace HotelBooking.Spec.StepDefinitions
{
    [Binding]
    public class CreateBooking
    {
        int firstNumber, secondNumber, sum;


        [Given(@"I have entered (.*) into the calculator")]
        public void GivenIHaveEnteredIntoTheCalculator(int a)
        {
            firstNumber = a;
        }

        [Given(@"I have also entered (.*) into the calculator")]
        public void GivenIHaveAlsoEnteredIntoTheCalculator(int b)
        {
            secondNumber = b;
        }

        [When(@"I press add")]
        public void WhenIPressAdd()
        {

        }

        [Then(@"the result should be (.*) on the screen")]
        public void ThenTheResultShouldBeOnTheScreen(int expectedSum)
        {
            Assert.Equal(expectedSum, sum);
        }
    }
}
