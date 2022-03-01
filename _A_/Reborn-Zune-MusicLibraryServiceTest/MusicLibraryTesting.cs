using Microsoft.VisualStudio.TestTools.UnitTesting;
using Reborn_Zune_MusicLibraryService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reborn_Zune_MusicLibraryServiceTest
{
    [TestClass]
    public class MusicLibraryTesting
    {
        [TestMethod]
        public async Task TestCreateLibraryInstanceAsync()
        {
            MusicLibraryService service = new MusicLibraryService();
            await Task.Delay(1000);
            Assert.IsNotNull(service.Library);
            service.Clean();
        }

        [TestMethod]
        public async Task TestMusicImageFileLoadedAsync()
        {
            MusicLibraryService service = new MusicLibraryService();
            await Task.Delay(1000);
            service.Library.Thumbnails.ToList().ForEach(t => Assert.IsNotNull(t.Image));
            service.Library.Musics.ToList().ForEach(t => Assert.IsNotNull(t.File));
            service.Clean();
        }

        [TestMethod]
        public async Task TestCreatePlaylist()
        {
            MusicLibraryService service = new MusicLibraryService();
            await Task.Delay(1000);
            service.CreatePlaylist("a");
            service.CreatePlaylist("a");
            Assert.IsTrue(service.Library.Playlists.Count(p => p.Name == "a") == 1);
            service.Clean();
        }

        [TestMethod]
        public async Task TestEditPlaylistName()
        {
            MusicLibraryService service = new MusicLibraryService();
            await Task.Delay(1000);
            service.CreatePlaylist("a");
            Assert.IsTrue(service.Library.Playlists.Count(p => p.Name == "a") == 1);
            service.EditPlaylistName("a", "b");
            Assert.IsTrue(service.Library.Playlists.Count(p => p.Name == "a") == 0);
            Assert.IsTrue(service.Library.Playlists.Count(p => p.Name == "b") == 1);
            service.Clean();
        }

        [TestMethod]
        public async Task TestAddSongsIntoPlaylist()
        {
            MusicLibraryService service = new MusicLibraryService();
            await Task.Delay(1000);
            service.CreatePlaylist("a");
            Assert.IsTrue(service.Library.Playlists.Count(p => p.Name == "a") == 1);
            var songs = service.Library.Musics;
            service.AddSongsToPlaylist("a", songs.ToList());
            Assert.IsTrue(service.Library.MInP.Count == songs.Count);
            service.Clean();
        }

        [TestMethod]
        public async Task TestRemoveSongsIntoPlaylist()
        {
            MusicLibraryService service = new MusicLibraryService();
            await Task.Delay(1000);
            service.CreatePlaylist("a");
            Assert.IsTrue(service.Library.Playlists.Count(p => p.Name == "a") == 1);
            var songs = service.Library.Musics;
            service.AddSongsToPlaylist("a", songs.ToList());
            Assert.IsTrue(service.Library.MInP.Count == songs.Count);
            service.RemoveSongsFromPlaylist("a", songs.ToList());
            Assert.IsTrue(service.Library.MInP.Count == 0);
            service.Clean();
        }

        [TestMethod]
        public async Task TestSameSongOnMuliplePlaylist()
        {
            MusicLibraryService service = new MusicLibraryService();
            await Task.Delay(1000);
            service.CreatePlaylist("a");
            service.CreatePlaylist("b");
            Assert.IsTrue(service.Library.Playlists.Count == 2);
            var songs = service.Library.Musics;
            service.AddSongsToPlaylist("a", songs.ToList());
            service.AddSongsToPlaylist("b", songs.ToList());

            Assert.IsTrue(service.Library.MInP.Count == songs.Count * 2);

            service.Clean();
        }
    }
}
