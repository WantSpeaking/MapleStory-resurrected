/*using io_service = asio.io_service;
using tcp = asio.ip.tcp;
using error_code = asio.error_code;*/




#if USE_ASIO

#define BOOST_DATE_TIME_NO_LIB
#define BOOST_REGEX_NO_LIB
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following #include directive was ignored:
//#include "asio.hpp"

namespace ms
{

	// Class that wraps an asio socket.
	public class SocketAsio : System.IDisposable
	{
		public SocketAsio()
		{
			this.resolver = ioservice;
			this.socket = ioservice;
		}
		public void Dispose()
		{
			if (socket.is_open())
			{
				error_code error = new error_code();
				socket.close(error);
			}
		}

		public bool open(string address, string port)
		{
			tcp.resolver.query query = new tcp.resolver.query(address, port);
			tcp.resolver.iterator endpointiter = resolver.resolve(query);
			error_code error = new error_code();
			asio.connect(socket, endpointiter, error);

			if (error == null)
			{
				uint result = socket.read_some(asio.buffer(buffer), error);
				return error == null && (result == HANDSHAKE_LEN);
			}

			return error == null;
		}
		public bool close()
		{
			error_code error = new error_code();
			socket.shutdown(tcp.socket.shutdown_both, error);
			socket.close(error);

			return error == null;
		}
		public uint receive(ref bool recvok)
		{
			if (socket.available() > 0)
			{
				error_code error = new error_code();
				uint result = socket.read_some(asio.buffer(buffer), error);
				recvok = error == null;

				return result;
			}

			return 0;
		}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: const sbyte* get_buffer() const
		public string get_buffer()
		{
			return buffer;
		}
		public bool dispatch(string bytes, uint length)
		{
			error_code error = new error_code();
			uint result = asio.write(socket, asio.buffer(bytes, length), error);

			return error == null && (result == length);
		}

		private io_service ioservice = new io_service();
		private tcp.resolver resolver = new tcp.resolver();
		private tcp.socket socket = new tcp.socket();
		private string buffer = new string(new char[MAX_PACKET_LENGTH]);
	}
}
#endif

#if USE_ASIO
#endif