updateSceneMain = {

	awake = function()
		print("awake....")
	end,
	
	start = function()
		print("start....")
	end,
	
	update = function()
		print("update....")
	end,
	
	lateUpdate = function()
		print("lateUpdate....")
	end,

	
}
updateSceneMain.conf = {
	"People.lua",
}

return updateSceneMain