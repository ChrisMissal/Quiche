var exec = require('child_process').exec
  , Winreg = require('winreg')
  , gutil = require('gulp-util')
  , map = require('map-stream');

module.exports = function(options){	

	if(!options) {
		options = {};
	}

	return map(function (file, cb) {
	
		// see: https://coderwall.com/p/0eds7q
		function isOSWin64() {
		  return process.arch === 'x64' || process.env.hasOwnProperty('PROCESSOR_ARCHITEW6432');
		}

		var regKey = new Winreg({
		hive: Winreg.HKLM,
		key : "\\SOFTWARE\\Microsoft\\MSBuild\\ToolsVersions\\4.0"
		});

		var pathValueName = isOSWin64() ? "MsBuildToolsPath" : "MsBuildToolsPath32";

		regKey.get(pathValueName, function(err, item) {
			if(err){			
				cb(err);
			}
			else {
				options.cwd = process.cwd();
				options.projectFile = file.path;
				options.msbuild = item.value + 'MSBuild.exe';				

				 var command = '<%= options.msbuild %> <%= options.args %> <%= file.path %>';

				 var cmd = gutil.template(command, {file:file, options : options});

				 gutil.log('MSBuild Command: ' + cmd);

				 exec(cmd, function(error, stdout, stderr){
				 	if(stderr) {
				 		gutil.log(stderr);
				 	}

				 	if(stdout){
				 		stdout = stdout.trim(); // Trim trailing cr-lf
				 		gutil.log(stdout);
				 	}

				 	cb(error, file);
				 });
			}
		});
	});
}
