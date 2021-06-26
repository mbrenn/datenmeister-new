var gulp = require('gulp');


gulp.task('default', function(cb)
{
    gulp.src('node_modules/popper.js/dist/esm/*.min.js').pipe(gulp.dest('wwwroot/js/'));
    gulp.src('node_modules/bootstrap/dist/js/*.esm.min.js').pipe(gulp.dest('wwwroot/js/'));
    gulp.src('node_modules/jquery/dist/*.js').pipe(gulp.dest('wwwroot/js/'));

    gulp.src('node_modules/requirejs/*.js').pipe(gulp.dest('wwwroot/js/'));

    console.log("JavaScript Files are copied");
    cb();
});