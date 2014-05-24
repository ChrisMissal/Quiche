var gulp = require('gulp')
   , clean = require('gulp-clean')
   , msbuild = require('./gulp-msbuild.js')   
   , exec = require('gulp-exec')
   , path = require('path')
   , features = require('./gulp-quiche-features.js')
   ;

var paths = {
   solution : "./src/Quiche.sln",
   testAssemblies : ['./src/Quiche.Tests/bin/Debug/*.Tests.dll'],   
   fixieRunner : './src/packages/Fixie.0.0.1.133/lib/net45/Fixie.Console.exe',
   featureFiles: './src/Quiche.Tests/bin/Debug/*features.csv'
}

gulp.task('compile', function() {
   return gulp.src(paths.solution)
              .pipe(msbuild({ args : '/t:clean;build /v:q /p:Configuration=DEBUG' }));
});

gulp.task('fixie-tests', ['compile'], function() {

   var fixieCommand = path.resolve(process.cwd(), paths.fixieRunner) + ' <%= file.path %>';

   gulp.src(paths.featureFiles).pipe(clean());

   return gulp.src(paths.testAssemblies)
              .pipe(exec(fixieCommand));
});

gulp.task('doc-features', ['fixie-tests'], function () {
   return gulp.src(paths.featureFiles)
              .pipe(features());
});

gulp.task('build', ['fixie-tests', 'doc-features']);

gulp.task('default', ['build']);
