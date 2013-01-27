dotLua
======
dotLua is a Lua wrapper for C# utilizing C# 4.0 dynamic class.

Example
======
	using (dynamic lua = new Lua())
	{
		lua.Do("test.lua");
		lua.say_hi();
	}

License
======
This program is free software. It comes without any warranty, to
the extent permitted by applicable law. You can redistribute it
and/or modify it under the terms of the Do What The Fuck You Want
To Public License, Version 2, as published by Sam Hocevar. See
http://sam.zoy.org/wtfpl/COPYING for more details.
