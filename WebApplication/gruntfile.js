/// <vs BeforeBuild='bower:install' />
module.exports = function (grunt) {
    grunt.initConfig({
        bower: {
            install: {
                options: {
                    cleanup: true
                }
            }
        }
    });
    grunt.loadNpmTasks('grunt-bower-task');
}


