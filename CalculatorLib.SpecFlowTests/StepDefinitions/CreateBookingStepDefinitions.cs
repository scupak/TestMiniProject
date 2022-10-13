using HotelBooking.Core;
using Moq;
using System;
using TechTalk.SpecFlow;
using Xunit;

namespace HotelBooking.Spec.StepDefinitions
{
    [Binding]
    public class CreateBookingStepDefinitions
    {

        private IBookingManager bookingManager;
        private Mock<IRepository<Room>> fakeRoomRepository;
        private Mock<IRepository<Booking>> fakeBookingRepository;

        private DateTime _fullyOccupiedStartDate = DateTime.Today.AddDays(10);
        private DateTime _fullyOccupiedEndDate = DateTime.Today.AddDays(20);
        
        private DateTime _startDate;
        private DateTime _endDate;
        
        private delegate bool Del();
        private Del _method;
        private Action _action;

        [When(@"The method runs")]
        public void WhenTheMethodRuns()
        {
            var rooms = new List<Room>
            {
                new Room { Id=1, Description="A" },
                new Room { Id=2, Description="B" },
            };

            // Create fake RoomRepository. 
            fakeRoomRepository = new Mock<IRepository<Room>>();

            // Implement fake GetAll() method.
            fakeRoomRepository.Setup(x => x.GetAll()).Returns(rooms);

            // Integers from 1 to 2 (using a range)
            fakeRoomRepository.Setup(x =>
            x.Get(It.IsInRange<int>(1, 2, Moq.Range.Inclusive))).Returns(rooms[1]);
            
            List<Booking> bookings = new List<Booking>
            {
                new Booking { Id=1, StartDate=_fullyOccupiedStartDate, EndDate=_fullyOccupiedEndDate, IsActive=true, CustomerId=1, RoomId=1 },
                new Booking { Id=2, StartDate=_fullyOccupiedStartDate, EndDate=_fullyOccupiedEndDate, IsActive=true, CustomerId=2, RoomId=2 },
            };

            // Create fake BookingRepository. 
            fakeBookingRepository = new Mock<IRepository<Booking>>();

            // Implement fake GetAll() method.
            fakeBookingRepository.Setup(x => x.GetAll()).Returns(bookings);

            fakeBookingRepository.Setup(x => x.Get(It.IsInRange<int>(1, 2, Moq.Range.Inclusive))).Returns(bookings[1]);


            bookingManager = new BookingManager(fakeBookingRepository.Object, fakeRoomRepository.Object);

            _method = () => bookingManager.CreateBooking(new Booking { StartDate = _startDate, EndDate = _endDate });

            _action = () => bookingManager.CreateBooking(new Booking { StartDate = _startDate, EndDate = _endDate });
        }

        [Then(@"an argument exception should be thrown")]
        public void ThenThenAnArgumentExceptionShouldBeThrown()
        {

            Assert.Throws<ArgumentException>(_action);
        }

        [Given(@"I have entered a start date (.*) days before today into the datetime pickers")]
        public void GivenIHaveEnteredAStartDateDaysBeforeTodayIntoTheDatetimePickers(int days)
        {
            _startDate = DateTime.Today.AddDays(-days);
        }

        [Given(@"I have entered an end date (.*) days after today into the datetime pickers")]
        public void GivenIHaveEnteredAnEndDateDaysAfterTodayIntoTheDatetimePickers(int days)
        {
            _endDate = DateTime.Today.AddDays(days);
        }

        [Given(@"I have entered a start date (.*) days after today into the datetime pickers")]
        public void GivenIHaveEnteredAStartDateDaysAfterTodayIntoTheDatetimePickers(int days)
        {
            _startDate = DateTime.Today.AddDays(days);
        }

        [Given(@"the occupied startdate is (.*) days after today and the enddate is (.*) days after today\.")]
        public void GivenTheOccupiedStartdateIsDaysAfterTodayAndTheEnddateIsDaysAfterToday_(int startDays, int endDays)
        {
            _fullyOccupiedStartDate = DateTime.Today.AddDays(startDays);
            _fullyOccupiedEndDate = DateTime.Today.AddDays(endDays);
        }

        [Then(@"the booking should be created")]
        public void ThenThenTheBookingShouldBeCreated()
        {
            Assert.True(_method());
        }

        [Then(@"the booking should not be created")]
        public void ThenThenTheBookingShouldNotBeCreated()
        {
            Assert.False(_method()); ;
        }

        [Given(@"I have entered the maximum end date value into the datetime pickers")]
        public void GivenIHaveEnteredTheMaximumEndDateValueIntoTheDatetimePickers()
        {
            _endDate = DateTime.MaxValue;
        }


    }
}
