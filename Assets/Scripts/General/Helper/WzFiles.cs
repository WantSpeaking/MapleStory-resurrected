#define USE_NX




#if ! USE_NX



// TODO: Remove below once WZ is implemented and has these functions
namespace nl
{
	public class bitmap
	{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: The implementation of the following method could not be found:
//		bitmap();
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: The implementation of the following method could not be found:
//		bitmap(bitmap NamelessParameter);

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void const* data() const
		public void data()
		{
			return 0;
		}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: ushort Width() const
		public ushort Width()
		{
			return 0;
		}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: ushort height() const
		public ushort height()
		{
			return 0;
		}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: uint id() const
		public uint id()
		{
			return 0;
		}
	}

	public class audio
	{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: The implementation of the following method could not be found:
//		audio();
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: The implementation of the following method could not be found:
//		audio(audio NamelessParameter);

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void const* data() const
		public void data()
		{
			return 0;
		}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: uint length() const
		public uint length()
		{
			return 0;
		}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: uint id() const
		public uint id()
		{
			return 0;
		}
	}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 NOTE: C# has no need of forward class declarations:
//	struct _file_data;
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: Typedefs defined in multiple preprocessor conditionals can only be replaced within the scope of the preprocessor conditional:
//	typedef System.Tuple<int, int> vector2i;
	public class node
	{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: The implementation of the following type could not be found:
//		struct data;
		public enum type : ushort
		{
			none = 0,
			integer = 1,
			real = 2,
			string = 3,
			vector = 4,
			bitmap = 5,
			audio = 6,
		}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: The implementation of the following method could not be found:
//		node();
		public node(node UnnamedParameter1)
		{
		}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 NOTE: This 'CopyFrom' method was converted from the original copy assignment operator:
//ORIGINAL LINE: node& operator =(node const&) = default;
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: The implementation of the following method could not be found:
//		node CopyFrom(node NamelessParameter);
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: node begin() const
		public node begin()
		{
			return new node();
		}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: node end() const
		public node end()
		{
			return new node();
		}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: node operator *() const
		public node Indirection()
		{
			return new node();
		}
		public static node operator ++(node ImpliedObject)
		{
			return *new ImpliedObject.node();
		}
		public static node operator ++(int UnnamedParameter1)
		{
			return new node();
		}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool operator ==(node const&) const
		public static bool operator == (node ImpliedObject, node UnnamedParameter1)
		{
			return false;
		}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool operator !=(node const&) const
		public static bool operator != (node ImpliedObject, node UnnamedParameter1)
		{
			return false;
		}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool operator <(node const&) const
		public static bool operator < (node ImpliedObject, node UnnamedParameter1)
		{
			return false;
		}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: explicit operator bool() const
		public static explicit operator bool(node ImpliedObject)
		{
			return false;
		}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: node operator [](uint) const
		public node this[uint UnnamedParameter1]
		{
			get
			{
				return new node();
			}
		}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: node operator [](int) const
		public node this[int UnnamedParameter1]
		{
			get
			{
				return new node();
			}
		}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: node operator [](uint) const
		public node this[uint UnnamedParameter1]
		{
			get
			{
				return new node();
			}
		}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: node operator [](int) const
		public node this[int UnnamedParameter1]
		{
			get
			{
				return new node();
			}
		}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: node operator [](ulong) const
		public node this[ulong UnnamedParameter1]
		{
			get
			{
				return new node();
			}
		}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: node operator [](long) const
		public node this[long UnnamedParameter1]
		{
			get
			{
				return new node();
			}
		}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: node operator [](string const&) const
		public node this[string UnnamedParameter1]
		{
			get
			{
				return new node();
			}
		}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: node operator [](sbyte const*) const
		public node this[string UnnamedParameter1]
		{
			get
			{
				return new node();
			}
		}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: node operator [](node const&) const
		public node this[node UnnamedParameter1]
		{
			get
			{
				return new node();
			}
		}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: operator byte() const
		public static implicit operator byte(node ImpliedObject)
		{
			return '0';
		}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: operator sbyte() const
		public static implicit operator sbyte(node ImpliedObject)
		{
			return '0';
		}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: operator ushort() const
		public static implicit operator ushort(node ImpliedObject)
		{
			return 0;
		}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: operator short() const
		public static implicit operator short(node ImpliedObject)
		{
			return 0;
		}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: operator uint() const
		public static implicit operator uint(node ImpliedObject)
		{
			return 0;
		}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: operator int() const
		public static implicit operator int(node ImpliedObject)
		{
			return 0;
		}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: operator uint() const
		public static implicit operator uint(node ImpliedObject)
		{
			return 0;
		}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: operator int() const
		public static implicit operator int(node ImpliedObject)
		{
			return 0;
		}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: operator ulong() const
		public static implicit operator ulong(node ImpliedObject)
		{
			return 0;
		}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: operator long() const
		public static implicit operator long(node ImpliedObject)
		{
			return 0;
		}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: operator float() const
		public static implicit operator float(node ImpliedObject)
		{
			return 0;
		}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: operator double() const
		public static implicit operator double(node ImpliedObject)
		{
			return 0;
		}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: operator double() const
		public static implicit operator double(node ImpliedObject)
		{
			return 0;
		}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: operator string() const
		public static implicit operator string(node ImpliedObject)
		{
			return "";
		}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: operator vector2i() const
		public static implicit operator vector2i(node ImpliedObject)
		{
			return vector2i();
		}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: operator bitmap() const
		public static implicit operator bitmap(node ImpliedObject)
		{
			return new bitmap();
		}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: operator audio() const
		public static implicit operator audio(node ImpliedObject)
		{
			return new audio();
		}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: long get_integer(long = 0) const
		public long get_integer(long UnnamedParameter1 = 0)
		{
			return 0;
		}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: double get_real(double = 0) const
		public double get_real(double UnnamedParameter1 = 0)
		{
			return 0;
		}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: string get_string(string = "") const
		public string get_string(string UnnamedParameter1 = "")
		{
			return "";
		}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: vector2i get_vector(vector2i = { 0, 0 }) const
		public vector2i get_vector(vector2i UnnamedParameter1 = {0, 0})
		{
			return vector2i();
		}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bitmap get_bitmap() const
		public bitmap get_bitmap()
		{
			return new bitmap();
		}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: audio get_audio() const
		public audio get_audio()
		{
			return new audio();
		}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool get_bool() const
		public bool get_bool()
		{
			return false;
		}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool get_bool(bool) const
		public bool get_bool(bool UnnamedParameter1)
		{
			return false;
		}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: int x() const
		public int x()
		{
			return 0;
		}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: int y() const
		public int y()
		{
			return 0;
		}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: string name() const
		public string name()
		{
			return "";
		}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: uint size() const
		public uint size()
		{
			return 0;
		}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: type data_type() const
		public type data_type()
		{
			return type.none;
		}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: node root() const
		public node root()
		{
			return new node();
		}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: node resolve(string) const
		public node resolve(string UnnamedParameter1)
		{
			return new node();
		}

		private node(data UnnamedParameter1, _file_data UnnamedParameter2)
		{
		}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: node get_child(sbyte const*, ushort) const
		private node get_child(string UnnamedParameter1, ushort UnnamedParameter2)
		{
		}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: long to_integer() const
		private long to_integer()
		{
		}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: double to_real() const
		private double to_real()
		{
		}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: string to_string() const
		private string to_string()
		{
		}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: vector2i to_vector() const
		private vector2i to_vector()
		{
		}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bitmap to_bitmap() const
		private bitmap to_bitmap()
		{
		}
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: audio to_audio() const
		private audio to_audio()
		{
		}
		private data m_data = null;
		private _file_data m_file = null;
		//friend file;
	}

	namespace nx
	{
	}
}
#endif

#if ! USE_NX

#endif