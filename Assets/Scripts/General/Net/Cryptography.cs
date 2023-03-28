#define USE_CRYPTO





using UnityEngine.UIElements;

namespace ms
{
	// Used to encrypt and decrypt packets for communication with the server
	public class Cryptography : System.IDisposable
	{
		// Obtain the initialization vector from the handshake
		public Cryptography(sbyte[] handshake)
		{
#if USE_CRYPTO
			for (int i = 0; i < NetConstants.HEADER_LENGTH; i++)
			{
				sendiv[i] = (byte)handshake[i + 7];
			}

			for (int i = 0; i < NetConstants.HEADER_LENGTH; i++)
			{
				recviv[i] = (byte)handshake[i + 11];
			}
			AppDebug.Log($"(sbyte[]) sendiv:{sendiv.ToDebugLog()}\t recviv:{recviv.ToDebugLog()}");
#endif
		}
		public Cryptography()
		{
            AppDebug.Log($"() sendiv:{sendiv.ToDebugLog()}\t recviv:{recviv.ToDebugLog()}");

        }
        public Cryptography(byte[] receiveIv, byte[] sendIv)
        {
			this.sendiv = sendIv;
			this.recviv = receiveIv;
            AppDebug.Log($"(byte[] byte[]) sendiv:{sendiv.ToDebugLog()}\t recviv:{recviv.ToDebugLog()}");

        }
        public void Dispose()
		{
		}

		// Encrypt a byte array with the given length and iv
		public void encrypt(sbyte[] bytes, int length)
		{
#if USE_CRYPTO
            //AppDebug.Log("send iv");
            //AppDebug.Log(sendiv.ToDebugLog());
            //AppDebug.Log("raw");
            //AppDebug.Log(bytes.ToDebugLog());
            mapleencrypt( bytes, length);
            //AppDebug.Log("mapleencrypt");
            //AppDebug.Log(bytes.ToDebugLog());
            aesofb( bytes, length, sendiv);
            //AppDebug.Log("aesofb");
            //AppDebug.Log(bytes.ToDebugLog());
#endif
        }
		// Decrypt a byte array with the given length and iv
		public void decrypt(sbyte[] bytes, int length)
		{
#if USE_CRYPTO
			aesofb( bytes, length, recviv);
			mapledecrypt( bytes, length);
#endif
		}
        public void aesofb(sbyte[] bytes, int length)
		{
            aesofb(bytes, length, recviv);
        }
     
        // Generate a header for the specified length and key
        //C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: void create_header(byte* buffer, int length) const
        public void create_header(sbyte[] buffer, int length)
		{
#if USE_CRYPTO
			const byte MAPLEVERSION = 83;

            var a = (int)(((sendiv[3] << 8) | (byte)sendiv[2]) ^ MAPLEVERSION);
			var b = a ^ length;

			buffer[0] = (sbyte)(a % 0x100);
			buffer[1] = (sbyte)(a / 0x100);
			buffer[2] = (sbyte)(b % 0x100);
			buffer[3] = (sbyte)(b / 0x100);
#else
			int length = (int)slength;

			for (int i = 0; i < HEADERLEN; i++)
			{
				buffer[i] = (byte)length;
				length = length >> 8;
			}
#endif
		}
		// Use the 4-byte header of a received packet to determine its length
		public int check_length(sbyte[] bytes)
		{
#if USE_CRYPTO
			long headermask = 0;

			for (int i = 0; i < 4; i++)
			{
				headermask = headermask| (uint)((byte)bytes[i] << (8 * i));
				//AppDebug.Log($"heaadMask:{headermask}");
			}
			var result = (int)((headermask >> 16) ^ (headermask & 0xFFFF));
			//AppDebug.Log($"headerBytes:{bytes.ToDebugLog()}");
            //AppDebug.Log($"check_length: {result}\t headerBytes:{bytes.ToDebugLog()}");

            return result;
#else
			int length = 0;

			for (int i = 0; i < HEADERLEN; i++)
			{
				length += (byte)bytes[i] << (8 * i);
			}

			return length;
#endif
		}

		// Add the maple custom encryption
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void mapleencrypt(byte* bytes, int length) const
		private void mapleencrypt(sbyte[] bytes, int length)
		{
			for (sbyte j = 0; j < 3; j++)
			{
				sbyte remember = 0;
				sbyte datalen = (sbyte)(length & 0xFF);

				for (int i = 0; i < length; i++)
				{
					sbyte cur = (sbyte)((rollleft(bytes[i], 3) + datalen) ^ remember);
					remember = cur;
					cur = rollright(cur, ((int)datalen) & 0xFF);
					bytes[i] = (sbyte)((sbyte)((~cur) & 0xFF) + 0x48);
					//AppDebug.Log($"bytes[{i}]= {bytes[i]}");
					datalen--;
				}

				remember = 0;
				datalen = (sbyte)(length & 0xFF);

				for (int i = length-1; i>=0;i--)
				{
					sbyte cur = (sbyte)((rollleft(bytes[i], 4) + datalen) ^ remember);
					remember = cur;
					bytes[i] = rollright((sbyte)(cur ^ 0x13), 3);
                    //AppDebug.Log($"bytes[{i}]= {bytes[i]}");
                    datalen--;
				}
			}
		}
		// Remove the maple custom encryption
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void mapledecrypt(byte* bytes, int length) const
		public void mapledecrypt(sbyte[] bytes, int length)
		{
			for (int i = 0; i < 3; i++)
			{
				byte remember = 0;
				byte datalen = (byte)(length & 0xFF);

				for (int j = length-1;j>=0; j--)
				{
					byte cur = (byte)(rollleft(bytes[j], 3) ^ 0x13);
					bytes[j] = rollright((sbyte)((cur ^ remember) - datalen), 4);
					remember = cur;
					datalen--;
				}

				remember = 0;
				datalen = (byte)(length & 0xFF);

				for (int j = 0; j < length; j++)
				{
					byte cur = (byte)((~(bytes[j] - 0x48)) & 0xFF);
					cur = (byte)rollleft((sbyte)cur, (int)datalen & 0xFF);
					bytes[j] = rollright((sbyte)((cur ^ remember) - datalen), 3);
					remember = cur;
					datalen--;
				}
			}
		}

        static byte[] maplebytes = { 0xEC, 0x3F, 0x77, 0xA4, 0x45, 0xD0, 0x71, 0xBF, 0xB7, 0x98, 0x20, 0xFC, 0x4B, 0xE9, 0xB3, 0xE1, 0x5C, 0x22, 0xF7, 0x0C, 0x44, 0x1B, 0x81, 0xBD, 0x63, 0x8D, 0xD4, 0xC3, 0xF2, 0x10, 0x19, 0xE0, 0xFB, 0xA1, 0x6E, 0x66, 0xEA, 0xAE, 0xD6, 0xCE, 0x06, 0x18, 0x4E, 0xEB, 0x78, 0x95, 0xDB, 0xBA, 0xB6, 0x42, 0x7A, 0x2A, 0x83, 0x0B, 0x54, 0x67, 0x6D, 0xE8, 0x65, 0xE7, 0x2F, 0x07, 0xF3, 0xAA, 0x27, 0x7B, 0x85, 0xB0, 0x26, 0xFD, 0x8B, 0xA9, 0xFA, 0xBE, 0xA8, 0xD7, 0xCB, 0xCC, 0x92, 0xDA, 0xF9, 0x93, 0x60, 0x2D, 0xDD, 0xD2, 0xA2, 0x9B, 0x39, 0x5F, 0x82, 0x21, 0x4C, 0x69, 0xF8, 0x31, 0x87, 0xEE, 0x8E, 0xAD, 0x8C, 0x6A, 0xBC, 0xB5, 0x6B, 0x59, 0x13, 0xF1, 0x04, 0x00, 0xF6, 0x5A, 0x35, 0x79, 0x48, 0x8F, 0x15, 0xCD, 0x97, 0x57, 0x12, 0x3E, 0x37, 0xFF, 0x9D, 0x4F, 0x51, 0xF5, 0xA3, 0x70, 0xBB, 0x14, 0x75, 0xC2, 0xB8, 0x72, 0xC0, 0xED, 0x7D, 0x68, 0xC9, 0x2E, 0x0D, 0x62, 0x46, 0x17, 0x11, 0x4D, 0x6C, 0xC4, 0x7E, 0x53, 0xC1, 0x25, 0xC7, 0x9A, 0x1C, 0x88, 0x58, 0x2C, 0x89, 0xDC, 0x02, 0x64, 0x40, 0x01, 0x5D, 0x38, 0xA5, 0xE2, 0xAF, 0x55, 0xD5, 0xEF, 0x1A, 0x7C, 0xA7, 0x5B, 0xA6, 0x6F, 0x86, 0x9F, 0x73, 0xE6, 0x0A, 0xDE, 0x2B, 0x99, 0x4A, 0x47, 0x9C, 0xDF, 0x09, 0x76, 0x9E, 0x30, 0x0E, 0xE4, 0xB2, 0x94, 0xA0, 0x3B, 0x34, 0x1D, 0x28, 0x0F, 0x36, 0xE3, 0x23, 0xB4, 0x03, 0xD8, 0x90, 0xC8, 0x3C, 0xFE, 0x5E, 0x32, 0x24, 0x50, 0x1F, 0x3A, 0x43, 0x8A, 0x96, 0x41, 0x74, 0xAC, 0x52, 0x33, 0xF0, 0xD9, 0x29, 0x80, 0xB1, 0x16, 0xD3, 0xAB, 0x91, 0xB9, 0x84, 0x7F, 0x61, 0x1E, 0xCF, 0xC5, 0xD1, 0x56, 0x3D, 0xCA, 0xF4, 0x05, 0xC6, 0xE5, 0x08, 0x49 };

        // Update a key
        //C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: void updateiv(byte* iv) const
        public void updateiv(byte[] iv)
		{
			//AppDebug.Log("updateiv before");
			//AppDebug.Log(iv.ToDebugLog());

            byte[] mbytes = { 0xF2, 0x53, 0x50, 0xC6 };

            for (int i = 0; i < 4; i++)
			{
				byte ivbyte = iv[i];
				mbytes[0] += (byte)(maplebytes[mbytes[1] & 0xFF] - ivbyte);
				mbytes[1] -= (byte)(mbytes[2] ^ maplebytes[ivbyte & 0xFF] & 0xFF);
				mbytes[2] ^= (byte)(maplebytes[mbytes[3] & 0xFF] + ivbyte);
				mbytes[3] += (byte)((maplebytes[ivbyte & 0xFF] & 0xFF) - (mbytes[0] & 0xFF));
				/*AppDebug.Log($"mbytes[0] |{mbytes[0]}");
				AppDebug.Log($"mbytes[1] |{mbytes[1]}");
				AppDebug.Log($"mbytes[2] |{mbytes[2]}");
				AppDebug.Log($"mbytes[3] |{mbytes[3]}");*/

				long mask = 0;
				mask = mask | ((uint)((mbytes[0]) & 0xFF));
				//AppDebug.Log($"mask[0] |{mask}");
				mask = mask | ((uint)((mbytes[1] << 8) & 0xFF00));
				//AppDebug.Log($"mask[1] |{mask}");
				mask = mask | ((uint)((mbytes[2] << 16) & 0xFF0000));
				//AppDebug.Log($"mask[2] |{mask}");
				mask = mask | ((uint)((mbytes[3] << 24) & 0xFF000000));
				//AppDebug.Log($"mask[3] |{mask}");
				mask = (mask >> 0x1D) | (uint)(mask << 3);
				//AppDebug.Log($"mask[4] |{mask}");

				for (int j = 0; j < 4; j++)
				{
					long value = mask >> (8 * j);
					mbytes[j] = (byte)(value & 0xFF);
					//AppDebug.Log($"mbytes[{j}] |{mbytes[j]}");
				}
			}

			for (int i = 0; i < 4; i++)
			{
				iv[i] = mbytes[i];
			}

			//AppDebug.Log("updateiv after");
			//AppDebug.Log(iv.ToDebugLog());
		}
		// Perform a roll-left operation
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: byte rollleft(byte data, int count) const
		private sbyte rollleft(sbyte data, int count)
		{
			int mask = (data & 0xFF) << (count % 8);

			return (sbyte)((mask & 0xFF) | (mask >> 8));
		}
		// Perform a roll-right operation
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: byte rollright(byte data, int count) const
		private sbyte rollright(sbyte data, int count)
		{
			int mask = ((data & 0xFF) << 8) >> (count % 8);

			return (sbyte)((mask & 0xFF) | (mask >> 8));
		}

		// Apply AES OFB to a byte array
		private void aesofb(sbyte[] bytes, int length, byte[] iv)
		{
			int blocklength = 0x5B0;
			int offset = 0;

			while (offset < length)
			{
				byte[] miv = new byte[16];

				for (int i = 0; i < 16; i++)
				{
					miv[i] = iv[i % 4];
				}

				int remaining = length - offset;

				if (remaining > blocklength)
				{
					remaining = blocklength;
				}

				for (int x = 0; x < remaining; x++)
				{
					int relpos = x % 16;

					if (relpos == 0)
					{
						aesencrypt( miv);
					}

					bytes[x + offset] ^= (sbyte)miv[relpos];
				}

				offset += blocklength;
				blocklength = 0x5B4;
			}

			updateiv(iv);
		}
		// Encrypt a byte array with AES
		private void aesencrypt( byte[] bytes)
		{
			byte round = 0;
            //AppDebug.Log("raw");
            //AppDebug.Log(bytes.ToDebugLog());
			addroundkey(bytes, round);
            //AppDebug.Log("addroundkey");
            //AppDebug.Log(bytes.ToDebugLog());

            for (round = 1; round < 14; round++)
			{
				subbytes(bytes);
                //AppDebug.Log("subbytes");
                //AppDebug.Log(bytes.ToDebugLog());

                shiftrows(bytes);
                //AppDebug.Log("shiftrows");
                //AppDebug.Log(bytes.ToDebugLog());

                mixcolumns(bytes);
                //AppDebug.Log("mixcolumns");
                //AppDebug.Log(bytes.ToDebugLog());

                addroundkey(bytes, round);
                //AppDebug.Log("addroundkey");
                //AppDebug.Log(bytes.ToDebugLog());

            }

            subbytes(bytes);
            //AppDebug.Log("subbytes");
            //AppDebug.Log(bytes.ToDebugLog());

            shiftrows(bytes);
            //AppDebug.Log("shiftrows");
            //AppDebug.Log(bytes.ToDebugLog());

            addroundkey(bytes, round);
            //AppDebug.Log("addroundkey");
            //AppDebug.Log(bytes.ToDebugLog());

        }
        // AES add round key step
        //C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: void addroundkey(byte* bytes, byte round) const
        private void addroundkey(byte[] bytes, byte round)
		{
			// This key is already expanded
			// Only works for versions lower than version 118
			byte[] maplekey = {0x13, 0x00, 0x00, 0x00, 0x08, 0x00, 0x00, 0x00, 0x06, 0x00, 0x00, 0x00, 0xB4, 0x00, 0x00, 0x00, 0x1B, 0x00, 0x00, 0x00, 0x0F, 0x00, 0x00, 0x00, 0x33, 0x00, 0x00, 0x00, 0x52, 0x00, 0x00, 0x00, 0x71, 0x63, 0x63, 0x00, 0x79, 0x63, 0x63, 0x00, 0x7F, 0x63, 0x63, 0x00, 0xCB, 0x63, 0x63, 0x00, 0x04, 0xFB, 0xFB, 0x63, 0x0B, 0xFB, 0xFB, 0x63, 0x38, 0xFB, 0xFB, 0x63, 0x6A, 0xFB, 0xFB, 0x63, 0x7C, 0x6C, 0x98, 0x02, 0x05, 0x0F, 0xFB, 0x02, 0x7A, 0x6C, 0x98, 0x02, 0xB1, 0x0F, 0xFB, 0x02, 0xCC, 0x8D, 0xF4, 0x14, 0xC7, 0x76, 0x0F, 0x77, 0xFF, 0x8D, 0xF4, 0x14, 0x95, 0x76, 0x0F, 0x77, 0x40, 0x1A, 0x6D, 0x28, 0x45, 0x15, 0x96, 0x2A, 0x3F, 0x79, 0x0E, 0x28, 0x8E, 0x76, 0xF5, 0x2A, 0xD5, 0xB5, 0x12, 0xF1, 0x12, 0xC3, 0x1D, 0x86, 0xED, 0x4E, 0xE9, 0x92, 0x78, 0x38, 0xE6, 0xE5, 0x4F, 0x94, 0xB4, 0x94, 0x0A, 0x81, 0x22, 0xBE, 0x35, 0xF8, 0x2C, 0x96, 0xBB, 0x8E, 0xD9, 0xBC, 0x3F, 0xAC, 0x27, 0x94, 0x2D, 0x6F, 0x3A, 0x12, 0xC0, 0x21, 0xD3, 0x80, 0xB8, 0x19, 0x35, 0x65, 0x8B, 0x02, 0xF9, 0xF8, 0x81, 0x83, 0xDB, 0x46, 0xB4, 0x7B, 0xF7, 0xD0, 0x0F, 0xF5, 0x2E, 0x6C, 0x49, 0x4A, 0x16, 0xC4, 0x64, 0x25, 0x2C, 0xD6, 0xA4, 0x04, 0xFF, 0x56, 0x1C, 0x1D, 0xCA, 0x33, 0x0F, 0x76, 0x3A, 0x64, 0x8E, 0xF5, 0xE1, 0x22, 0x3A, 0x8E, 0x16, 0xF2, 0x35, 0x7B, 0x38, 0x9E, 0xDF, 0x6B, 0x11, 0xCF, 0xBB, 0x4E, 0x3D, 0x19, 0x1F, 0x4A, 0xC2, 0x4F, 0x03, 0x57, 0x08, 0x7C, 0x14, 0x46, 0x2A, 0x1F, 0x9A, 0xB3, 0xCB, 0x3D, 0xA0, 0x3D, 0xDD, 0xCF, 0x95, 0x46, 0xE5, 0x51, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00};

			byte offset = (byte)(round * 16);

			for (byte i = 0; i < 16; i++)
			{
				bytes[i] ^= maplekey[i + offset];
			}
		}
		// AES sub bytes step
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void subbytes(byte* bytes) const
		private void subbytes(byte[] bytes)
		{
			// Rijndael substitution box
			byte[] subbox = {0x63, 0x7C, 0x77, 0x7B, 0xF2, 0x6B, 0x6F, 0xC5, 0x30, 0x01, 0x67, 0x2B, 0xFE, 0xD7, 0xAB, 0x76, 0xCA, 0x82, 0xC9, 0x7D, 0xFA, 0x59, 0x47, 0xF0, 0xAD, 0xD4, 0xA2, 0xAF, 0x9C, 0xA4, 0x72, 0xC0, 0xB7, 0xFD, 0x93, 0x26, 0x36, 0x3F, 0xF7, 0xCC, 0x34, 0xA5, 0xE5, 0xF1, 0x71, 0xD8, 0x31, 0x15, 0x04, 0xC7, 0x23, 0xC3, 0x18, 0x96, 0x05, 0x9A, 0x07, 0x12, 0x80, 0xE2, 0xEB, 0x27, 0xB2, 0x75, 0x09, 0x83, 0x2C, 0x1A, 0x1B, 0x6E, 0x5A, 0xA0, 0x52, 0x3B, 0xD6, 0xB3, 0x29, 0xE3, 0x2F, 0x84, 0x53, 0xD1, 0x00, 0xED, 0x20, 0xFC, 0xB1, 0x5B, 0x6A, 0xCB, 0xBE, 0x39, 0x4A, 0x4C, 0x58, 0xCF, 0xD0, 0xEF, 0xAA, 0xFB, 0x43, 0x4D, 0x33, 0x85, 0x45, 0xF9, 0x02, 0x7F, 0x50, 0x3C, 0x9F, 0xA8, 0x51, 0xA3, 0x40, 0x8F, 0x92, 0x9D, 0x38, 0xF5, 0xBC, 0xB6, 0xDA, 0x21, 0x10, 0xFF, 0xF3, 0xD2, 0xCD, 0x0C, 0x13, 0xEC, 0x5F, 0x97, 0x44, 0x17, 0xC4, 0xA7, 0x7E, 0x3D, 0x64, 0x5D, 0x19, 0x73, 0x60, 0x81, 0x4F, 0xDC, 0x22, 0x2A, 0x90, 0x88, 0x46, 0xEE, 0xB8, 0x14, 0xDE, 0x5E, 0x0B, 0xDB, 0xE0, 0x32, 0x3A, 0x0A, 0x49, 0x06, 0x24, 0x5C, 0xC2, 0xD3, 0xAC, 0x62, 0x91, 0x95, 0xE4, 0x79, 0xE7, 0xC8, 0x37, 0x6D, 0x8D, 0xD5, 0x4E, 0xA9, 0x6C, 0x56, 0xF4, 0xEA, 0x65, 0x7A, 0xAE, 0x08, 0xBA, 0x78, 0x25, 0x2E, 0x1C, 0xA6, 0xB4, 0xC6, 0xE8, 0xDD, 0x74, 0x1F, 0x4B, 0xBD, 0x8B, 0x8A, 0x70, 0x3E, 0xB5, 0x66, 0x48, 0x03, 0xF6, 0x0E, 0x61, 0x35, 0x57, 0xB9, 0x86, 0xC1, 0x1D, 0x9E, 0xE1, 0xF8, 0x98, 0x11, 0x69, 0xD9, 0x8E, 0x94, 0x9B, 0x1E, 0x87, 0xE9, 0xCE, 0x55, 0x28, 0xDF, 0x8C, 0xA1, 0x89, 0x0D, 0xBF, 0xE6, 0x42, 0x68, 0x41, 0x99, 0x2D, 0x0F, 0xB0, 0x54, 0xBB, 0x16};

			for (byte i = 0; i < 16; i++)
			{
				bytes[i] = subbox[bytes[i]];
			}
		}
		// AES shift rows step
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void shiftrows(byte* bytes) const
		private void shiftrows(byte[] bytes)
		{
			byte remember = bytes[1];
			bytes[1] = bytes[5];
			bytes[5] = bytes[9];
			bytes[9] = bytes[13];
			bytes[13] = remember;

			remember = bytes[10];
			bytes[10] = bytes[2];
			bytes[2] = remember;

			remember = bytes[3];
			bytes[3] = bytes[15];
			bytes[15] = bytes[11];
			bytes[11] = bytes[7];
			bytes[7] = remember;

			remember = bytes[14];
			bytes[14] = bytes[6];
			bytes[6] = remember;
		}
		// AES mix columns step
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void mixcolumns(byte* bytes) const
		private void mixcolumns(byte[] bytes)
		{
			for (byte i = 0; i < 16; i += 4)
			{
				byte cpy0 = bytes[i];
				byte cpy1 = bytes[i + 1];
				byte cpy2 = bytes[i + 2];
				byte cpy3 = bytes[i + 3];

				byte mul0 = gmul(bytes[i]);
				byte mul1 = gmul(bytes[i + 1]);
				byte mul2 = gmul(bytes[i + 2]);
				byte mul3 = gmul(bytes[i + 3]);

                /* AppDebug.Log($"mul0:{mul0} {cpy0}");
                AppDebug.Log($"mul1:{mul1} {cpy1}");
                AppDebug.Log($"mul2:{mul2} {cpy2}");
                AppDebug.Log($"mul3:{mul3} {cpy3}"); */

                bytes[i] = (byte)(mul0 ^ cpy3 ^ cpy2 ^ mul1 ^ cpy1);
				bytes[i + 1] = (byte)(mul1 ^ cpy0 ^ cpy3 ^ mul2 ^ cpy2);
				bytes[i + 2] = (byte)(mul2 ^ cpy1 ^ cpy0 ^ mul3 ^ cpy3);
				bytes[i + 3] = (byte)(mul3 ^ cpy2 ^ cpy1 ^ mul0 ^ cpy0);

				/* AppDebug.Log($"mul0:{mul0} {bytes[i]}");
				AppDebug.Log($"mul1:{mul1} {bytes[i+1]}");
				AppDebug.Log($"mul2:{mul2} {bytes[i+2]}");
				AppDebug.Log($"mul3:{mul3} {bytes[i+3]}"); */
            }
		}
		// Perform a Galois multiplication
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: byte gmul(byte x) const
		public byte gmul(byte x)
		{
            byte a = (byte)(x << 1);
            byte b = (byte)((sbyte)x >> 7);
            byte c = ((byte)(0x1B & (byte)((sbyte)x >> 7)));
            byte d = (byte)((x << 1) ^ (0x1B & (byte)((sbyte)x >> 7)));
			//AppDebug.Log($"gmul:{a} {b} {c} {d}");

            return (byte)((x << 1) ^ (0x1B & (byte)((sbyte)x >> 7)));
		}
/*  */
#if USE_CRYPTO
		private byte[] sendiv = new byte[NetConstants.HEADER_LENGTH] { 70, 114, 122, 82 };
		private byte[] recviv = new byte[NetConstants.HEADER_LENGTH] { 82, 48, 120, 115 };
#endif
	}
}
