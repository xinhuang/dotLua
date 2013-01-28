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

GlobalValue = 3
