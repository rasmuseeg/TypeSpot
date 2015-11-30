(function() {
    'use strict';

    angular
        .module('app')
        .directive('wysiwyg', wysiwyg);

    wysiwyg.$inject = ['$window'];
    
    function wysiwyg($window) {
        // Usage:
        //     <wysigwy></wysigwy>
        // Creates:
        // 
        var directive = {
            link: link,
            restrict: 'EA'
        };
        return directive;

        function link($scope, $element, $attrs) {
            $element.css({
                'overflow': 'hidden',
                'min-height': '200px'
            });
            $element.wysiwyg();

            var $input = $($attrs.target);
            var $form = $($input[0].form);

            $form.on('submit', function () {
                $input.val($element.cleanHtml());
            });
        }
    }

})();