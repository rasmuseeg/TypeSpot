/*
This file in the main entry point for defining grunt tasks and using grunt plugins.
Click here to learn more. http://go.microsoft.com/fwlink/?LinkID=513275&clcid=0x409
*/
module.exports = function (grunt) {

    // Plugin loading
    grunt.loadNpmTasks('less-plugin-autoprefix');
    grunt.loadNpmTasks('grunt-contrib-less');
    grunt.loadNpmTasks('grunt-contrib-watch');

    // Configuration
    grunt.initConfig({
        pkg: grunt.file.readJSON('package.json'),
        less: {
            development: {
                plugins: [
                  new (require('less-plugin-autoprefix'))({ browsers: ["last 2 versions", ">= ie 8"] })
                ],
                sourceMap: true,
                sourceMapFilename: "content/css/site.map.css",
                sourceMapUrl: "/content/css/site.map.css",
                files: {
                    "content/css/site.css": "content/less/site.less"
                }
            }
        },
        watch: {
            less: {
                files: ['Content/less/**/*.less'],  //watched files
                tasks: ['less'],  //tasks to run
                
            }
        },
    });

    // Task definition
    grunt.registerTask("default", ["watch:less"]);
};