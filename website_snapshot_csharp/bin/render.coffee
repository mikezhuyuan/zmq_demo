sys = require 'system' 
page = require('webpage').create()
[url, width, height, output] = sys.args[1..]

page.onConsoleMessage = (msg) -> console.log msg

page.viewportSize = "width": width, "height": height
begin = Date.now()
page.open url, (status) ->
	page.render output
	console.log "STATUS:" + status + ",ELAPSED:"+(Date.now() - begin)
	phantom.exit()