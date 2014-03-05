var map = require('map-stream')
    , csv = require('csv')
	, fs = require('fs')
	, _ = require('lodash')
	, path = require('path')
	;

module.exports = function () {
	return map(function (file, cb) {
		var baseName = path.basename(file.path, '.csv');
		var dest = baseName + '.md';
		var stream = fs.createWriteStream(dest);
		stream.once('open', function(fd) {
			stream.write('# Features of Version ' + baseName.split('-')[1] + '\n\n');
			csv()
			.from(file.path)
			.on('record', function (row, index) {
				var feature = { type: row[0], version: row[2], className: row[3], method: row[4], desc: row[5] },
					removed = feature.type === 'removed';

				stream.write('* ' + (removed ? '~~' : '') + feature.className + '.' + feature.method + ' - ' + feature.desc + (removed ? '~~ (no longer available)' : '') + '\n');
			})
			.on('end', function () {
				stream.end();
			});
		});
	});
}
