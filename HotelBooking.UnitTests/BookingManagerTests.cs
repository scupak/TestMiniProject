using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using HotelBooking.Core;
using HotelBooking.UnitTests.Fakes;
using Moq;
using Xunit;

namespace HotelBooking.UnitTests
{
    public class BookingManagerTests
    {
        private IBookingManager bookingManager;
        private Mock<IRepository<Room>> fakeRoomRepository;
        private Mock<IRepository<Booking>> fakeBookingRepository;

        public BookingManagerTests()
        {
            /*
            DateTime start = DateTime.Today.AddDays(10);
            DateTime end = DateTime.Today.AddDays(20);
            IRepository<Booking> bookingRepository = new FakeBookingRepository(start, end);
            IRepository<Room> roomRepository = new FakeRoomRepository();*/

            /*copy from rooms controller tests 
             */
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

            DateTime fullyOccupiedStartDate = DateTime.Today.AddDays(10);
            DateTime fullyOccupiedEndDate = DateTime.Today.AddDays(20);

            List<Booking> bookings = new List<Booking>
            {
                new Booking { Id=1, StartDate=fullyOccupiedStartDate, EndDate=fullyOccupiedEndDate, IsActive=true, CustomerId=1, RoomId=1 },
                new Booking { Id=2, StartDate=fullyOccupiedStartDate, EndDate=fullyOccupiedEndDate, IsActive=true, CustomerId=2, RoomId=2 },
            };

            // Create fake BookingRepository. 
            fakeBookingRepository = new Mock<IRepository<Booking>>();

            // Implement fake GetAll() method.
            fakeBookingRepository.Setup(x => x.GetAll()).Returns(bookings);

            fakeBookingRepository.Setup(x => x.Get(It.IsInRange<int>(1, 2, Moq.Range.Inclusive))).Returns(bookings[1]);


            bookingManager = new BookingManager(fakeBookingRepository.Object, fakeRoomRepository.Object);
        }
        /*
         * [UnitOfWorkName]_[ScenarioUnderTest]_[ExpectedBehavior]
        */
        [Theory]
        [MemberData(nameof(GetData), parameters: TestData.PastDates)]
        public void FindAvailableRoom_StartDateNotInTheFuture_ThrowsArgumentException(DateTime date)
        {
            //Arrange

            // Act
            Action act = () => bookingManager.FindAvailableRoom(date, date);

            // Assert
            Assert.Throws<ArgumentException>(act);
        }

        [Theory]
        [MemberData(nameof(GetData), parameters: TestData.AvailableDates)]
        public void FindAvailableRoom_RoomAvailable_RoomIdNotMinusOne(DateTime date)
        {
            //Arrange

            // Act
            int roomId = bookingManager.FindAvailableRoom(date, date);
            // Assert
            Assert.NotEqual(-1, roomId);
        }

        [Theory]
        [MemberData(nameof(GetData), parameters: TestData.NotAvailableDates)]
        public void FindAvailableRoom_RoomNotAvailable_RoomIdIsMinusOne(DateTime date)
        {
            //Arrange

            // Act
            int roomId = bookingManager.FindAvailableRoom(date, date);
            // Assert
            Assert.Equal(-1, roomId);
        }
        /*
          public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
        public int CustomerId { get; set; }
        public int RoomId { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual Room Room { get; set; }*/
        [Theory]
        [MemberData(nameof(GetData), parameters: TestData.AvailableBookings)]
        public void CreateBooking_RoomAvailable_ReturnsTrue(DateTime startDate, DateTime endDate, bool isActive, int customerId, int roomId)
        {
            //Arrange
            Booking booking = new Booking {StartDate = startDate, EndDate = endDate, IsActive = isActive, CustomerId = customerId, RoomId = roomId };
            // Act
            bool result = bookingManager.CreateBooking(booking);
            // Assert
            Assert.True(result);
            fakeBookingRepository.Verify(x => x.Add(booking), Times.Once);
        }

        [Theory]
        [MemberData(nameof(GetData), parameters: TestData.NotAvailableBookings)]
        public void CreateBooking_RoomNotAvailable_ReturnsFalse(DateTime startDate, DateTime endDate, bool isActive, int customerId, int roomId)
        {
            //Arrange
            Booking booking = new Booking { StartDate = startDate, EndDate = endDate, IsActive = isActive, CustomerId = customerId, RoomId = roomId };
            // Act
            bool result = bookingManager.CreateBooking(booking);
            // Assert
            Assert.False(result);
            fakeBookingRepository.Verify(x => x.Add(It.IsAny<Booking>()), Times.Never);
        }
        /*
         public List<DateTime> GetFullyOccupiedDates(DateTime startDate, DateTime endDate)
        {
            if (startDate > endDate)
                throw new ArgumentException("The start date cannot be later than the end date.");

            List<DateTime> fullyOccupiedDates = new List<DateTime>();
            int noOfRooms = roomRepository.GetAll().Count();
            var bookings = bookingRepository.GetAll();

            if (bookings.Any())
            {
                for (DateTime d = startDate; d <= endDate; d = d.AddDays(1))
                {
                    var noOfBookings = from b in bookings
                                       where b.IsActive && d >= b.StartDate && d <= b.EndDate
                                       select b;
                    if (noOfBookings.Count() >= noOfRooms)
                        fullyOccupiedDates.Add(d);
                }
            }
            return fullyOccupiedDates;
        }
         */

        [Theory]
        [MemberData(nameof(GetData), parameters: TestData.DateAndExpectedDate)]
        public void GetFullyOccupiedDates_KnownOccupiedDates_ReturnsRightDates(DateTime InputStartDate, DateTime InputEndDate, List<DateTime> ExpetedfullyOccupiedDates)
        {
            //Arrange

            // Act
            List<DateTime> result = bookingManager.GetFullyOccupiedDates(InputStartDate, InputEndDate);
            // Assert
            Assert.Equal(ExpetedfullyOccupiedDates, result);

        }


        public static IEnumerable<object[]> GetData(TestData testData)
        {

            return testData switch
            {
                TestData.AvailableDates => new List<object[]>
                            {
                                new object[] { DateTime.Today.AddDays(1) },
                                new object[] { DateTime.Today.AddDays(21) },
                            },
                TestData.NotAvailableDates => new List<object[]>
                            {
                                new object[] { DateTime.Today.AddDays(10) },
                                new object[] { DateTime.Today.AddDays(20) },
                            },
                TestData.DateAndExpectedDate => new List<object[]>
                            {
                                new object[] { DateTime.Today.AddDays(10) ,DateTime.Today.AddDays(13) , new List<DateTime>{ DateTime.Today.AddDays(10) , 
                                    DateTime.Today.AddDays(11), DateTime.Today.AddDays(12), DateTime.Today.AddDays(13) } },
                                new object[] { DateTime.Today.AddDays(1) ,DateTime.Today.AddDays(9) , new List<DateTime>() },
                            },
                TestData.PastDates => new List<object[]>
                            {
                                new object[] { DateTime.Today },
                                new object[] { DateTime.Today.AddDays(-1) },
                            },
                TestData.AvailableBookings => new List<object[]>
                            {
                                new object[] { DateTime.Today.AddDays(1), DateTime.Today.AddDays(1), true, 1, 1 },
                                new object[] { DateTime.Today.AddDays(21), DateTime.Today.AddDays(21), true, 1, 1 },
                            },
                TestData.NotAvailableBookings => new List<object[]>
                            {
                                new object[] { DateTime.Today.AddDays(10), DateTime.Today.AddDays(10), true, 1, 1 },
                                new object[] { DateTime.Today.AddDays(20), DateTime.Today.AddDays(20), true, 1, 1 },
                            },

                _ => null,
            };
        }

        public enum TestData
        {
            PastDates,
            AvailableDates,
            NotAvailableDates,
            AvailableBookings,
            NotAvailableBookings,
            DateAndExpectedDate,
        }

    }
}
