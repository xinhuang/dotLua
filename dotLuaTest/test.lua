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

function return_0()
	return 0
end

function return_0_1()
	return 0, 1
end

function return_0_str()
	return 0, "Hi~"
end

GlobalNumber = 3
GlobalString = "Good luck~"
GlobalBoolean = true

GlobalTable = { Field = 3.14, }

function GlobalTable.Pi_x(times)
	return 3.14 * times
end
