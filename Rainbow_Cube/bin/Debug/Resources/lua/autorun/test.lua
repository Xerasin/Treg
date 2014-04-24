--require 'luanet'
import 'OpenTK'
local t = 0
hook.Add("Think","RotateEnts", function()
	local tbl = ents.GetAll()
	for k,v in pairs(tbl) do
		v.Angles.Yaw = v.Angles.Yaw + 2
		if v:GetClass() == "env_spotlight" then
			local col = v.Color
			v.Color = Vector3(math.random(), math.random(), math.random())
			--print(v.Position.Y, y)
		end
	end
	t = t + 1
	if t == 100 then
		
		print("Butts")
		t = 0
	end
end)
local tbl = ents.GetAll()
for k,v in pairs(tbl) do
	print(k, ": ", v)
end
local test = ents.Create("sent_popcorn")
test.Position.Y = 5
for I=0,360,40 do
	local test = ents.Create("env_spotlight")
	local x = math.cos(math.rad(I)) * 10
	local z = math.sin(math.rad(I)) * 10
	test.Position = Vector3(x, 0, z)
	test.Color = Vector3(math.random(), math.random(), math.random())
	test.AmbientIntensity = 0
	test.DiffuseIntensity = 0.5
	test.Constant = 0
	test.Linear = 0.1
	test.Direction = Vector3(0, -1, 0)
	test.Cutoff = 0.1
	test.Enabled = true
end