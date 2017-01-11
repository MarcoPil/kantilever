using InfoSupport.WSA.Infrastructure;
using Kantilever.Magazijnbeheer.Commands;
using Kantilever.Magazijnbeheer.DAL;
using Kantilever.Magazijnbeheer.Entities;
using Kantilever.Magazijnbeheer.Events;
using Kantilever.Magazijnbeheer.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Kantilever.Magazijnbeheer.Test
{
    public class MagazijnServiceTest
    {
        [Fact]
        public void ZetArtikelInMagazijn_ZetNieuwArtikelInmagazijn()
        {
            // Arrange
            var dbOptions = CreateNewContextOptions();
            var publisherMock = new Mock<IEventPublisher>(MockBehavior.Loose);
            var target = new MagazijnService(dbOptions, publisherMock.Object);
            var command = new ZetArtikelInMagazijnCommand { ArtikelID = 3, Aantal = 15 };

            // Act
            target.ZetArtikelInMagazijn(command);

            // Assert
            using (var context = new MagazijnContext(dbOptions))
            {
                var voorraad = context.Voorraad.FirstOrDefault(v => v.ArtikelID == 3);
                Assert.NotNull(voorraad);
                Assert.Equal(3, voorraad.ArtikelID);
                Assert.Equal(15, voorraad.Voorraad);
            }
        }

        [Fact]
        public void ZetArtikelInMagazijn_publishes_ArtikelInMagazijnGezet()
        {
            // Arrange
            var dbOptions = CreateNewContextOptions();
            var publisherMock = new Mock<IEventPublisher>(MockBehavior.Strict);
            publisherMock.Setup(publisher => publisher.Publish(It.IsAny<ArtikelInMagazijnGezet>()));

            var target = new MagazijnService(dbOptions, publisherMock.Object);
            var command = new ZetArtikelInMagazijnCommand { ArtikelID = 3, Aantal = 15 };

            // Act
            target.ZetArtikelInMagazijn(command);

            // Assert
            publisherMock.Verify(publisher => publisher.Publish(
                It.Is<ArtikelInMagazijnGezet>(evt => evt.ArtikelID == 3 && evt.Voorraad == 15)));
        }

        [Fact]
        public void ZetArtikelInMagazijn_voegtArtikelenToeAanBestaande()
        {
            // Arrange
            var dbOptions = CreateNewContextOptions();
            using (var context = new MagazijnContext(dbOptions))
            {
                context.Voorraad.Add(new ArtikelVoorraad { ArtikelID = 7, Voorraad = 10 });
                context.SaveChanges();
            }
            var publisherMock = new Mock<IEventPublisher>(MockBehavior.Loose);
            var target = new MagazijnService(dbOptions, publisherMock.Object);
            var command = new ZetArtikelInMagazijnCommand { ArtikelID = 7, Aantal = 3 };

            // Act
            target.ZetArtikelInMagazijn(command);

            // Assert
            using (var context = new MagazijnContext(dbOptions))
            {
                var voorraad = context.Voorraad.FirstOrDefault(v => v.ArtikelID == 7);
                Assert.NotNull(voorraad);
                Assert.Equal(7, voorraad.ArtikelID);
                Assert.Equal(13, voorraad.Voorraad);
            }
        }


        [Fact]
        public void ZetArtikelInMagazijn_publishes_ArtikelInMagazijnGezet_metVerhoogdeVoorraad()
        {
            // Arrange
            var dbOptions = CreateNewContextOptions();
            using (var context = new MagazijnContext(dbOptions))
            {
                context.Voorraad.Add(new ArtikelVoorraad { ArtikelID = 17, Voorraad = 10 });
                context.SaveChanges();
            }
            var publisherMock = new Mock<IEventPublisher>(MockBehavior.Strict);
            publisherMock.Setup(publisher => publisher.Publish(It.IsAny<ArtikelInMagazijnGezet>()));

            var target = new MagazijnService(dbOptions, publisherMock.Object);
            var command = new ZetArtikelInMagazijnCommand { ArtikelID = 17, Aantal = 4 };

            // Act
            target.ZetArtikelInMagazijn(command);

            // Assert
            publisherMock.Verify(publisher => publisher.Publish(
                It.Is<ArtikelInMagazijnGezet>(evt => evt.ArtikelID == 17 && evt.Voorraad == 14)));
        }

        [Fact]
        public void HaalArtikelUitMagazijnCommand_HaaltBestaandArtikelUitMagazijn()
        {
            
            // Arrange
            var dbOptions = CreateNewContextOptions();
            using (var context = new MagazijnContext(dbOptions))
            {
                context.Voorraad.Add(new ArtikelVoorraad { ArtikelID = 7, Voorraad = 10 });
                context.SaveChanges();
            }
            var publisherMock = new Mock<IEventPublisher>(MockBehavior.Loose);
            var target = new MagazijnService(dbOptions, publisherMock.Object);
            var command = new HaalArtikelUitMagazijnCommand { ArtikelID = 7, Aantal = 4 };

            // Act
            target.HaalArtikelUitMagazijn(command);

            // Assert
            using (var context = new MagazijnContext(dbOptions))
            {
                var voorraad = context.Voorraad.FirstOrDefault(v => v.ArtikelID == 7);
                Assert.NotNull(voorraad);
                Assert.Equal(6, voorraad.Voorraad);
            }
        }

        [Fact]
        public void HaalArtikelUitMagazijnCommand_publishes_ArtikelInMagazijnGezet_metVerhoogdeVoorraad()
        {
            // Arrange
            var dbOptions = CreateNewContextOptions();
            using (var context = new MagazijnContext(dbOptions))
            {
                context.Voorraad.Add(new ArtikelVoorraad { ArtikelID = 17, Voorraad = 10 });
                context.SaveChanges();
            }
            var publisherMock = new Mock<IEventPublisher>(MockBehavior.Strict);
            publisherMock.Setup(publisher => publisher.Publish(It.IsAny<ArtikelUitMagazijnGehaald>()));

            var target = new MagazijnService(dbOptions, publisherMock.Object);
            var command = new HaalArtikelUitMagazijnCommand { ArtikelID = 17, Aantal = 2 };

            // Act
            target.HaalArtikelUitMagazijn(command);

            // Assert
            publisherMock.Verify(publisher => publisher.Publish(
                It.Is<ArtikelUitMagazijnGehaald>(evt => evt.ArtikelID == 17 && evt.Voorraad == 8)));
        }

        [Fact]
        public void HaalArtikelUitMagazijnCommand_throwsArtikelOnbekendException_alsArtikelNietBestaat()
        {

            // Arrange
            var dbOptions = CreateNewContextOptions();
            var publisherMock = new Mock<IEventPublisher>(MockBehavior.Loose);
            var target = new MagazijnService(dbOptions, publisherMock.Object);
            var command = new HaalArtikelUitMagazijnCommand { ArtikelID = 7, Aantal = 4 };

            // Act
            Action act = () => target.HaalArtikelUitMagazijn(command);

            // Assert
            Assert.Throws<ArtikelOnbekendException>(act);
        }

        [Fact]
        public void HaalArtikelUitMagazijnCommand_throwsOnvoldoendeVoorraadException_alsArtikelNietBestaat()
        {

            // Arrange
            var dbOptions = CreateNewContextOptions();
            using (var context = new MagazijnContext(dbOptions))
            {
                context.Voorraad.Add(new ArtikelVoorraad { ArtikelID = 17, Voorraad = 3 });
                context.SaveChanges();
            }
            var publisherMock = new Mock<IEventPublisher>(MockBehavior.Loose);
            var target = new MagazijnService(dbOptions, publisherMock.Object);
            var command = new HaalArtikelUitMagazijnCommand { ArtikelID = 17, Aantal = 4 };

            // Act
            Action act = () => target.HaalArtikelUitMagazijn(command);

            // Assert
            Assert.Throws<OnvoldoendeVoorraadException>(act);
        }


        [Fact]
        public void BijOnvoldoendeVoorraadExceptionWordtEr_Geen_EventGepubliceerd()
        {

            // Arrange
            var dbOptions = CreateNewContextOptions();
            using (var context = new MagazijnContext(dbOptions))
            {
                context.Voorraad.Add(new ArtikelVoorraad { ArtikelID = 17, Voorraad = 3 });
                context.SaveChanges();
            }
            var publisherMock = new Mock<IEventPublisher>(MockBehavior.Strict);
            publisherMock.Setup(publisher => publisher.Publish(It.IsAny<ArtikelUitMagazijnGehaald>()));
            var target = new MagazijnService(dbOptions, publisherMock.Object);
            var command = new HaalArtikelUitMagazijnCommand { ArtikelID = 17, Aantal = 4 };

            // Act
            Action act = () => target.HaalArtikelUitMagazijn(command);

            // Assert
            Assert.Throws<OnvoldoendeVoorraadException>(act);
            publisherMock.Verify(publisher => publisher.Publish(It.IsAny<ArtikelUitMagazijnGehaald>()), Times.Never);
        }

        #region Settings for in-memory database
        private static DbContextOptions<MagazijnContext> CreateNewContextOptions()
        {
            // Create a fresh service provider, and therefore a fresh 
            // InMemory database instance.
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            // Create a new options instance telling the context to use an
            // InMemory database and the new service provider.
            var builder = new DbContextOptionsBuilder<MagazijnContext>();
            builder.UseInMemoryDatabase()
                   .UseInternalServiceProvider(serviceProvider);

            return builder.Options;
        }
        #endregion
    }
}
