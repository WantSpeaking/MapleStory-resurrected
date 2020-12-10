using System.IO;
using NAudio.Wave;

namespace MapleLib.WzLib.WzProperties
{
    public partial class WzBinaryProperty
    {
        #region Cast Values

        // 歌曲/音效 时长 可以判断 是否播放完毕...
        public int Time() => Length;

        // 支持mono game的原生音频播放 ex: SoundEffect.FromStream(new MemoryStream(Wav()))   ;   
        public byte[] Wav()
        {
            // 需要引入 NAudio 依赖 我用的是.net core 3.1 ,引入的NAudio 1.10.0
            using (var mp3Stream = new MemoryStream(GetBytes(false)))
            using (var mp3Reader = new Mp3FileReader(mp3Stream))
            using (var sourceProvider = WaveFormatConversionStream.CreatePcmStream(mp3Reader))
            using (var outStream = new MemoryStream())
            using (var waveFileWriter = new WaveFileWriter(outStream, sourceProvider.WaveFormat))
            {
                var buffer = new byte[sourceProvider.WaveFormat.AverageBytesPerSecond * 4];
                while (true)
                {
                    var count = sourceProvider.Read(buffer, 0, buffer.Length);
                    if (count == 0) break;
                    waveFileWriter.Write(buffer, 0, count);
                }

                mp3Stream.Dispose();
                mp3Reader.Dispose();
                sourceProvider.Dispose();
                waveFileWriter.Dispose();
                var bytes = outStream.GetBuffer();
                outStream.Dispose();
                return bytes;
            }
			
            /* using var mp3Stream = new MemoryStream(GetBytes(false));
			using var mp3Reader = new Mp3FileReader(mp3Stream);
			using var sourceProvider = WaveFormatConversionStream.CreatePcmStream(mp3Reader);
			using var outStream = new MemoryStream();
			using var waveFileWriter = new WaveFileWriter(outStream, sourceProvider.WaveFormat); */

        }

        public override byte[] GetWavData(out WavInfo info)
        {
            info.length = Length;
            return Wav();
        }

        #endregion
    }
}