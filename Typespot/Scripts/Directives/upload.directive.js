(function() {
    'use strict';

    angular
        .module('app')
        .directive('upload', upload);

    upload.$inject = ['$window'];
    
    function upload ($window) {
        // Usage:
        //     <upload></upload>
        // Creates:
        // <upload>
        // <button>$label</button>
        // <input type="file" id="$id" name="$name" style="display:none;" />
        // </upload>
        var directive = {
            link: link,
            restrict: 'EA'
        };

        return directive;

        function link($scope, $element, $attrs) {
            var $input = $('<input />', {
                'accept':$attrs.accept,
                'type': 'file',
                'style': 'display:none;',
                'id': $attrs.id,
                'name':$attrs.name
            });
            
            var $button = $($attrs.target);

            $button.on('click', function () {
                $input.click();
            });
           
            $element.append($input);
            $element.append($button);

            // TODO: preview image
            //$input.on('change', function () {
            //    if (x.files.length == 0) {
            //        $button.text("Select one or more files.");
            //    } else {
            //        for (var i = 0; i < x.files.length; i++) {
            //            txt += "<br><strong>" + (i + 1) + ". file</strong><br>";
            //            var file = x.files[i];
            //            if ('name' in file) {
            //                txt += "name: " + file.name + "<br>";
            //            }
            //            if ('size' in file) {
            //                txt += "size: " + file.size + " bytes <br>";
            //            }
            //        }
            //    }
            //});
        };
    }

})();