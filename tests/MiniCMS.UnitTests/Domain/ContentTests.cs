using System;
using MiniCMS.Domain.Entities;
using Xunit;

namespace MiniCMS.UnitTests.Domain
{
    public class ContentTests
    {
        [Fact]
        public void NewContent_ShouldHaveDraftStatus()
        {
            // Arrange
            var appId = Guid.NewGuid();
            var schemaId = Guid.NewGuid();
            var data = "{\"title\": \"Hello\"}";

            // Act
            var content = new Content(appId, schemaId, data);

            // Assert
            Assert.Equal(ContentStatus.Draft, content.Status);
            Assert.Equal(1, content.Version);
            Assert.Equal(data, content.Data);
        }

        [Fact]
        public void Publish_ShouldChangeStatusAndSetUser()
        {
            // Arrange
            var content = new Content(Guid.NewGuid(), Guid.NewGuid(), "{}");
            var userId = "admin";

            // Act
            content.Publish(userId);

            // Assert
            Assert.Equal(ContentStatus.Published, content.Status);
            Assert.Equal(userId, content.PublishedBy);
            Assert.NotNull(content.PublishedAt);
        }

        [Fact]
        public void Publish_ShouldThrowIfAlreadyPublished()
        {
            // Arrange
            var content = new Content(Guid.NewGuid(), Guid.NewGuid(), "{}");
            content.Publish("user");

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => content.Publish("user"));
        }
    }
}
