dotLua
======
dotLua is a Lua wrapper for C# utilizing C# 4.0 dynamic class.

Example
======
test.lua:
	function say_hi(...)
		if ... ~= nil then
			message = "Hi"
			for _, v in pairs({...}) do
				message = message .. ", " .. v
			end
			print(message)
		else
			print("Hi from Lua")
		end
	end

	function raise(message)
		error(message)
	end

In C#:
	using (dynamic lua = new Lua())
	{
			lua.Do("test.lua");
			lua.say_hi();
			lua.say_hi("dotLua");
			lua.say_hi(3.1415926);
			lua.say_hi(3.1415926, "dotLua", "Sunday");

			try
			{
				lua.raise("Boom!");
			}
			catch (InvocationException e)
			{
				Console.WriteLine(e.Message);
			}
	}

License
======
This program is free software. It comes without any warranty, to
the extent permitted by applicable law. You can redistribute it
and/or modify it under the terms of the Do What The Fuck You Want
To Public License, Version 2, as published by Sam Hocevar. See
http://sam.zoy.org/wtfpl/COPYING for more details.
