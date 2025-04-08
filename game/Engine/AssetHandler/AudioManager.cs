using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;

namespace Blok3Game.Engine.AssetHandler
{
    /// <summary>
    ///     Class for creating an <see cref="AudioManager" />.
    /// </summary>
    public sealed class AudioManager
    {
        private static readonly string FilePathContentAudioEffects = Path.Combine("Content", "Audio", "AudioEffects");
        private static readonly string FilePathAudioEffects = Path.Combine("Audio", "AudioEffects");
        private static readonly string FilePathContentMusic = Path.Combine("Content", "Audio", "Music");
        private static readonly string FilePathMusic = Path.Combine("Audio", "Music");

        private readonly Dictionary<string, Song> songs = new Dictionary<string, Song>();
        private readonly Dictionary<string, SoundEffect> soundEffects = new Dictionary<string, SoundEffect>();

        /// <summary>
        /// Music volume property.
        /// </summary>
        /// <remarks> Use this to change the music volume. </remarks>
        public float MusicVolume { get; set; } = 0.5f;

        /// <summary>
        ///     Sound effect volume property.
        /// </summary>
        /// <remarks> Use this to change the sound effects volume. </remarks>
        public float EffectVolume { get; set; } = 0.5f;

        /// <summary>
        ///     Loads all sound effect assets.
        /// </summary>
        public void LoadAllAudio(ContentManager contentManager)
        {
            LoadSoundEffects(contentManager);
            LoadSongs();
        }

        private void LoadSoundEffects(ContentManager contentManager)
        {
            foreach (var filePath in Directory.GetFiles(FilePathContentAudioEffects))
            {
                var assetName = Path.GetFileNameWithoutExtension(filePath);

                soundEffects.Add(assetName, contentManager.Load<SoundEffect>(Path.Combine(FilePathAudioEffects, assetName)));
            }
        }

        private void LoadSongs()
        {
            foreach (var filePath in Directory.GetFiles(FilePathContentMusic))
            {
                var songName = Path.GetFileNameWithoutExtension(filePath);

                if (songs.ContainsKey(songName)) continue;

                songs.Add(songName, Song.FromUri(songName,
                    new Uri(Path.Combine(FilePathContentMusic, songName + ".ogg"), UriKind.Relative)));
            }

        }

        /// <summary>
        ///     Loads all song assets.
        /// </summary>
        public void LoadAllSongs(ContentManager contentManager)
        {
            foreach (var filePath in Directory.GetFiles(FilePathContentMusic))
            {
                var assetName = Path.GetFileNameWithoutExtension(filePath);

                songs.Add(assetName, contentManager.Load<Song>(Path.Combine(FilePathMusic, assetName)));
            }
        }

        /// <summary>
        /// Play song from the song dictionary
        /// </summary>
        /// <param name="assetName"> 
		/// Song file to start playing, without the extension. This name should be the samen as the file name.
		/// </param>
        /// <param name="repeat">
		/// Whether or not the song should repeat.
		/// </param>
        public void PlaySong(string assetName, bool repeat = false)
        {
            if (string.IsNullOrEmpty(assetName)) throw new ArgumentNullException(nameof(assetName));

            MediaPlayer.Volume = MusicVolume;
            MediaPlayer.IsRepeating = repeat;

            MediaPlayer.Play(songs[assetName]);
        }

        /// <summary>
        /// Stops all playing music.
        /// </summary>
        public void StopSong()
        {
            MediaPlayer.Stop();
        }

        /// <summary>
        /// Pauses all music.
        /// </summary>
        public void PauseSong()
        {
            MediaPlayer.Pause();
        }

        /// <summary>
        /// Resumes all paused music.
        /// </summary>
        public void ResumeSong()
        {
            MediaPlayer.Resume();
        }

        /// <summary>
        /// Sets the music volume with the <see cref="MusicVolume" /> property.
        /// </summary>
        public void ChangeMusicVolume()
        {
            MediaPlayer.Volume = MusicVolume;
        }

        /// <summary>
        /// Play sound effect from the effects dictionary.
        /// </summary>
        /// <param name="assetName"> Sound effect to play. </param>
        /// <param name="pitch"> Float to change the pitch of sound effect. </param>
        /// <param name="pan"> Float to change the panning of the sound effect, -1.0 left, 1.0 right. </param>
        public void PlaySoundEffect(string assetName, float pitch = 0, float pan = 0f)
        {
            if (string.IsNullOrEmpty(assetName))
            {
                throw new ArgumentNullException(nameof(assetName));
            }

            if (!soundEffects.ContainsKey(assetName))
            {
                throw new ArgumentException($"Sound effect ({assetName}) not found in dictionary. Did you use the correct name?");
            }

            soundEffects[assetName].Play(EffectVolume, pitch, pan);
        }
    }
}