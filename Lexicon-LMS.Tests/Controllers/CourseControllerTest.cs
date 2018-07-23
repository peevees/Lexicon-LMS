using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Lexicon_LMS;
using Lexicon_LMS.Controllers;
using NSubstitute;

namespace Lexicon_LMS.Tests.Controllers
{
    [TestClass]
    public class CourseControllerTest
    {
        [TestMethod]
        public void Index()
        {
            //// Arrange
            //CoursesController controller = new CoursesController();
            var controller = Substitute.For<CoursesController>();
            //// Act
            //ViewResult result = controller.Index() as ViewResult;



            //// Assert
            //Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Create()
        {
        //    // Arrange
        //    CoursesController controller = new CoursesController();

        //    // Act
        //    ViewResult result = controller.Create() as ViewResult;

        //    // Assert
        //    Assert.AreEqual("Your application description page.", result.Model);
        }

        [TestMethod]
        public void Details()
        {
            //// Arrange
            //CoursesController controller = new CoursesController();
            //int Id = 1;

            //// Act
            //ViewResult result = controller.Details(Id) as ViewResult;

            //// Assert
            //Assert.IsNotNull(result);
        }
    }
}
