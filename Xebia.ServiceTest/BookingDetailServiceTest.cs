using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Xebia.Model;
using Xebia.Service;
using Xebia.Service.Interface;

namespace Xebia.ServiceTest
{
    [TestClass]
    public class BookingDetailServiceTest
    {
        private IBookingDetailServiceType<BookingDetailService> roomRepository;

        private IBookingDetailService bookingDetailService;

        [TestInitialize]
        public void StartTest()
        {
            this.roomRepository = new Mock<IBookingDetailServiceType<BookingDetailService>>().Object;
        }

        [TestMethod]
        public void GetEmployeeExpense()
        {
            var BookedBy = 1;
            var EmployeeExpense = this.bookingDetailService.GetEmployeeExpense(BookedBy);
            Assert.IsNotNull(EmployeeExpense);
            Assert.IsInstanceOfType(EmployeeExpense, typeof(MeetingDetails));
        }


        [TestCleanup]
        public void CleanTest()
        {
        }
    }
}
