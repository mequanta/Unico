var gulp = require('gulp');
//var jshint = require('gulp-jshint');
//var concat = require('gulp-concat');
//var rename = require('gulp-rename');
//var uglify = require('gulp-uglify');
var rimraf = require('rimraf');
var copy = require('gulp-copy');
//var less = require('gulp-less');
//var htmlmin = require('gulp-htmlmin');
//var imagemin = require('gulp-imagemin');
//var wiredep = require('wiredep').stream;

gulp.task('bower', function () {
  gulp.src('./app/ide.html')
    .pipe(wiredep({
      exclude: [],
    }))
    .pipe(gulp.dest('../wwwdist'));
});

gulp.task('clean', function (cb) {
  rimraf('../wwwdist', cb);
});

gulp.task('copy', function() {
  return gulp.src(['nls/**/*'])
    .pipe(copy('../wwwdist'));
});

// Lint JS
gulp.task('lint', function() {
    return gulp.src('src/*.js')
        .pipe(jshint())
        .pipe(jshint.reporter('default'));
});

// Concat & Minify JS
gulp.task('minify', function(){
    return gulp.src('src/*.js')
        .pipe(concat('all.js'))
        .pipe(gulp.dest('dist'))
        .pipe(rename('all.min.js'))
        .pipe(uglify())
        .pipe(gulp.dest('dist'));
});

// Default
gulp.task('default', ['clean', 'copy']);