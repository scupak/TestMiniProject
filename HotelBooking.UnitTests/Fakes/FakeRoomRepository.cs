using System.Collections.Generic;
using HotelBooking.Core;

namespace HotelBooking.UnitTests.Fakes
{
    public class FakeRoomRepository : IRepository<Room>
    {
        // This field is exposed so that a unit test can validate that the
        // Add method was invoked.
        public bool addWasCalled = false;

        public void Add(Room entity)
        {
            addWasCalled = true;
        }

        // This field is exposed so that a unit test can validate that the
        // Edit method was invoked.
        public bool editWasCalled = false;

        public void Edit(Room entity)
        {
            editWasCalled = true;
        }

        public Room Get(int id)
        {
            return new Room { Id = 1, Description = "A" };
        }

        public IEnumerable<Room> GetAll()
        {
            List<Room> rooms = new List<Room>
            {
                new Room { Id=1, Description="A" },
                new Room { Id=2, Description="B" },
            };
            return rooms;
        }

        // This field is exposed so that a unit test can validate that the
        // Remove method was invoked.
        public bool removeWasCalled = false;

        public void Remove(int id)
        {
            removeWasCalled = true;
        }
    }
}
