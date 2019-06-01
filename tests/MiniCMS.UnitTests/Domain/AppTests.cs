using System;
using MiniCMS.Domain.Entities;
using Xunit;

namespace MiniCMS.UnitTests.Domain
{
    public class AppTests
    {
        [Fact]
        public void NewApp_ShouldHaveCorrectNameAndDisplayName()
        {
            // Arrange
            var name = "Test App";
            var displayName = "My Test App";

            // Act
            var app = new App(name, displayName);

            // Assert
            Assert.Equal("test-app", app.Name);
            Assert.Equal(displayName, app.DisplayName);
            Assert.False(app.IsArchived);
            Assert.Empty(app.Contributors);
        }

        [Fact]
        public void SetName_ShouldThrowIfEmpty()
        {
            // Arrange
            var app = new App("valid", "Valid");

            // Act & Assert
            Assert.Throws<ArgumentException>(() => app.SetName(""));
        }

        [Fact]
        public void AddContributor_ShouldAddUser()
        {
            // Arrange
            var app = new App("app", "App");
            var userId = "user1";
            var role = ContributorRole.Editor;

            // Act
            app.AddContributor(userId, role);

            // Assert
            Assert.Single(app.Contributors);
            Assert.Equal(userId, app.Contributors[0].UserId);
            Assert.Equal(role, app.Contributors[0].Role);
        }
    }
}
