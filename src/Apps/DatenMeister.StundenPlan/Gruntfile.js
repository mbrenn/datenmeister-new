module.exports = function (grunt) {
    grunt.initConfig({
        uglify: {
            all_src: {
                options: {
                    sourceMap: true,
                    sourceMapName: 'resources/sourceMap.map'
                },
                src: 'resources/*.js',
                dest: 'resources/stundenplan.min.js'
            }
        }
    });

    grunt.loadNpmTasks('grunt-contrib-uglify');
    grunt.registerTask('default', ['uglify']);
};

