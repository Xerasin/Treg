--require 'luanet'
import 'OpenTK'
local t = 0
hook.Add("Think","RotateEnts", function()
	local tbl = ents.GetAll()
	for k,v in pairs(tbl) do
		--v.Angles.Yaw = v.Angles.Yaw + 2
		if v:GetClass() == "env_spotlight" then
			v.Color = Vector3(math.random(), math.random(), math.random())
		end
	end
end)
local tbl = ents.GetAll()
for k,v in pairs(tbl) do
	print(k, ": ", v)
end
--[[local test = ents.Create("ent_popcorn")
test.Position.Y = 5]]
for I=0,360,40 do
	local test = ents.Create("env_spotlight")
	local x = math.cos(math.rad(I)) * 10
	local z = math.sin(math.rad(I)) * 10
	test.Position = Vector3(x, 5, z)
	test.Color = Vector3(math.random(), math.random(), math.random())
	test.AmbientIntensity = 0.2
	test.DiffuseIntensity = 0
	test.Constant = 0.5
	test.Linear = 0
	test.Direction = Vector3(0, -1, 0)
	test.Cutoff = 0.9
	test.Enabled = true
end